//
// Copyright (c) 2008-2011, Kenneth Bell
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//

namespace AlIO.Nfs
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    internal sealed class RpcTcpTransport : IDisposable
    {
        private const int RetryLimit = 20;

        private string _address;
        private int _port;
        private int _localPort;
        private Socket _socket;
        private NetworkStream _tcpStream;

        public RpcTcpTransport(string address, int port)
            : this(address, port, 0)
        {
        }

        public RpcTcpTransport(string address, int port, int localPort)
        {
            _address = address;
            _port = port;
            _localPort = localPort;
        }

        public void Dispose()
        {
            if (_tcpStream != null)
            {
                _tcpStream.Dispose();
                _tcpStream = null;
            }

            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }
        }

        public byte[] Send(byte[] message)
        {
            int retries = 0;
            int retryLimit = RetryLimit;
            Exception lastException = null;

            bool isNewConnection = _socket == null;
            if (isNewConnection)
            {
                retryLimit = 1;
            }

            byte[] response = null;
            while (response == null && retries < retryLimit)
            {
                while (retries < retryLimit && (_socket == null || !_socket.Connected))
                {
                    try
                    {
                        if (_tcpStream != null)
                        {
                            _tcpStream.Close();
                            _tcpStream = null;
                        }

                        if (_socket != null)
                        {
                            _socket.Close();
                            _socket = null;
                        }

                        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        _socket.NoDelay = true;
                        if (_localPort != 0)
                        {
                            _socket.Bind(new IPEndPoint(0, _localPort));
                        }

                        _socket.Connect(_address, _port);
                        _tcpStream = new NetworkStream(_socket, false);
                    }
                    catch (IOException connectException)
                    {
                        retries++;
                        lastException = connectException;

                        if (!isNewConnection)
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    catch (SocketException se)
                    {
                        retries++;
                        lastException = se;

                        if (!isNewConnection)
                        {
                            Thread.Sleep(1000);
                        }
                    }
                }

                if (_tcpStream != null)
                {
                    try
                    {
                        byte[] header = new byte[4];
                        Utilities.WriteBytesBigEndian((uint)(0x80000000 | (uint)message.Length), header, 0);
                        _tcpStream.Write(header, 0, 4);
                        _tcpStream.Write(message, 0, message.Length);
                        _tcpStream.Flush();

                        response = Receive();
                    }
                    catch (IOException sendReceiveException)
                    {
                        lastException = sendReceiveException;

                        _tcpStream.Close();
                        _tcpStream = null;
                        _socket.Close();
                        _socket = null;
                    }

                    retries++;
                }
            }

            if (response == null)
            {
                throw new IOException(string.Format(CultureInfo.InvariantCulture, "Unable to send RPC message to {0}:{1}", _address, _port), lastException);
            }

            return response;
        }

        private byte[] Receive()
        {
            MemoryStream ms = null;
            bool lastFragFound = false;

            while (!lastFragFound)
            {
                byte[] header = Utilities.ReadFully(_tcpStream, 4);
                uint headerVal = Utilities.ToUInt32BigEndian(header, 0);

                lastFragFound = (headerVal & 0x80000000) != 0;
                byte[] frag = Utilities.ReadFully(_tcpStream, (int)(headerVal & 0x7FFFFFFF));

                if (ms != null)
                {
                    ms.Write(frag, 0, frag.Length);
                }
                else if (!lastFragFound)
                {
                    ms = new MemoryStream();
                    ms.Write(frag, 0, frag.Length);
                }
                else
                {
                    return frag;
                }
            }

            return ms.ToArray();
        }
    }
}

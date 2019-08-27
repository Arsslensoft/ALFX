//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE.
//
//  This material may not be duplicated in whole or in part, except for 
//  personal use, without the express written consent of the author. 
//
//  Email:  ianier@hotmail.com
//
//  Copyright (C) 1999-2003 Ianier Munoz. All Rights Reserved.

using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace Al.AI
{
    public class WaveData
    {
        private static readonly short BitsPerByte = 8;
        private static readonly short MaxBits = 16;
        public short[][] Samples { get; private set; }
        public short CompressionCode { get; private set; }
        public short NumberOfChannels { get; private set; }
        public int SampleRate { get; private set; }
        public int BytesPerSecond { get; private set; }
        public short BitsPerSample { get; private set; }
        public short BlockAlign { get; private set; }
        public int NumberOfFrames { get; private set; }

        /// <summary>
        /// Reads a Wave file from the input stream, but doesn't close the stream
        /// </summary>
        /// <param name="stream">Input WAVE file stream</param>
        public WaveData(Stream stream)
        {
            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                binaryReader.ReadChars(4); //"RIFF"
                int length = binaryReader.ReadInt32();
                binaryReader.ReadChars(4); //"WAVE"
                string chunkName = new string(binaryReader.ReadChars(4)); //"fmt "
                int chunkLength = binaryReader.ReadInt32();
                this.CompressionCode = binaryReader.ReadInt16(); //1 for PCM/uncompressed
                this.NumberOfChannels = binaryReader.ReadInt16();
                this.SampleRate = binaryReader.ReadInt32();
                this.BytesPerSecond = binaryReader.ReadInt32();
                this.BlockAlign = binaryReader.ReadInt16();
                this.BitsPerSample = binaryReader.ReadInt16();
                if ((MaxBits % BitsPerSample) != 0)
                {
                    throw new Exception("The input stream uses an unhandled SignificantBitsPerSample parameter");
                }
                binaryReader.ReadChars(chunkLength - 16);
                chunkName = new string(binaryReader.ReadChars(4));
                try
                {
                    while (chunkName.ToLower() != "data")
                    {
                        binaryReader.ReadChars(binaryReader.ReadInt32());
                        chunkName = new string(binaryReader.ReadChars(4));
                    }
                }
                catch
                {
                    throw new Exception("Input stream misses the data chunk");
                }
                chunkLength = binaryReader.ReadInt32();
                this.NumberOfFrames = (chunkLength * BitsPerByte) / (this.NumberOfChannels * this.BitsPerSample);
                this.Samples = SplitChannels(binaryReader, this.NumberOfChannels, this.BitsPerSample, this.NumberOfFrames);
            }
        }

        /// <summary>
        /// Splits the channels of a binary sequence.
        /// </summary>
        /// <param name="binaryReader">The binary reader which contains the binary sequence.</param>
        /// <param name="numberOfChannels">The number of channels.</param>
        /// <param name="bitsPerSample">The bits per sample.</param>
        /// <param name="numberOfFrames">The number of frames.</param>
        /// <returns>The samples arranged by channel and frame</returns>
        public static short[][] SplitChannels(BinaryReader binaryReader, short numberOfChannels, short bitsPerSample, int numberOfFrames)
        {
            var samples = new short[numberOfChannels][];
            for (int channel = 0; channel < numberOfChannels; channel++)
            {
                samples[channel] = new short[numberOfFrames];
            }
            short readedBits = 0;
            short numberOfReadedBits = 0;
            for (int frame = 0; frame < numberOfFrames; frame++)
            {
                for (int channel = 0; channel < numberOfChannels; channel++)
                {
                    while (numberOfReadedBits < bitsPerSample)
                    {
                        readedBits |= (short)(Convert.ToInt16(binaryReader.ReadByte()) << numberOfReadedBits);
                        numberOfReadedBits += BitsPerByte;
                    }
                    var numberOfExcessBits = numberOfReadedBits - bitsPerSample;
                    samples[channel][frame] = (short)(readedBits >> numberOfExcessBits);
                    readedBits %= (short)(1 << numberOfExcessBits);
                    numberOfReadedBits = (short)numberOfExcessBits;
                }
            }
            return samples;
        }
    }
	internal class WaveInHelper
	{
		public static void Try(int err)
		{
			if (err != WaveNative.MMSYSERR_NOERROR)
				throw new Exception(err.ToString());
		}
	}

	public delegate void BufferDoneEventHandler(IntPtr data, int size);

	internal class WaveInBuffer : IDisposable
	{
		public WaveInBuffer NextBuffer;

		private AutoResetEvent m_RecordEvent = new AutoResetEvent(false);
		private IntPtr m_WaveIn;

		private WaveNative.WaveHdr m_Header;
		private byte[] m_HeaderData;
		private GCHandle m_HeaderHandle;
		private GCHandle m_HeaderDataHandle;

		private bool m_Recording;

		internal static void WaveInProc(IntPtr hdrvr, int uMsg, int dwUser, ref WaveNative.WaveHdr wavhdr, int dwParam2)
		{
			if (uMsg == WaveNative.MM_WIM_DATA)
			{
				try
				{
					GCHandle h = (GCHandle)wavhdr.dwUser;
					WaveInBuffer buf = (WaveInBuffer)h.Target;
					buf.OnCompleted();
				}
				catch
				{
				}
			}
		}

		public WaveInBuffer(IntPtr waveInHandle, int size)
		{
			m_WaveIn = waveInHandle;

			m_HeaderHandle = GCHandle.Alloc(m_Header, GCHandleType.Pinned);
			m_Header.dwUser = (IntPtr)GCHandle.Alloc(this);
			m_HeaderData = new byte[size];
			m_HeaderDataHandle = GCHandle.Alloc(m_HeaderData, GCHandleType.Pinned);
			m_Header.lpData = m_HeaderDataHandle.AddrOfPinnedObject();
			m_Header.dwBufferLength = size;
			WaveInHelper.Try(WaveNative.waveInPrepareHeader(m_WaveIn, ref m_Header, Marshal.SizeOf(m_Header)));
		}
		~WaveInBuffer()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (m_Header.lpData != IntPtr.Zero)
			{
				WaveNative.waveInUnprepareHeader(m_WaveIn, ref m_Header, Marshal.SizeOf(m_Header));
				m_HeaderHandle.Free();
				m_Header.lpData = IntPtr.Zero;
			}
			m_RecordEvent.Close();
			if (m_HeaderDataHandle.IsAllocated)
				m_HeaderDataHandle.Free();
			GC.SuppressFinalize(this);
		}

		public int Size
		{
			get { return m_Header.dwBufferLength; }
		}

		public IntPtr Data
		{
			get { return m_Header.lpData; }
		}

		public bool Record()
		{
			lock(this)
			{
				m_RecordEvent.Reset();
				m_Recording = WaveNative.waveInAddBuffer(m_WaveIn, ref m_Header, Marshal.SizeOf(m_Header)) == WaveNative.MMSYSERR_NOERROR;
				return m_Recording;
			}
		}

		public void WaitFor()
		{
			if (m_Recording)
				m_Recording = m_RecordEvent.WaitOne();
			else
				Thread.Sleep(0);
		}

		private void OnCompleted()
		{
			m_RecordEvent.Set();
			m_Recording = false;
		}
	}

	public class WaveInRecorder : IDisposable
	{
		private IntPtr m_WaveIn;
		private WaveInBuffer m_Buffers; // linked list
		private WaveInBuffer m_CurrentBuffer;
		private Thread m_Thread;
		private BufferDoneEventHandler m_DoneProc;
		private bool m_Finished;

		private WaveNative.WaveDelegate m_BufferProc = new WaveNative.WaveDelegate(WaveInBuffer.WaveInProc);

		public static int DeviceCount
		{
			get { return WaveNative.waveInGetNumDevs(); }
		}

		public WaveInRecorder(int device, WaveFormat format, int bufferSize, int bufferCount, BufferDoneEventHandler doneProc)
		{
			m_DoneProc = doneProc;
            WaveInHelper.Try(WaveNative.waveInOpen(out m_WaveIn, device, format, m_BufferProc, IntPtr.Zero, WaveNative.CALLBACK_FUNCTION));
			AllocateBuffers(bufferSize, bufferCount);
			for (int i = 0; i < bufferCount; i++)
			{
				SelectNextBuffer();
				m_CurrentBuffer.Record();
			}
			WaveInHelper.Try(WaveNative.waveInStart(m_WaveIn));
			m_Thread = new Thread(new ThreadStart(ThreadProc));
			m_Thread.Start();
		}
		~WaveInRecorder()
		{
			Dispose();
		}
		public void Dispose()
		{
			if (m_Thread != null)
				try
				{
					m_Finished = true;
					if (m_WaveIn != IntPtr.Zero)
						WaveNative.waveInReset(m_WaveIn);
					WaitForAllBuffers();
					m_Thread.Join();
					m_DoneProc = null;
					FreeBuffers();
					if (m_WaveIn != IntPtr.Zero)
						WaveNative.waveInClose(m_WaveIn);
				}
				finally
				{
					m_Thread = null;
					m_WaveIn = IntPtr.Zero;
				}
			GC.SuppressFinalize(this);
		}
		private void ThreadProc()
		{
			while (!m_Finished)
			{
				Advance();
				if (m_DoneProc != null && !m_Finished)
					m_DoneProc(m_CurrentBuffer.Data, m_CurrentBuffer.Size);
				m_CurrentBuffer.Record();
			}
		}
		private void AllocateBuffers(int bufferSize, int bufferCount)
		{
			FreeBuffers();
			if (bufferCount > 0)
			{
				m_Buffers = new WaveInBuffer(m_WaveIn, bufferSize);
				WaveInBuffer Prev = m_Buffers;
				try
				{
					for (int i = 1; i < bufferCount; i++)
					{
						WaveInBuffer Buf = new WaveInBuffer(m_WaveIn, bufferSize);
						Prev.NextBuffer = Buf;
						Prev = Buf;
					}
				}
				finally
				{
					Prev.NextBuffer = m_Buffers;
				}
			}
		}
		private void FreeBuffers()
		{
			m_CurrentBuffer = null;
			if (m_Buffers != null)
			{
				WaveInBuffer First = m_Buffers;
				m_Buffers = null;

				WaveInBuffer Current = First;
				do
				{
					WaveInBuffer Next = Current.NextBuffer;
					Current.Dispose();
					Current = Next;
				} while(Current != First);
			}
		}
		private void Advance()
		{
			SelectNextBuffer();
			m_CurrentBuffer.WaitFor();
		}
		private void SelectNextBuffer()
		{
			m_CurrentBuffer = m_CurrentBuffer == null ? m_Buffers : m_CurrentBuffer.NextBuffer;
		}
		private void WaitForAllBuffers()
		{
			WaveInBuffer Buf = m_Buffers;
			while (Buf.NextBuffer != m_Buffers)
			{
				Buf.WaitFor();
				Buf = Buf.NextBuffer;
			}
		}
	}
}

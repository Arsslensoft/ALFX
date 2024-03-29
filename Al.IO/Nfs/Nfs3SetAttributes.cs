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
    internal enum Nfs3SetTimeMethod
    {
        NoChange = 0,
        ServerTime = 1,
        ClientTime = 2
    }

    internal sealed class Nfs3SetAttributes
    {
        public bool SetMode { get; set; }

        public UnixFilePermissions Mode { get; set; }

        public bool SetUid { get; set; }

        public uint Uid { get; set; }

        public bool SetGid { get; set; }

        public uint Gid { get; set; }

        public bool SetSize { get; set; }

        public long Size { get; set; }

        public Nfs3SetTimeMethod SetAccessTime { get; set; }

        public Nfs3FileTime AccessTime { get; set; }

        public Nfs3SetTimeMethod SetModifyTime { get; set; }

        public Nfs3FileTime ModifyTime { get; set; }

        public void Write(XdrDataWriter writer)
        {
            writer.Write(SetMode);
            if (SetMode)
            {
                writer.Write((int)Mode);
            }

            writer.Write(SetUid);
            if (SetUid)
            {
                writer.Write(Uid);
            }

            writer.Write(SetGid);
            if (SetGid)
            {
                writer.Write(Gid);
            }

            writer.Write(SetSize);
            if (SetSize)
            {
                writer.Write(Size);
            }

            writer.Write((int)SetAccessTime);
            if (SetAccessTime == Nfs3SetTimeMethod.ClientTime)
            {
                AccessTime.Write(writer);
            }

            writer.Write((int)SetModifyTime);
            if (SetModifyTime == Nfs3SetTimeMethod.ClientTime)
            {
                ModifyTime.Write(writer);
            }
        }
    }
}

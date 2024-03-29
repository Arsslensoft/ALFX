﻿//
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

namespace AlIO.ApplePartitionMap
{
    using System;
    using System.IO;
    using AlIO.Partitions;

    internal sealed class PartitionMapEntry : PartitionInfo, IByteArraySerializable
    {
        public ushort Signature;
        public uint MapEntries;
        public uint PhysicalBlockStart;
        public uint PhysicalBlocks;
        public string Name;
        public string Type;
        public uint LogicalBlockStart;
        public uint LogicalBlocks;
        public uint Flags;
        public uint BootBlock;
        public uint BootBytes;

        private Stream _diskStream;

        public PartitionMapEntry(Stream diskStream)
        {
            _diskStream = diskStream;
        }

        public int Size
        {
            get { return 512; }
        }

        public override long FirstSector
        {
            get { return PhysicalBlockStart; }
        }

        public override long LastSector
        {
            get { return PhysicalBlockStart + PhysicalBlocks - 1; }
        }

        public override Guid GuidType
        {
            get { return Guid.Empty; }
        }

        public override byte BiosType
        {
            get { return 0xAF; }
        }

        public override string TypeAsString
        {
            get { return Type; }
        }

        internal override PhysicalVolumeType VolumeType
        {
            get { return PhysicalVolumeType.ApplePartition; }
        }

        public override SparseStream Open()
        {
            return new SubStream(_diskStream, PhysicalBlockStart * 512, PhysicalBlocks * 512);
        }

        public int ReadFrom(byte[] buffer, int offset)
        {
            Signature = Utilities.ToUInt16BigEndian(buffer, offset + 0);
            MapEntries = Utilities.ToUInt32BigEndian(buffer, offset + 4);
            PhysicalBlockStart = Utilities.ToUInt32BigEndian(buffer, offset + 8);
            PhysicalBlocks = Utilities.ToUInt32BigEndian(buffer, offset + 12);
            Name = Utilities.BytesToString(buffer, offset + 16, 32).TrimEnd('\0');
            Type = Utilities.BytesToString(buffer, offset + 48, 32).TrimEnd('\0');
            LogicalBlockStart = Utilities.ToUInt32BigEndian(buffer, offset + 80);
            LogicalBlocks = Utilities.ToUInt32BigEndian(buffer, offset + 84);
            Flags = Utilities.ToUInt32BigEndian(buffer, offset + 88);
            BootBlock = Utilities.ToUInt32BigEndian(buffer, offset + 92);
            BootBytes = Utilities.ToUInt32BigEndian(buffer, offset + 96);

            return 512;
        }

        public void WriteTo(byte[] buffer, int offset)
        {
            throw new NotImplementedException();
        }
    }
}

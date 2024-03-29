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

namespace AlIO.HfsPlus
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal sealed class FileBuffer : AlIO.Buffer
    {
        private Context _context;
        private ForkData _baseData;

        public FileBuffer(Context context, ForkData baseData)
        {
            _context = context;
            _baseData = baseData;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Capacity
        {
            get { return (long)_baseData.LogicalSize; }
        }

        public override int Read(long pos, byte[] buffer, int offset, int count)
        {
            int totalRead = 0;

            int limitedCount = (int)Math.Min(count, Math.Max(0, Capacity - pos));

            while (totalRead < limitedCount)
            {
                long extentLogicalStart;
                ExtentDescriptor extent = FindExtent(pos, out extentLogicalStart);
                long extentStreamStart = extent.StartBlock * (long)_context.VolumeHeader.BlockSize;
                long extentSize = extent.BlockCount * (long)_context.VolumeHeader.BlockSize;

                long extentOffset = (pos + totalRead) - extentLogicalStart;
                int toRead = (int)Math.Min(limitedCount - totalRead, extentSize - extentOffset);

                Stream volStream = _context.VolumeStream;
                volStream.Position = extentStreamStart + extentOffset;
                int numRead = volStream.Read(buffer, offset + totalRead, toRead);

                totalRead += numRead;
            }

            return totalRead;
        }

        public override void Write(long pos, byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void SetCapacity(long value)
        {
            throw new NotSupportedException();
        }

        public override IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
        {
            return new StreamExtent[] { new StreamExtent(start, Math.Min(start + count, Capacity) - start) };
        }

        private ExtentDescriptor FindExtent(long pos, out long extentLogicalStart)
        {
            uint blocksSeen = 0;
            uint block = (uint)(pos / _context.VolumeHeader.BlockSize);
            for (int i = 0; i < _baseData.Extents.Length; ++i)
            {
                if (blocksSeen + _baseData.Extents[i].BlockCount > block)
                {
                    extentLogicalStart = blocksSeen * (long)_context.VolumeHeader.BlockSize;
                    return _baseData.Extents[i];
                }

                blocksSeen += _baseData.Extents[i].BlockCount;
            }

            throw new NotImplementedException("Fragmented files");
        }
    }
}

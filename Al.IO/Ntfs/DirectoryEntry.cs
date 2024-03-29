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

namespace AlIO.Ntfs
{
    internal class DirectoryEntry
    {
        private Directory _directory;
        private FileRecordReference _fileReference;
        private FileNameRecord _fileDetails;

        public DirectoryEntry(Directory directory, FileRecordReference fileReference, FileNameRecord fileDetails)
        {
            _directory = directory;
            _fileReference = fileReference;
            _fileDetails = fileDetails;
        }

        public FileRecordReference Reference
        {
            get { return _fileReference; }
        }

        public FileNameRecord Details
        {
            get { return _fileDetails; }
        }

        public bool IsDirectory
        {
            get { return (_fileDetails.Flags & FileAttributeFlags.Directory) != 0; }
        }

        public string SearchName
        {
            get
            {
                string fileName = _fileDetails.FileName;
                if (fileName.IndexOf('.') == -1)
                {
                    return fileName + ".";
                }
                else
                {
                    return fileName;
                }
            }
        }

        internal void UpdateFrom(File file)
        {
            file.FreshenFileName(_fileDetails, true);
            _directory.UpdateEntry(this);
        }
    }
}

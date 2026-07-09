using System;
using System.IO;

namespace Core.IO
{
    public class FileFrameNameTable : FileBase
    {
        public FileFrameNameTable(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "FNT";
        }

    }
}

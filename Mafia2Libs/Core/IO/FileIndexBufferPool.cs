using System;
using System.IO;
using ResourceTypes.BufferPools;

namespace Core.IO
{
    public class FileIndexBufferPool : FileBase
    {
        public FileIndexBufferPool(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "IBP";
        }

    }
}

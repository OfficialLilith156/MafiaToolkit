using System;
using System.Collections.Generic;
using System.IO;
using Mafia2Tool;
using ResourceTypes.BufferPools;
using ResourceTypes.FrameResource;
using Utils.Models;

namespace Core.IO
{
    public class FileVertexBufferPool : FileBase
    {
        public FileVertexBufferPool(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "VBP";
        }

    }
}

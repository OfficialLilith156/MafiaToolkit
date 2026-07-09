using System;
using System.IO;
using ResourceTypes.Collisions;

namespace Core.IO
{
    public class FileCollision : FileBase
    {
        public FileCollision(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "COL";
        }
    }
}

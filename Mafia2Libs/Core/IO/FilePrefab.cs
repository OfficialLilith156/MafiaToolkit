using Mafia2Tool;
using System.Collections.Generic;
using System.IO;

namespace Core.IO
{
    class FilePrefab : FileBase
    {
        public FilePrefab(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "PRF";
        }

        public override bool Open()
        {
            var definitions = new List<string>();
            string directory = file.DirectoryName;
            if (directory != null)
            {
                foreach (string actorPath in Directory.GetFiles(directory, "*.act"))
                {
                    var actorFileInfo = new FileInfo(actorPath);
                    var actorFile = new FileActor(actorFileInfo);
                    definitions.AddRange(actorFile.GetDefinitionList());
                }
            }

            PrefabEditor editor = new PrefabEditor(file);
            editor.InitEditor(definitions);
            return true;
        }
    }
}

using Mafia2Tool;
using ResourceTypes.Actors;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.IO
{
    class FileActor : FileBase
    {
        public FileActor(FileInfo info) : base(info)
        {
        }

        public List<string> GetDefinitionList()
        {
            try
            {
                Actor actors = new Actor(file);

                List<string> definitions = new List<string>();

                for (int i = 0; i < actors.Definitions.Count; i++)
                {
                    definitions.Add(actors.Definitions[i].Name);
                }

                return definitions;
            }
            catch
            {
                string Message = string.Format("ERROR: Failed to read actor file: {0}", GetName());
                Console.WriteLine(Message);

                return new List<string>();
            }
        }

        public override string GetExtensionUpper()
        {
            return "ACT";
        }

        public override bool Open()
        {
            ActorEditor editor = new ActorEditor(file);
            return true;
        }

        public override bool CanConvertXboxToPC()
        {
            return true;
        }

        public override string ConvertXboxToPC()
        {
            // Reading auto-detects the byte order from the header.
            Actor actors = new Actor(file);

            if (!actors.IsBigEndian)
            {
                throw new InvalidOperationException(
                    string.Format("'{0}' is already a PC (Little Endian) actor.", GetName()));
            }

            if (actors.UsedFallbackRead)
            {
                // The full parse failed, so only a partial model was loaded. Writing
                // it back would produce a corrupt file, so refuse and surface the
                // underlying reason instead.
                throw new InvalidOperationException(string.Format(
                    "Could not fully parse the Xbox actor '{0}', so it cannot be safely converted.\nReason: {1}",
                    GetName(),
                    actors.PrimaryReadException != null ? actors.PrimaryReadException.Message : "unknown"));
            }

            string outputPath = GetConvertedPCPath();

            // The write path always emits the canonical PC field layout; the byte
            // order is controlled purely by the writer flag, so writing as Little
            // Endian produces a valid PC file.
            actors.WriteToFile(outputPath, false);

            return outputPath;
        }
    }
}

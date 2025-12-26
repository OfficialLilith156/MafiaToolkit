using Mafia2Tool.Forms;
using Microsoft.Win32;
using ResourceTypes.ImageFileList;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Core.IO
{
    public class FileIFL : FileBase
    {
        public FileIFL(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "IFL";
        }

        public override bool Open()
        {
            ImageFileList img = new ImageFileList();
            using (var ms = new MemoryStream(File.ReadAllBytes(file.FullName)))
                img.ReadFromFile(ms, false);

            using (var editor = new IFLEditorForm(img.Images.Select(i => i.Name).ToList()))
            {
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(file.FullName, file.FullName + "_old", true);
                    img.Images = editor.Textures.Select(t => new ImageFileList.Image { Name = t.Name }).ToArray();
                    img.WriteToFile(file.FullName, false);
                }
            }

            return true;
        }


        public override void Save()
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog()
            {
                InitialDirectory = Path.GetDirectoryName(file.FullName),
                FileName = Path.GetFileNameWithoutExtension(file.FullName),
                Filter = "XML (*.xml)|*.xml"
            };

            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageFileList IMGFileList = new ImageFileList();
                IMGFileList.ConvertFromXML(openFile.FileName);

                File.Copy(file.FullName, file.FullName + "_old", true);
                IMGFileList.WriteToFile(file.FullName, false);
            }
        }


        public override bool CanContextMenuOpen()
        {
            return true;
        }

        public override string GetContextMenuOpenTitle()
        {
            return "Convert To (.xml)";
        }

        public override bool CanContextMenuSave()
        {
            return true;
        }

        public override string GetContextMenuSaveTitle()
        {
            return "Convert From (.xml)";
        }
    }
}

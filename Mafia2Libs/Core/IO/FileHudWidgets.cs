using Mafia2Tool.Forms;
using ResourceTypes.Navigation;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Core.IO
{
    public class FileHudWidgets : FileBase
    {
        public FileHudWidgets(FileInfo info) : base(info)
        {

        }

        public override string GetExtensionUpper()
        {
            return "Xml";
        }

        public override bool Open()
        {
            var editorForm = new HudWidgets(file.FullName);
            editorForm.Show();
            return false;
        }

        public override bool CanContextMenuOpen()
        {
            return true;
        }

        public override string GetContextMenuOpenTitle()
        {
            return "Open with Info DAT";
        }

        public override bool CanContextMenuSave()
        {
            return false;
        }

        public override string GetContextMenuSaveTitle()
        {
            return "Save DAT";
        }
    }
}

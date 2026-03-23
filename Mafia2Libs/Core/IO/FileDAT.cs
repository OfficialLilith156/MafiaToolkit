using Mafia2Tool.Forms;
using ResourceTypes.Navigation;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Core.IO
{
    public class FileDAT : FileBase
    {
        public FileDAT(FileInfo info) : base(info)
        {

        }

        public override string GetExtensionUpper()
        {
            return "DAT";
        }

        public override bool Open()
        {
            var editorForm = new DATEditor(file);
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

using Mafia2Tool.Forms;
using ResourceTypes.Navigation;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Core.IO
{
    public class FileNavigation_OBJ : FileBase
    {
        public FileNavigation_OBJ(FileInfo info) : base(info)
        {

        }

        public override string GetExtensionUpper()
        {
            return "NOV";
        }

        public override bool Open()
        {
            var editorForm = new SizeNOVEditor(file.FullName);
            editorForm.Show();

            return true;
        }

        public override bool CanContextMenuOpen()
        {
            return true;
        }

        public override string GetContextMenuOpenTitle()
        {
            return "Open with Info NOV";
        }

        public override bool CanContextMenuSave()
        {
            return false;
        }

        public override string GetContextMenuSaveTitle()
        {
            return "Save Navigation Data";
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ResourceTypes.GameParams;
using Utils.Language;
using Utils.Settings;
namespace Mafia2Tool
{
    public partial class GameParamsEditor : Form
    {
        private FileInfo paramsFile;
        private GameParamsFile paramsData;
        private bool bIsFileEdited;

        public GameParamsEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            paramsFile = file;
            BuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Editing GameParams File.");
        }

        private void Localise()
        {
            Text = Language.GetString("$GAMEPARAMS_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            Button_Tools.Text = Language.GetString("$TOOLS");
            Button_ExportXml.Text = Language.GetString("$EXPORT_XML");
            Button_ImportXml.Text = Language.GetString("$IMPORT_XML");         
            Button_Save.Enabled = true;
            Button_ExportXml.Enabled = true;
            Button_ImportXml.Enabled = true;
            Button_Tools.Enabled = true;
            PropertyGrid_Main.Enabled = false;
        }

        private void BuildData()
        {
            TreeView_Main.Nodes.Clear();
            try
            {
                paramsData = new GameParamsFile(paramsFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file: {ex.Message}", "GameParams Editor",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Root node shows file info
            string fileName = Path.GetFileName(paramsFile.FullName);
            TreeNode rootNode = new TreeNode($"GameParams: {fileName}");
            rootNode.Tag = paramsData;
            // Add header info
            TreeNode headerNode = new TreeNode($"Header: {paramsData.Name} (Flags: 0x{paramsData.Flags:X8})");
            headerNode.Tag = paramsData;
            rootNode.Nodes.Add(headerNode);
           
            TreeNode entriesNode = new TreeNode($"Entries ({paramsData.Entries.Count(e => !(e is GameParamUnknown9Entry))})");
            foreach (var entry in paramsData.Entries.Where(e => !(e is GameParamUnknown9Entry)))
            {
                TreeNode entryNode = CreateEntryNode(entry);
                entriesNode.Nodes.Add(entryNode);
            }
            rootNode.Nodes.Add(entriesNode);
            TreeView_Main.Nodes.Add(rootNode);
            rootNode.Expand();
            entriesNode.Expand(); 
        }

        private TreeNode CreateEntryNode(GameParamEntry entry)
        {
            string typeName = entry.GetType().Name.Replace("GameParam", "").Replace("Entry", "");
            TreeNode node = new TreeNode($"[{typeName}] {entry.ParamName}");
            node.Tag = entry;

            if (entry is GameParamContainerEntry container)
            {
                foreach (var child in container.Children.Where(c => !(c is GameParamUnknown9Entry)))
                {
                    node.Nodes.Add(CreateEntryNode(child));
                }
            }
            return node;
        }

        private void Save()
        {
            try
            {
                File.Copy(paramsFile.FullName, paramsFile.FullName + "_old", true);
                paramsData.WriteToFile(paramsFile.FullName);
                Text = Language.GetString("$GAMEPARAMS_EDITOR_TITLE"); 
                bIsFileEdited = false;
                MessageBox.Show("File saved successfully!", "GameParams Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "GameParams Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Reload()
        {
            PropertyGrid_Main.SelectedObject = null;
            TreeView_Main.SelectedNode = null;
            BuildData();
            Text = Language.GetString("$GAMEPARAMS_EDITOR_TITLE"); 
            bIsFileEdited = false;
        }

        private void ExportXml()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "XML|*.xml";
            saveFile.FileName = Path.GetFileNameWithoutExtension(paramsFile.Name) + ".xml";
            saveFile.InitialDirectory = paramsFile.DirectoryName;
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    paramsData.ConvertToXML(saveFile.FileName);
                    MessageBox.Show($"Successfully exported to XML:\n{saveFile.FileName}", "GameParams Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting to XML: {ex.Message}", "GameParams Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ImportXml()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "XML|*.xml";
            openFile.CheckFileExists = true;
            openFile.InitialDirectory = paramsFile.DirectoryName;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    paramsData.ConvertFromXML(openFile.FileName);
                    MessageBox.Show($"Successfully imported from XML:\n{openFile.FileName}\n\nNote: The editor view will now reload to reflect the changes.", "GameParams Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reload();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing from XML: {ex.Message}", "GameParams Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OnNodeSelect(object sender, TreeViewEventArgs e)
        {

            if (e.Node?.Tag is GameParamEntry entry && !(entry is GameParamUnknown9Entry))
            {
                PropertyGrid_Main.SelectedObject = e.Node.Tag;
                PropertyGrid_Main.Enabled = true;
            }
            else
            {
                PropertyGrid_Main.SelectedObject = null;
                PropertyGrid_Main.Enabled = false;
            }
        }

        private void PropertyGrid_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            MarkAsEdited();
        }

        private void GameParamsEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                DialogResult saveChanges = MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "Toolkit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question); 
                if (saveChanges == DialogResult.Yes)
                {
                    Save();
                }
                else if (saveChanges == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void MarkAsEdited()
        {
            if (!bIsFileEdited)
            {
                Text = Language.GetString("$GAMEPARAMS_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void Button_Save_OnClick(object sender, EventArgs e) => Save();
        private void Button_Reload_OnClick(object sender, EventArgs e) => Reload();
        private void Button_Exit_OnClick(object sender, EventArgs e) => Close();
        private void Button_ExportXml_OnClick(object sender, EventArgs e) => ExportXml();
        private void Button_ImportXml_OnClick(object sender, EventArgs e) => ImportXml();
    }
}
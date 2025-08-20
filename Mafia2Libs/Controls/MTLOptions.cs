using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Utils.Language;
using Utils.Settings;

namespace Forms.OptionControls
{
    public partial class MTLOptions : UserControl
    {
        public MTLOptions()
        {
            InitializeComponent();
            Localise();
            LoadSettings();
        }

        private void Localise()
        {
            groupMTL.Text = Language.GetString("$MATERIAL_LIBS");
            removeSelectedButton.Text = Language.GetString("$MATERIAL_LIB_REMOVE");
            addLibraryButton.Text = Language.GetString("$MATERIAL_LIB_ADD");
            MTLsToLoadText.Text = Language.GetString("$MATERIAL_LIB_SELECTED");
        }

        private void LoadSettings()
        {
            string[] files = GameStorage.Instance.GetSelectedGame().Materials.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string file in files)
            {
                MTLListBox.Items.Add(file);
            }
        }

        private void AddLibrary_Click(object sender, EventArgs e)
        {
            if (MTLBrowser.ShowDialog() == DialogResult.OK)
            {
                foreach(string file in MTLBrowser.FileNames)
                {
                    MTLListBox.Items.Add(file);
                }
            }

            UpdateINIKey();
        }

        private void RemoveSelected_Click(object sender, EventArgs e)
        {
            if (MTLListBox.SelectedItems.Count == 0)
                return;

            var itemsToRemove = new List<object>(MTLListBox.SelectedItems.Cast<object>());

            foreach (var item in itemsToRemove)
            {
                MTLListBox.Items.Remove(item);
            }

            UpdateINIKey();
        }

        private void UpdateINIKey()
        {
            string value = "";

            foreach(string file in MTLListBox.Items)
            {
                value += file;
                value += ",";
            }

            GameStorage.Instance.GetSelectedGame().Materials = value;
        }
    }
}

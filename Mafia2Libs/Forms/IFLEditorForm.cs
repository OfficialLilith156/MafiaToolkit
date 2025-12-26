using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    public partial class IFLEditorForm : Form
    {
        public List<TextureWrapper> Textures { get; private set; }

        public class TextureWrapper
        {
            [Category("Texture")]
            [DisplayName("Name")]
            public string Name { get; set; }

            public TextureWrapper(string name)
            {
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public IFLEditorForm(List<string> textures)
        {
            Textures = textures.Select(t => new TextureWrapper(t)).ToList();
            InitializeComponent();
            UpdateListBox();
        }

        private void UpdateListBox()
        {
            listBoxImages.Items.Clear();
            listBoxImages.Items.AddRange(Textures.ToArray());
        }

        private void listBoxImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxImages.SelectedIndex >= 0)
                propertyGrid1.SelectedObject = Textures[listBoxImages.SelectedIndex];
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Textures.Add(new TextureWrapper("new_texture.dds"));
            UpdateListBox();
            listBoxImages.SelectedIndex = Textures.Count - 1;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int idx = listBoxImages.SelectedIndex;
            if (idx >= 0)
            {
                Textures.RemoveAt(idx);
                UpdateListBox();
                if (Textures.Count > 0)
                {
                    listBoxImages.SelectedIndex = Math.Min(idx, Textures.Count - 1);
                }
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int idx = listBoxImages.SelectedIndex;
            if (idx > 0)
            {
                var tmp = Textures[idx - 1];
                Textures[idx - 1] = Textures[idx];
                Textures[idx] = tmp;
                UpdateListBox();
                listBoxImages.SelectedIndex = idx - 1;
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int idx = listBoxImages.SelectedIndex;
            if (idx >= 0 && idx < Textures.Count - 1)
            {
                var tmp = Textures[idx + 1];
                Textures[idx + 1] = Textures[idx];
                Textures[idx] = tmp;
                UpdateListBox();
                listBoxImages.SelectedIndex = idx + 1;
            }
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;    
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
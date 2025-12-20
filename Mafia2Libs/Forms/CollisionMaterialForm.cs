using System;
using System.Windows.Forms;
using ResourceTypes.Collisions;

namespace Mafia2Tool.Forms
{
    public partial class CollisionMaterialSelectForm : Form
    {
        public CollisionMaterials SelectedMaterial { get; private set; }

        public CollisionMaterialSelectForm()
        {
            Text = "Select Collision Material";
            Width = 300;
            Height = 140;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            var combo = new ComboBox
            {
                Left = 15,
                Top = 15,
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            combo.Items.AddRange(Enum.GetNames(typeof(CollisionMaterials)));
            combo.SelectedIndex = 0;
            var okButton = new Button
            {
                Text = "OK",
                Left = 110,
                Width = 75,
                Top = 50,
                DialogResult = DialogResult.OK
            };
            AcceptButton = okButton;
            Controls.Add(combo);
            Controls.Add(okButton);
            okButton.Click += (_, _) =>
            {
                SelectedMaterial = Enum.Parse<CollisionMaterials>(combo.SelectedItem.ToString());
                DialogResult = DialogResult.OK;
                Close();
            };
        }
    }
}
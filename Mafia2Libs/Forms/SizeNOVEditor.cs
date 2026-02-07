using Core.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    public partial class SizeNOVEditor : Form
    {
        private string filePath = string.Empty;
        private byte[] originalFileBytes;
        private (int TagPosition, int idPosition, int Tag2Position) currentPositions;

        public SizeNOVEditor(string filePath = "")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(filePath))
            {
                this.filePath = filePath;
                txtFilePath.Text = filePath;
                UpdateTitle();
                ReadAndDisplayInfo(filePath);
            }
        }

        private void UpdateTitle()
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                string fileName = System.IO.Path.GetFileName(filePath);
                this.Text = $"Info NOV - {fileName}";
            }
            else
            {
                this.Text = "Info NOV";
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "NAV_OBJ_FILE (*.nov)|*.nov";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (!string.IsNullOrEmpty(filePath))
                {
                    openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(filePath);

                }
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                    filePath = openFileDialog.FileName;
                    UpdateTitle();
                    ReadAndDisplayInfo(openFileDialog.FileName);
                }
            }
        }

        private void ReadAndDisplayInfo(string filePath)
        {
            try
            {
                originalFileBytes = System.IO.File.ReadAllBytes(filePath);
                currentPositions = FindIDAndSize(originalFileBytes);
                if (currentPositions.idPosition >= 0 && currentPositions.idPosition + 4 <= originalFileBytes.Length &&
                    currentPositions.TagPosition >= 0 && currentPositions.TagPosition + 4 <= originalFileBytes.Length &&
                    currentPositions.Tag2Position >= 0 && currentPositions.Tag2Position + 4 <= originalFileBytes.Length)
                {
                    byte[] idBytes = new byte[4];
                    Array.Copy(originalFileBytes, currentPositions.idPosition, idBytes, 0, 4);
                    uint id = BitConverter.ToUInt32(idBytes, 0);

                    byte[] tagBytes = new byte[4];
                    Array.Copy(originalFileBytes, currentPositions.TagPosition, tagBytes, 0, 4);
                    uint tag = BitConverter.ToUInt32(tagBytes, 0);

                    byte[] tag2Bytes = new byte[4];
                    Array.Copy(originalFileBytes, currentPositions.Tag2Position, tag2Bytes, 0, 4);
                    uint tag2 = BitConverter.ToUInt32(tag2Bytes, 0);

                    txtTagHexBytes.Text = BitConverter.ToString(tagBytes).Replace("-", " ");
                    txtTagDecimal.Text = tag.ToString();
                    txtTagHex.Text = $"0x{tag:X}";

                    txtIDHexBytes.Text = BitConverter.ToString(idBytes).Replace("-", " ");
                    txtIdDecimal.Text = id.ToString();
                    txtIDHex.Text = $"0x{id:X}";

                    txtTag2HexBytes.Text = BitConverter.ToString(tag2Bytes).Replace("-", " ");
                    txtTag2Decimal.Text = tag2.ToString();
                    txtTag2Hex.Text = $"0x{tag2:X}";

                    DisplayFileContext(originalFileBytes, currentPositions.TagPosition, currentPositions.idPosition, currentPositions.Tag2Position);

                    btnSave.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Could not find ID, Tag, or Tag2 in the file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\r\nError reading the file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
            }
        }

        private (int TagPosition, int idPosition, int Tag2Position) FindIDAndSize(byte[] fileBytes)
        {
            for (int i = 0; i < fileBytes.Length - 16; i++)
            {
                if (fileBytes[i] == 0x02 && fileBytes[i + 1] == 0x00 && fileBytes[i + 2] == 0x00 && fileBytes[i + 3] == 0x00)
                {
                    int tagPos = i + 4;
                    int idPos = tagPos + 4;
                    int tag2Pos = idPos + 4;

                    if (tag2Pos + 3 < fileBytes.Length)
                    {
                        return (tagPos, idPos, tag2Pos);
                    }
                }
            }
            if (fileBytes.Length >= 12)
            {
                int tagPos = fileBytes.Length - 12;
                int idPos = fileBytes.Length - 8;
                int tag2Pos = fileBytes.Length - 4;
                return (tagPos, idPos, tag2Pos);
            }
            return (-1, -1, -1);
        }

        private void DisplayFileContext(byte[] fileBytes, int tagPosition, int idPosition, int tag2Position)
        {

            int minPos = Math.Min(Math.Min(tagPosition, idPosition), tag2Position);
            int maxPos = Math.Max(Math.Max(tagPosition, idPosition), tag2Position);

            int start = Math.Max(0, minPos - 16);
            int end = Math.Min(fileBytes.Length, maxPos + 20);
            int length = end - start;

            byte[] context = new byte[length];
            Array.Copy(fileBytes, start, context, 0, length);

            txtContextHexBytes.Text = BitConverter.ToString(context).Replace("-", " ");

            int highlightTagStart = (tagPosition - start) * 3;
            int highlightIdStart = (idPosition - start) * 3;
            int highlightTag2Start = (tag2Position - start) * 3;

            if (txtContextHexBytes is RichTextBox rtb)
            {
                rtb.SelectAll();
                rtb.SelectionBackColor = Color.White;

                if (highlightTagStart >= 0 && highlightTagStart + 11 < rtb.Text.Length)
                {
                    rtb.Select(highlightTagStart, 11);
                    rtb.SelectionBackColor = Color.LightBlue;
                }

                if (highlightIdStart >= 0 && highlightIdStart + 11 < rtb.Text.Length)
                {
                    rtb.Select(highlightIdStart, 11);
                    rtb.SelectionBackColor = Color.LightGreen;
                }

                if (highlightTag2Start >= 0 && highlightTag2Start + 11 < rtb.Text.Length)
                {
                    rtb.Select(highlightTag2Start, 11);
                    rtb.SelectionBackColor = Color.LightYellow;
                }

                rtb.Select(0, 0);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("First, open the file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                byte[] modifiedBytes = GetModifiedData();
                System.IO.File.WriteAllBytes(filePath, modifiedBytes);
                originalFileBytes = modifiedBytes;
                MessageBox.Show("The file has been successfully saved!", "Preservation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "NAV_OBJ_FILE (*.nov)|*.nov";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = System.IO.Path.GetFileName(filePath);
                if (!string.IsNullOrEmpty(filePath))
                {
                    saveFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(filePath);
                }
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] modifiedBytes = GetModifiedData();
                        System.IO.File.WriteAllBytes(saveFileDialog.FileName, modifiedBytes);
                        MessageBox.Show("The file has been successfully saved!", "Preservation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public byte[] GetModifiedData()
        {
            if (originalFileBytes == null)
                return null;

            byte[] modifiedBytes = new byte[originalFileBytes.Length];
            Array.Copy(originalFileBytes, modifiedBytes, originalFileBytes.Length);
            try
            {
                if (!string.IsNullOrEmpty(txtTagHexBytes.Text))
                {
                    string[] tagHexBytes = txtTagHexBytes.Text.Split(' ');
                    if (tagHexBytes.Length == 4)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            modifiedBytes[currentPositions.TagPosition + i] = Convert.ToByte(tagHexBytes[i], 16);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(txtTagDecimal.Text) && uint.TryParse(txtTagDecimal.Text, out uint tagValue))
                {
                    byte[] tagBytes = BitConverter.GetBytes(tagValue);
                    Array.Copy(tagBytes, 0, modifiedBytes, currentPositions.TagPosition, 4);
                }
                if (!string.IsNullOrEmpty(txtIDHexBytes.Text))
                {
                    string[] idHexBytes = txtIDHexBytes.Text.Split(' ');
                    if (idHexBytes.Length == 4)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            modifiedBytes[currentPositions.idPosition + i] = Convert.ToByte(idHexBytes[i], 16);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(txtIdDecimal.Text) && uint.TryParse(txtIdDecimal.Text, out uint idValue))
                {
                    byte[] idBytes = BitConverter.GetBytes(idValue);
                    Array.Copy(idBytes, 0, modifiedBytes, currentPositions.idPosition, 4);
                }
                if (!string.IsNullOrEmpty(txtTag2HexBytes.Text))
                {
                    string[] tag2HexBytes = txtTag2HexBytes.Text.Split(' ');
                    if (tag2HexBytes.Length == 4)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            modifiedBytes[currentPositions.Tag2Position + i] = Convert.ToByte(tag2HexBytes[i], 16);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(txtTag2Decimal.Text) && uint.TryParse(txtTag2Decimal.Text, out uint tag2Value))
                {
                    byte[] tag2Bytes = BitConverter.GetBytes(tag2Value);
                    Array.Copy(tag2Bytes, 0, modifiedBytes, currentPositions.Tag2Position, 4);
                }

                return modifiedBytes;
            }
            catch (Exception)
            {
                return originalFileBytes;
            }
        }

        private void UpdateValueFromHex(object sender, EventArgs e)
        {
            if (sender == txtTagHex && uint.TryParse(txtTagHex.Text.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, null, out uint tagValue))
            {
                txtTagDecimal.Text = tagValue.ToString();
                byte[] bytes = BitConverter.GetBytes(tagValue);
                txtTagHexBytes.Text = BitConverter.ToString(bytes).Replace("-", " ");
            }
            else if (sender == txtIDHex && uint.TryParse(txtIDHex.Text.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, null, out uint idValue))
            {
                txtIdDecimal.Text = idValue.ToString();
                byte[] bytes = BitConverter.GetBytes(idValue);
                txtIDHexBytes.Text = BitConverter.ToString(bytes).Replace("-", " ");
            }
            else if (sender == txtTag2Hex && uint.TryParse(txtTag2Hex.Text.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, null, out uint tag2Value))
            {
                txtTag2Decimal.Text = tag2Value.ToString();
                byte[] bytes = BitConverter.GetBytes(tag2Value);
                txtTag2HexBytes.Text = BitConverter.ToString(bytes).Replace("-", " ");
            }
        }

        private void UpdateValueFromDecimal(object sender, EventArgs e)
        {
            if (sender == txtTagDecimal && uint.TryParse(txtTagDecimal.Text, out uint tagValue))
            {
                txtTagHex.Text = $"0x{tagValue:X}";
                byte[] bytes = BitConverter.GetBytes(tagValue);
                txtTagHexBytes.Text = BitConverter.ToString(bytes).Replace("-", " ");
            }
            else if (sender == txtIdDecimal && uint.TryParse(txtIdDecimal.Text, out uint idValue))
            {
                txtIDHex.Text = $"0x{idValue:X}";
                byte[] bytes = BitConverter.GetBytes(idValue);
                txtIDHexBytes.Text = BitConverter.ToString(bytes).Replace("-", " ");
            }
            else if (sender == txtTag2Decimal && uint.TryParse(txtTag2Decimal.Text, out uint tag2Value))
            {
                txtTag2Hex.Text = $"0x{tag2Value:X}";
                byte[] bytes = BitConverter.GetBytes(tag2Value);
                txtTag2HexBytes.Text = BitConverter.ToString(bytes).Replace("-", " ");
            }
        }

        private void UpdateValueFromHexBytes(object sender, EventArgs e)
        {
            if (sender == txtTagHexBytes)
            {
                string[] bytesStr = txtTagHexBytes.Text.Split(' ');
                if (bytesStr.Length == 4 && bytesStr.All(b => byte.TryParse(b, System.Globalization.NumberStyles.HexNumber, null, out _)))
                {
                    byte[] bytes = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        bytes[i] = Convert.ToByte(bytesStr[i], 16);
                    }
                    uint value = BitConverter.ToUInt32(bytes, 0);
                    txtTagDecimal.Text = value.ToString();
                    txtTagHex.Text = $"0x{value:X}";
                }
            }
            else if (sender == txtIDHexBytes)
            {
                string[] bytesStr = txtIDHexBytes.Text.Split(' ');
                if (bytesStr.Length == 4 && bytesStr.All(b => byte.TryParse(b, System.Globalization.NumberStyles.HexNumber, null, out _)))
                {
                    byte[] bytes = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        bytes[i] = Convert.ToByte(bytesStr[i], 16);
                    }
                    uint value = BitConverter.ToUInt32(bytes, 0);
                    txtIdDecimal.Text = value.ToString();
                    txtIDHex.Text = $"0x{value:X}";
                }
            }
            else if (sender == txtTag2HexBytes)
            {
                string[] bytesStr = txtTag2HexBytes.Text.Split(' ');
                if (bytesStr.Length == 4 && bytesStr.All(b => byte.TryParse(b, System.Globalization.NumberStyles.HexNumber, null, out _)))
                {
                    byte[] bytes = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        bytes[i] = Convert.ToByte(bytesStr[i], 16);
                    }
                    uint value = BitConverter.ToUInt32(bytes, 0);
                    txtTag2Decimal.Text = value.ToString();
                    txtTag2Hex.Text = $"0x{value:X}";
                }
            }
        }
    }
}

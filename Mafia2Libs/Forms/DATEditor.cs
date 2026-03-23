using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    public partial class DATEditor : Form
    {
        private FileInfo file;
        private DataTable dataTable;
        private string currentFilePath;
        public DATEditor(FileInfo file)
        {
            InitializeComponent();
            dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(string));
            dataTable.Columns.Add("Text", typeof(string));
            dataGridView.DataSource = dataTable;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            currentFilePath = file.FullName;
            LoadFile(currentFilePath);
        }

        private void LoadFile(string path)
        {
            dataTable.Clear();
            try
            {
                string[] lines = File.ReadAllLines(path, Encoding.UTF8);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    int colonIndex = line.IndexOf(':');
                    if (colonIndex > 0)
                    {
                        string id = line.Substring(0, colonIndex);
                        string text = colonIndex < line.Length - 1 ? line.Substring(colonIndex + 1) : "";
                        DataRow row = dataTable.NewRow();
                        row["Id"] = id;
                        row["Text"] = text;
                        dataTable.Rows.Add(row);
                    }
                    else
                    {
                        DataRow row = dataTable.NewRow();
                        row["Id"] = line;
                        row["Text"] = "";
                        dataTable.Rows.Add(row);
                    }
                }
                dataTable.DefaultView.RowFilter = null;
                tbSearch.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File download error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }

            SaveFile(currentFilePath);
        }

        private void SaveFile(string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true)))
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string id = row["Id"].ToString();
                        string text = row["Text"].ToString();
                        sw.WriteLine($"{id}:{text}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string search = tbSearch.Text.Trim();
            if (string.IsNullOrEmpty(search))
            {
                dataTable.DefaultView.RowFilter = null;
            }
            else
            {
                string escapedSearch = search.Replace("'", "''");
                dataTable.DefaultView.RowFilter = $"Id LIKE '%{escapedSearch}%' OR Text LIKE '%{escapedSearch}%'";
            }
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            tbSearch.Text = "";
            dataTable.DefaultView.RowFilter = null;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow != null)
            {
                string text = dataGridView.CurrentRow.Cells["Text"].Value.ToString();
            }
            else
            {
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow != null)
            {
                DataRowView drv = (DataRowView)dataGridView.CurrentRow.DataBoundItem;
                if (MessageBox.Show($"Delete entry from ID = {drv["Id"]}?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    drv.Row.Delete();
                }
            }
            else
            {
                MessageBox.Show("Select the entry to delete.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string id = tbAddId.Text.Trim();
            string text = tbAddText.Text;

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Enter ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow[] existing = dataTable.Select($"Id = '{id.Replace("'", "''")}'");
            if (existing.Length > 0)
            {
                MessageBox.Show("The ID already exists. Please enter a different ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow newRow = dataTable.NewRow();
            newRow["Id"] = id;
            newRow["Text"] = text;
            dataTable.Rows.Add(newRow);
            tbAddId.Clear();
            tbAddText.Clear();
        }
    }
}

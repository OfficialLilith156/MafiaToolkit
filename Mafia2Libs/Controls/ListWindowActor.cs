using ResourceTypes.Actors;
using ResourceTypes.FrameResource;
using System.Collections.Generic;
using System.Windows.Forms;
using Utils.Language;

namespace Mafia2Tool
{
    public partial class ListWindowActor : Form
    {
        private FrameResource frameResource = null;
        private bool frameMode = false;
        private bool searchMode = false;
        //public object chosenObject = null;
        public List<object> chosenObjects = new List<object>();
        private const string ROOT_STRING = "root (-1)";
        ParentInfo.ParentType type = 0;

        public ListWindowActor()
        {
            InitializeComponent();
            checkedListBox1.CheckOnClick = true;
        }

        public void PopulateForm(ParentInfo.ParentType ParentType,FrameResource fr)
        {
            frameResource = fr;
            labelInfo.Text = Language.GetString("$SELECT_PARENT") + '\n' + Language.GetString("$HOW_TO_SEARCH");
            type = ParentType;
            frameMode = true;
            checkedListBox1.Items.Add(ROOT_STRING);
            if (ParentType == ParentInfo.ParentType.ParentIndex2)
            {
                foreach (KeyValuePair<int, FrameHeaderScene> entry in frameResource.FrameScenes)
                {
                    checkedListBox1.Items.Add(entry.Value);
                }
            }
            foreach (KeyValuePair<int, object> entry in frameResource.FrameObjects)
            {
                checkedListBox1.Items.Add(entry.Value);
            }
        }

        public void PopulateForm(List<ActorEntry> items)
        {
            labelInfo.Text = Language.GetString("$SELECT_ITEM");
            foreach(var item in items)
            {
                checkedListBox1.Items.Add(item);
            }
        }

        private void SearchForms()
        {
            if (frameMode)
            {
                checkedListBox1.Items.Clear();
                foreach (KeyValuePair<int, object> entry in frameResource.FrameObjects)
                {
                    FrameObjectBase obj = entry.Value as FrameObjectBase;

                    if (obj.Name.String.Contains(SearchBox.Text))
                    {
                        checkedListBox1.Items.Add(entry.Value);
                    }
                }
            }
        }

        private void SearchOnClick(object sender, System.EventArgs e)
        {
            if (!searchMode)
            {
                searchMode = true;
                SearchBox.Clear();
            }
        }

        private void SearchOnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (searchMode && e.KeyChar == 13)
            {
                SearchForms();
                searchMode = false;
            }
        }

        //private void OnItemSelect(object sender, System.EventArgs e)
        //{
        //    if (checkedListBox1.SelectedItem != null)
        //    {
        //        chosenObjects = (checkedListBox1.SelectedItem.ToString() == ROOT_STRING) ? null : checkedListBox1.SelectedItem;       
        //        DialogResult = DialogResult.OK;
        //        Close();
        //    }
        //}
        private void btnProcessSelected_Click(object sender, System.EventArgs e)
        {
            chosenObjects.Clear();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                object obj = (item.ToString() == ROOT_STRING) ? null : item;
                chosenObjects.Add(obj);
                ProcessObject(obj);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ProcessObject(object obj)
        {
            if (obj != null)
            {
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MassRename
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnClear.Click += new EventHandler(btnClear_Click);
            btnDown.Click += new EventHandler(btnDown_Click);
            btnRemove.Click += new EventHandler(btnRemove_Click);
            btnRename.Click += new EventHandler(btnRename_Click);
            btnUp.Click += new EventHandler(btnUp_Click);
            lstFiles.DragEnter += new DragEventHandler(lstFiles_DragEnter);
            lstFiles.DragDrop += new DragEventHandler(lstFiles_DragDrop);
            this.ResizeEnd += new EventHandler(frmMain_ResizeEnd);
        }

        void frmMain_ResizeEnd(object sender, EventArgs e)
        {
            Debug.Print(this.Size.ToString());
        }

        void lstFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData("FileDrop", false);
            foreach (string file in files)
                lstFiles.Items.Add(file);
        }

        void lstFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        void btnUp_Click(object sender, EventArgs e)
        {
            var i = lstFiles.SelectedIndex;
            var tmp = lstFiles.Items[i];
            lstFiles.Items[i] = lstFiles.Items[i - 1];
            lstFiles.Items[i - 1] = tmp;
            lstFiles.SelectedIndex = i - 1;
        }

        void btnRename_Click(object sender, EventArgs e)
        {
            var start = int.Parse(txtStart.Text);
            var i = start;

            foreach (string file in lstFiles.Items)
            {
                var title = txtTitle.Text.Trim();
                var ext = System.IO.Path.GetExtension(file);
                var path = System.IO.Path.GetDirectoryName(file);
                var strBuilder = new StringBuilder();
                strBuilder.Append(title).Append(" - ");
                strBuilder.Append(i.ToString("000"));
                strBuilder.Append(ext);

                var newFile = System.IO.Path.Combine(path, strBuilder.ToString());
                
                System.IO.File.Move(file, newFile);
                Debug.WriteLine(newFile);
                i++;
            }

            MessageBox.Show("Done!");
            lstFiles.Items.Clear();
        }

        void btnRemove_Click(object sender, EventArgs e)
        {
            lstFiles.Items.RemoveAt(lstFiles.SelectedIndex);
        }

        void btnDown_Click(object sender, EventArgs e)
        {
            var i = lstFiles.SelectedIndex;
            var tmp = lstFiles.Items[i];
            lstFiles.Items[i] = lstFiles.Items[i + 1];
            lstFiles.Items[i + 1] = tmp;
            lstFiles.SelectedIndex = i + 1;
        }

        void btnClear_Click(object sender, EventArgs e)
        {
            lstFiles.Items.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var result = dlgOpen.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
                return;

            lstFiles.Items.AddRange(dlgOpen.FileNames);
        }
    }
}

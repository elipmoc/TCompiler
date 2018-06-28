using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TGUI
{
    public partial class NewProjectForm : Form
    {
        public String projectName = "";
        public String pathName = "";
        private bool makeFlag = false;
        public NewProjectForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            pathTextBox.Text = folderBrowserDialog.SelectedPath;
        }

        private void makeButton_Click(object sender, EventArgs e)
        {
            makeFlag = true;
            projectName = projectTextBox.Text;
            pathName = pathTextBox.Text;
        }

        private void NewProjectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (makeFlag)
            {
                makeFlag = false;
                if ((projectName == "" || pathName == ""))
                {
                    MessageBox.Show("名前か場所がカラです！", "error!");
                    e.Cancel = true;
                }
                else if (!System.IO.Directory.Exists(pathName))
                {
                    MessageBox.Show("場所が存在しません！", "error!");
                    e.Cancel = true;
                }
            }
        }
    }
}

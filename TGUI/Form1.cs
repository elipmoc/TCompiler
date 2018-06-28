using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TGUI
{
    public partial class Form1 : Form
    {
        string projectPath = "";
        string projectName = "";
        TCompilerCommand tcc = new TCompilerCommand();

        const string formTitle = "Micsosoft Visual Stdio";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            textBox1.Text = tcc.GetVersion();

        }

        private void 新規プロジェクト作成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var npf = new NewProjectForm();
            if (npf.ShowDialog() == DialogResult.OK) {
                this.Text = npf.projectName + " - " + formTitle;
                projectPath= npf.pathName + "\\" + npf.projectName;
                projectName = npf.projectName;
                System.IO.Directory.CreateDirectory(projectPath);
                System.IO.File.Create(projectPath + "\\source.txt").Close();
                fileNameLabel.Text = "source.txt";
            };
        }

        private void ファイル保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileSave();
        }

        private void fileSave()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(editTextBox.Text);
            var file = new System.IO.StreamWriter(projectPath + "\\source.txt");

            using (file)
            {
                file.Write(editTextBox.Text.ToArray());
            }
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            fileSave();
            textBox1.Text=tcc.Build(projectPath + "\\source.txt",projectName ,projectPath);
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            fileSave();
            textBox1.Text = tcc.Run(projectPath + "\\source.txt");
        }
    }
}

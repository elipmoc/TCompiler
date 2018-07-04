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
                setProject(npf.projectName, npf.pathName+"\\"+npf.projectName);
            };
        }

        private void プロジェクトを開くToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                setProject(System.IO.Path.GetFileName(folderBrowserDialog1.SelectedPath), folderBrowserDialog1.SelectedPath);
            }
        }

        private void setProject(string projectName,string projectPath)
        {
            this.Text = projectName + " - " + formTitle;
            this.projectPath = projectPath;
            this.projectName = projectName;
            fileNameLabel.Text = "source.txt";
            if(System.IO.Directory.Exists(projectPath)==false)
                System.IO.Directory.CreateDirectory(projectPath);
            if (System.IO.File.Exists(projectPath + "\\source.txt")==false)
                System.IO.File.Create(projectPath + "\\source.txt").Close();
            var file = System.IO.File.OpenText(projectPath + "\\source.txt");
            using (file)
                editTextBox.Text = file.ReadToEnd();
        }

        private void ファイル保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileSave();
        }

        private void fileSave()
        {
            if (projectName == "") return;
            var bytes = System.Text.Encoding.UTF8.GetBytes(editTextBox.Text);
            var file = new System.IO.StreamWriter(projectPath + "\\source.txt");

            using (file)
                file.Write(editTextBox.Text.ToArray());
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            if (projectName == "") return;
            fileSave();
            textBox1.Text=tcc.Build(projectPath + "\\source.txt",projectName ,projectPath);
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            if (projectName == "") return;
            fileSave();
            textBox1.Text=tcc.Build(projectPath + "\\source.txt",projectName ,projectPath);
            tcc.Run(projectPath + "\\source.txt");
        }


    }
}

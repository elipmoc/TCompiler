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

        const string formTitle = "Micsosoft Visual Stdio";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var process = new Process();
            process.StartInfo.FileName=
                @"..\..\..\TCompiler\bin\Debug\tc.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.Arguments = "-v";
            process.Start();
            var io = process.StandardOutput;
            textBox1.Text = io.ReadToEnd();
            process.WaitForExit();
            process.Close();

        }

        private void 新規プロジェクト作成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var npf = new NewProjectForm();
            if (npf.ShowDialog() == DialogResult.OK) {
                this.Text = npf.projectName + " - " + formTitle;
                projectPath= npf.pathName + "\\" + npf.projectName;
                System.IO.Directory.CreateDirectory(projectPath);
                System.IO.File.Create(projectPath + "\\source.txt").Close();
                fileNameLabel.Text = "source.txt";
            };
        }

        private void ファイル保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bytes=System.Text.Encoding.UTF8.GetBytes(editTextBox.Text);
            var file=System.IO.File.OpenWrite(projectPath + "\\source.txt");

            using (file)
            {
                file.Write(bytes, 0, bytes.Length);
            }
        }
    }
}

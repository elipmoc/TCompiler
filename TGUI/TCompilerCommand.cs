using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGUI
{
    class TCompilerCommand:IDisposable
    {
        readonly Process p;
        public TCompilerCommand()
        {
            p = new Process();
            p.StartInfo.FileName =
                @"..\..\..\TCompiler\bin\Debug\tc.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = true;

        }

        public string GetVersion()
        {
            p.StartInfo.Arguments = "-v";
            p.Start();
            p.WaitForExit();
            return p.StandardOutput.ReadToEnd()+p.StandardError.ReadToEnd();
        }

        public string Build(string from,string assemblyName,string to)
        {
            p.StartInfo.Arguments = "-o "+from+" "+assemblyName+" "+to;
            p.Start();
            p.WaitForExit();
            return p.StandardOutput.ReadToEnd()+p.StandardError.ReadToEnd();
        }

        public void Run(string source)
        {
            var pp = new Process();
            pp.StartInfo.FileName =
                @"..\..\..\TCompiler\bin\Debug\tc.exe";
            pp.StartInfo.Arguments = "-run " + source;
            pp.Start();
        }

        public void Dispose()
        {
            p.Close();
        }
    }
}

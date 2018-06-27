using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCompiler
{
    static class buildInFunc
    {
        public static int Print(int x)
        {
            Console.WriteLine(x);
            return x;
        }
    }
}

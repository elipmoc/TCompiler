using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCompiler
{
    public static class buildInFunc
    {
        public static int Print(int x)
        {
            Console.WriteLine(x);
            return x;
        }

        public static int Scan()
        {
            int result;
            while (int.TryParse(Console.ReadLine(), out result)==false) {

            }
            return result;
        }
    }
}

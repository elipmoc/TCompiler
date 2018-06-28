using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TCompiler
{
    class LabelTable
    {
        public Dictionary<string, LabelTarget> table = new Dictionary<string, LabelTarget>();

        public LabelTarget Find(string name)
        {
            if (table.ContainsKey(name) == false)
                table[name] = Expression.Label();
            return table[name];
        }
    }
}

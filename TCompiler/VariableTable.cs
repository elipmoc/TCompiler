using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TCompiler
{
    class VariableTable
    {
        int nest = -1;
        public List<Dictionary<string, ParameterExpression>> table = new List<Dictionary<string, ParameterExpression>>();

        public VariableTable()
        {
            In();
        }

        public void In()
        {
            nest++;
            table.Add(new Dictionary<string, ParameterExpression>());
        }

        public void Out()
        {
            table.RemoveAt(nest);
            nest--;

        }

        public ParameterExpression Find(string name)
        {
            for (int i = nest; i > -1; i--)
            {
                if (table[i].ContainsKey(name) == true)
                    return table[i][name];
            }
            return null;
        }

        public ParameterExpression FindNowNest(string name)
        {
            if (table[nest].ContainsKey(name) == true)
                return table[nest][name];
            return null;
        }

        public ParameterExpression[] GetNowNestParamList()
        {
            return table[nest].Values.ToArray();
        }

        public void Register(string name, ParameterExpression parameter)
        {
            if (table[nest].ContainsKey(name) == true)
                return;
            table[nest][name] = parameter;
        }
    }
}

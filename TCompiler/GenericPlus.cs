using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TCompiler
{
    public class GenericPlus<T>
    {
        public GenericPlus()
        {
            var availableT = new Type[]
            {
            typeof(int), typeof(uint), typeof(short), typeof(ushort), typeof(long), typeof(ulong), typeof(byte),
            typeof(decimal), typeof(double),typeof(string)
            };
            if (!availableT.Contains(typeof(T)))
            {
                throw new NotSupportedException();
            }
            var p1 = Expression.Parameter(typeof(T));
            var p2 = Expression.Parameter(typeof(T));
            Add = Expression.Lambda<Func<T, T, T>>(Expression.Add(p1, p2), p1, p2).Compile();

        }

        public Func<T, T, T> Add { get; private set; }
    }
}

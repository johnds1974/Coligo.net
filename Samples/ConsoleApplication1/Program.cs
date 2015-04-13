using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Collection<object> cs = new Collection<object>();

            if (cs is IList<object>)
            {
                ((IList)cs).Add("Hi");
            }

            ((ICollection<object>)cs).Add("hi");

            Type t = cs.GetType();

            var le = new Func<string>(() => Prop);
            Expression<Func<string>> f = () => Prop;
            var b = (MemberExpression) f.Body;
            var name = b.Member.Name;

            PropGetter(() => Prop);
        }

        static string Prop { get; set; }

        static void PropGetter(Expression<Func<string>> me)
        {
            var mexpr = me.Body as MemberExpression;
            if (mexpr != null)
            {
                var name = mexpr.Member.Name;
            }
        }
    }
}

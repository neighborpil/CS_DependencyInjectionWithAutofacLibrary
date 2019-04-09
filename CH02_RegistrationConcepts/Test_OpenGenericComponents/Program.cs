using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_OpenGenericComponents
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>));

            var container = builder.Build();
            var myList = container.Resolve<IList<int>>();
            Console.WriteLine(myList.GetType());
            Console.ReadKey();
        }
    }
}

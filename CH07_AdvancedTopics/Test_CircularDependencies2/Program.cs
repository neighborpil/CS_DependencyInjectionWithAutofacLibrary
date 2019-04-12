using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_CircularDependencies2
{
    public class Parent
    {
        public Parent(Child child)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        public Child Child { get; set; }

        public override string ToString()
        {
            return "Parent";
        }
    }

    public class Child
    {
        public Parent Parent { get; set; }

        public override string ToString()
        {
            return "Child";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var b = new ContainerBuilder();
            b.RegisterType<Parent>()
                .InstancePerLifetimeScope();
            b.RegisterType<Child>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            using (var c = b.Build())
            {
                Console.WriteLine(c.Resolve<Parent>().Child.Parent);
            }

            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_CircularDependencies
{
    public class Parent
    {
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
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

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

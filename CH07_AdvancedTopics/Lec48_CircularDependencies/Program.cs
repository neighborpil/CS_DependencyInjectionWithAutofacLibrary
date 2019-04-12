using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec48_CircularDependencies
{
    public class ParentWithProperty
    {
        public ChildWithProperty Child { get; set; }

        public override string ToString()
        {
            return "Parent";
        }
    }

    public class ChildWithProperty
    {
        public ParentWithProperty Parent { get; set; }

        public override string ToString()
        {
            return "Child";
        }
    }
    /*
    # Circular dependencies
     - 두개 이상의 클래스가 서로를 injecting하여 stackoveflow

    */
    class Program
    {
        static void Main(string[] args)
        {
            // 1. property - property(o)
            
            var b = new ContainerBuilder();
            b.RegisterType<ParentWithProperty>()
                .InstancePerLifetimeScope() // InstancePerDependencyInjection은 지원 X
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            b.RegisterType<ChildWithProperty>()
                .InstancePerLifetimeScope() // 한개만 존재해야 한다
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            using (var c = b.Build())
            {
                Console.WriteLine(c.Resolve<ParentWithProperty>().Child.ToString());
                Console.WriteLine(c.Resolve<ChildWithProperty>().Parent.ToString());
            }

            Console.ReadKey();
        }
    }
}

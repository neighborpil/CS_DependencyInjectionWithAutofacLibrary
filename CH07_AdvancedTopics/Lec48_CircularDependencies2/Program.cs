using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec48_CircularDependencies2
{
    public class ParentWithConstructor
    {
        public ChildWithProperty Child;

        public ParentWithConstructor(ChildWithProperty child)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        public override string ToString()
        {
            return "Parent with a ChildWithProperty";
        }
    }

    public class ChildWithProperty
    {
        public ParentWithConstructor Parent { get; set; }

        public override string ToString()
        {
            return "Child";
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // 2. property - property with constructor(o)
            var b = new ContainerBuilder();
            b.RegisterType<ParentWithConstructor>()
                .InstancePerLifetimeScope();
            b.RegisterType<ChildWithProperty>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            using (var c = b.Build())
            {
                Console.WriteLine(c.Resolve<ParentWithConstructor>().Child.Parent);
            }

            // 3. property with constructor - property with constructor(x)
            // autofac은 지원하지 않는다

            Console.ReadKey();
        }
    }
}

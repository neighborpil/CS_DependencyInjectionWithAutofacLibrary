using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_PropertyAndMethodInjection
{
    public class Parent
    {
        public override string ToString()
        {
            return "I'm father";
        }
    }

    public class Child
    {
        public string Name { get; set; }
        public Parent Parent { get; set; }

        public void SetParent(Parent parent)
        {
            this.Parent = parent;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Parent>();
            /*
            // 1.
            builder.RegisterType<Child>().PropertiesAutowired();

            // 2.
            builder.RegisterType<Child>()
                .WithProperty("Parent", new Parent());
            
            // 3.
            builder.Register(c =>
            {
                var child = new Child();
                child.SetParent(c.Resolve<Parent>());
                return child;
            });
            */

            // 4.
            builder.RegisterType<Child>()
                .OnActivated(e =>
                {
                    var p = e.Context.Resolve<Parent>();
                    e.Instance.SetParent(p);
                });

            var container = builder.Build();
            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent);
            Console.ReadKey();

        }
    }
}

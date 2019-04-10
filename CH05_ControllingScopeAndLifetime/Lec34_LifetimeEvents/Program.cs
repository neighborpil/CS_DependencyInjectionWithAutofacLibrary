using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec34_LifetimeEvents
{
    public class Parent
    {
        public override string ToString()
        {
            return "I am your father";
        }
    }

    public class Child
    {
        public string Name { get; set; }
        public Parent Parent { get; set; }


        public Child()
        {
            Console.WriteLine("Child being created");
        }

        public void SetParent(Parent parent)
        {
            Parent = parent;
        }

        public virtual string ToString()
        {
            return "Hi, there";
        }
    }

    class BadChild : Child
    {
        public override string ToString()
        {
            return "I hate you";
        }
    }

    public class ParentChildModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Parent>();
            builder.Register(c => new Child() { Parent = c.Resolve<Parent>() });
        }
    }

    

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Parent>();
            builder.RegisterType<Child>()
                .OnActivating(a =>
                {
                    Console.WriteLine("Child activating");
                    //a.Instance.Parent = a.Context.Resolve<Parent>();
                    a.ReplaceInstance(new BadChild());
                })
                .OnActivated(a =>
                {
                    Console.WriteLine("Child activated");
                })
                .OnRelease(a =>
                {
                    Console.WriteLine("Child is about to be removed");
                });

            
            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var child = scope.Resolve<Child>();
                var parent = child.Parent;
                Console.WriteLine(child.ToString());
                Console.WriteLine(parent);
            }

            Console.ReadKey();
        }
    }
}

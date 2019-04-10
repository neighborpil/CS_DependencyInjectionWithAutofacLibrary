using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_LifetimeEvent
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
                    a.Instance.Parent = a.Context.Resolve<Parent>();
                    Console.WriteLine("Child being activating");
                })
                .OnActivated(a => Console.WriteLine("Child is activated"))
                .OnRelease(a => Console.WriteLine("Child is released"));

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var parent = scope.Resolve<Child>().Parent;
                Console.WriteLine(parent);
            }

            Console.ReadKey();
        }
    }
}

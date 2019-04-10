using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_RunningCodeAtStartup
{
    public class MyClass : IStartable
    {
        public MyClass()
        {
            Console.WriteLine("My Class Created");
        }

        public void Start()
        {
            Console.WriteLine("Started");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MyClass>()
                .AsSelf()
                .As<IStartable>()
                .SingleInstance();

            var container = builder.Build();
            container.Resolve<MyClass>();

            Console.ReadKey();
        }
    }
}

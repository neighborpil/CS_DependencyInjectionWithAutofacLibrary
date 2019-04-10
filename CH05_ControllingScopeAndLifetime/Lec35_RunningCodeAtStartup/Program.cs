using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec35_RunningCodeAtStartup
{
    public class MyClass : IStartable // 시작할 클래스에 구현해야한다
    {
        public MyClass()
        {
            Console.WriteLine("MyClass created");
        }
        public void Start()
        {
            Console.WriteLine("Container being built");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MyClass>()
                .AsSelf()
                .As<IStartable>() // 시작할 때 호출된다
                .SingleInstance();

            var container = builder.Build();
            container.Resolve<MyClass>();

            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec16_PropertyAndMethodInjection
{
    public class Parent
    {
        public override string ToString()
        {
            return "I'm your father";
        }
    }

    public class Child
    {
        public string Name { get; set; }
        public Parent Parent { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {
            /* Property Injection */

            /*
            // 1. 일반적인 상황
            var builder = new ContainerBuilder();
            builder.RegisterType<Parent>();
            builder.RegisterType<Child>();

            var container = builder.Build();
            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent); // parent는 null이기 때문에 아무것도 안나온다

            */

            /*
            // 2. PropertiesAutowired()로 생성하여 준다
            var builder = new ContainerBuilder();
            builder.RegisterType<Parent>();
            builder.RegisterType<Child>().PropertiesAutowired(); // property의 경우 기본값인 null이 아니라, 해당하는 객체를 생성해준다

            var container = builder.Build();
            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent); // parent가 자동으로 생성되었다
            */

            // 3. 직접 Property를 생성하여 등록하기
            var builder = new ContainerBuilder();
            builder.RegisterType<Parent>();
            builder.RegisterType<Child>();
            builder.RegisterType<Child>()
                .WithProperty("Parent", new Parent());
            var container = builder.Build();
            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent);

            Console.ReadKey();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.ResolveAnything;

namespace Lec45_RegistrationSources
{
    interface ICanSpeak
    {
        void Speak();
    }

    class Person : ICanSpeak

    {
        public void Speak()
        {
            Console.WriteLine("Hello");
        }
    }

    class Person2 : ICanSpeak
    {
        public void Speak()
        {
            Console.WriteLine("Hello2");
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            using (var c = builder.Build())
            {
                c.Resolve<Person>().Speak();
            }

            Console.ReadKey();
        }
    }
}

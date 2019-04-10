using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_InstanceScope
{
    public interface ILog
    {
        void Write(string message);
    }

    public class ConsoleLog : ILog
    {
        public ConsoleLog()
        {
            Console.WriteLine($"Creating a console log! {DateTime.Now.Ticks}");
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            /*
            // make instances per every call
            builder.RegisterType<ConsoleLog>()
                .InstancePerDependency();

            using (var container = builder.Build())
            {
                using (var scope1 = container.BeginLifetimeScope())
                {
                    for (int i = 0; i < 3; i++)
                        scope1.Resolve<ConsoleLog>();
                }

                using (var scope2 = container.BeginLifetimeScope())
                {
                    for (int i = 0; i < 3; i++)
                        scope2.Resolve<ConsoleLog>();
                }
            }
            

            // make an instance just one time
            builder.RegisterType<ConsoleLog>()
                .SingleInstance();

            using (var container = builder.Build())
            {
                using (var scope1 = container.BeginLifetimeScope())
                {
                    for (int i = 0; i < 3; i++)
                        scope1.Resolve<ConsoleLog>();
                }
            }

            // make an instance for specific scope
            builder.RegisterType<ConsoleLog>()
                .InstancePerMatchingLifetimeScope("foo");

            using (var container = builder.Build())
            {
                using (var scope1 = container.BeginLifetimeScope("foo"))
                {
                    for (int i = 0; i < 3; i++)
                        scope1.Resolve<ConsoleLog>();

                    using (var scope2 = scope1.BeginLifetimeScope())
                    {
                        for (int i = 0; i < 3; i++)
                            scope2.Resolve<ConsoleLog>();
                    }
                }
            }
            */

            // make instances per every scope
            builder.RegisterType<ConsoleLog>()
                .InstancePerLifetimeScope();

            using (var container = builder.Build())
            {
                using (var scope1 = container.BeginLifetimeScope())
                {
                    for (int i = 0; i < 3; i++)
                        scope1.Resolve<ConsoleLog>();
                }

                using (var scope2 = container.BeginLifetimeScope())
                {
                    for (int i = 0; i < 3; i++)
                        scope2.Resolve<ConsoleLog>();
                }
            }


            Console.ReadKey();
        }
    }
}

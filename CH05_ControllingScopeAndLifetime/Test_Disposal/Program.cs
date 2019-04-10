using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_Disposal
{
    public interface ILog
    {
        void Write(string message);
    }


    public class ConsoleLog : ILog, IDisposable
    {
        public ConsoleLog()
        {
            Console.WriteLine($"Creating a console log! {DateTime.Now.Ticks}");
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
        }

        public void Dispose()
        {
            Console.WriteLine($"Destroying consolelog");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*
            // 1. 
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>()
                .As<ILog>();

            var container = builder.Build();
            using (var scope1 = container.BeginLifetimeScope())
            {
                scope1.Resolve<ILog>();
            }
            

            // 2. 
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>()
                .As<ILog>().ExternallyOwned();

            var container = builder.Build();
            using (var scope1 = container.BeginLifetimeScope())
            {
                scope1.Resolve<ILog>(); // not calling Dispose()
            }
            

            // 3. 
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new ConsoleLog()).As<ILog>();

            var container = builder.Build();
            using (var scope1 = container.BeginLifetimeScope())
            {
                scope1.Resolve<ILog>(); // not calling Dispose()
            }

            using (var scope2 = container.BeginLifetimeScope())
            {
                scope2.Resolve<ILog>(); // not calling Dispose()
            }
            */

            // 4. 
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new ConsoleLog()).As<ILog>();

            using(var container = builder.Build())
            using (var scope1 = container.BeginLifetimeScope())
            {
                scope1.Resolve<ILog>(); // calling Dispose()
            }

            Console.ReadKey();
        }
    }
}

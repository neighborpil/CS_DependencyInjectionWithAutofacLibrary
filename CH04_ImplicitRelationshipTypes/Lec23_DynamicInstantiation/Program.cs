using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.OwnedInstances;

namespace Lec23_DynamicInstantiation
{
    public interface ILog : IDisposable
    {
        void Write(string message);
    }

    public class ConsoleLog : ILog
    {
        public ConsoleLog()
        {
            Console.WriteLine($"Console log created at {DateTime.Now.Ticks}");
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
        }

        public void Dispose()
        {
            Console.WriteLine("Console logger no longer required");
        }
    }

    public class SmsLog : ILog
    {
        private readonly string phoneNumber;

        public SmsLog(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public void Write(string message)
        {
            Console.WriteLine($"SMS to {phoneNumber}: {message}");
        }


        public void Dispose()
        {
        }
    }

    public class Reporting
    {
        private Func<ConsoleLog> consoleLog;

        public Reporting(Func<ConsoleLog> consoleLog)
        {
            this.consoleLog = consoleLog ?? throw new ArgumentNullException(nameof(consoleLog));
        }

        public void Report()
        {
            consoleLog().Write("Reporting to console");
            consoleLog().Write("And again");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<Reporting>();
            using (var c = builder.Build())
            {
                c.Resolve<Reporting>().Report();
            }

            Console.ReadKey();
        }
    }
}

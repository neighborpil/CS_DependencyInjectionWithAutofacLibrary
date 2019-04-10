using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_ImplicitRelationshipTypes
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
        private Lazy<ConsoleLog> log;

        public Reporting(Lazy<ConsoleLog> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            Console.WriteLine("Reporting class created");
        }

        public void Report()
        {
            log.Value.Write("Log started");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<Reporting>();

            using (var container = builder.Build())
            {
                container.Resolve<Reporting>().Report();
            }

            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_LifetimeEvent2
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
            Console.WriteLine("Console log no longer needed");
        }
    }

    public class SMSLog : ILog
    {
        private string phoneNumber;

        public SMSLog(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public void Write(string message)
        {
            Console.WriteLine($"SMS to {phoneNumber} : {message}");
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>().AsSelf();
            builder.Register<ILog>(c => c.Resolve<ConsoleLog>())
                .OnActivating(a =>
                {
                    a.ReplaceInstance(new SMSLog("+123456"));
                });

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var log = scope.Resolve<ILog>();
                log.Write("Testing"); // exception
            }

            Console.ReadKey();
        }
    }
}

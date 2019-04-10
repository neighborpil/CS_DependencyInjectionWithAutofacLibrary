using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec25_Enumeration
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
        private IList<ILog> allLogs;

        public Reporting(IList<ILog> allLogs)
        {
            this.allLogs = allLogs ?? throw new ArgumentNullException(nameof(allLogs));
        }

        public void Report()
        {
            foreach (var log in allLogs)
            {
                log.Write($"Hello, this is {log.GetType().Name}");
            }
        }
    }
    /*
    # Enumeration
     - enumeration을 inject하면
     - 만약 그냥 단일 interface를 등록하면 만약 구현된 것이 없으면 exception이다
     - 하지만 enumerable을 등록하면 empty list를 반환하므로 exception이 없다
    */
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>().As<ILog>();
            builder.Register(c => new SmsLog("+123455")).As<ILog>();
            builder.RegisterType<Reporting>();
            using (var c = builder.Build())
            {
                c.Resolve<Reporting>().Report();
            }

            Console.ReadKey();
        }
    }
}

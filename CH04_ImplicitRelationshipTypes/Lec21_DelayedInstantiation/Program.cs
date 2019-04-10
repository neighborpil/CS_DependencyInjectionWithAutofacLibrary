using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec21_DelayedInstantiation
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
        private Lazy<ConsoleLog> log; // 지연 호출하려면 Lazy API 사용하면 된다

        public Reporting(Lazy<ConsoleLog> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            Console.WriteLine($"Reporting component created");
        }

        public void Report()
        {
            log.Value.Write("Log started");
        }
    }

    public class ReportingNotLazy
    {
        private ConsoleLog log;

        public ReportingNotLazy(ConsoleLog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            Console.WriteLine($"Not lazy reporting component created");
        }

        public void Report()
        {
            log.Write("Not lazy Log started");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            #region Lazy API

            //new Lazy<ConsoleLog>(() => new ConsoleLog());;

            #endregion


            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<Reporting>();
            using (var c = builder.Build())
            {
                c.Resolve<Reporting>().Report();
            }

            /*
            // Result
            Reporting component created
            Console log created at 636904989780558690 // 호출이 될 때 생성된다
            Log started
            Console logger no longer required
            */
            Console.WriteLine();

            var builder2 = new ContainerBuilder();
            builder2.RegisterType<ConsoleLog>();
            builder2.RegisterType<ReportingNotLazy>();
            using (var c = builder2.Build())
            {
                c.Resolve<ReportingNotLazy>().Report();
            }

            /*
            // Result
            Console log created at 636904989780578571
            Not lazy reporting component created
            Not lazy Log started
            Console logger no longer required
            */

            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.OwnedInstances;

namespace Lec22_ControlledInstantiation
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
        private Owned<ConsoleLog> log;

        public Reporting(Owned<ConsoleLog> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            Console.WriteLine("Reporting initialized");
        }

        public void ReportOnce()
        {
            log.Value.Write("Log started");
            log.Dispose(); // ConsoleLog의 Dispose()가 아니라 Autofac의 Dispose()이다
        }
    }

    /*
    # Owned dependency
     - 특정 클래스를 사용할 때만 생성되는 클래스의 의존성
     - 예를들면 차를 세차 할 때만 세제를 쓰고 버린다
    */
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<Reporting>();
            using (var c = builder.Build())
            {
                c.Resolve<Reporting>().ReportOnce();
                Console.WriteLine("Done Reporting");

            }

            /*
            // result
            Console log created at 636904999109882571
            Reporting initialized
            Log started
            Console logger no longer required // 사용이 끝나고 바로 dispose된다
            Done Reporting
            */
            Console.ReadKey();
        }
    }
}

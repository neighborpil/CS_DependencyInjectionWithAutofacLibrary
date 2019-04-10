using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Indexed;

namespace Lec27_KeyedServiceLookup
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
        private IIndex<string, ILog> logs;

        public Reporting(IIndex<string, ILog> logs)
        {
            this.logs = logs ?? throw new ArgumentNullException(nameof(logs));
        }

        public void Report()
        {
            logs["sms"].Write("Starting report output");
        }
    }

    /*
    # Keyed Service Lookup
     - dictionary와 비슷 key를 가지고 원하는 서비스를 바로 찾는다

    */
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>().Keyed<ILog>("cmd");
            builder.Register(c => new SmsLog("+12345")).Keyed<ILog>("sms");
            builder.RegisterType<Reporting>();

            using (var c = builder.Build())
            {
                c.Resolve<Reporting>().Report();
            }


            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Metadata;

namespace Test_MetadataInterrogation
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

    public class Settings
    {
        public string LogMod { get; set; }
    }

    public class Reporting
    {
        private Meta<ConsoleLog, Settings> log;

        public Reporting(Meta<ConsoleLog, Settings> consoleLog)
        {
            this.log = consoleLog ?? throw new ArgumentNullException(nameof(consoleLog));
        }

        public void Report()
        {
            log.Value.Write("starting");

            //if(log.Metadata["mode"] as string == "verbose")
            //    Console.WriteLine($"VERBOSE MODE: message at {DateTime.Now}");

            if(log.Metadata.LogMod == "verbose")
                Console.WriteLine($"VERBOSE MODE: log writing: {DateTime.Now.Ticks}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<ConsoleLog>()
            //    .WithMetadata("mode", "verbose");
            builder.RegisterType<ConsoleLog>()
                .WithMetadata<Settings>(c => c.For(x => x.LogMod, "verbose"));
            builder.RegisterType<Reporting>();

            using (var c = builder.Build())
            {
                c.Resolve<Reporting>().Report();
            }

            Console.ReadKey();
        }
    }
}

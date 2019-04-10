using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Metadata;

namespace Lec26_MetadataInterrogation
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
        public string LogMode { get; set; }
    }

    public class Reporting
    {
        private Meta<ConsoleLog, Settings> log;

        public Reporting(Meta<ConsoleLog, Settings> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void Report()
        {
            log.Value.Write("Staring report");

            //if (log.Metadata["mode"] as string == "verbose") // strongly typed
            //    log.Value.Write($"VERBOSE MODE: Logger started on {DateTime.Now}");

            if (log.Metadata.LogMode == "verbose") // strongly typed
                log.Value.Write($"VERBOSE MODE: Logger started on {DateTime.Now}");
        }
    }

    /*
    # Metadata Interrogation
     - Allow you to attach metadata to components
     - Let you make decisions when resolving
     - Inject and store a Meta<T>
    */

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<ConsoleLog>().WithMetadata("mode", "info");
            //builder.RegisterType<ConsoleLog>().WithMetadata("mode", "verbose");
            builder.RegisterType<ConsoleLog>()
                .WithMetadata<Settings>(c => c.For(x => x.LogMode, "verbose"));
            builder.RegisterType<Reporting>();
            using (var c = builder.Build())
            {
                c.Resolve<Reporting>().Report();
            }

            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_Decorators
{
    public interface IReportingService
    {
        void Report();
    }

    public class ReportingService : IReportingService
    {
        public void Report()
        {
            Console.WriteLine("Here is your report");

        }
    }

    public class ReportingServiceWithLogging : IReportingService
    {
        private IReportingService service;

        public ReportingServiceWithLogging(IReportingService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void Report()
        {
            Console.WriteLine("Logging");
            service.Report();
            Console.WriteLine("Logging");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ReportingService>().Named<IReportingService>("reporting");
            containerBuilder.RegisterDecorator<IReportingService>(
                (context, service) => new ReportingServiceWithLogging(service), "reporting");

            using (var c = containerBuilder.Build())
            {
                var r = c.Resolve<IReportingService>();
                r.Report();
            }
            Console.ReadKey();
        }
    }
}

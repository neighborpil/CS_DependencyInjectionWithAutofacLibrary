using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec47_Decorators
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
        private IReportingService decorated;

        public ReportingServiceWithLogging(IReportingService decorated)
        {
            this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        }

        public void Report()
        {
            Console.WriteLine("Commencing log ...");
            decorated.Report();
            Console.WriteLine("Commencing log ...");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Decoration pattern을 사용하려고 한다. 하지만 injecting에 문제가 있다

            /*
            // 1. 자기자신을 계속 injecting하여 무한루프돈다
            var b = new ContainerBuilder();
            b.RegisterType<ReportingServiceWithLogging>().As<IReportingService>();
            */

            // 2. 
            var b = new ContainerBuilder();
            b.RegisterType<ReportingService>().Named<IReportingService>("reporting"); // 이름을 부여한다
            b.RegisterDecorator<IReportingService>(
                (context, service) => new ReportingServiceWithLogging(service), "reporting"
            );

            using (var c = b.Build())
            {
                var r = c.Resolve<IReportingService>();
                r.Report();
            }

            Console.ReadKey();
        }
    }
}

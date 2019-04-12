using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;

namespace Lec51_TypeInterceptors
{
    public class CallLogger : IInterceptor
    {
        private TextWriter output;

        public CallLogger(TextWriter output)
        {
            this.output = output ?? throw new ArgumentNullException(nameof(output));
        }

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            output.WriteLine("Calling method {0} with args {1}",
                methodName,
                string.Join(",", 
                    invocation.Arguments.Select(a => (a ?? "").ToString())));
            invocation.Proceed();
            output.WriteLine("Done calling {0}, result was {1}",
                methodName, invocation.ReturnValue);
        }
    }

    public interface IAudit
    {
        int Start(DateTime reportDate);
    }

    [Intercept(typeof(CallLogger))] // 이 클래스에서 일어나는 모든 operatioin을 intercept
    public class Audit : IAudit
    {
        public virtual int Start(DateTime reportDate)
        {
            Console.WriteLine($"Starting reporting on {reportDate}");
            return 42;
        }
    }

    /*
    # Type interceptors
     - logging기능을 dynamic proxy를 통하여 모든 메소드에 붙이려고 한다
     - Autofac.Extras.DynamicProxy 사용
     - intercepte any invoke and methods
     - castle api 사용
    */
    class Program
    {
        static void Main(string[] args)
        {
            var cb = new ContainerBuilder();
            cb.Register(c => new CallLogger(Console.Out))
                .As<IInterceptor>()
                .AsSelf();
            cb.RegisterType<Audit>()
                .As<IAudit>()
                .EnableClassInterceptors();

            using (var c = cb.Build())
            {
                var audit = c.Resolve<IAudit>();
                audit.Start(DateTime.Now);
            }

            Console.ReadKey();
        }
    }
}

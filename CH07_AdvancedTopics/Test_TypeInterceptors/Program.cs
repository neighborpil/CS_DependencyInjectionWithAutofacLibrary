using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;

namespace Test_TypeInterceptors
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
            output.WriteLine("{0} invoked: {1}",
                methodName,
                string.Join(",", invocation.Arguments.Select(a => (a ?? "").ToString())));
            invocation.Proceed();
            output.WriteLine("{0} returned: {1}",
                methodName, invocation.ReturnValue);
        }
    }

    public interface IAudit
    {
        int Start(DateTime reporDate);
    }

    [Intercept(typeof(CallLogger))]
    public class Audit : IAudit
    {
        public virtual int Start(DateTime reporDate)
        {
            Console.WriteLine($"Audit Started on {reporDate}");
            return (int)reporDate.Ticks;
        }
    }

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

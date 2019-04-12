using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.AggregateService;

namespace Test_AggregateService
{
    public interface IService1 { }
    public interface IService2 { }
    public interface IService3 { }
    public interface IService4 { }

    public class Class1 : IService1 { }
    public class Class2 : IService2 { }
    public class Class3 : IService3 { }

    public class Class4 : IService4
    {
        private string name;

        public Class4(string name)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }

    public interface IAggregateService
    {
        IService1 service1 { get; }
        IService2 service2 { get; }
        IService3 service3 { get; }
        IService4 service4(string name);
    }

    public class Consumer
    {
        private IAggregateService allservices;

        public IAggregateService Allservices => allservices;

        public Consumer(IAggregateService services)
        {
            this.allservices = services ?? throw new ArgumentNullException(nameof(services));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var cb = new ContainerBuilder();
            cb.RegisterAggregateService<IAggregateService>();
            cb.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => t.Name.StartsWith("Class"))
                .AsImplementedInterfaces();
            cb.RegisterType<Consumer>();

            using (var c = cb.Build())
            {
                var consumer = c.Resolve<Consumer>();
                Console.WriteLine(consumer.Allservices.service2.GetType().Name);
            }

            Console.ReadKey();
        }
    }
}

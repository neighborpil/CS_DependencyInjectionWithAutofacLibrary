using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.AggregateService;

namespace Lec50_AggregateServices
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

    public interface IMyAggregateService // 여러 인터페이스를 묶어준다
    {
        IService1 Service1 { get; }
        IService2 Service2 { get; }
        IService3 Service3 { get; }
        //IService4 Service4 { get; }
        IService4 GetFourthService(string name); // 만약 concrete 클래스에 생성자가 있다면, 메소드로 만들면 자동 연결한다
    }

    public class Consumer
    {
        // 1. 이렇게 할 필요 없이
        //private IService1 service1;
        //private IService2 service2;
        //private IService3 service3;
        //private IService4 service4;

        //public Consumer(IService1 service1, IService2 service2, IService3 service3, IService4 service4)
        //{
        //    this.service1 = service1 ?? throw new ArgumentNullException(nameof(service1));
        //    this.service2 = service2 ?? throw new ArgumentNullException(nameof(service2));
        //    this.service3 = service3 ?? throw new ArgumentNullException(nameof(service3));
        //    this.service4 = service4 ?? throw new ArgumentNullException(nameof(service4));
        //}

        // 2. 묶은 인터페이스를 사용 할 수 있다
        public IMyAggregateService allServices;
        public IMyAggregateService AllServices => allServices;

        public Consumer(IMyAggregateService allServices)
        {
            this.allServices = allServices ?? throw new ArgumentNullException(nameof(allServices));
        }
    }
    /*
    # Aggregate Services
     - 아주 많은 서비스가 있을 때 이를 묶어주는 것
     - Autofac.Extras.AggregateService 패키지 설치 필요
    */
    class Program
    {
        static void Main(string[] args)
        {
            var cb = new ContainerBuilder();
            cb.RegisterAggregateService<IMyAggregateService>(); // 각각에 해당하는 dynamic class를 inject해준다
            cb.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => t.Name.StartsWith("Class"))
                .AsImplementedInterfaces();
            cb.RegisterType<Consumer>();
            using (var c = cb.Build())
            {
                var consumer = c.Resolve<Consumer>();
                Console.WriteLine(consumer.AllServices.Service3.GetType().Name);
                Console.WriteLine(consumer.AllServices.GetFourthService("test").GetType().Name);
            }

            Console.ReadKey();
        }
    }
}

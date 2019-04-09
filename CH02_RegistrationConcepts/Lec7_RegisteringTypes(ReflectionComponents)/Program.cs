using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec7_RegisteringTypes_ReflectionComponents_
{
    public interface ILog
    {
        void Write(string message);
    }

    public class ConsoleLog : ILog
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class Engine
    {
        private ILog log;
        private int id;

        public Engine(ILog log)
        {
            this.log = log;
            id = new Random().Next();
        }

        public void Ahead(int power)
        {
            log.Write($"Engine [{id}] ahead {power}");
        }
    }

    public class Car
    {
        private Engine engine;
        private ILog log;

        public Car(Engine engine, ILog log)
        {
            this.engine = engine;
            this.log = log;
        }

        public void Go()
        {
            engine.Ahead(100);
            log.Write("Car going forawrd...");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
            var builder = new ContainerBuilder(); // 처음에 컨테이너 빌더를 만들고 사용되는 모든 클래스를 등록한다
            /* Components - components등록은 수동으로*/
            builder.RegisterType<ConsoleLog>().As<ILog>();
            builder.RegisterType<Engine>(); // component registering에 빠진게 있으면 exception
            builder.RegisterType<Car>();

            var/*IContainer*/ container = builder.Build(); // 빌드가 되면 그때 recursive하게 클래스들이 등록된다
            var car = container.Resolve<Car>();
            car.Go();

            // 아래와 같이 ILog를 사용하려하면 동작하지 않는다. ConsoleLog를 ILog로 등록했기 때문에 components가 없는거로 간주
            // var log = container.Resolve(ConsoleLog);
            // 따라서 위에서 components를 등록할 때 .AsSelf()도 추가해주어야 한다
            //builder.RegisterType<ConsoleLog>().As<ILog>().AsSelf();

            Console.ReadKey();
        }
    }
}

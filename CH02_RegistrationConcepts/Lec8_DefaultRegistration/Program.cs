using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec8_DefaultRegistration
{
    public interface ILog
    {
        void Write(string message);
    }

    public interface IConsole
    {
    }

    public class ConsoleLog : ILog, IConsole
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class EmailLog : ILog
    {
        private const string adminEmail = "admin@foo.com";

        public void Write(string message)
        {
            Console.WriteLine($"Email send to {adminEmail} : {message}");
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

            #region Dependency Injection
            var builder = new ContainerBuilder();
            //builder.RegisterType< /*ConsoleLog*/EmailLog>().As<ILog>(); // EmailLog로 바꾸려면 여기만 바꾸면 된다
            builder.RegisterType<ConsoleLog>()
                .As<ILog>()
                .As<IConsole>(); // 두개 다의 default로 등록 되었다
            //builder.RegisterType<ConsoleLog>().As<ILog>(); // 만약 여러개가 등록된다면 가장 아래에 등록된것을 기준으로 한다
            builder.RegisterType<ConsoleLog>().As<ILog>().PreserveExistingDefaults(); // 이 구문 쓰면 먼저 등록된게 없다면 사용
            builder.RegisterType<Engine>();
            builder.RegisterType<Car>();

            var container = builder.Build();
            var car = container.Resolve<Car>();
            car.Go();
            #endregion

            #region Former way
            // 기존의 방법으로는 Engine, Car 내부에 다 들어가서 바꿔줘야 한다
            //var log = new ConsoleLog();
            //var engine = new Engine(log);
            //var car2 = new Car(engine, log);
            //car2.Go(); 
            #endregion

            Console.ReadKey();
        }
    }
}

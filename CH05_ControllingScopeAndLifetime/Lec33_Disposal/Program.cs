using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec33_Disposal
{
    public interface ILog
    {
        void Write(string message);
    }

    public interface IConsole
    {

    }

    public class ConsoleLog : ILog, IConsole, IDisposable
    {
        public ConsoleLog()
        {
            Console.WriteLine($"Creating a console log! {DateTime.Now.Ticks}");
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
        }

        public void Dispose()
        {
            Console.WriteLine("Console log no longer needed");
        }
    }

    public class EmailLog : ILog
    {
        private const string adminEmail = "admin@foo.com";

        public void Write(string message)
        {
            Console.WriteLine($"Email sent to {adminEmail} : {message}");
        }
    }

    public class SMSLog : ILog
    {
        private string phoneNumber;

        public SMSLog(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public void Write(string message)
        {
            Console.WriteLine($"SMS to {phoneNumber} : {message}");
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

        public Engine(ILog log, int id)
        {
            this.log = log;
            this.id = id;
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

        public Car(Engine engine)
        {
            this.engine = engine;
            this.log = new EmailLog();
        }

        public Car(Engine engine, ILog log)
        {
            this.engine = engine;
            this.log = log;
        }

        public void Go()
        {
            engine.Ahead(100);
            log.Write("Car going forward...");
        }
    }

    public class Parent
    {
        public override string ToString()
        {
            return "I am your father";
        }
    }

    public class Child
    {
        public string Name { get; set; }
        public Parent Parent { get; set; }

        public void SetParent(Parent parent)
        {
            Parent = parent;
        }
    }

    public class ParentChildModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Parent>();
            builder.Register(c => new Child() { Parent = c.Resolve<Parent>() });
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*
            // 1. IDisposable 구현하기
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<ConsoleLog>();
            }

            // 2. Dispose를 호출하고 싶지 않을 때: .ExternallyOwned()
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>().ExternallyOwned();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<ConsoleLog>();
            }
            

            // 3. 인스턴스를 등록하였을 때, 그냥은 Dispose()가 호출되지 않는다 => 다른 scope에서 호출 할 수 있으므로 허용하지 않는다
            var builder = new ContainerBuilder();
            //builder.RegisterType<ConsoleLog>();
            builder.RegisterInstance(new ConsoleLog());
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<ConsoleLog>();
            }

            using (var scope2 = container.BeginLifetimeScope()) // 여기서 사용 할 수 있으므로 IDispose() 호출 X
            {
                scope2.Resolve<ConsoleLog>();
            }
            */

            // 4. 인스턴스를 등록하였어도 Dispose()를 호출하고 싶다면, container자체가 종료될 때 호출된다
            var builder = new ContainerBuilder();
            //builder.RegisterType<ConsoleLog>();
            builder.RegisterInstance(new ConsoleLog());
            using(var container = builder.Build())
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<ConsoleLog>();
            }

            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec31_InstanceScope
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
        public ConsoleLog()
        {
            Console.WriteLine($"Creating a console log! {DateTime.Now.Ticks}");
        }

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
            Console.WriteLine($"Email sent to {adminEmail} : {message}");
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
            var builder = new ContainerBuilder();


            /*
            // 1. make new instance every time
            builder.RegisterType<ConsoleLog>() // 여기서 behavior를 등록하여 lifetime을 조절 할 수 있다
                .InstancePerDependency(); // ConsoleLog를 호출 할 때마다 새로운 ConsoleLog를 생성하여 제공

            var container = builder.Build();
            using (var scope1 = container.BeginLifetimeScope())
            {
                for (int i = 0; i < 3; i++)
                    scope1.Resolve<ConsoleLog>();
            }
            using (var scope2 = container.BeginLifetimeScope())
            {
                for (int i = 0; i < 3; i++)
                    scope2.Resolve<ConsoleLog>();
            }
            
            // 2. use just one instance
            builder.RegisterType<ConsoleLog>().As<ILog>()
                .SingleInstance(); // 싱글톤이 되어 호출될 때마다 이게 계속 사용된다


            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var log = scope.Resolve<ILog>();
                log.Write("Testing!");

                scope.Resolve<ILog>();
                log.Write("Testing again");

            } // disposed basically

            */

            // 3. create instances for each scope
            builder.RegisterType<ConsoleLog>()
                .InstancePerMatchingLifetimeScope("foo");

            var container = builder.Build();
            using (var scope1 = container.BeginLifetimeScope("foo")) // 여기서만 인스턴스 생성
            {
                for (int i = 0; i < 3; i++)
                    scope1.Resolve<ConsoleLog>();

                using (var scope2 = scope1.BeginLifetimeScope())
                {
                    for (int i = 0; i < 3; i++)
                        scope2.Resolve<ConsoleLog>();
                }
            }


            //using (var scope3 = container.BeginLifetimeScope())
            //{
            //    scope3.Resolve<ConsoleLog>(); // crash
            //}

            Console.ReadKey();
        }
    }
}

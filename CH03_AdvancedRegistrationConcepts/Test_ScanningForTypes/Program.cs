using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;

namespace Test_ScanningForTypes
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

    public class SMSLog : ILog
    {
        public string phoneNumber;

        public SMSLog(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public void Write(string message)
        {
            Console.WriteLine($"SMS to {phoneNumber}: {message}");
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
            log.Write("Car going forawrd...");
        }
    }

    public class Parent
    {
        public override string ToString()
        {
            return "I'm father";
        }
    }

    public class Child
    {
        public string Name { get; set; }
        public Parent Parent { get; set; }

        public void SetParent(Parent parent)
        {
            this.Parent = parent;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var builder = new ContainerBuilder();
            //builder.RegisterAssemblyTypes(assembly)
            //    .Where(a => a.Name.EndsWith("Log"))
            //    .Except<SMSLog>()
            //    .Except<ConsoleLog>(c => c.As<ILog>().SingleInstance())
            //    .AsSelf();

            builder.RegisterAssemblyTypes(assembly)
                .Where(a => a.Name.EndsWith("Log") && a.Name.StartsWith("Email"))
                .Except<SMSLog>()
                .As<ILog>();
            builder.RegisterType<Engine>()
                .WithParameter(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(int) && pi.Name == "id",
                    (pi, ctx) => "123"));
            builder.RegisterType<Car>()
                .UsingConstructor(typeof(Engine));

            var container = builder.Build();
            var car = container.Resolve<Car>();
            car.Go();

            Console.ReadKey();
        }
    }
}

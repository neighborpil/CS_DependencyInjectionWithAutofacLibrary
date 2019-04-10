using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;

namespace Test_PassingParametersToRegister
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

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            /*
            // 1. named parameter
            
            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter("phoneNumber", "+12345");
            
            // 2. typed parameter 
            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter(new TypedParameter(typeof(string), "+123456"));

            // 3. resolved parameter
            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "phoneNumber",
                    (pi, ctx) => "+1234567"));
            
            var container = builder.Build();
            var log = container.Resolve<ILog>();
            */

            // 4. setting parameters when resolving
            builder.Register((c, p) => new SMSLog(p.Named<string>("phoneNumber")))
                .As<ILog>();
            var container = builder.Build();
            Random random = new Random();
            var log = container.Resolve<ILog>(new NamedParameter("phoneNumber", "+1234"));
            log.Write("messsss");

            var builder2 = new ContainerBuilder();
            builder2.Register((c, p) => new SMSLog(p.Named<string>("phoneNumber")))
                .As<ILog>();

            var container2 = builder2.Build();
            var log2 = container2.Resolve<ILog>(new NamedParameter("phoneNumber", "+123"));
            log2.Write("ssses");

            Console.ReadKey();
        }
    }
}

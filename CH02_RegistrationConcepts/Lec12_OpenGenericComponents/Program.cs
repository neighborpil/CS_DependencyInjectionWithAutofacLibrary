using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec12_OpenGenericComponents
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

            // IList<T> --> List<T>
            // IList<int> --> List<int>
            builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>)); // generic 등록, 내가 작성하 것도 등록 가능

            IContainer container = builder.Build();
            var myList = container.Resolve<IList<int>>();
            Console.WriteLine(myList.GetType().Name);
            Console.WriteLine(myList.GetType());

            Console.ReadKey();

        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;

namespace Lec15_PassingParametersToRegister
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
            log.Write("Car going forawrd...");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            // 파라미터 전달하는 방법

            /*
            // 1. named parameter 
            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter("phoneNumber", "+1234567");

            var container = builder.Build();
            var log = container.Resolve<ILog>();
            */

            /*
            // 2. typed parameter
            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter(new TypedParameter(typeof(string), "+1234567"));

            var container = builder.Build();
            var log = container.Resolve<ILog>();
            */

            /*
            // 3. resolved parameter 
            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter(
                    new ResolvedParameter(
                        // predicate : 이 값이 어떤것인지 서술하는 부분
                        (ParameterInfo pi, IComponentContext ctx) => pi.ParameterType == typeof(string) && pi.Name == "phoneNumber",
                        // value accessor : 실제 값
                        (ParameterInfo pi, IComponentContext ctx) => "+1234567"
                        ));

            var container = builder.Build();
            var log = container.Resolve<ILog>();
            */

            // 4. lamda expression
            Random random = new Random();
            builder.Register((c, p) => new SMSLog(p.Named<string>("phoneNumber")))
                .As<ILog>();

            Console.WriteLine("About to build container...");
            var container = builder.Build();
            // 실제 사용(resolve) 할 때 값을 넣는다
            var log = container.Resolve<ILog>(new NamedParameter("phoneNumber", random.Next().ToString()));
            
            log.Write("test message");

            Console.ReadKey();
        }
    }
}

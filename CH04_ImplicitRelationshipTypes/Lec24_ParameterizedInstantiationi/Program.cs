﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec24_ParameterizedInstantiationi
{
    public interface ILog : IDisposable
    {
        void Write(string message);
    }

    public class ConsoleLog : ILog
    {
        public ConsoleLog()
        {
            Console.WriteLine($"Console log created at {DateTime.Now.Ticks}");
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
        }

        public void Dispose()
        {
            Console.WriteLine("Console logger no longer required");
        }
    }

    public class SmsLog : ILog
    {
        private readonly string phoneNumber;

        public SmsLog(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public void Write(string message)
        {
            Console.WriteLine($"SMS to {phoneNumber}: {message}");
        }


        public void Dispose()
        {
        }
    }
    
    public class Reporting
    {
        private Func<ConsoleLog> consoleLog;
        private Func<string, SmsLog> smsLog;

        public Reporting(Func<ConsoleLog> consoleLog, Func<string, SmsLog> smsLog)
        {
            this.consoleLog = consoleLog ?? throw new ArgumentNullException(nameof(consoleLog));
            this.smsLog = smsLog ?? throw new ArgumentNullException(nameof(smsLog));
        }

        public void Report()
        {
            consoleLog().Write("call consolelog");
            consoleLog().Write("again");

            smsLog("+12345").Write("Texting admins...");
        }
    }

    /*
    # parameterized instantiation
     - dynamic하게 인스턴스를 생성
     - register 또는 resolve할 때가 아니더라도 인스턴스를 생성하고 변수를 대입하는 것이 가능하다
    */
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<SmsLog>();
            builder.RegisterType<Reporting>();
            using (var c = builder.Build())
            {
                c.Resolve<Reporting>().Report();
            }

            Console.ReadKey();
        }
    }
}

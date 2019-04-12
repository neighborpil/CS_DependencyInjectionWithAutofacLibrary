using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;

namespace Lec40_JSON_XML_ConfigurationWithMicrosoftConfig
{
    public interface IOperation
    {
        float Calculate(float a, float b);
    }

    public class Addition : IOperation
    {
        public float Calculate(float a, float b)
        {
            return a + b;
        }
    }

    public class Multiplication : IOperation
    {
        public float Calculate(float a, float b)
        {
            return a * b;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Microsoft Configuraion: 이거를 이용하여 json파일을 불러온다
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json"); // JSON 내에 namespace 지정하는거 주의!
            IConfigurationRoot configuration = configBuilder.Build();


            // Autofac Configuration: 불러온 config file을 autofac에서 사용한다
            var containerBuilder = new ContainerBuilder();
            var configModule = new ConfigurationModule(configuration);
            containerBuilder.RegisterModule(configModule);

            using (var container = containerBuilder.Build())
            {
                float a = 3, b = 4;
                foreach (IOperation op in container.Resolve<IList<IOperation>>())
                {
                    Console.WriteLine($"{op.GetType().Name} of {a} and {b} = {op.Calculate(a,b)}");
                }
            }

                Console.ReadKey();
        }
    }
}

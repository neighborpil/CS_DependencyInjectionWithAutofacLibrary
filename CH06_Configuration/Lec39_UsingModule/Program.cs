using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec39_UsingModule
{
    public interface IVehicle
    {
        void Go();
    }

    public class Truck : IVehicle
    {
        private IDriver driver;

        public Truck(IDriver driver)
        {
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        public void Go()
        {
            driver.Drive();
        }
    }

    public interface IDriver
    {
        void Drive();
    }

    public class CrazyDriver : IDriver
    {
        public void Drive()
        {
            Console.WriteLine("Going too fast and crashing into a tree");
        }
    }

    public class SaneDriver : IDriver
    {
        public void Drive()
        {
            Console.WriteLine("Driving safely to destination");
        }
    }

    public class TransportModule : Module
    {
        public bool ObeySpeedLimit { get; set; } // 외부의 요소를 통하여 간단하게 어떤 것을 사용할 지 정할 수 있다

        protected override void Load(ContainerBuilder builder)
        {
            if (ObeySpeedLimit)
                builder.RegisterType<SaneDriver>().As<IDriver>();
            else
                builder.RegisterType<CrazyDriver>().As<IDriver>();

            builder.RegisterType<Truck>().As<IVehicle>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builer = new ContainerBuilder();
            builer.RegisterModule(new TransportModule { ObeySpeedLimit = true });
            using (var c = builer.Build())
            {
                c.Resolve<IVehicle>().Go();
            }

            Console.ReadKey();
        }
    }
}

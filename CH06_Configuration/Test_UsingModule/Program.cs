using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_UsingModule
{
    public interface IDriver
    {
        void Drive();
    }

    public class SaneDriver : IDriver
    {
        public void Drive()
        {
            Console.WriteLine("Driving safely to destination");

        }
    }

    public class CrazyDriver : IDriver
    {
        public void Drive()
        {
            Console.WriteLine("Going too fast and crashing into a tree");

        }
    }

    public interface IVehicle
    {
        void Go();
    }

    class Truck : IVehicle
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

    public class TransportModule : Module
    {
        public bool ObeySpeedLimit { get; set; }

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
            var builder = new ContainerBuilder();
            //builder.RegisterModule(new TransportModule());
            builder.RegisterModule(new TransportModule(){ObeySpeedLimit = true});

            using (var c = builder.Build())
            {
                var truck = c.Resolve<IVehicle>();
                truck.Go();
            }

            Console.ReadKey();
        }
    }
}

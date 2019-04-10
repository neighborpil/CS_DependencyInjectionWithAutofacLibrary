using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec32_CaptiveDependencies
{
    /*
    # Captive Dependencies
     - 오랜기간 생존하는 component가 짧은 기간 생존하는 개체를 계속 가지고 있는 문제

    */

    public interface IResource
    {

    }

    public class SingletonResource : IResource
    {

    }

    public class InstancePerDependencyResource : IResource, IDisposable
    {
        public InstancePerDependencyResource()
        {
            Console.WriteLine("Instance per dep created");
        }

        public void Dispose()
        {
            Console.WriteLine("Instance per dep destroyed");
        }
    }

    public class ResourceManager
    {
        public IEnumerable<IResource> Resources { get; set; }

        public ResourceManager(IEnumerable<IResource> resources)
        {
            Resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ResourceManager>().SingleInstance();
            builder.RegisterType<SingletonResource>().As<IResource>()
                .SingleInstance();
            builder.RegisterType<InstancePerDependencyResource>().As<IResource>();

            using (var container = builder.Build())
            using (var scopoe = container.BeginLifetimeScope())
            {
                scopoe.Resolve<ResourceManager>(); 
                // ResorceManager가 싱글톤이라 InstancePerDependencyResource의 lifetime이 종속된다
            }

            Console.ReadKey();
        }
    }
}

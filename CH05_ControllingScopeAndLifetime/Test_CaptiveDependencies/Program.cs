using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_CaptiveDependencies
{
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
            Console.WriteLine("Instance created");
        }

        public void Dispose()
        {
            Console.WriteLine("Instance destroyed");
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
            builder.RegisterType<SingletonResource>()
                .As<IResource>()
                .SingleInstance();
            builder.RegisterType<InstancePerDependencyResource>()
                .As<IResource>();

            using (var c = builder.Build())
            using (var scope1 = c.BeginLifetimeScope())
            {
                scope1.Resolve<ResourceManager>();
            }

            Console.ReadKey();
        }
    }
}

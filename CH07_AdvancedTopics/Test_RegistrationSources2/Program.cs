using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;

namespace Test_RegistrationSources2
{
    public abstract class BaseHandler
    {
        public virtual string Handle(string message)
        {
            return "Handled: " + message;
        }
    }

    public class HandlerA : BaseHandler
    {
        public override string Handle(string message)
        {
            return "Handled by HandlerA: " + message;
        }
    }

    public class HandlerB : BaseHandler
    {
        public override string Handle(string message)
        {
            return "Handled by HandlerB: " + message;
        }
    }

    public interface IHandlerFactory
    {
        T GetHandler<T>() where T : BaseHandler;
    }

    public class HandlerFactory : IHandlerFactory
    {
        public T GetHandler<T>() where T : BaseHandler
        {
            return Activator.CreateInstance<T>();
        }
    }

    public class ConsumerA
    {
        private HandlerA handlerA;

        public ConsumerA(HandlerA handlerA)
        {
            this.handlerA = handlerA ?? throw new ArgumentNullException(nameof(handlerA));
        }

        public void DoWork()
        {
            Console.WriteLine(handlerA.Handle($"{this.GetType().Name}"));
        }
    }

    public class ConsumerB
    {
        private HandlerB handlerB;

        public ConsumerB(HandlerB handlerB)
        {
            this.handlerB = handlerB ?? throw new ArgumentNullException(nameof(handlerB));
        }

        public void DoWork()
        {
            Console.WriteLine(handlerB.Handle($"{this.GetType().Name}"));
        }
    }

    public class HandlerRegistrationSource : IRegistrationSource
    {
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            var swt = service as IServiceWithType;
            if (swt == null
                || swt.ServiceType == null
                || !swt.ServiceType.IsAssignableTo<BaseHandler>())
            {

                yield break;
            }

            yield return new ComponentRegistration(
                Guid.NewGuid(),
                new DelegateActivator(
                    swt.ServiceType,
                    (context, parameters) =>
                    {
                        var provider = context.Resolve<IHandlerFactory>();
                        var method = provider.GetType().GetMethod("GetHandler").MakeGenericMethod(swt.ServiceType);
                        return method.Invoke(provider, null);
                    }),
                    new CurrentScopeLifetime(),
                    InstanceSharing.None,
                    InstanceOwnership.OwnedByLifetimeScope,
                    new[] {service},
                    new ConcurrentDictionary<string, object>()
                );
        }

        public bool IsAdapterForIndividualComponents => false;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<HandlerFactory>().As<IHandlerFactory>();
            builder.RegisterSource(new HandlerRegistrationSource());
            builder.RegisterType<ConsumerA>();
            builder.RegisterType<ConsumerB>();

            using (var c = builder.Build())
            {
                c.Resolve<ConsumerA>().DoWork();
                c.Resolve<ConsumerB>().DoWork();
            }

            Console.ReadKey();
        }
    }
}

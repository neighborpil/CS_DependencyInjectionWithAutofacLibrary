using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;

namespace Lec16_PropertyAndMethodInjection2
{
    public class Parent
    {
        public override string ToString()
        {
            return "I'm your father";
        }
    }

    public class Child
    {
        public string Name { get; set; }
        public Parent Parent { get; set; }

        public void SetParent(Parent parent)
        {
            this.Parent = parent;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ContainerBuilder();
            builder.RegisterType<Parent>();

            /*
            // 4. Method를 활용하여 Property를 등록하기(Setter가 있을 경우에만)
            builder.Register(c =>
            {
                var child = new Child();
                child.SetParent(c.Resolve<Parent>());
                return child;
            });

            
            */

            // 5. Event Handler를 이용하여 생성
            builder.RegisterType<Child>()
                .OnActivated((IActivatedEventArgs<Child> e) =>
                {
                    var p = e.Context.Resolve<Parent>();
                    e.Instance.SetParent(p);
                });

            var container = builder.Build();
            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent);

            Console.ReadKey();
        }
    }
}

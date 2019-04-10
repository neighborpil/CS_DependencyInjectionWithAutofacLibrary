using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec18_ScanningForModules
{
    public class Parent
    {
        public override string ToString()
        {
            return "I'm father";
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

    /*
    # module
     - 하나의 거대한 registraion을 하는 것보다, 여러개로 나누어서 등록하여 사용하는 것
    */

    public class ParentChildModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Parent>();
            builder.Register(c => new Child() {Parent = c.Resolve<Parent>()});
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(Program).Assembly);
            builder.RegisterAssemblyModules<ParentChildModule>(typeof(Program).Assembly);

            var container = builder.Build();
            Console.WriteLine(container.Resolve<Child>().Parent);

            Console.ReadKey();
        }
    }
}

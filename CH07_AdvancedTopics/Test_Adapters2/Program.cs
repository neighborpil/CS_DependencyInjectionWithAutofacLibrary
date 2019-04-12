using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Metadata;

namespace Test_Adapters2
{
    public interface ICommand
    {
        void Execute();
    }

    public class OpenCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Openning a file");
        }
    }

    public class SaveCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Saving a file");
        }
    }

    public class Button
    {
        private ICommand command;
        private string name;
        public string Name => name;

        public Button(ICommand command, string name)
        {
            this.command = command ?? throw new ArgumentNullException(nameof(command));
            this.name = name;
        }

        public void Click()
        {
            Console.WriteLine($"{Name}");
            command.Execute();
        }
    }

    public class Editor
    {
        IEnumerable<Button> buttons;

        public Editor(IEnumerable<Button> buttons)
        {
            this.buttons = buttons ?? throw new ArgumentNullException(nameof(buttons));
        }

        public void ClickAll()
        {
            foreach (var button in buttons)
            {
                button.Click();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<OpenCommand>().As<ICommand>().WithMetadata("Name", "Open");
            builder.RegisterType<SaveCommand>().As<ICommand>().WithMetadata("Name", "Save");
            builder.RegisterAdapter<Meta<ICommand>, Button>(cmd => new Button(cmd.Value, (string)cmd.Metadata["Name"]));
            builder.RegisterType<Editor>();

            using (var c = builder.Build())
            {
                c.Resolve<Editor>().ClickAll();
            }

            Console.ReadKey();

        }
    }
}

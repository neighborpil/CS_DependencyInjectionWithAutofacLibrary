using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Test_Adapters
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

        public Button(ICommand command)
        {
            this.command = command ?? throw new ArgumentNullException(nameof(command));
        }

        public void Click()
        {
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
            builder.RegisterType<OpenCommand>().As<ICommand>();
            builder.RegisterType<SaveCommand>().As<ICommand>();
            //builder.RegisterType<Button>();
            builder.RegisterAdapter<ICommand, Button>(cmd => new Button(cmd));
            builder.RegisterType<Editor>();

            using (var c = builder.Build())
            {
                c.Resolve<Editor>().ClickAll();
            }

            Console.ReadKey();

        }
    }
}

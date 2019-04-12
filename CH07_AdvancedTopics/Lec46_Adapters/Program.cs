using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lec46_Adapters
{
    public interface ICommand
    {
        void Execute();
    }

    class SaveCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Saving a file");
        }
    }

    class OpenCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Opening a file");
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
        private IEnumerable<Button> buttons;

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
            /*
            // 1. 실제 해보면 버튼이 하나만 등록이 된다
            var b = new ContainerBuilder();
            b.RegisterType<SaveCommand>().As<ICommand>();
            b.RegisterType<OpenCommand>().As<ICommand>();
            b.RegisterType<Button>();
            b.RegisterType<Editor>();

            using (var c = b.Build())
            {
                var editor = c.Resolve<Editor>();
                editor.ClickAll(); 
            }
            */

            // 2. RegisterAdapter를 이용하여 버튼을 여러개 등록하여 사용하는 방법
            var b = new ContainerBuilder();
            b.RegisterType<SaveCommand>().As<ICommand>();
            b.RegisterType<OpenCommand>().As<ICommand>();
            //b.RegisterType<Button>();
            b.RegisterAdapter<ICommand, Button>(cmd => new Button(cmd));
            b.RegisterType<Editor>();

            using (var c = b.Build())
            {
                var editor = c.Resolve<Editor>();
                editor.ClickAll();
            }

            Console.ReadKey();
        }
    }
}

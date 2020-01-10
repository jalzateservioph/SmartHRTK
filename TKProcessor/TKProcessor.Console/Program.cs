using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                ConsoleMode();
            }
            else
            {
                CommandLineMode(args);
            }
        }

        static void ConsoleMode()
        {
            string input;

            do
            {
                System.Console.Write("Enter command: ");

                input = System.Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    string[] args = input.Split(' ');
                    CommandLineMode(args);
                }
            }
            while (!input.StartsWith("exit"));
        }

        static void CommandLineMode(string[] args)
        {
            if (args.Length == 0)
            {
                // show all available commands
            }

            ICommand cmd = CommandFactory.Create(args[0]);

            System.Console.WriteLine(cmd.Execute(args)?.ToString() ?? "Command did not send an output");
        }
    }
}

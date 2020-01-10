using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Console.Commands;

namespace TKProcessor.Console
{
    public interface ICommand
    {
        string Command { get; }
        string CommandHelp { get; }

        object Execute(params string[] args);
    }

    public static class CommandFactory
    {
        public static ICommand Create(string token)
        {
            if (string.Compare(token, "user_add") == 0)
                return new AddUserCommand();

            return null;
        }
    }
}

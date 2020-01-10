using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Services.Maintenance;

namespace TKProcessor.Console.Commands
{
    public class AddUserCommand : ICommand
    {
        public string Command => "user_add";

        public string CommandHelp => $"user_add [Name] [Username] [Password(optional, default is 'Password1')]";

        public object Execute(params string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Length == 2)
                return "Too few arguments supplied" + Environment.NewLine + CommandHelp;

            string result = "";

            using (UserService service = new UserService())
            {
                var outUser = service.Add(args[1], args[2], args.Length == 4 ? args[3] : "Password1");

                result = outUser == null ? $"User was not created" : JsonConvert.SerializeObject(outUser);
            }

            return result;
        }
    }
}

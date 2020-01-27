using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Services.Maintenance;

namespace TKProcessor.Console.Commands
{
    public class CreateDatabaseCommand : ICommand
    {
        public string Command => "database_migrate";

        public string CommandHelp => "";

        public object Execute(params string[] args)
        {
            System.Console.WriteLine("Generating/Updating database...");

            DatabaseService.Migrate();

            System.Console.WriteLine("Database created. Press any key to close.");

            System.Console.ReadLine();

            return null;
        }
    }
}

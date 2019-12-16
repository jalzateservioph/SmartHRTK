using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Services.Maintenance;

namespace TKProcessor.DBGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Timekeepin Database Generator");

            new Program();
        }

        public Program()
        {
            Console.WriteLine("Generating/Updating database...");

            DatabaseService.Migrate();

            Console.WriteLine("Database created. Press any key to close.");

            Console.ReadLine();
        }
    }
}

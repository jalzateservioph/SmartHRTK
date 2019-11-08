using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models.DP;

namespace TKProcessor.Services.Maintenance
{
    public class DatabaseService
    {
        public static void Migrate()
        {
            using (TKContext context = new TKContext())
            {
                context.Database.Migrate();

                if (context.User.Count() == 0)
                {
                    context.User.Add(new Models.TK.User()
                    {
                        Name = "Administrator",
                        Username = "admin",
                        Password = Hash("Password1")
                    });

                    context.SaveChanges();
                }
            }
        }

        private static string Hash(string input)
        {
            string salt = ConfigurationManager.AppSettings["salt"];

            SHA256Managed hasher = new SHA256Managed();

            string saltedInput = String.Concat(input, salt);

            byte[] hashedDataBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(saltedInput));

            return Convert.ToBase64String(hashedDataBytes);
        }

        public static bool TryConnectTK()
        {
            try
            {
                using (TKContext context = new TKContext())
                {
                    return (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

                }
            }
            catch
            {
                return false;
            }
        }

        public static bool TryConnectDP()
        {
            try
            {
                using (DPContext context = new DPContext())
                {
                    return (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool TryConnectSHR()
        {
            try
            {
                using (SHRContext context = new SHRContext())
                {
                    return (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

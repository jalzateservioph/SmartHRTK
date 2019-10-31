using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Models.TK;
using System.Security.Cryptography;
using System.Configuration;
namespace TKProcessor.Services
{
    public class UserService : TKService<User>
    {
        public bool TryLogin(string username, string password, out User user)
        {
            try
            {
                user = Context.User.FirstOrDefault(i => i.Username == username && i.Password == Hash(password));

                return user != default(User);
            }
            catch (Exception ex)
            {
                Context.ErrorLog.Add(new ErrorLog(ex));
                Context.SaveChanges();

                throw ex;
            }
        }

        public bool TryLogin(User user)
        {
            var result = TryLogin(user.Username, user.Password, out User validUser);

            user.Id = validUser.Id;
            user.Name = validUser.Name;
            user.DPUserId = validUser.DPUserId;

            return result;
        }

        private string Hash(string input)
        {
            string salt = ConfigurationManager.AppSettings["salt"];
            SHA256Managed hasher = new SHA256Managed();
            string saltedInput = String.Concat(input, salt);

            byte[] hashedDataBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(saltedInput));

            return Convert.ToBase64String(hashedDataBytes);
        }
    }
}

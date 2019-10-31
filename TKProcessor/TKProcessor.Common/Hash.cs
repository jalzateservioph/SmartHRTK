using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.Common
{
    public static class Hashing
    {        public static string Hash(string input, string salt)
        {
            SHA256Managed hasher = new SHA256Managed();

            string saltedInput = string.Concat(input, salt);

            byte[] hashedDataBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(saltedInput));

            return Convert.ToBase64String(hashedDataBytes);
        }
    }
}

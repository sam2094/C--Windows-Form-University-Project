using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Univer
{
    static class Extensions
    {
        public static bool IsNotEmpty(string[] values, string checkValue)
        {
            foreach (string val in values)
            {
                if (val == checkValue)
                {
                    return false;
                }
            }
            return true;
        }

        public static string hashPassword(this string password)
        {
            byte[] byteAr = new ASCIIEncoding().GetBytes(password);
            byte[] hashedAr = new SHA256Managed().ComputeHash(byteAr);
            string hashedPassword = new ASCIIEncoding().GetString(hashedAr);
            return hashedPassword;
        }
        //Yaradilb,lakin rahatciliq ucun istifade etmedim
    }
}

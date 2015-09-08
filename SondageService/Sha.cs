using System;
using System.Security.Cryptography;
using System.Text;

namespace USherbrooke.ServiceModel.Sondage
{
    class Sha
    {
        public static string HachPassword(string clearPassword)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(clearPassword);
            byte[] protectedBytes;
            SHA1 sha = new SHA1CryptoServiceProvider();
            // This is one implementation of the abstract class SHA1.
            protectedBytes = sha.ComputeHash(bytes);
            return Convert.ToBase64String(protectedBytes);
        }

        public static void ShowHachPassword(string clearPassword)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(clearPassword);
            byte[] protectedBytes;
            SHA1 sha = new SHA1CryptoServiceProvider();
            // This is one implementation of the abstract class SHA1.
            protectedBytes = sha.ComputeHash(bytes);
            Console.WriteLine(Convert.ToBase64String(protectedBytes));
        }

    }
}

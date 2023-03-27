using System.Security.Cryptography;

namespace Invoices_Manager_API.Security
{
    public class PasswordHasher
    {
        private const int SALTSIZE = 32;
        private const int ITERATIONS = 10000;
        private const int KEYSIZE = 32;

        public static string GenerateSalt()
        {
            byte[] salt = new byte[SALTSIZE];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }


        public static string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, ITERATIONS, HashAlgorithmName.SHA512))
            {
                byte[] hash = pbkdf2.GetBytes(KEYSIZE);
                byte[] hashBytes = new byte[SALTSIZE + KEYSIZE];
                Array.Copy(saltBytes, 0, hashBytes, 0, SALTSIZE);
                Array.Copy(hash, 0, hashBytes, SALTSIZE, KEYSIZE);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] saltBytes = new byte[SALTSIZE];
            Array.Copy(hashBytes, 0, saltBytes, 0, SALTSIZE);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, ITERATIONS, HashAlgorithmName.SHA512))
            {
                byte[] hash = pbkdf2.GetBytes(KEYSIZE);
                for (int i = 0; i < KEYSIZE; i++)
                {
                    if (hashBytes[i + SALTSIZE] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}

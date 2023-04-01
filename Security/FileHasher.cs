using System.IO;
using System.Security.Cryptography;

namespace Invoices_Manager_API.Security
{
    public class FileHasher
    {
        public static string? GetMd5Hash(string tempFilePath)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(tempFilePath);

            try
            {
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
            }
            catch
            {
                return null;
            }
        }
    }
}
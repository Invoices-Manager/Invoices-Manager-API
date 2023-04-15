using Invoices_Manager_API.Models;

namespace Invoices_Manager_API.Core
{
    internal class FileCore
    {
        private const string SYS_PATH = @"Data\";
        private static readonly string CACHE_PATH = Path.Combine(SYS_PATH, "Cache");
        private static readonly string USERDATA_PATH = Path.Combine(SYS_PATH, "Userdata");

        internal static void SaveInvoiceFile(string tempFilePath, UserModel user)
        {
            //get the path
            string filePath = Path.Combine(USERDATA_PATH, user.Username);

            //check if the Path exist
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //saves the file in the path
            File.Move(tempFilePath, Path.Combine(filePath, Path.GetFileName(tempFilePath)));
        }
    }
}
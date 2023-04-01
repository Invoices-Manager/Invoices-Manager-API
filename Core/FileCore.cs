using Invoices_Manager_API.Models;

namespace Invoices_Manager_API.Core
{
    internal class FileCore
    {
        private const string SYSPATH = @"Data\";
        internal static void SaveFile(string tempFilePath, UserModel user)
        {
            //get the path
            string filePath = Path.Combine(SYSPATH, user.Username);

            //check if the Path exist
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //saves the file in the path
            File.Move(tempFilePath, Path.Combine(filePath, Path.GetFileName(tempFilePath)));
        }
    }
}
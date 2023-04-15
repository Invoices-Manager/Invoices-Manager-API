using Invoices_Manager_API.Models;
using System.Reflection.Metadata;

namespace Invoices_Manager_API.Core
{
    internal class FileCore
    {
        private const string SYS_PATH = @"Data\";
        private static readonly string CACHE_PATH = Path.Combine(SYS_PATH, "Cache");
        private static readonly string USERDATA_PATH = Path.Combine(SYS_PATH, "Userdata");

        public static void MoveInvoiceFile_IntoUserFolder(string tempFilePath, string fileId, UserModel user)
        {
            //get the path
            string filePath = Path.Combine(USERDATA_PATH, user.Username);

            //check if the Path exist
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //saves the file in the path
            File.Move(tempFilePath, Path.Combine(filePath, fileId + ".pdf"));
        }

        public static FileInfo SaveInvoiceFile_IntoCacheFolder(string invoiceFileBase64)
        {
            //check if the cache Path exist
            if (!Directory.Exists(CACHE_PATH))
                Directory.CreateDirectory(CACHE_PATH);

            //get UUID
            var fileId = Guid.NewGuid().ToString();
            
            //save file in temp folder
            var tempFilePath = Path.Combine(CACHE_PATH, fileId + ".pdf");
            var bytes = Convert.FromBase64String(invoiceFileBase64);
            File.WriteAllBytes(tempFilePath, bytes);
            
            //Set TTL for the file (5 minutes)
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1 * 60 * 1000;
            timer.Elapsed += (sender, e) =>
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
                timer.Stop();
            };
            timer.Start();

            return new FileInfo(tempFilePath);
        }
    }
}
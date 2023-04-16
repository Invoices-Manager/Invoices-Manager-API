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

        public static void DeleteInvoiceFile_FromUserFolder(string fileID, UserModel user)
        {
            //get the path
            string filePath = Path.Combine(USERDATA_PATH, user.Username);

            //check if the Path exist
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //delete the file if it exist
            if (File.Exists(Path.Combine(filePath, fileID + ".pdf")))
                File.Delete(Path.Combine(filePath, fileID + ".pdf"));
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
            //The file will be deleted, if the file does not get moved into the user folder
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
        
        public static string GetInvoiceFileBase64(string fileID, UserModel user)
        {
            //get the path
            string filePath = Path.Combine(USERDATA_PATH, user.Username);

            //get the file
            var file = File.ReadAllBytes(Path.Combine(filePath, fileID + ".pdf"));

            //return the file
            return Convert.ToBase64String(file);
        }

        public static Task DeleteUserFolder(UserModel user)
        {
            //get the path
            string filePath = Path.Combine(USERDATA_PATH, user.Username);

            //check if the Path exist (expection handling)
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //delete the folder
            return Task.Run(() => Directory.Delete(filePath, true));
        }
    }
}
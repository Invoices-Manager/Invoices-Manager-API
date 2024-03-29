﻿namespace Invoices_Manager_API.Core
{
    public class FileCore
    {
        private const string SYS_PATH = @"Data\";
        private static readonly string CACHE_PATH = Path.Combine(SYS_PATH, "Cache");
        private static readonly string USERDATA_PATH = Path.Combine(SYS_PATH, "Userdata");

        
        public static void MoveInvoiceFile_IntoUserFolder(string tempFilePath, string fileId, UserModel user)
        {
            //get the path
            string filePath = Path.Combine(USERDATA_PATH, user.Username, "Invoices");

            //check if the Path exist
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //saves the file in the path
            File.Move(tempFilePath, Path.Combine(filePath, fileId));
        }

        public static void MoveBackUpFile_IntoUserFolder(string tempFilePath, string fileId, UserModel user)
        {
            //get the path
            string filePath = Path.Combine(USERDATA_PATH, user.Username, "Backups");

            //check if the Path exist
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //saves the file in the path
            File.Move(tempFilePath, Path.Combine(filePath, fileId + ".bkup"));
        }

        public static void DeleteInvoiceFile_FromUserFolder(string fileID, UserModel user)
        {
            //get the path
            string filePath = Path.Combine(USERDATA_PATH, user.Username, "Invoices");

            //check if the Path exist
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //delete the file if it exist
            if (File.Exists(Path.Combine(filePath, fileID)))
                File.Delete(Path.Combine(filePath, fileID));
        }

        public static void DeleteBackUpFile_FromUserFolder(string fileID, UserModel user)
        {
            //get the path
            string filePath = Path.Combine(USERDATA_PATH, user.Username, "Backups");

            //check if the Path exist
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //delete the file if it exist
            if (File.Exists(Path.Combine(filePath, fileID + ".bkup")))
                File.Delete(Path.Combine(filePath, fileID + ".bkup"));
        }

        public static FileInfo SaveInvoiceFile_IntoCacheFolder(string invoiceFileBase64)
        {
            //check if the cache Path exist
            if (!Directory.Exists(CACHE_PATH))
                Directory.CreateDirectory(CACHE_PATH);

            //get UUID
            var fileId = Guid.NewGuid().ToString();
            
            //save file in temp folder
            var tempFilePath = Path.Combine(CACHE_PATH, fileId);
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
            string filePath = Path.Combine(USERDATA_PATH, user.Username, "Invoices");

            //get the file
            var file = File.ReadAllBytes(Path.Combine(filePath, fileID));

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

        public static void ClearCacheFolder()
        {
            //check if the cache Path exist
            if (!Directory.Exists(CACHE_PATH))
                return;

            //delete the folder
            Directory.Delete(CACHE_PATH, true);
        }

        public static string GetBackUpFilePath(string fileID, UserModel user)
        {
            //get the path
            string filePath = Path.Combine(USERDATA_PATH, user.Username, "Backups");

            //check if the Path exist
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //return the file path
            return Path.Combine(filePath, fileID + ".bkup");
        }

        public static string GetUserCachePath(UserModel user, params string[] paths)
        {
            string[] allPaths = new string[2 + paths.Length];
            allPaths[0] = CACHE_PATH;
            allPaths[1] = user.Username;

            Array.Copy(paths, 0, allPaths, 2, paths.Length);

            string directoryPath = Path.Combine(allPaths);
            Directory.CreateDirectory(directoryPath);

            return directoryPath;
        }
        
        public static string GetUserDataPath(UserModel user, params string[] paths)
        {
            string[] allPaths = new string[2 + paths.Length];
            allPaths[0] = USERDATA_PATH;
            allPaths[1] = user.Username;

            Array.Copy(paths, 0, allPaths, 2, paths.Length);

            string directoryPath = Path.Combine(allPaths);
            Directory.CreateDirectory(directoryPath);

            return directoryPath;
        }
    }
}
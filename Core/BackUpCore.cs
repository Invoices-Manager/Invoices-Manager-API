using Newtonsoft.Json;
using System.IO.Compression;

namespace Invoices_Manager_API.Core
{
    public class BackUpCore : IAsyncDisposable
    {
        readonly UserModel _user;

        readonly string backUpPath;
        readonly string cacheFolderPath;


        public BackUpCore(UserModel user)
        {
            _user = user;

            string backUpName = $"{DateTime.Now.ToString("yyyy-MM-dd_HH-mm")}_BackUp_From_{_user.Username}.zip";
            backUpPath = Path.Combine(FileCore.GetUserDataPath(_user, "BackUps"), backUpName);
            cacheFolderPath = FileCore.GetUserCachePath(_user, "BackUps", backUpName.Split('.')[0]);
        }

        /*
             BackUp Struct:

             2024-01-07_13-44_BackUp_From_{user.Username}.zip
             |-- Notebooks.json
             |-- Invoices.json
             |-- Invoices
                 |-- *hashFromFile*.pdf
                 |-- *hashFromFile*.pdf
                 |-- *hashFromFile*.pdf
        */
        
        public Task GenerateBackUp()
        {
            //get data
            string userNotebookAsJson = JsonConvert.SerializeObject(_user.Notebook);
            string userInvoicesAsJson = JsonConvert.SerializeObject(_user.Invoices);
            List<string> filePaths = new List<string>();

            //save files to cache
            File.WriteAllText(Path.Combine(cacheFolderPath, "Notebooks.json"), userNotebookAsJson);
            File.WriteAllText(Path.Combine(cacheFolderPath, "Invoices.json"), userInvoicesAsJson);

            foreach (InvoiceModel invoice in _user.Invoices)
            {
                string filePath = Path.Combine(cacheFolderPath, $"{invoice.FileID}.pdf");
                byte[] fileBytes = Convert.FromBase64String(FileCore.GetInvoiceFileBase64(invoice.FileID, _user));

                File.WriteAllBytes(filePath, fileBytes);
                filePaths.Add(filePath);
            }
            
            //create zip
            using (ZipArchive zip = ZipFile.Open(backUpPath, ZipArchiveMode.Create))
            {
                //add files
                zip.CreateEntryFromFile(Path.Combine(cacheFolderPath, "Notebooks.json"), "Notebooks.json");
                zip.CreateEntryFromFile(Path.Combine(cacheFolderPath, "Invoices.json"), "Invoices.json");

                //add files to folders
                foreach (var filePath in filePaths)
                    zip.CreateEntryFromFile(filePath, $"Invoices/{Path.GetFileName(filePath)}");
            }

            return Task.CompletedTask;
        }

        public string GetBackUpBase64()
        {
            return Convert.ToBase64String(File.ReadAllBytes(backUpPath));
        }

        public async ValueTask DisposeAsync()
        {
            await Task.Run(() =>
            {
                Directory.Delete(FileCore.GetUserDataPath(_user, "BackUps"), true);
                Directory.Delete(FileCore.GetUserCachePath(_user, "BackUps"), true);
            });
        }
    }
}

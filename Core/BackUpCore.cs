using Invoices_Manager_API.Controllers.v01;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Invoices_Manager_API.Core
{
    public class BackUpCore
    {
        private static Queue<UserModel> backUpQueue = new Queue<UserModel>();
        private readonly ILogger<WorkerCore> _logger;
        private readonly DataBaseContext _db;
        private readonly CancellationToken _cancellationToken;

        public BackUpCore(ILogger<WorkerCore> logger, DataBaseContext db, CancellationToken cancellationToken)
        {
            _logger = logger;
            _db = db;
            _cancellationToken = cancellationToken;
        }

        //add the user to the queue
        public static int AddUserToQueue(UserModel user)
        {
            backUpQueue.Enqueue(user);
            return GetQueueCount();
        }

        //get the users current place in the queue
        public static int GetQueuePlace(UserModel user)
        {
            return backUpQueue.ToList().FindIndex(x => x.Id == user.Id) + 1;
        }

        //check if the user is already in the queue
        public static bool IsUserInQueue(UserModel user)
        {
            return backUpQueue.Any(x => x.Id == user.Id);
        }

        //get the queue count
        public static int GetQueueCount()
        {
            return backUpQueue.Count;
        }
        
        //init the worker
        public void InitBackUpWorker()
        {
            //create a new thread
            Thread thread = new Thread(BackUpWork);
            thread.IsBackground = true;
            thread.Start();
        }

        private async void BackUpWork()
        {
            try
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    var s = _db.Note.ToList();
                    await Task.Delay(5000, _cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Task was cancelled, exit gracefully
                _logger.LogInformation("Worker Task was cancelled.");
            }
        }
    }
}

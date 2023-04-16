using Invoices_Manager_API.Controllers.v01;
using System.Collections;

namespace Invoices_Manager_API.Core
{
    public class BackUpCore
    {
        private static Queue<UserModel> backUpQueue = new Queue<UserModel>();
        private readonly ILogger<BackUpController> _logger;
        private readonly DataBaseContext _db;

        public BackUpCore(ILogger<BackUpController> logger, DataBaseContext db)
        {
            _logger = logger;
            _db = db;
            InitWorker();
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
        public static void InitWorker()
        {
            //create a new thread
            Thread thread = new Thread(BackUpWork);
            thread.IsBackground = true;
            thread.Start();
        }

        private void BackUpWork()
        {
            while (true)
            {
                var ss = _db.Note.ToList();
            }
        }
    }
}

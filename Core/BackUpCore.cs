using System.Collections;

namespace Invoices_Manager_API.Core
{
    public class BackUpCore
    {
        private static Queue<UserModel> backUpQueue = new Queue<UserModel>();
        
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
    }
}

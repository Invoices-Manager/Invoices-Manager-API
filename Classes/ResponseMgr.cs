using System.Net.Http.Formatting;

namespace Invoices_Manager_API.Classes
{
    public class ResponseMgr
    {
        public static Object CreateResponse(int statusCode, Guid traceId, string message, params object[] args)
        {
            DateTime dateTime = DateTime.Now;

            return new
            {
                statusCode,
                traceId,
                dateTime,
                message,
                args
            };
        }
    }
}

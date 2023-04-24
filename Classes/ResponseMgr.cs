namespace Invoices_Manager_API.Classes
{
    public class ResponseMgr
    {
        public static Object CreateResponse(int statusCode, Guid traceId, string message, Dictionary<string, object>? args = null)
        {
            DateTime dateTime = DateTime.Now;

            if (args is null)
                args = new Dictionary<string, object>();

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

using Invoices_Manager_API.Classes;
using Microsoft.AspNetCore.Mvc;

namespace Invoices_Manager_API.Controllers.v01
{
    [ApiController]
    [TypeFilter(typeof(AuthFilter))]
    [Route("api/v01/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly DataBaseContext _db;

        public InvoiceController(ILogger<InvoiceController> logger, DataBaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        private async Task<UserModel?> GetCurrentUser()
        {
            // Get the bearer token from the header
            var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
            var user = await UserCore.GetCurrentUser(_db, bearerToken);
            return user;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();
            
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The user does not exist"));

            //get all invoices
            var invoices = user.Invoices.ToList();

            //return all invoices
            return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "All invoices", invoices));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();
            
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The user does not exist"));

            //check if there is an id
            if (id == 0 || id < 0)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The id is not valid", id));

            //get the invoice
            var invoice = user.Invoices.Find(x => x.Id == id);

            //check if the invoice exist
            if (invoice is null)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The invoice does not exist", id));

            //return the invoice
            object result = new { invoice, base64 = FileCore.GetInvoiceFileBase64(invoice.FileID, user) };
            return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "The invoice", result));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InvoiceWrapperModel wrapper)
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();
            
            InvoiceModel newInvoice = wrapper.NewInvoice;
            string invoiceFileBase64 = wrapper.InvoiceFileBase64;

            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The user does not exist"));

            //check if the file is there
            if (string.IsNullOrEmpty(invoiceFileBase64))
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The file is null"));

            //check if the invoice is null
            if (newInvoice == null)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The invoice is null"));

            //check if the invoice is valid
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The invoice is not valid", newInvoice));

            //check if the user already set an id
            if (newInvoice.Id != 0)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "You are not allowed to set the Id!", newInvoice.Id));

            //check if the user already set an file id
            if (!string.IsNullOrEmpty(newInvoice.FileID))
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "You are not allowed to set the FileID!", newInvoice.FileID));

            //check if the enums are valid
            if (!Enum.IsDefined(typeof(ImportanceStateEnum), newInvoice.ImportanceState))
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The ImportanceState enum is illegal", ImportanceState.GetEnums()));

            if (!Enum.IsDefined(typeof(MoneyStateEnum), newInvoice.MoneyState))
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The MoneyState enum is illegal", MoneyState.GetEnums()));

            if (!Enum.IsDefined(typeof(PaidStateEnum), newInvoice.PaidState))
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The PaidState enum is illegal", PaidState.GetEnums()));

            //save the Invoice into the cache folder
            FileInfo invoiceFileInfo = FileCore.SaveInvoiceFile_IntoCacheFolder(invoiceFileBase64);

            //check if the file is not bigger than 32MB
            if (invoiceFileInfo.Length > 32 * 1024 * 1024)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The file is too big (32 mb)"));

            //get the file id
            var fileId = Security.FileHasher.GetMd5Hash(invoiceFileInfo.FullName);

            //check if the file id already exist
            if (user.Invoices.Any(x => x.FileID == fileId))
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The file already exist"));

            //check if the file id is valid
            if (string.IsNullOrEmpty(fileId))
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The file id is not valid"));

            //move the file to the new path
            FileCore.MoveInvoiceFile_IntoUserFolder(invoiceFileInfo.FullName, fileId, user);

            //set the creation date
            newInvoice.CaptureDate = DateTime.Now;

            //set the file id
            newInvoice.FileID = fileId;

            //add the invoices to the db
            user.Invoices.Add(newInvoice);
            await _db.SaveChangesAsync();

            //return the invoices
            return CreatedAtAction(nameof(Get), new { id = newInvoice.Id }, newInvoice);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] InvoiceModel editInvoice)
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();
            
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The user does not exist"));

            //check if the invoice is null
            if (editInvoice == null)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The invoice is null"));

            //check if the user provide a id
            if (editInvoice.Id == 0)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "You are not allowed to set the Id!", editInvoice.Id));

            //check if the invoices is valid
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The invoice is not valid", editInvoice));

            //check if the invoices exist
            if (!user.Invoices.Any(x => x.Id == editInvoice.Id))
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The invoice does not exist", editInvoice.Id));

            //get the invoice id
            int index = user.Invoices.FindIndex(x => x.Id == editInvoice.Id);

            //create a new invoice and delete the old one
            editInvoice.Id = user.Invoices[index].Id;
            editInvoice.FileID = user.Invoices[index].FileID;
            editInvoice.CaptureDate = user.Invoices[index].CaptureDate;

            //remove the old invoice and add the new invoice
            _db.Invoice.Remove(user.Invoices[index]);
            user.Invoices.Add(editInvoice);

            //saves the edit to the db
            await _db.SaveChangesAsync();

            //return the invoices
            return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "The invoice was successfully edited", editInvoice));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();
            
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The user does not exist"));

            //check if there is an id
            if (id == 0 || id < 0)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The id is not valid", id));

            //check if the invoices exist
            if (!user.Invoices.Any(x => x.Id == id))
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The invoice does not exist", id));

            //get the invoices
            var invoices = user.Invoices.Find(x => x.Id == id);

            //check the invoice
            if (invoices is null)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The invoice is null", id));

            //remove the invoice
            _db.Invoice.Remove(invoices);

            //saves the changes to the db
            await _db.SaveChangesAsync();

            //remove the file
            FileCore.DeleteInvoiceFile_FromUserFolder(invoices.FileID, user);

            //return the invoice
            return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "The invoice was successfully deleted", invoices));
        }
    }
}

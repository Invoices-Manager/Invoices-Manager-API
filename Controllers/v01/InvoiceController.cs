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
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //get all invoices
            var invoices = user.Invoices.ToList();

            //return all invoices
            return Ok(invoices);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if there is an id
            if (id == 0 || id < 0)
                return BadRequest("The id is not valid");

            //check if the invoice exist
            if (!user.Invoices.Any(x => x.Id == id))
                return NotFound($"The invoice with the id '{id}' does not exist");

            //get the invoice
            var invoice = user.Invoices.Find(x => x.Id == id);

            //return the invoices
            object result;
            
            if (invoice is not null)
                result = new  { invoice = invoice, base64 = FileCore.GetInvoiceFileBase64(invoice.FileID, user) };
            else
                result = new { invoice = invoice};

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InvoiceWrapperModel wrapper)
        {
            InvoiceModel newInvoice = wrapper.NewInvoice;
            string invoiceFileBase64 = wrapper.InvoiceFileBase64;

            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if the file is there
            if (string.IsNullOrEmpty(invoiceFileBase64))
                return BadRequest("The file is null");

            //check if the invoice is null
            if (newInvoice == null)
                return BadRequest("The invoice is null");

            //check if the invoice is valid
            if (!ModelState.IsValid)
                return BadRequest("The invoice is not valid");

            //check if the user already set an id
            if (newInvoice.Id != 0)
                return BadRequest("You are not allowed to set the Id! You get one assigned");

            //check if the user already set an file id
            if (!string.IsNullOrEmpty(newInvoice.FileID))
                return BadRequest("You are not allowed to set the File Id!");

            //check if the enums are valid
            if (!Enum.IsDefined(typeof(ImportanceStateEnum), newInvoice.ImportanceState))
                return BadRequest("The ImportanceState enum is illegal");

            if (!Enum.IsDefined(typeof(MoneyStateEnum), newInvoice.MoneyState))
                return BadRequest("The MoneyState enum is illegal");

            if (!Enum.IsDefined(typeof(PaidStateEnum), newInvoice.PaidState))
                return BadRequest("The PaidState enum is illegal");

            //save the Invoice into the cache folder
            FileInfo invoiceFileInfo = FileCore.SaveInvoiceFile_IntoCacheFolder(invoiceFileBase64);

            //check if the file is not bigger than 32MB
            if (invoiceFileInfo.Length > 32 * 1024 * 1024)
                return BadRequest("The file is bigger than 32MB");

            //get the file id
            var fileId = Security.FileHasher.GetMd5Hash(invoiceFileInfo.FullName);

            //check if the file id already exist
            if (user.Invoices.Any(x => x.FileID == fileId))
                return BadRequest("The file already exist!");

            //check if the file id is valid
            if (string.IsNullOrEmpty(fileId))
                return BadRequest("The file is corrupt!");

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
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if the invoice is null
            if (editInvoice == null)
                return BadRequest("The invoice is null");

            //check if the user provide a id
            if (editInvoice.Id == 0)
                return BadRequest("You must provide an ID whose entry you want to edit");

            //check if the invoices is valid
            if (!ModelState.IsValid)
                return BadRequest("The invoice is not valid");

            //check if the invoices exist
            if (!user.Invoices.Any(x => x.Id == editInvoice.Id))
                return NotFound($"The invoice with the id '{editInvoice.Id}' does not exist");

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
            return Ok(editInvoice);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if there is an id
            if (id == 0 || id < 0)
                return BadRequest("The id is not valid");

            //check if the invoices exist
            if (!user.Invoices.Any(x => x.Id == id))
                return NotFound($"The invoice with the id '{id}' does not exist");

            //get the invoices
            var invoices = user.Invoices.Find(x => x.Id == id);

            //check the invoice
            if (invoices is null)
                return BadRequest("The invoices is null");

            //remove the invoice
            _db.Invoice.Remove(invoices);

            //saves the changes to the db
            await _db.SaveChangesAsync();

            //remove the file
            FileCore.DeleteInvoiceFile_FromUserFolder(invoices.FileID, user);

            //return the invoice
            return Ok(invoices);
        }
    }
}

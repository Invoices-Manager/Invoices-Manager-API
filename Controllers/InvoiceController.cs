using Invoices_Manager_API.Core;
using Invoices_Manager_API.Filters;
using Invoices_Manager_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Invoices_Manager_API.Controllers
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
            return Ok(invoice);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InvoiceModel newInvoice, IFormFile invoiceFile)
        {
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if the invoice is null
            if (newInvoice == null)
                return BadRequest("The invoice is null");

            //check if the invoice is valid
            if (!ModelState.IsValid)
                return BadRequest("The invoice is not valid");

            //check if the user already set an id
            if (newInvoice.Id != 0)
                return BadRequest("You are not allowed to set the Id! You get one assigned");

            //check if a file was uploaded
            if (invoiceFile == null || invoiceFile.Length == 0)
                return BadRequest("No file has been uploaded.");

            //check if the file is a pdf
            if (Path.GetExtension(invoiceFile.FileName) != ".pdf")
                return BadRequest("The uploaded file must be a PDF file.");

            //check if the file is larger than 32mb
            const int MAXFILESIZE = 32 * 1024 * 1024; // 32 MB
            if (invoiceFile.Length > MAXFILESIZE)
                return BadRequest($"The maximum file size is {MAXFILESIZE / 1024 / 1024} MB.");

            //save the file into the temp
            string tempFilePath = Path.GetTempFileName();
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
                invoiceFile.CopyTo(stream);

            //save the file to the file system
            await Task.Run(() => { FileCore.SaveFile(tempFilePath, user); });

            //get md5 hash from file and set it as file id
            string? md5Hash = Security.FileHasher.GetMd5Hash(tempFilePath);
            if (md5Hash is null)
                return BadRequest("Something is wrong with your file");
            newInvoice.FileID = md5Hash;

            //set the creation date
            newInvoice.CaptureDate = DateTime.Now;

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
            return Ok(user.Invoices[index]);
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

            //return the invoices
            return Ok(invoices);
        }
    }
}

using Invoices_Manager_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx;

namespace Invoices_Manager_API.Controllers
{
    [ApiController]
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

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            //get all invoices
            var invoices = await _db.Invoice.ToArrayAsync();

            //return all invoices
            return Ok(invoices);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            //check if there is an id
            if (id == 0 || id < 0)
                return BadRequest("The id is not valid");

            //check if the invoice exist
            if (!_db.Invoice.Any(x => x.Id == id))
                return NotFound($"The invoice with the id '{id}' does not exist");

            //get the invoice
            var invoice = await _db.Invoice.FirstOrDefaultAsync(x => x.Id == id);

            //return the invoice
            return Ok(invoice);
        }

        [HttpGet]
        [Route("GetFile")]
        public async Task<IActionResult> GetFile(int id)
        {
            return StatusCode(501, "Not Implemented");
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InvoiceModel newInvoice)
        {
            
            //check if the invoice is null
            if (newInvoice == null)
                return BadRequest("The invoice is NULL");

            //check if the invoice is valid
            if (!ModelState.IsValid)
                return BadRequest("The invoice is not valid");

            if (newInvoice.Id != 0)
                return BadRequest("You are not allowed to set the ID! You get one assigned");

            //add the invoice
            await _db.Invoice.AddAsync(newInvoice);

            //save the changes
            await _db.SaveChangesAsync();

            //return the invoice via get method
            return CreatedAtAction(nameof(Get), new { id = newInvoice.Id }, newInvoice);
        }
        
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] InvoiceModel editInvoice)
        {
            //check if the invoice is null
            if (editInvoice == null)
                return BadRequest("The invoice is NULL");

            //check if the invoice is valid
            if (!ModelState.IsValid)
                return BadRequest("The invoice is not valid");

            //check if the invoice exist
            if (!_db.Invoice.Any(x => x.Id == editInvoice.Id))
                return NotFound($"The invoice with the id '{editInvoice.Id}' does not exist");

            //update the invoice
            _db.Invoice.Update(editInvoice);

            //save the changes
            await _db.SaveChangesAsync();

            //return the invoice via get method
            return AcceptedAtAction(nameof(Get), new { id = editInvoice.Id }, editInvoice);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            //check if there is an id
            if (id == 0 || id < 0)
                return BadRequest("The id is not valid");

            //check if the invoice exist
            if (!_db.Invoice.Any(x => x.Id == id))
                return NotFound($"The invoice with the id '{id}' does not exist");

            //get the invoice
            var invoice = await _db.Invoice.FirstOrDefaultAsync(x => x.Id == id);

            //check if the invoice is null
            if (invoice == null)
                return BadRequest("The invoice is NULL");

            //delete the invoice
            _db.Invoice.Remove(invoice);

            //save the changes
            await _db.SaveChangesAsync();

            //return the invoice
            return Ok(invoice);
        }
    }
}

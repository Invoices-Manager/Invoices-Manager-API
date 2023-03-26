using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}

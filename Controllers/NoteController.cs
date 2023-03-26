using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Invoices_Manager_API.Controllers
{
    [ApiController]
    [Route("api/v01/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly ILogger<NoteController> _logger;
        private readonly DataBaseContext _db;

        public NoteController(ILogger<NoteController> logger, DataBaseContext db)
        {
            _logger = logger;
            _db = db;
        }
    }
}

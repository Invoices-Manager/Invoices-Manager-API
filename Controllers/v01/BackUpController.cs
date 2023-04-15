using Microsoft.AspNetCore.Mvc;

namespace Invoices_Manager_API.Controllers.v01
{
    [ApiController]
    [Route("api/v01/[controller]")]
    public class BackUpController : ControllerBase
    {
        private readonly ILogger<BackUpController> _logger;
        private readonly DataBaseContext _db;

        public BackUpController(ILogger<BackUpController> logger, DataBaseContext db)
        {
            _logger = logger;
            _db = db;
        }
    }
}

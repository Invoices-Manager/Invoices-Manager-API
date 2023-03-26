using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Invoices_Manager_API.Controllers
{
    [ApiController]
    [Route("api/v01/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly DataBaseContext _db;

        public UserController(ILogger<UserController> logger, DataBaseContext db)
        {
            _logger = logger;
            _db = db;
        }
    }
}

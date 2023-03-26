using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace Invoices_Manager_API.Controllers
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

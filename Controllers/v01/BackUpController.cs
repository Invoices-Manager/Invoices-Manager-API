namespace Invoices_Manager_API.Controllers.v01
{
    [ApiController]
    [TypeFilter(typeof(AuthFilter))]
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


        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();

            //get the user
            UserModel? user;
            using (var _uc = new UserCore(_db))
            {
                var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
                user = await _uc.GetCurrentUser(bearerToken, GetUserTypeEnum.Invoices);
                if (user is null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "An error occured while getting the user, faulty bearer token"));

                //check if there is an id
                if (id == 0 || id < 0)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The id is not valid", new Dictionary<string, object> { { "id", id } }));

           
            }
        }
    }
}

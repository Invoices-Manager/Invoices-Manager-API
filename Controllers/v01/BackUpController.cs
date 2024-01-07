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
        public async Task<IActionResult> Get()
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();

            //get the user
            UserModel? user;
            using (var _uc = new UserCore(_db))
            {
                var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
                user = await _uc.GetCurrentUser(bearerToken, GetUserTypeEnum.All);
                
                if (user is null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "An error occured while getting the user, faulty bearer token"));

                await using (BackUpCore _bc = new BackUpCore(user))
                {
                    await _bc.GenerateBackUp();
                    string backUpBase64 =  _bc.GetBackUpBase64();
                 
                    return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "BackUp generated", new Dictionary<string, object> { { "base64", backUpBase64 } }));
                }
            }
        }
    }
}
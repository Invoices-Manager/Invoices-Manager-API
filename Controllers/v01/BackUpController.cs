using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

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
            // not implemented yet
            return await Task.FromResult(new StatusCodeResult(501));

            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //get all back ups
            var backUps = user.BackUpInfos.ToList();

            //return all back ups
            return Ok(backUps);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            // not implemented yet
            return await Task.FromResult(new StatusCodeResult(501));

            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if there is an id
            if (id == 0 || id < 0)
                return BadRequest("The id is not valid");

            //get the back up
            var backUp = user.BackUpInfos.Find(x => x.Id == id);

            //check if the back up exist
            if (backUp is null)
                return NotFound($"The back up with the id '{id}' does not exist");

            //return the back up
            return Ok(backUp);
        }

        [HttpGet]
        [Route("Download")]
        public async Task<IActionResult> Download(int id)
        {
            // not implemented yet
            return await Task.FromResult(new StatusCodeResult(501));

            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if there is an id
            if (id == 0 || id < 0)
                return BadRequest("The id is not valid");

            //get the back up
            var backUp = user.BackUpInfos.Find(x => x.Id == id);

            //check if the back up exist
            if (backUp is null)
                return NotFound($"The back up with the id '{id}' does not exist");

            //get the file
            string filePath = FileCore.GetBackUpFilePath(backUp.FileID, user);

            //check if the file exist
            if (filePath is null)
                return NotFound($"The file with the id '{id}' does not exist");

            //open a filestream for the chunking
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "application/octet-stream")
            {
                FileDownloadName = Path.GetFileName(filePath),
                EnableRangeProcessing = true
            };
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload()
        {
            // not implemented yet
            return await Task.FromResult(new StatusCodeResult(501));
        }

        [HttpPost]
        [Route("GenerateBackUp")]
        public async Task<IActionResult> GenerateBackUp()
        {
            // not implemented yet
            return await Task.FromResult(new StatusCodeResult(501));

            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if the user is already in the queue
            if (BackUpCore.IsUserInQueue(user))
                return Conflict("You are already in the queue");

            int place = BackUpCore.AddUserToQueue(user);

            //return the back up
            return Ok($"You place in the Queue '{place}'");
        }

        [HttpGet]
        [Route("QueuePlace")]
        public async Task<IActionResult> QueuePlace()
        {
            // not implemented yet
            return await Task.FromResult(new StatusCodeResult(501));

            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if the user is already in the queue
            if (!BackUpCore.IsUserInQueue(user))
                return Conflict("You are not in the queue");

            int place = BackUpCore.GetQueuePlace(user);

            //return the back up
            return Ok($"You place in the Queue '{place}'");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            // not implemented yet
            return await Task.FromResult(new StatusCodeResult(501));

            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if there is an id
            if (id == 0 || id < 0)
                return BadRequest("The id is not valid");

            //get the back up
            var backUp = user.BackUpInfos.Find(x => x.Id == id);

            //check if the back up exist
            if (backUp is null)
                return NotFound($"The back up with the id '{id}' does not exist");

            //remove the back up from the db
            _db.BackUpInfo.Remove(backUp);

            //remove the back up from the user
            user.BackUpInfos.Remove(backUp);

            //save the changes
            await _db.SaveChangesAsync();

            //return the back up
            return Ok(backUp);
        }
    }
}

namespace Invoices_Manager_API.Controllers.v01
{
    [ApiController]
    [TypeFilter(typeof(AuthFilter))]
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

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();

            //get the user
            using (var _uc = new UserCore())
            {
                var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
                var user = await _uc.GetCurrentUser(_db, bearerToken, GetUserTypeEnum.Notes);
                if (user is null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "An error occured while getting the user, faulty bearer token"));


                if (user == null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The user does not exist"));

                //get all notes
                var notes = user.Notebook.ToList();


                //return all notes
                return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "All notes", new Dictionary<string, object> { { "notes", notes } }));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();

            //get the user
            using (var _uc = new UserCore())
            {
                var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
                var user = await _uc.GetCurrentUser(_db, bearerToken, GetUserTypeEnum.Notes);
                if (user is null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "An error occured while getting the user, faulty bearer token"));


                //check if there is an id
                if (id == 0 || id < 0)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The id is not valid", new Dictionary<string, object> { { "id", id } }));

                //get the note
                var note = user.Notebook.Find(x => x.Id == id);

                if (note == null)
                    return new NotFoundObjectResult(ResponseMgr.CreateResponse(404, traceId, "The note does not exist", new Dictionary<string, object> { { "id", id } }));

                //return the note
                return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, $"the note with the id: {id} "
                    , new Dictionary<string, object> {
                     { "note", note }
                    }));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] NoteModel newNote)
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();

            //get the user
            using (var _uc = new UserCore())
            {
                var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
                var user = await _uc.GetCurrentUser(_db, bearerToken, GetUserTypeEnum.Notes);
                if (user is null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "An error occured while getting the user, faulty bearer token"));


                //check if the note is null
                if (newNote == null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The note is null"));

                //check if the note is valid
                if (!ModelState.IsValid)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The note is not valid"));

                //check if the user already set an id
                if (newNote.Id != 0)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The note already have an id", new Dictionary<string, object> { { "id", newNote.Id } }));

                //set the creation and edit date
                newNote.CreationDate = DateTime.Now;
                newNote.LastEditDate = DateTime.Now;

                //add the note to the db
                user.Notebook.Add(newNote);
                await _db.SaveChangesAsync();

                //return the note
                return new CreatedAtActionResult("Get", "Note", new { id = newNote.Id }, ResponseMgr.CreateResponse(201, traceId, "Note created successfully", new Dictionary<string, object> { { "note", newNote } }));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] NoteModel note)
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();

            //get the user
            using (var _uc = new UserCore())
            {
                var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
                var user = await _uc.GetCurrentUser(_db, bearerToken, GetUserTypeEnum.Notes);
                if (user is null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "An error occured while getting the user, faulty bearer token"));


                //check if the note is null
                if (note == null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The note is null"));

                //check if the note is valid
                if (!ModelState.IsValid)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The note is not valid"));

                //check if the note exist
                if (!user.Notebook.Any(x => x.Id == note.Id))
                    return new NotFoundObjectResult(ResponseMgr.CreateResponse(404, traceId, "The note does not exist", new Dictionary<string, object> { { "id", note.Id } }));

                //update the note
                int index = user.Notebook.FindIndex(x => x.Id == note.Id);

                // Check if the model was found
                if (index >= 0)
                {
                    user.Notebook[index].Name = note.Name == null ? user.Notebook[index].Name : note.Name;
                    user.Notebook[index].Value = note.Value == null ? user.Notebook[index].Value : note.Value;
                    user.Notebook[index].LastEditDate = DateTime.Now;
                }

                //saves the edit to the db
                await _db.SaveChangesAsync();

                //return the note
                return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "The note was updated", new Dictionary<string, object> { { "note", user.Notebook[index] } }));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();

            //get the user
            using (var _uc = new UserCore())
            {
                var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
                var user = await _uc.GetCurrentUser(_db, bearerToken, GetUserTypeEnum.Notes);
                if (user is null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "An error occured while getting the user, faulty bearer token"));


                //check if there is an id
                if (id == 0 || id < 0)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The id is not valid", new Dictionary<string, object> { { "id", id } }));

                //check if the note exist
                if (!user.Notebook.Any(x => x.Id == id))
                    return new NotFoundObjectResult(ResponseMgr.CreateResponse(404, traceId, "The note does not exist", new Dictionary<string, object> { { "id", id } }));

                //get the note
                var note = user.Notebook.Find(x => x.Id == id);

                //check the note
                if (note is null)
                    return new NotFoundObjectResult(ResponseMgr.CreateResponse(404, traceId, "The note does not exist", new Dictionary<string, object> { { "id", id } }));

                //remove the note
                _db.Note.Remove(note);

                //saves the changes to the db
                await _db.SaveChangesAsync();

                //return the note
                return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "The note was deleted", new Dictionary<string, object> { { "note", note } }));
            }
        }
    }
}

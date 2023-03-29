using Invoices_Manager_API.Core;
using Invoices_Manager_API.Filters;
using Invoices_Manager_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Invoices_Manager_API.Controllers
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
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //get all notes
            var notes = user.Notebook.ToList();
            
            //return all notes
            return Ok(notes);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if there is an id
            if (id == 0 || id < 0)
                return BadRequest("The id is not valid");

            //check if the note exist
            if (!user.Notebook.Any(x => x.Id == id))
                return NotFound($"The note with the id '{id}' does not exist");

            //get the note
            var note = user.Notebook.Find(x => x.Id == id);

            //return the note
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] NoteModel newNote)
        {
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if the note is null
            if (newNote == null)
                return BadRequest("The note is null");

            //check if the note is valid
            if (!ModelState.IsValid)
                return BadRequest("The note is not valid");

            //check if the user already set an id
            if (newNote.Id != 0)
                return BadRequest("You are not allowed to set the Id! You get one assigned");

            //set the creation and edit date
            newNote.CreationDate = DateTime.Now;
            newNote.LastEditDate = DateTime.Now;
            
            //add the note to the db
            user.Notebook.Add(newNote);
            await _db.SaveChangesAsync();

            //return the note
            return Ok(newNote);
        }
        
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] NoteModel note)
        {
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if the note is null
            if (note == null)
                return BadRequest("The note is null");

            //check if the note is valid
            if (!ModelState.IsValid)
                return BadRequest("The note is not valid");

            //check if the note exist
            if (!user.Notebook.Any(x => x.Id == note.Id))
                return NotFound($"The note with the id '{note.Id}' does not exist");

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
            return Ok(user.Notebook[index]);
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            //get the user
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("The user is null");

            //check if there is an id
            if (id == 0 || id < 0)
                return BadRequest("The id is not valid");

            //check if the note exist
            if (!user.Notebook.Any(x => x.Id == id))
                return NotFound($"The note with the id '{id}' does not exist");

            //get the note
            var note = user.Notebook.Find(x => x.Id == id);

            //check the note
            if (note is null)
                return BadRequest("The note is null");

            //remove the note
            user.Notebook.Remove(note);

            //saves the changes to the db
            await _db.SaveChangesAsync();

            //return the note
            return Ok(note);
        }
    }
}

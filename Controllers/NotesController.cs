using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mongo.Data;
using Mongo.Dtos;
using Mongo.Models;
using MongoDB.Bson;
using Mongo.Helpers;

namespace Mongo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private readonly INoteRepository _noteRepository;

        public NotesController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Note>> Get()
        {
            return await _noteRepository.GetAllNotes(GetUserId());
        }

        // GET api/notes/5 - retrieves a specific note using either Id or InternalId (BSonId)
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var note = await _noteRepository.GetNote(GetUserId(), id);
            if(note == null) return NotFound();
            return Ok(note);
        }

        // POST api/notes - creates a new note
        [HttpPost]
        public void Post([FromBody] NoteCreateDto newNote)
        {
            _noteRepository.AddNote(new Note
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Body = newNote.Body,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                UserId = GetUserId()
            });
        }

        // PUT api/notes/5 - updates a specific note
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]string value)
        {
            _noteRepository.UpdateNote(GetUserId(), id, value);
        }

        // DELETE api/notes/5 - deletes a specific note
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _noteRepository.RemoveNote(GetUserId(), id);
        }

        private Guid GetUserId() => new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
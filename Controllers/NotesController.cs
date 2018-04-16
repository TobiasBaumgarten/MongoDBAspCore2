using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mongo.Data;
using Mongo.Dtos;
using Mongo.Models;
using MongoDB.Bson;

namespace Mongo.Controllers
{
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
            return await _noteRepository.GetAllNotes();
        }

        // GET api/notes/5 - retrieves a specific note using either Id or InternalId (BSonId)
        [HttpGet("{id}")]
        public async Task<Note> Get(string id)
        {
            return await _noteRepository.GetNote(id) ?? new Note();
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
                UserId = newNote.UserId
            });
        }

        // PUT api/notes/5 - updates a specific note
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]string value)
        {
            _noteRepository.UpdateNote(id, value);
        }

        // DELETE api/notes/5 - deletes a specific note
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _noteRepository.RemoveNote(id);
        }
    }
}
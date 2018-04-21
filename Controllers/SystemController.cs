using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Mongo.Data;
using Mongo.Models;
using MongoDB.Bson;

namespace Mongo.Controllers
{
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        private readonly INoteRepository _noteRepository;
        private readonly IHostingEnvironment env;

        public SystemController(INoteRepository noteRepository, IHostingEnvironment env)
        {
            this.env = env;
            _noteRepository = noteRepository;
        }

        // Call an initialization - api/system/init
        [HttpGet("{setting}")]
        public string Get(string setting)
        {
            if (!env.IsDevelopment())
            {
                return "This is only for Development";
            }
            if (setting == "init")
            {
                _noteRepository.RemoveAllNotes();
                var notes = new List<Note> 
                {
                    new Note()
                    {
                        Body = "Test note 1",
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        UserId = new Guid("2f577b77-72bd-4e6f-9507-b52970f37762")
                    },
                    new Note()
                    {
                        Body = "Test note 2",
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        UserId = new Guid("2f577b77-72bd-4e6f-9507-b52970f37762")
                    },
                    new Note()
                    {
                        Body = "Test note 3",
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        UserId = new Guid("2f577b77-72bd-4e6f-9507-b52970f37762")
                    },
                    new Note()
                    {
                        Body = "Other Users Note",
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        UserId = new Guid("2f577b77-72bd-4e6f-9507-b52970f37761")
                    }
                };
                notes.ForEach(n =>
                {
                    n.Id = ObjectId.GenerateNewId().ToString();
                    _noteRepository.AddNote(n);
                });
                return "Done";
            }

            return "Unknown";
        }
    }
}
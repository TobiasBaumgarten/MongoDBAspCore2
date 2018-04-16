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
                        UserId = 1
                    },
                    new Note()
                    {
                        Body = "Test note 2",
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        UserId = 1
                    },
                    new Note()
                    {
                        Body = "Test note 3",
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        UserId = 2
                    },
                    new Note()
                    {
                        Body = "Test note 4",
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        UserId = 2
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
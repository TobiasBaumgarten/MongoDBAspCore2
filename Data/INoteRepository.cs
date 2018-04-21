using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mongo.Models;

namespace Mongo.Data
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllNotes(Guid userId);
        Task<Note> GetNote(Guid userId, string id);

        // add new note document
        Task AddNote(Note item);

        // remove a single document / note
        Task<bool> RemoveNote(Guid userId, string id);

        // update just a single document / note
        Task<bool> UpdateNote(Guid userId, string id, string body);

        // demo interface - full document update
        Task<bool> UpdateNote(Guid userId, string id, Note item);

        // should be used with high cautious, only in relation with demo setup
        Task<bool> RemoveAllNotes(Guid userId);
        // Only for Development
        Task<bool> RemoveAllNotes();
    }
}
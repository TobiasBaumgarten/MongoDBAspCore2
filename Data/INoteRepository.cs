using System.Collections.Generic;
using System.Threading.Tasks;
using Mongo.Models;

namespace Mongo.Data
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllNotes();
        Task<Note> GetNote(string id);

        // add new note document
        Task AddNote(Note item);

        // remove a single document / note
        Task<bool> RemoveNote(string id);

        // update just a single document / note
        Task<bool> UpdateNote(string id, string body);

        // demo interface - full document update
        Task<bool> UpdateNote(string id, Note item);

        // should be used with high cautious, only in relation with demo setup
        Task<bool> RemoveAllNotes();
    }
}
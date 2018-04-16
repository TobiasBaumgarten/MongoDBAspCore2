using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.Data
{
    public class NoteRepository : INoteRepository
    {
        private readonly NoteContext _context = null;

        public NoteRepository(IOptions<MongoDbSettings> settings)
        {
            _context = new NoteContext(settings);
        }

        public async Task<IEnumerable<Note>> GetAllNotes()
        {
            return await _context.Notes
                    .Find(_ => true).ToListAsync();
        }

        // query after Id or InternalId (BSonId value)
        public async Task<Note> GetNote(string id)
        {
            // ObjectId internalId = GetInternalId(id);
            return await _context.Notes
                            .Find(note => note.Id == id)
                            .FirstOrDefaultAsync();
        }
        /* 
                private ObjectId GetInternalId(string id)
                {
                    ObjectId internalId;
                    if (!ObjectId.TryParse(id, out internalId))
                        internalId = ObjectId.Empty;

                    return internalId;
                } */

        public async Task AddNote(Note item)
        {
            try
            {
                await _context.Notes.InsertOneAsync(item);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<bool> RemoveNote(string id)
        {
                DeleteResult actionResult
                    = await _context.Notes.DeleteOneAsync(
                        Builders<Note>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
        }

        public async Task<bool> UpdateNote(string id, string body)
        {
            var filter = Builders<Note>.Filter.Eq(s => s.Id, id);
            var update = Builders<Note>.Update
                            .Set(s => s.Body, body)
                            .CurrentDate(s => s.UpdatedOn);

                UpdateResult actionResult
                    = await _context.Notes.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdateNote(string id, Note item)
        {
                ReplaceOneResult actionResult = await _context.Notes
                    .ReplaceOneAsync(n => n.Id.Equals(id)
                                        , item
                                        , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
        }

        public async Task<bool> RemoveAllNotes()
        {
                DeleteResult actionResult
                    = await _context.Notes.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
        }

    }
}
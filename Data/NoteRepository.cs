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
        private readonly MongoContext _context = null;

        public NoteRepository(IOptions<MongoDbSettings> settings)
        {
            _context = new MongoContext(settings);
        }

        public async Task<IEnumerable<Note>> GetAllNotes(Guid userId)
        {
            return await _context.Notes
                    .Find(n => n.UserId == userId).ToListAsync();
        }

        // query after Id or InternalId (BSonId value)
        public async Task<Note> GetNote(Guid userId, string id)
        {
            // ObjectId internalId = GetInternalId(id);
            return await _context.Notes
                            .Find(note => note.UserId == userId && note.Id == id)
                            .FirstOrDefaultAsync();
        }

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

        public async Task<bool> RemoveNote(Guid userId, string id)
        {
            DeleteResult actionResult = await _context.Notes.DeleteOneAsync
                (
                    Builders<Note>.Filter.Eq(n => n.UserId, userId) &
                    Builders<Note>.Filter.Eq(n => n.Id, id)
                );

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public async Task<bool> UpdateNote(Guid userId, string id, string body)
        {
            var filter = Builders<Note>.Filter.Eq(s => s.Id, id) & Builders<Note>.Filter.Eq(n => n.UserId, userId);
            var update = Builders<Note>.Update
                            .Set(s => s.Body, body)
                            .CurrentDate(s => s.UpdatedOn);

            UpdateResult actionResult
                = await _context.Notes.UpdateOneAsync(filter, update);

            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdateNote(Guid userId, string id, Note item)
        {
            var filter = Builders<Note>.Filter.Eq(s => s.Id, id) & Builders<Note>.Filter.Eq(n => n.UserId, userId);
            ReplaceOneResult result = await _context.Notes.ReplaceOneAsync(filter, item);
            return result.IsAcknowledged
                && result.ModifiedCount > 0;
        }

        public async Task<bool> RemoveAllNotes(Guid userId)
        {
            var filter = Builders<Note>.Filter.Eq(n => n.UserId, userId);
            DeleteResult actionResult
                = await _context.Notes.DeleteManyAsync(filter);

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
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
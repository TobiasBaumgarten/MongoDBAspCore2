namespace Mongo.Dtos
{
    public class NoteCreateDto
    {
        public string Id { get; set; }
        public string Body { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
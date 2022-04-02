using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace GameLobbySignalRTemplate.Shared.Models.Alias
{
    public class Prefix
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; } = null!;
    }
}

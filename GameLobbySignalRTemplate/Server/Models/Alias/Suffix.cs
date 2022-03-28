﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace GameLobbySignalRTemplate.Server.Models
{
    public class Suffix
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; } = null!;
    }
}

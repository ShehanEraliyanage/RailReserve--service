using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace RailReserve.Model
{
    [CollectionName("travelers")]
    public class Traveler : MongoIdentityUser<Guid>
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string NIC { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}

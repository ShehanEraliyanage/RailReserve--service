using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using ThirdParty.Json.LitJson;

namespace RailReserve.Model
{
    [CollectionName("travelers")]
    public class Traveler : MongoIdentityUser<Guid>
    {
       
        public string NIC { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}

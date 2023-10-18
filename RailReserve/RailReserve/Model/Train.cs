using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RailReserve.Model
{
    public class Train
    {
        [BsonId]
        public string? id { get; set; } 

        public string name {  get; set; }

        public string[] classes {  get; set; }

        public string seats {  get; set; }
    }
}

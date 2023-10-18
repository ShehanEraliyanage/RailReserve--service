using MongoDB.Bson.Serialization.Attributes;

namespace RailReserve.Model
{
    public class Schedule
    {
        [BsonId]
        public string? id { get; set; }

        public string trainId { get; set; }

        public string startingTime { get; set; }

        public string arrivalTime { get; set; }

        public string startingPlace { get; set; }

        public string destination { get; set; }

        public List<StopPlaceAndTime> stopPlaceAndTime { get; set; }

        public string price { get; set; }

    }
}

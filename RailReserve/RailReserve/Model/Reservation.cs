using MongoDB.Bson.Serialization.Attributes;

namespace RailReserve.Model
{
    public class Reservation
    {
        [BsonId]
        public string? id { get; set; }

        public string scheduleId { get; set; }

        public string travelerId { get; set; }

        public string bookingDate { get; set; }

        public string reservationDate { get; set; }

        public string noOfTickets { get; set; }

        public string paymentStatus { get; set; }

        public string bookingStatus { get; set; }
    }
}

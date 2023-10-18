namespace RailReserve.Configurations
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionTraveler {  get; set; } = null!;
        public string CollectionTrain {  get; set; } = null!;
        public string CollectionSchedule { get; set; } = null!;
        public string CollectionReservation { get; set; } = null!;
    }
}

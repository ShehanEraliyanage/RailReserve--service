using RailReserve.Dto;
using RailReserve.Model;

namespace RailReserve.Repository
{
    public interface IReservationService
    {
        public Task<ResponsData> GetAsync();

        public Task<ResponsData> GetAsync(string id);

        public Task<ResponsData> CreateAsync(Reservation reservation);

        public Task<ResponsData> UpdateAsync(Reservation reservation);

        public Task<ResponsData> Removeasync(string id);

        public  Task<ResponsData> GetPendingByTravelerIDAsync(string id);

        public Task<ResponsData> GetFineshByTravelerIDAsync(string id);

        public Task<ResponsData> GetByScheduleIDAsync(string id);
    }
}

using RailReserve.Dto;
using RailReserve.Model;

namespace RailReserve.Repository
{
    public interface IScheduleService
    {
        public Task<ResponsData> GetAsync();

        public Task<ResponsData> GetAsync(string id);

        public Task<ResponsData> CreateAsync(Schedule schedule);

        public Task<ResponsData> UpdateAsync(Schedule schedule);

        public Task<ResponsData> Removeasync(string id);

        public Task<ResponsData> GetSearchAsync(ScheduleSearchRequest schedule);
    }
}

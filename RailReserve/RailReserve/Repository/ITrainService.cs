using MongoDB.Driver;
using RailReserve.Dto;
using RailReserve.Model;

namespace RailReserve.Repository
{
    public interface ITrainService
    {
        public  Task<ResponsData> GetAsync();

        public Task<ResponsData> GetAsync(string id);

        public Task<ResponsData> CreateAsync(Train train);

        public Task<ResponsData> UpdateAsync(Train train);

        public Task<ResponsData> Removeasync(string id);

    }
}

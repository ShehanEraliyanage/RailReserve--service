using RailReserve.Dto;
using RailReserve.Dtos;

namespace RailReserve.Service
{
    public interface ITravelerService
    {
        public Task<TravelerRegisterResponse> RegisterAsync(TravelerRegisterRequest request);
        public Task<TravelerLoginResponse> LoginAsync(TravelerLoginRequest request);
        public Task<ResponsData> DeactiveAsync(String nic);
        public Task<ResponsData> ActiveAsync(String nic);
        public Task<ResponsData> GetAsync(string nic);
        public  Task<ResponsData> GetAsync();
    }
}

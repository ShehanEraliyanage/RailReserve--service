using RailReserve.Dtos;

namespace RailReserve.Service
{
    public interface ITravelerService
    {
        public Task<TravelerRegisterResponse> RegisterAsync(TravelerRegisterRequest request);
    }
}

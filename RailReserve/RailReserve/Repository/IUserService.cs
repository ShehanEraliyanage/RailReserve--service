using RailReserve.Dto;
using RailReserve.Dtos;

namespace RailReserve.Repository
{
    public interface IUserService
    {
        public Task<ResponsData> CreateAsync(CreateRoleRequest roleRequest);
        public Task<UserRegisterResponse> RegisterAsync(UserRegisterRequest request);
        public Task<UserLoginResponse> LoginAsync(UserLoginRequest request);
    }
}

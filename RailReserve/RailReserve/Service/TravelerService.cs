using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using RailReserve.Configurations;
using RailReserve.Dtos;
using RailReserve.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RailReserve.Service
{
    public class TravelerService : ITravelerService
    {

        private readonly UserManager<Traveler> _travelerManager;
        private readonly IMongoCollection<Traveler> _driverCollection;

        public TravelerService(UserManager<Traveler> userManager, IOptions<DatabaseSettings> databaseSettings)
        {
            _travelerManager = userManager;
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _driverCollection = mongoDb.GetCollection<Traveler>(databaseSettings.Value.CollectionTraveler);
        }

        public async Task<TravelerRegisterResponse> RegisterAsync(TravelerRegisterRequest request)
        {
            try
            {
                  var results = _driverCollection.Find(x=> (x.NIC == request.NIC)).FirstOrDefaultAsync();
                  if (results.Result != null) return new TravelerRegisterResponse { Success = false, Message = "NIC already exists" };

                Traveler traveler = new Traveler
                {
                    NIC = request.NIC,
                    FullName = request.FullName,
                    Status = "Active",
                    Email = request.Email,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    UserName = request.Username,
                    PhoneNumber = request.PhoneNumber

                };
                var createUserResult = await _travelerManager.CreateAsync(traveler, request.Password);
                if (!createUserResult.Succeeded) return new TravelerRegisterResponse { Message = $"Create user failed {createUserResult?.Errors?.First()?.Description}", Success = false };
                //user is created...
                //then add user to a role...
          
                //all is still well..
                return new TravelerRegisterResponse
                {
                    Success = true,
                    Message = "User registered successfully"
                };

            }
            catch (Exception ex)
            {
                return new TravelerRegisterResponse { Message = ex.Message, Success = false };
            }
        }



        public async Task<TravelerLoginResponse> LoginAsync(TravelerLoginRequest request)
        {
            try
            {

                var user = await _travelerManager.FindByEmailAsync(request.Email);
                if (user is null) return new TravelerLoginResponse {  Success = false, Message = "Invalid email/password" };

                //all is well if ew reach here
                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
                var roles = await _travelerManager.GetRolesAsync(user);
                var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
                claims.AddRange(roleClaims);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1swek3u4uo2u4a6e"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddMinutes(30);

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds

                    );

                return new TravelerLoginResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Message = "Login Successful",
                    Email = user?.Email,
                    Success = true,
                    UserId = user?.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new TravelerLoginResponse { Success = false, Message = ex.Message };
            }


        }

    }
}

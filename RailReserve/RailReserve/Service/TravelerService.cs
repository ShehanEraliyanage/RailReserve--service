using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbGenericRepository.Models;
using RailReserve.Dtos;
using RailReserve.Model;

namespace RailReserve.Service
{
    public class TravelerService : ITravelerService
    {

        private readonly UserManager<Traveler> _travelerManager;

        public TravelerService(UserManager<Traveler> userManager)
        {
            _travelerManager = userManager;
        }

        public async Task<TravelerRegisterResponse> RegisterAsync(TravelerRegisterRequest request)
        {
            try
            {

                MongoClient client = new MongoClient("mongodb+srv://hirusha:hirusha@e-commerce-system.zfvw1fj.mongodb.net/RailReserve?retryWrites=true&w=majority");
                var travelersCollection = client.GetDatabase("RailReserve").GetCollection<Traveler>("travelers");
                var filter = Builders<Traveler>.Filter.Eq("NIC", request.NIC);
                var results = await travelersCollection.Find(filter).Limit(1).SingleAsync(); ;
                if (results != null) return new TravelerRegisterResponse { Message = "NIC already exists", Success = false }
              
               
               // var userExists = await _travelerManager.FindByEmailAsync(request.Email);
               // if (userExists != null) return new TravelerRegisterResponse { Message = "User already exists", Success = false };

                //if we get here, no user with this email..

                Traveler userExists = new Traveler
                {
                    NIC = request.NIC,
                    FullName = request.FullName,
                    Email = request.Email,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    UserName = request.Username,
                    PhoneNumber = request.PhoneNumber

                };
                var createUserResult = await _travelerManager.CreateAsync(userExists, request.Password);
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

    }
}

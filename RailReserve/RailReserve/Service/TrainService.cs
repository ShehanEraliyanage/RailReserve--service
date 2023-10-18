using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RailReserve.Configurations;
using RailReserve.Dto;
using RailReserve.Model;
using RailReserve.Repository;

namespace RailReserve.Service
{
    public class TrainService : ITrainService
    {

        private readonly IMongoCollection<Train> _driverCollection;

        public TrainService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _driverCollection = mongoDb.GetCollection<Train>(databaseSettings.Value.CollectionTrain);
        }


        public async Task<ResponsData> GetAsync()
        {
            try
            {
                var result = await _driverCollection.Find(_ => true).ToListAsync();

                if (result.Count == 0) return new ResponsData
                {
                    Success = false,
                    Message = "No data",
                    Data = null
                };

                return new ResponsData { Success = true, Message = "Success", Data = result };

            }
            catch (Exception ex)
            {
                return new ResponsData
                {
                    Success = false,
                    Message = ex.ToString(),
                    Data = null
                };
            }
        }
        public async Task<ResponsData> GetAsync(string id)
        {
            try
            {
                if (id.Length > 5) return new ResponsData
                {
                    Success = false,
                    Message = "Invalid train id",
                    Data = null
                };

                var result = await _driverCollection.Find(x => x.id == id).FirstOrDefaultAsync();

                if (result is null) return new ResponsData
                {
                    Success = false,
                    Message = "No data",
                    Data = null
                };

                return new ResponsData
                {
                    Success = true,
                    Message = "Success",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponsData
                {
                    Success = false,
                    Message = ex.ToString(),
                    Data = null
                };
            }
        }
        public async Task<ResponsData> CreateAsync(Train train)
        {
            try
            {
                if (train.id.Equals("") || train.id.Equals(null)) return new ResponsData
                {
                    Success = false,
                    Message = "Train id should not be null or empty",
                    Data = null
                };

                if (train.id.Length > 5) return new ResponsData
                {
                    Success = false,
                    Message = "Invalid train id",
                    Data = null
                };

                var result = await GetAsync(train.id);
                if (result.Success.Equals(true)) return new ResponsData
                {
                    Success = false,
                    Message = "Train id already use",
                    Data = null
                };

                await _driverCollection.InsertOneAsync(train);

                return await GetAsync(train.id) ;
            }
            catch (Exception ex)
            {
                return new ResponsData
                {
                    Success = false,
                    Message = ex.ToString(),
                    Data = null
                };
            }
        }
        public async Task<ResponsData> UpdateAsync(Train train)
        {
            try
            {
                if (train.id.Equals("") || train.id.Equals(null)) return new ResponsData
                {
                    Success = false,
                    Message = "Train id should not be null or empty",
                    Data = null
                };

                if (train.id.Length > 5) return new ResponsData
                {
                    Success = false,
                    Message = "Invalid train id",
                    Data = null
                };

                var result = await this.GetAsync(train.id);
                if (result.Success.Equals(true))
                {
                    await _driverCollection.ReplaceOneAsync(x => x.id == train.id, train);
                }

                return await GetAsync(train.id);
            }
            catch (Exception ex)
            {
                return new ResponsData
                {
                    Success = false,
                    Message = ex.ToString(),
                    Data = null
                };
            }
        }
        public async Task<ResponsData> Removeasync(string id) {
            try
            {
                if (id.Equals("") || id.Equals(null)) return new ResponsData
                {
                    Success = false,
                    Message = "Train id should not be null or empty",
                    Data = null
                };

                if (id.Length > 5) return new ResponsData
                {
                    Success = false,
                    Message = "Invalid train id",
                    Data = null
                };

                var result = await this.GetAsync(id);

                if (result.Success.Equals(false))
                {
                    return result;
                }

                await _driverCollection.DeleteOneAsync(x => x.id == id);

                return new ResponsData
                {
                    Success = true,
                    Message = "Success",
                    Data = result.Data
                };
            }
            catch (Exception ex)
            {
                return new ResponsData
                {
                    Success = false,
                    Message = ex.ToString(),
                    Data = null
                };
            }
        }
            

    }
}

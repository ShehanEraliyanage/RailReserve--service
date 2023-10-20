using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RailReserve.Configurations;
using RailReserve.Dto;
using RailReserve.Model;
using RailReserve.Repository;

namespace RailReserve.Service
{
    public class ScheduleService : IScheduleService
    {
        private readonly IMongoCollection<Schedule> _driverCollection;

        public ScheduleService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _driverCollection = mongoDb.GetCollection<Schedule>(databaseSettings.Value.CollectionSchedule);
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
                    Message = "Invalid schedule id",
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


        public async Task<ResponsData> CreateAsync(Schedule schedule)
        {
            try
            {
                if (schedule.id.Equals("") || schedule.id.Equals(null)) return new ResponsData
                {
                    Success = false,
                    Message = "Schedule id should not be null or empty",
                    Data = null
                };

                if (schedule.id.Length > 5) return new ResponsData
                {
                    Success = false,
                    Message = "Invalid schedule id",
                    Data = null
                };

                var result = await GetAsync(schedule.id);
                if (result.Success.Equals(true)) return new ResponsData
                {
                    Success = false,
                    Message = "Schedule id already use",
                    Data = null
                };

                await _driverCollection.InsertOneAsync(schedule);

                return await GetAsync(schedule.id);
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


        public async Task<ResponsData> UpdateAsync(Schedule schedule)
        {
            try
            {
                if (schedule.id.Equals("") || schedule.id.Equals(null)) return new ResponsData
                {
                    Success = false,
                    Message = "Schedule id should not be null or empty",
                    Data = null
                };

                if (schedule.id.Length > 5) return new ResponsData
                {
                    Success = false,
                    Message = "Invalid schedule id",
                    Data = null
                };

                var result = await this.GetAsync(schedule.id);
                if (result.Success.Equals(true))
                {
                    await _driverCollection.ReplaceOneAsync(x => x.id == schedule.id, schedule);
                }

                return await GetAsync(schedule.id);
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
        public async Task<ResponsData> Removeasync(string id)
        {
            try
            {
                if (id.Equals("") || id.Equals(null)) return new ResponsData
                {
                    Success = false,
                    Message = "Schedule id should not be null or empty",
                    Data = null
                };

                if (id.Length > 5) return new ResponsData
                {
                    Success = false,
                    Message = "Invalid schedule id",
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


        public async Task<ResponsData> GetSearchAsync(ScheduleSearchRequest schedule)
        {
            try
            {

                var result = await _driverCollection.Find
                    (x => x.startingPlace == schedule.startingPlace && x.destination == schedule.destination).ToListAsync();
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



        //public async Task<List<Schedule>> GetAsync() =>
        //    await _driverCollection.Find(_ => true).ToListAsync();

        //public async Task<Schedule> GetAsync(string id) =>
        //    await _driverCollection.Find(x => x.id == id).FirstOrDefaultAsync();

        //public async Task CreateAsync(Schedule schedule) =>
        //    await _driverCollection.InsertOneAsync(schedule);

        //public async Task UpdateAsync(Schedule schedule) =>
        //    await _driverCollection.ReplaceOneAsync(x => x.id == schedule.id, schedule);

        //public async Task<DeleteResult> Removeasync(string id) =>
        //    await _driverCollection.DeleteOneAsync(x => x.id == id);


    }
}

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RailReserve.Configurations;
using RailReserve.Dto;
using RailReserve.Model;
using RailReserve.Repository;

namespace RailReserve.Service
{
    public class ReservationService : IReservationService
    {
        private readonly IMongoCollection<Reservation> _driverCollection;

        public ReservationService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _driverCollection = mongoDb.GetCollection<Reservation>(databaseSettings.Value.CollectionReservation);
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
                    Message = "Invalid reservation id",
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
        public async Task<ResponsData> CreateAsync(Reservation reservation)
        {
            try
            {
                if (reservation.id.Equals("") || reservation.id.Equals(null)) return new ResponsData
                {
                    Success = false,
                    Message = "Reservation id should not be null or empty",
                    Data = null
                };

                if (reservation.id.Length > 5) return new ResponsData
                {
                    Success = false,
                    Message = "Invalid reservation id",
                    Data = null
                };

                var result = await GetAsync(reservation.id);
                if (result.Success.Equals(true)) return new ResponsData
                {
                    Success = false,
                    Message = "Reservation id already use",
                    Data = null
                };

                await _driverCollection.InsertOneAsync(reservation);

                return await GetAsync(reservation.id);
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
        public async Task<ResponsData> UpdateAsync(Reservation reservation)
        {
            try
            {
                if (reservation.id.Equals("") || reservation.id.Equals(null)) return new ResponsData
                {
                    Success = false,
                    Message = "Reservation id should not be null or empty",
                    Data = null
                };

                if (reservation.id.Length > 5) return new ResponsData
                {
                    Success = false,
                    Message = "Invalid reservation id",
                    Data = null
                };

                var result = await this.GetAsync(reservation.id);
                if (result.Success.Equals(true))
                {
                    await _driverCollection.ReplaceOneAsync(x => x.id == reservation.id, reservation);
                }

                return await GetAsync(reservation.id);
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
                    Message = "Reservation id should not be null or empty",
                    Data = null
                };

                if (id.Length > 5) return new ResponsData
                {
                    Success = false,
                    Message = "Invalid reservation id",
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

        public async Task<ResponsData> GetByTravelerIDAsync(string id)
        {
            try
            {
                if (id.Length > 5) return new ResponsData
                {
                    Success = false,
                    Message = "Invalid traveler id",
                    Data = null
                };

                var result = await _driverCollection.Find(x => x.travelerId == id).FirstOrDefaultAsync();

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

        public async Task<ResponsData> GetByScheduleIDAsync(string id)
        {
            try
            {
                if (id.Length > 5) return new ResponsData
                {
                    Success = false,
                    Message = "Invalid schedule id",
                    Data = null
                };

                var result = await _driverCollection.Find(x => x.scheduleId == id).FirstOrDefaultAsync();

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
    }
}

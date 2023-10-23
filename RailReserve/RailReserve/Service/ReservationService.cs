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

                DateTime bookingDate = DateTime.Parse(reservation.bookingDate);
                DateTime reservationDate = DateTime.Parse(reservation.reservationDate);

                // Calculate the difference in days
                int daysDifference = (reservationDate - bookingDate).Days;

                // Check if the reservation is within 30 days from the booking date
                if (!(daysDifference >= 0 && daysDifference <= 30)) return new ResponsData
                {
                    Success = false,
                    Message = "Reservation date must be within 30 days from the booking date.",
                    Data = null
                };


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


                DateTime currentDate = DateTime.Now;
                var result = await this.GetAsync(reservation.id);

                if (result.Success.Equals(true))
                {
                    Reservation reservationCheck = (Reservation)result.Data;
                    DateTime reservationDate = DateTime.Parse(reservationCheck.reservationDate);
                    int daysDifference = (reservationDate - currentDate).Days;

                    if (daysDifference >= 5)
                    {
                        await _driverCollection.ReplaceOneAsync(x => x.id == reservation.id, reservation);
                    }
                    else
                    {
                        return new ResponsData
                        {
                            Success = false,
                            Message = "Reservation must be at least 5 days in advance from the current date.",
                            Data = null
                        };
                    }
                   
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
                DateTime currentDate = DateTime.Now;
                if (result.Success.Equals(false))
                {
                    return result;
                }
                else
                {
                    Reservation reservationCheck = (Reservation)result.Data;
                    DateTime reservationDate = DateTime.Parse(reservationCheck.reservationDate);
                    int daysDifference = (reservationDate - currentDate).Days;

                    if (daysDifference >= 5)
                    {
                        await _driverCollection.DeleteOneAsync(x => x.id == id);
                        return new ResponsData
                        {
                            Success = true,
                            Message = "Success",
                            Data = result.Data
                        };
                    }
                    else
                    {
                        return new ResponsData
                        {
                            Success = false,
                            Message = "Reservation must be at least 5 days in advance from the current date.",
                            Data = null
                        };
                    }
                }


              
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

        public async Task<ResponsData> GetPendingByTravelerIDAsync(string id)
        {
            try
            {
                var result = await _driverCollection.Find(x => x.travelerId == id && x.bookingStatus == "p").ToListAsync();

                if (result.Count == 0) return new ResponsData
                {
                    Success = false,
                    Message = "No reservation",
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



        public async Task<ResponsData> GetFineshByTravelerIDAsync(string id)
        {
            try
            {
                var result = await _driverCollection.Find(x => x.travelerId == id && x.bookingStatus == "f").ToListAsync();

                if (result.Count == 0) return new ResponsData
                {
                    Success = false,
                    Message = "No reservation",
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

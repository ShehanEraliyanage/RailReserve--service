using System.ComponentModel.DataAnnotations;

namespace RailReserve.Dto
{
    public class ScheduleSearchRequest
    {
        [Required]
        public string startingPlace { get; set; } = string.Empty;

        [Required]
        public string destination { get; set; } = string.Empty;
    }
}

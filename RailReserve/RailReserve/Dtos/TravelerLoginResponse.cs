namespace RailReserve.Dtos
{
    public class TravelerLoginResponse
    {
        public bool Success { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}

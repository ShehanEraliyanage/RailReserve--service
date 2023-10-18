namespace RailReserve.Dto
{
    public class ResponsData
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = false;
        public object? Data { get; set; } =  null;
    }
}

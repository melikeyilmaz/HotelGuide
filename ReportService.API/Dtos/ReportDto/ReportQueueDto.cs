namespace ReportService.API.Dtos.ReportDto
{
    public class ReportQueueDto
    {
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int ContactCount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; } 
    }
}

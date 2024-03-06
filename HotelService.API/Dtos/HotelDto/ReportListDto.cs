namespace HotelService.API.Dtos.HotelDto
{
    public class ReportListDto
    {
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int ContactCount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

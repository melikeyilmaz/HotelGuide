namespace HotelService.API.Dtos.HotelDto
{
    public class ResponsibilityAddDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public Guid HotelId { get; set; }
    }
}

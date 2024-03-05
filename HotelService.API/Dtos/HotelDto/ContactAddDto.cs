namespace HotelService.API.Dtos.HotelDto
{
    public class ContactAddDto
    {
        public Guid HotelId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
    }
}

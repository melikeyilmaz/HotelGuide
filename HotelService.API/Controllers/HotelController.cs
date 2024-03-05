using HotelService.API.Dtos.HotelDto;
using HotelService.Data.Contexts;
using HotelService.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly Context _context;
        public HotelController(Context context)
        {
            _context = context;
        }

        //Otel oluşturma
        [HttpPost("addhotel")]
        public async Task<ActionResult<HotelAddDto>> CreateHotel(HotelAddDto dto)
        {
                var hotel = new Hotel
                {
                    Name = dto.Name,

                };

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            return Ok("Otel oluşturma başarılı.");
        }

        //Otel kaldırma
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveHotel(Guid id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            _context.Hotels.Remove(hotel);

            await _context.SaveChangesAsync();

            return Ok("Otel başarıyla kaldırıldı.");
        }

        //Otel iletişim bilgisi ekleme
        [HttpPost("addcontact")]
        public async Task<ActionResult<ContactAddDto>> AddContact(ContactAddDto dto)
        {
            var hotel = await _context.Hotels.FindAsync(dto.HotelId);
            if (hotel == null)
            {
                return NotFound();
            }
            var contact = new Contact
            {

                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Location = dto.Location,
                HotelId = dto.HotelId
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return Ok("İletişim bilgisi başarıyla eklendi.");
        }

        //Otel iletişim bilgisi kaldırma
        [HttpDelete("removecontact/{id}")]
        public async Task<IActionResult> RemoveContact(Guid id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            _context.Contacts.Remove(contact);

            await _context.SaveChangesAsync();

            return Ok("İletişim bilgisi başarıyla kaldırıldı.");
        }

        //Otel yetkilisi eklenmesi
        [HttpPost("addresponsibility")]
        public async Task<ActionResult<ContactAddDto>> AddResponsibility(ResponsibilityAddDto dto)
        {
            var hotel = await _context.Hotels.FindAsync(dto.HotelId);
            if (hotel == null)
            {
                return NotFound();
            }
            var responsibility = new Responsibility
            {
                HotelId = dto.HotelId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Title = dto.Title
            };

            _context.Responsibilities.Add(responsibility);
            await _context.SaveChangesAsync();

            return Ok("Yetkili bilgisi başarıyla eklendi.");
        }


        //Otel yetkilisi silinmesi
        [HttpDelete("removeresponsibility/{id}")]
        public async Task<IActionResult> RemoveResponsibility(Guid id)
        {
            var responsibility = await _context.Responsibilities.FindAsync(id);
            if (responsibility == null)
            {
                return NotFound();
            }
            _context.Responsibilities.Remove(responsibility);

            await _context.SaveChangesAsync();

            return Ok("Sorumlu bilgisi başarıyla kaldırıldı.");
        }


        //Otel yetkililerinin listelenmesi
        [HttpGet("getresponsibilities")] 
        public async Task<ActionResult<IEnumerable<ResponsibilityListDto>>> GetResponsibilities(Guid hotelId)
        {
            var hotel = await _context.Hotels.Include(h => h.Responsibilities).FirstOrDefaultAsync(h => h.Id == hotelId);
            if (hotel == null)
            {
                return NotFound();
            }

            var responsibilities = hotel.Responsibilities.Select(r => new ResponsibilityListDto
            {
                Title = r.Title,
                FirstName = r.FirstName,
                LastName = r.LastName,

            }).ToList();

            return Ok(responsibilities);
        }

    }
}

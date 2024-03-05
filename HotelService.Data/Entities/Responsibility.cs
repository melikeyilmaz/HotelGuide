using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Data.Entities
{
    public class Responsibility
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }

    }
}

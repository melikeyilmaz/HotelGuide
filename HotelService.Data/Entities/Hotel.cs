using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Data.Entities
{
    public class Hotel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<Responsibility> Responsibilities { get; set; }
    }
}

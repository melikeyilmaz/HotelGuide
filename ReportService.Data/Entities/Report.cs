using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Data.Entities
{
    public class Report
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int PhoneCount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Today;

    }
}

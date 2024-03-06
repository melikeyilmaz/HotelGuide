using Microsoft.EntityFrameworkCore;
using ReportService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Data.Context
{
    public class ReportContext:DbContext
    {
        public DbSet<Report> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=HotelReportDb;User Id=postgres;Password=123456789;");
        }
    }
}

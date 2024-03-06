using HotelService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Data.Contexts
{
    public class Context:DbContext
    {
        //public Context(DbContextOptions<Context> options) : base(options)
        //{
        //}

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Responsibility> Responsibilities { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=HotelGuideDb;User Id=postgres;Password=123456789;");
        }
    }
}

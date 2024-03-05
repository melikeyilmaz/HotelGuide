using System;
using System.Threading.Tasks;
using HotelService.API.Controllers;
using HotelService.API.Dtos.HotelDto;
using HotelService.Data.Contexts;
using HotelService.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;


namespace HotelServiceAPI.UnitTest
{
    public class HotelControllerTests
    {
        private DbContextOptions<Context> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new Context(_options))
            {
                context.Database.EnsureCreated();
            }
        }

        [Test]
        public async Task CreateHotel_ReturnsOk()
        {
            // Arrange
            using (var context = new Context(_options))
            {
                var controller = new HotelController(context);
                var dto = new HotelAddDto { Name = "Test Hotel" };

                // Act
                var result = await controller.CreateHotel(dto);

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                Assert.AreEqual("Otel oluþturma baþarýlý.", ((OkObjectResult)result.Result).Value);

                // Verify that hotel is added to the database
                var hotelsCount = await context.Hotels.CountAsync();
                Assert.AreEqual(1, hotelsCount);
            }
        }


        [Test]
        public async Task RemoveHotel_Returns_OkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new Context(options))
            {
                var controller = new HotelController(context);

                var hotel = new Hotel { Id = Guid.NewGuid(), Name = "Test Hotel" };
                context.Hotels.Add(hotel);
                await context.SaveChangesAsync();

                // Act
                var result = await controller.RemoveHotel(hotel.Id);

                // Assert
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
            }
        }

    }
}
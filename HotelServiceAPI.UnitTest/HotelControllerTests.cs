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



        [Test]
        public async Task AddContact_Returns_OkResult_When_HotelExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new Context(options))
            {
                var controller = new HotelController(context);

                // Test için bir otel oluþturulur ve veritabanýna eklenir
                var hotel = new Hotel { Id = Guid.NewGuid(), Name = "Test Hotel" };
                context.Hotels.Add(hotel);
                await context.SaveChangesAsync();

                // Test için bir iletiþim bilgisi DTO'su oluþturulur
                var dto = new ContactAddDto
                {
                    HotelId = hotel.Id,
                    PhoneNumber = "1234567890",
                    Email = "test@example.com",
                    Location = "Test Location"
                };

                // Act
                // AddContact yöntemi çaðrýlýr
                var result = await controller.AddContact(dto);

                // Assert
                // Sonuç, HTTP 200 OK yanýtý döndürmeli
                Assert.That(result, Is.InstanceOf<ActionResult<ContactAddDto>>());
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            }
        }


        [Test]
        public async Task AddContact_Returns_NotFound_When_HotelDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new Context(options))
            {
                var controller = new HotelController(context);

                // Act
                // Test için olmayan bir otel kimliði belirlenir
                var dto = new ContactAddDto
                {
                    HotelId = Guid.NewGuid(), // Rastgele bir otel kimliði
                    PhoneNumber = "1234567890",
                    Email = "test@example.com",
                    Location = "Test Location"
                };

                // AddContact yöntemi çaðrýlýr
                var result = await controller.AddContact(dto);

                // Assert
                // Sonuç, HTTP 404 Not Found yanýtý döndürmeli
                Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
            }
        }


        [Test]
        public async Task RemoveContact_Returns_OkResult_When_ContactExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new Context(options))
            {
                var controller = new HotelController(context);

                // Test için bir iletiþim bilgisi oluþturulur ve veritabanýna eklenir
                var contact = new Contact
                {
                    Id = Guid.NewGuid(),
                    PhoneNumber = "1234567890",
                    Email = "test@example.com",
                    Location = "Test Location"
                };
                context.Contacts.Add(contact);
                await context.SaveChangesAsync();

                // Act
                // RemoveContact yöntemi çaðrýlýr
                var result = await controller.RemoveContact(contact.Id);

                // Assert
                // Sonuç, HTTP 200 OK yanýtý döndürmeli
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
            }
        }

        [Test]
        public async Task RemoveContact_Returns_NotFound_When_ContactDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new Context(options))
            {
                var controller = new HotelController(context);

                // Act
                // Test için olmayan bir iletiþim bilgisi kimliði belirlenir
                var contactId = Guid.NewGuid(); // Rastgele bir iletiþim bilgisi kimliði

                // RemoveContact yöntemi çaðrýlýr
                var result = await controller.RemoveContact(contactId);

                // Assert
                // Sonuç, HTTP 404 Not Found yanýtý döndürmeli
                Assert.That(result, Is.InstanceOf<NotFoundResult>());
            }
        }


        [Test]
        public async Task AddResponsibility_Returns_OkResult_When_HotelExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new Context(options))
            {
                var controller = new HotelController(context);

                // Test için bir otel oluþturulur ve veritabanýna eklenir
                var hotel = new Hotel { Id = Guid.NewGuid(), Name = "Test Hotel" };
                context.Hotels.Add(hotel);
                await context.SaveChangesAsync();

                // Test için bir yetkili bilgisi DTO'su oluþturulur
                var dto = new ResponsibilityAddDto
                {
                    HotelId = hotel.Id,
                    FirstName = "John",
                    LastName = "Doe",
                    Title = "Manager"
                };

                // Act
                // AddResponsibility yöntemi çaðrýlýr
                var result = await controller.AddResponsibility(dto);

                // Assert
                // Sonuç, HTTP 200 OK yanýtý döndürmeli
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            }
        }



        [Test]
        public async Task AddResponsibility_Returns_NotFound_When_HotelDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new Context(options))
            {
                var controller = new HotelController(context);

                // Act
                // Test için olmayan bir otel kimliði belirlenir
                var dto = new ResponsibilityAddDto
                {
                    HotelId = Guid.NewGuid(), // Rastgele bir otel kimliði
                    FirstName = "John",
                    LastName = "Doe",
                    Title = "Manager"
                };

                // AddResponsibility yöntemi çaðrýlýr
                var result = await controller.AddResponsibility(dto);

                // Assert
                // Sonuç, HTTP 404 Not Found yanýtý döndürmeli
                Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
            }
        }



        [Test]
        public async Task RemoveResponsibility_Returns_OkResult_When_ResponsibilityExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new Context(options))
            {
                var controller = new HotelController(context);

                // Test için bir yetkili bilgisi oluþturulur ve veritabanýna eklenir
                var responsibility = new Responsibility { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Title = "Manager" };
                context.Responsibilities.Add(responsibility);
                await context.SaveChangesAsync();

                // Act
                // RemoveResponsibility yöntemi çaðrýlýr
                var result = await controller.RemoveResponsibility(responsibility.Id);

                // Assert
                // Sonuç, HTTP 200 OK yanýtý döndürmeli
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
            }
        }

        [Test]
        public async Task RemoveResponsibility_Returns_NotFound_When_ResponsibilityDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new Context(options))
            {
                var controller = new HotelController(context);

                // Act
                // Test için olmayan bir yetkili bilgisi kimliði belirlenir
                var responsibilityId = Guid.NewGuid(); // Rastgele bir sorumlu bilgisi kimliði

                // RemoveResponsibility yöntemi çaðrýlýr
                var result = await controller.RemoveResponsibility(responsibilityId);

                // Assert
                // Sonuç, HTTP 404 Not Found yanýtý döndürmeli
                Assert.That(result, Is.InstanceOf<NotFoundResult>());
            }
        }




























    }
}
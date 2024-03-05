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
                Assert.AreEqual("Otel olu�turma ba�ar�l�.", ((OkObjectResult)result.Result).Value);

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

                // Test i�in bir otel olu�turulur ve veritaban�na eklenir
                var hotel = new Hotel { Id = Guid.NewGuid(), Name = "Test Hotel" };
                context.Hotels.Add(hotel);
                await context.SaveChangesAsync();

                // Test i�in bir ileti�im bilgisi DTO'su olu�turulur
                var dto = new ContactAddDto
                {
                    HotelId = hotel.Id,
                    PhoneNumber = "1234567890",
                    Email = "test@example.com",
                    Location = "Test Location"
                };

                // Act
                // AddContact y�ntemi �a�r�l�r
                var result = await controller.AddContact(dto);

                // Assert
                // Sonu�, HTTP 200 OK yan�t� d�nd�rmeli
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
                // Test i�in olmayan bir otel kimli�i belirlenir
                var dto = new ContactAddDto
                {
                    HotelId = Guid.NewGuid(), // Rastgele bir otel kimli�i
                    PhoneNumber = "1234567890",
                    Email = "test@example.com",
                    Location = "Test Location"
                };

                // AddContact y�ntemi �a�r�l�r
                var result = await controller.AddContact(dto);

                // Assert
                // Sonu�, HTTP 404 Not Found yan�t� d�nd�rmeli
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

                // Test i�in bir ileti�im bilgisi olu�turulur ve veritaban�na eklenir
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
                // RemoveContact y�ntemi �a�r�l�r
                var result = await controller.RemoveContact(contact.Id);

                // Assert
                // Sonu�, HTTP 200 OK yan�t� d�nd�rmeli
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
                // Test i�in olmayan bir ileti�im bilgisi kimli�i belirlenir
                var contactId = Guid.NewGuid(); // Rastgele bir ileti�im bilgisi kimli�i

                // RemoveContact y�ntemi �a�r�l�r
                var result = await controller.RemoveContact(contactId);

                // Assert
                // Sonu�, HTTP 404 Not Found yan�t� d�nd�rmeli
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

                // Test i�in bir otel olu�turulur ve veritaban�na eklenir
                var hotel = new Hotel { Id = Guid.NewGuid(), Name = "Test Hotel" };
                context.Hotels.Add(hotel);
                await context.SaveChangesAsync();

                // Test i�in bir yetkili bilgisi DTO'su olu�turulur
                var dto = new ResponsibilityAddDto
                {
                    HotelId = hotel.Id,
                    FirstName = "John",
                    LastName = "Doe",
                    Title = "Manager"
                };

                // Act
                // AddResponsibility y�ntemi �a�r�l�r
                var result = await controller.AddResponsibility(dto);

                // Assert
                // Sonu�, HTTP 200 OK yan�t� d�nd�rmeli
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
                // Test i�in olmayan bir otel kimli�i belirlenir
                var dto = new ResponsibilityAddDto
                {
                    HotelId = Guid.NewGuid(), // Rastgele bir otel kimli�i
                    FirstName = "John",
                    LastName = "Doe",
                    Title = "Manager"
                };

                // AddResponsibility y�ntemi �a�r�l�r
                var result = await controller.AddResponsibility(dto);

                // Assert
                // Sonu�, HTTP 404 Not Found yan�t� d�nd�rmeli
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

                // Test i�in bir yetkili bilgisi olu�turulur ve veritaban�na eklenir
                var responsibility = new Responsibility { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Title = "Manager" };
                context.Responsibilities.Add(responsibility);
                await context.SaveChangesAsync();

                // Act
                // RemoveResponsibility y�ntemi �a�r�l�r
                var result = await controller.RemoveResponsibility(responsibility.Id);

                // Assert
                // Sonu�, HTTP 200 OK yan�t� d�nd�rmeli
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
                // Test i�in olmayan bir yetkili bilgisi kimli�i belirlenir
                var responsibilityId = Guid.NewGuid(); // Rastgele bir sorumlu bilgisi kimli�i

                // RemoveResponsibility y�ntemi �a�r�l�r
                var result = await controller.RemoveResponsibility(responsibilityId);

                // Assert
                // Sonu�, HTTP 404 Not Found yan�t� d�nd�rmeli
                Assert.That(result, Is.InstanceOf<NotFoundResult>());
            }
        }




























    }
}
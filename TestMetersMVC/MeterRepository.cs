using AutoMapper;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MetersMVC.Tests.Repositories
{
    public class MeterRepositoryTests
    {
        private readonly Mock<MeterDbContext> _contextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<MeterRepository>> _loggerMock;
        private readonly MeterRepository _repository;

        public MeterRepositoryTests()
        {
            _contextMock = new Mock<MeterDbContext>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<MeterRepository>>();
            _repository = new MeterRepository(_contextMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllMeters_ReturnsListOfMappedMeters()
        {
            // Arrange
            var meterEntities = new List<MeterEntity>
            {
                new MeterEntity { Id = 1, Type = "Electric" },
                new MeterEntity { Id = 2, Type = "Water" }
            };

            var mappedMeters = new List<Meter>
            {
                new Meter { Id = 1, Type = "Electric" },
                new Meter { Id = 2, Type = "Water" }
            };

            var dbSetMock = new Mock<DbSet<MeterEntity>>();
            dbSetMock.As<IAsyncEnumerable<MeterEntity>>()
                     .Setup(m => m.GetAsyncEnumerator(default))
                     .Returns(new TestAsyncEnumerator<MeterEntity>(meterEntities.GetEnumerator()));
            dbSetMock.As<IQueryable<MeterEntity>>().Setup(m => m.Provider).Returns(meterEntities.AsQueryable().Provider);
            dbSetMock.As<IQueryable<MeterEntity>>().Setup(m => m.Expression).Returns(meterEntities.AsQueryable().Expression);
            dbSetMock.As<IQueryable<MeterEntity>>().Setup(m => m.ElementType).Returns(meterEntities.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<MeterEntity>>().Setup(m => m.GetEnumerator()).Returns(meterEntities.GetEnumerator());

            _contextMock.Setup(c => c.Meters).Returns(dbSetMock.Object);
            _mapperMock.Setup(m => m.Map<IEnumerable<Meter>>(meterEntities)).Returns(mappedMeters);

            // Act
            var result = await _repository.GetAllMeters();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Electric", result.First().Type);
            Assert.Equal("Water", result.Skip(1).First().Type);
        }

        [Fact]
        public async Task GetAllMeters_WhenDbContextThrowsException_LogsErrorAndThrows()
        {
            // Arrange
            _contextMock.Setup(c => c.Meters).Throws(new System.Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => _repository.GetAllMeters());
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while fetching all meters.")),
                    It.IsAny<System.Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

    }
}

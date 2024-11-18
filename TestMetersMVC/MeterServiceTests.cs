using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business;
using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MetersMVC
{
    public class MeterServiceTests
    {
        private readonly Mock<IMeterRepository> _meterRepositoryMock;
        private readonly Mock<ILogger<MeterService>> _loggerMock;
        private readonly MeterService _meterService;

        public MeterServiceTests()
        {
            _meterRepositoryMock = new Mock<IMeterRepository>();
            _loggerMock = new Mock<ILogger<MeterService>>();

            _meterService = new MeterService(_meterRepositoryMock.Object, null);
        }

        [Fact]
        public async Task GetAllMeters_ReturnsAllMeters()
        {
            // Arrange
            var expectedMeters = new List<Meter>
            {
                new Meter { Id = 1, Type = "Electric" },
                new Meter { Id = 2, Type = "Water" }
            };
            _meterRepositoryMock.Setup(repo => repo.GetAllMeters()).ReturnsAsync(expectedMeters);

            // Act
            var result = await _meterService.GetAllMeters();

            // Assert
            Assert.Equal(expectedMeters, result);
            _meterRepositoryMock.Verify(repo => repo.GetAllMeters(), Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Fetching all meters")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetMeterById_ReturnsMeter_WhenMeterExists()
        {
            // Arrange
            var meter = new Meter { Id = 1, Type = "Electric" };
            _meterRepositoryMock.Setup(repo => repo.GetMeterById(1)).ReturnsAsync(meter);

            // Act
            var result = await _meterService.GetMeterById(1);

            // Assert
            Assert.Equal(meter, result);
            _meterRepositoryMock.Verify(repo => repo.GetMeterById(1), Times.Once);
        }

        [Fact]
        public async Task GetMeterById_ReturnsNull_WhenMeterDoesNotExist()
        {
            // Arrange
            _meterRepositoryMock.Setup(repo => repo.GetMeterById(It.IsAny<int>())).ReturnsAsync((Meter)null);

            // Act
            var result = await _meterService.GetMeterById(1);

            // Assert
            Assert.Null(result);
            _meterRepositoryMock.Verify(repo => repo.GetMeterById(1), Times.Once);
        }

        [Fact]
        public async Task AddMeter_CallsRepositoryAndLogs()
        {
            // Arrange
            var meter = new Meter { Id = 1, Type = "Electric" };

            
            await _meterService.AddMeter(meter);

          
            _meterRepositoryMock.Verify(repo => repo.AddMeter(meter), Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Adding meter")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateMeter_CallsRepository()
        {
            // Arrange
            var meter = new Meter { Id = 1, Type = "Electric" };

            // Act
            await _meterService.UpdateMeter(meter);

            // Assert
            _meterRepositoryMock.Verify(repo => repo.UpdateMeter(meter), Times.Once);
        }

        [Fact]
        public async Task DeleteMeter_CallsRepository()
        {
            // Arrange
            int meterId = 1;

            // Act
            await _meterService.DeleteMeter(meterId);

            // Assert
            _meterRepositoryMock.Verify(repo => repo.DeleteMeter(meterId), Times.Once);
        }
    }
}

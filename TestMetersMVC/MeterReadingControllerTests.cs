using AutoMapper;
using Core.Interfaces;
using Core.Models;
using MetersMVC.Controllers;
using MetersMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace MetersMVC
{
    public class MeterReadingControllerTests
    {
        private readonly MeterReadingController _controller;
        private readonly Mock<IMeterReadingService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<MeterReadingController>> _mockLogger;

        public MeterReadingControllerTests()
        {
            _mockService = new Mock<IMeterReadingService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<MeterReadingController>>();

            _controller = new MeterReadingController(_mockService.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfMeterReadings()
        {
            // Arrange
            var meterReadings = new List<MeterReading>
            {
                new MeterReading { Id = 1, MeterId = 1, Reading = "100", ReadingDate = DateTime.Now },
                new MeterReading { Id = 2, MeterId = 1, Reading = "150", ReadingDate= DateTime.Now }
            };
            var meterReadingViewModels = new List<MeterReadingViewModel>
            {
                new MeterReadingViewModel { Id = 1, MeterId = 1, Reading = "100", ReadingDate= DateTime.Now },
                new MeterReadingViewModel { Id = 2, MeterId = 2, Reading = "100", ReadingDate = DateTime.Now }
            };

            _mockService.Setup(s => s.GetAllMeterReadings()).ReturnsAsync(meterReadings);
            _mockMapper.Setup(m => m.Map<List<MeterReadingViewModel>>(meterReadings)).Returns(meterReadingViewModels);

            
            var result = await _controller.Index();

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<MeterReadingViewModel>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenReadingDoesNotExist()
        {
            
            int meterReadingId = 1;
            _mockService.Setup(s => s.GetMeterReadingById(meterReadingId)).ReturnsAsync((MeterReading)null);

            
            var result = await _controller.Details(meterReadingId);

            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WithNewMeterReadingViewModel()
        {
         
            int meterId = 1;

            
            var result = _controller.Create(meterId);

           
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<MeterReadingViewModel>(viewResult.Model);
            Assert.Equal(meterId, model.MeterId);
        }

        [Fact]
        public async Task Create_Post_ReturnsRedirectToAction_WhenModelStateIsValid()
        {
           
            var readingViewModel = new MeterReadingViewModel { Id = 1, MeterId = 1, Reading = "200", ReadingDate = DateTime.Now };
            var meterReading = new MeterReading { Id = 1, MeterId = 1, Reading = "200", ReadingDate = DateTime.Now };

            _mockMapper.Setup(m => m.Map<MeterReading>(readingViewModel)).Returns(meterReading);
            _mockService.Setup(s => s.AddMeterReading(meterReading)).Returns(Task.CompletedTask);

          
            var result = await _controller.Create(readingViewModel);

           
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Create_Post_ReturnsViewResult_WithModelStateErrors_WhenModelIsInvalid()
        {
            
            var readingViewModel = new MeterReadingViewModel { MeterId = 1 }; 
            _controller.ModelState.AddModelError("Value", "Required");

            
            var result = await _controller.Create(readingViewModel);

           
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(readingViewModel, viewResult.Model);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenReadingDoesNotExist()
        {
          
            int meterReadingId = 1;
            _mockService.Setup(s => s.GetMeterReadingById(meterReadingId)).ReturnsAsync((MeterReading)null);

            
            var result = await _controller.Edit(meterReadingId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenReadingDoesNotExist()
        {
           
            int meterReadingId = 1;
            _mockService.Setup(s => s.GetMeterReadingById(meterReadingId)).ReturnsAsync((MeterReading)null);

            
            var result = await _controller.Delete(meterReadingId);

            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToAction_WhenDeletionSucceeds()
        {
            
            int meterReadingId = 1;
            _mockService.Setup(s => s.DeleteMeterReading(meterReadingId)).Returns(Task.CompletedTask);

           
            var result = await _controller.DeleteConfirmed(meterReadingId);

            
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}

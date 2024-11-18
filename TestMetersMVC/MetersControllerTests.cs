using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MetersMVC.Controllers;
using MetersMVC.ViewModels;
using Moq;

namespace MetersMVC
{
    public class MetersControllerTests
    {
        private readonly Mock<IMeterService> _meterServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<MetersController>> _loggerMock;
        private readonly MetersController _controller;

        public MetersControllerTests()
        {
            _meterServiceMock = new Mock<IMeterService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<MetersController>>();
            _controller = new MetersController(_meterServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithMeters()
        {
         
            var meters = new List<Meter> { new Meter { Id = 1, Type = "Electric" } };
            var meterViewModels = new List<MeterViewModel> { new MeterViewModel { Id = 1, Type = "Electric" } };
            _meterServiceMock.Setup(service => service.GetAllMeters()).ReturnsAsync(meters);
            _mapperMock.Setup(m => m.Map<List<MeterViewModel>>(meters)).Returns(meterViewModels);

            var result = await _controller.Index();

          
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<MeterViewModel>>(viewResult.Model);
            Assert.Equal(1, model.Count);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenMeterDoesNotExist()
        {
            
            int meterId = 1;
            _meterServiceMock.Setup(service => service.GetMeterById(meterId)).ReturnsAsync((Meter)null);

           
            var result = await _controller.Details(meterId);

            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidModel_RedirectsToIndex()
        {
            
            var meterViewModel = new MeterViewModel { Id = 1, Type = "Electric" };
            var meter = new Meter { Id = 1, Type = "Electric" };
            _mapperMock.Setup(m => m.Map<Meter>(meterViewModel)).Returns(meter);
            _meterServiceMock.Setup(service => service.AddMeter(meter)).Returns(Task.CompletedTask);

          
            var result = await _controller.Create(meterViewModel);

            
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            
            var meterViewModel = new MeterViewModel { Id = 1, Type = "Electric" };
            var meter = new Meter { Id = 1, Type = "Electric" };
            _mapperMock.Setup(m => m.Map<Meter>(meterViewModel)).Returns(meter);
            _meterServiceMock.Setup(service => service.UpdateMeter(meter)).Returns(Task.CompletedTask);

            var result = await _controller.Edit(1, meterViewModel);

            
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_DeletesMeterAndRedirectsToIndex()
        {
           
            int meterId = 1;
            _meterServiceMock.Setup(service => service.DeleteMeter(meterId)).Returns(Task.CompletedTask);

           
            var result = await _controller.DeleteConfirmed(meterId);

           
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        
    }
}

using AutoMapper;
using Infrastructure.Entities; // Make sure this points to the right namespace for your entities
using MetersMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business;
using Core.Models;

namespace MetersMVC.Controllers
{
    public class MeterReadingController : Controller
    {
        private readonly IMeterReadingService _meterReadingService;
        private readonly IMapper _mapper;

        public MeterReadingController(IMeterReadingService meterReadingService, IMapper mapper)
        {
            _meterReadingService = meterReadingService;
            _mapper = mapper;
        }

        // GET: MeterReading
        public async Task<IActionResult> Index()
        {
            // Fetch all meter readings from the service
            var meterReadings = await _meterReadingService.GetAllMeterReadings();

            // Map to the view model using AutoMapper
            var meterReadingViewModels = _mapper.Map<List<MeterReadingViewModel>>(meterReadings);

            // Return the view with the mapped view models
            return View(meterReadingViewModels);
        }


        // GET: MeterReading/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MeterReading/Create
        [HttpPost]
        public async Task<IActionResult> Create(MeterReadingViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map the view model to your business model
                var reading = _mapper.Map<MeterReading>(model);

                // Call your service to add the reading (you need to implement this)
                await _meterReadingService.AddMeterReading(reading);

                return RedirectToAction("Index"); // Redirect to the index or wherever you want
            }

            return View(model); // Return the view with the model if validation fails
        }


        // GET: MeterReading/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var meterReadingEntity = await _meterReadingService.GetMeterReadingById(id);
            if (meterReadingEntity == null)
            {
                return NotFound();
            }

            var meterReadingViewModel = _mapper.Map<MeterReadingViewModel>(meterReadingEntity);
            return View(meterReadingViewModel);
        }

        // POST: MeterReading/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MeterReadingViewModel meterReadingViewModel)
        {
            if (id != meterReadingViewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                // Map MeterReadingViewModel to the Core.Models.MeterReading
                var meterReading = _mapper.Map<MeterReading>(meterReadingViewModel);

                // Call the service with the correctly mapped entity
                await _meterReadingService.UpdateMeterReading(meterReading);

                return RedirectToAction(nameof(Index));
            }

            return View(meterReadingViewModel);
        }

        // GET: MeterReading/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var meterReadingEntity = await _meterReadingService.GetMeterReadingById(id);
            if (meterReadingEntity == null)
            {
                return NotFound();
            }

            var meterReadingViewModel = _mapper.Map<MeterReadingViewModel>(meterReadingEntity);
            return View(meterReadingViewModel);
        }

        // POST: MeterReading/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _meterReadingService.DeleteMeterReading(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddReading(int meterId)
        {
            var model = new MeterReadingViewModel
            {
                MeterId = meterId
            };
            return View(model);
        }

    }
}

using AutoMapper;
using Core.Interfaces;
using Core.Models;
using MetersMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Core.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetersMVC.Controllers
{
    public class MeterReadingController : Controller
    {
        private readonly IMeterReadingService _meterReadingService;
        private readonly IMapper _mapper;
      //  private readonly ILogger<MeterReadingController> _logger;
     // private readonly Core.Logging.ILogger _logger;

        public MeterReadingController(IMeterReadingService meterReadingService, IMapper mapper, ILogger<MeterReadingController> logger)
        {
            _meterReadingService = meterReadingService;
            _mapper = mapper;
          //  _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all meter readings.
        /// </summary>
        /// <returns>Returns a view displaying the list of meter readings.</returns>
        public async Task<IActionResult> Index()
        {
            try
            {
              //  _logger.LogInformation("Fetching all meter readings.");
                var readings = await _meterReadingService.GetAllMeterReadings();
                var readingViewModels = _mapper.Map<List<MeterReadingViewModel>>(readings);
                //  _logger.LogInformation("Successfully fetched all meter readings.");
                
               

                return View(readingViewModels);
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, "Error occurred while fetching meter readings.");
                return RedirectToAction("Error", new { message = "An error occurred while retrieving meter readings." });
            }
        }

        /// <summary>
        /// Retrieves details of a specific meter reading.
        /// </summary>
        /// <param name="id">The ID of the meter reading.</param>
        /// <returns>Returns a view displaying the details of the meter reading.</returns>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
               // _logger.LogInformation("Fetching details for meter reading with ID {Id}.", id);
                var reading = await _meterReadingService.GetMeterReadingById(id);
                if (reading == null)
                {
                   // _logger.LogWarning("Meter reading with ID {Id} not found.", id);
                    return NotFound();
                }

                var readingViewModel = _mapper.Map<MeterReadingViewModel>(reading);
               // _logger.LogInformation("Successfully fetched details for meter reading with ID {Id}.", id);
                return View(readingViewModel);
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, "Error occurred while fetching details for meter reading with ID {Id}.", id);
                return RedirectToAction("Error", new { message = "An error occurred while retrieving the meter reading details." });
            }
        }

        /// <summary>
        /// Displays the create meter reading form.
        /// </summary>
        /// <param name="meterId">The ID of the meter associated with the new reading.</param>
        /// <returns>Returns a view for creating a new meter reading.</returns>
        public IActionResult Create(int meterId)
        {
           // _logger.LogInformation("Displaying create meter reading form for meter ID {MeterId}.", meterId);
            var readingViewModel = new MeterReadingViewModel
            {
                MeterId = meterId // Set the MeterId for the reading
            };
            return View(readingViewModel);
        }

        /// <summary>
        /// Processes the creation of a new meter reading.
        /// </summary>
        /// <param name="readingViewModel">The view model containing meter reading data.</param>
        /// <returns>Redirects to the Index page if successful, otherwise reloads the create view with validation errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MeterReadingViewModel readingViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   // _logger.LogInformation("Creating a new meter reading.");
                    var reading = _mapper.Map<MeterReading>(readingViewModel);
                    await _meterReadingService.AddMeterReading(reading);
                  //  _logger.LogInformation("Successfully created a new meter reading.");
                    return RedirectToAction(nameof(Index));
                }

              //  _logger.LogWarning("Model state invalid while creating a new meter reading.");
                return View(readingViewModel);
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, "Error occurred while creating a new meter reading.");
                return RedirectToAction("Error", new { message = "An error occurred while creating the meter reading." });
            }
        }

        /// <summary>
        /// Displays the edit form for a specific meter reading.
        /// </summary>
        /// <param name="id">The ID of the meter reading to edit.</param>
        /// <returns>Returns a view with the meter reading data pre-populated for editing.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
               // _logger.LogInformation("Fetching meter reading with ID {Id} for editing.", id);
                var reading = await _meterReadingService.GetMeterReadingById(id);
                if (reading == null)
                {
                   // _logger.LogWarning("Meter reading with ID {Id} not found for editing.", id);
                    return NotFound();
                }

                var readingViewModel = _mapper.Map<MeterReadingViewModel>(reading);
               // _logger.LogInformation("Displaying edit form for meter reading with ID {Id}.", id);
                return View(readingViewModel);
            }
            catch (Exception ex)
            {
              //  _logger.LogError(ex, "Error occurred while fetching meter reading for editing.");
                return RedirectToAction("Error", new { message = "An error occurred while retrieving the meter reading for editing." });
            }
        }

        /// <summary>
        /// Processes the update of an existing meter reading.
        /// </summary>
        /// <param name="id">The ID of the meter reading to update.</param>
        /// <param name="readingViewModel">The view model containing updated meter reading data.</param>
        /// <returns>Redirects to the Index page if successful, otherwise reloads the edit view with validation errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MeterReadingViewModel readingViewModel)
        {
            try
            {
                if (id != readingViewModel.Id)
                {
                  //  _logger.LogWarning("Meter reading ID mismatch: URL ID {Id} does not match form ID {FormId}.", id, readingViewModel.Id);
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                   // _logger.LogInformation("Updating meter reading with ID {Id}.", id);
                    var reading = _mapper.Map<MeterReading>(readingViewModel);
                    await _meterReadingService.UpdateMeterReading(reading);
                  //  _logger.LogInformation("Successfully updated meter reading with ID {Id}.", id);
                    return RedirectToAction(nameof(Index));
                }

                //_logger.LogWarning("Model state invalid while editing meter reading with ID {Id}.", id);
                return View(readingViewModel);
            }
            catch (Exception ex)
            {
              //  _logger.LogError(ex, "Error occurred while updating meter reading with ID {Id}.", id);
                return RedirectToAction("Error", new { message = "An error occurred while updating the meter reading." });
            }
        }

        /// <summary>
        /// Displays the delete confirmation view for a specific meter reading.
        /// </summary>
        /// <param name="id">The ID of the meter reading to delete.</param>
        /// <returns>Returns a view for confirming the deletion of the meter reading.</returns>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
               // _logger.LogInformation("Fetching meter reading with ID {Id} for deletion.", id);
                var reading = await _meterReadingService.GetMeterReadingById(id);
                if (reading == null)
                {
                   // _logger.LogWarning("Meter reading with ID {Id} not found for deletion.", id);
                    return NotFound();
                }

                var readingViewModel = _mapper.Map<MeterReadingViewModel>(reading);
              //  _logger.LogInformation("Displaying delete confirmation view for meter reading with ID {Id}.", id);
                return View(readingViewModel);
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, "Error occurred while fetching meter reading for deletion.");
                return RedirectToAction("Error", new { message = "An error occurred while retrieving the meter reading for deletion." });
            }
        }

        /// <summary>
        /// Confirms and processes the deletion of a specific meter reading.
        /// </summary>
        /// <param name="id">The ID of the meter reading to delete.</param>
        /// <returns>Redirects to the Index page if successful.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
              //  _logger.LogInformation("Deleting meter reading with ID {Id}.", id);
                await _meterReadingService.DeleteMeterReading(id);
              //  _logger.LogInformation("Successfully deleted meter reading with ID {Id}.", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, "Error occurred while deleting meter reading with ID {Id}.", id);
                return RedirectToAction("Error", new { message = "An error occurred while deleting the meter reading." });
            }
        }

        /// <summary>
        /// Displays the error view with a custom message.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        /// <returns>Returns an error view with the message.</returns>
        public IActionResult Error(string message)
        {
          //  _logger.LogError("MetersController: Error action triggered with message: {ErrorMessage}", message);

            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
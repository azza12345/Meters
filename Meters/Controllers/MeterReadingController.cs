using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Meters.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        private readonly IMeterReadingService _meterReadingService;
        private readonly IMapper _mapper;
        private readonly ILogger<MeterReadingController> _logger;

        public MeterReadingController(IMeterReadingService meterReadingService, IMapper mapper, ILogger<MeterReadingController> logger)
        {
            _meterReadingService = meterReadingService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/meterreading
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeterReadingViewModel>>> GetMeterReadings()
        {
            try
            {
                _logger.LogInformation("Fetching all meter readings.");
                var meterReadings = await _meterReadingService.GetAllMeterReadings();
                var meterReadingViewModels = _mapper.Map<IEnumerable<MeterReadingViewModel>>(meterReadings);
                _logger.LogInformation("Successfully fetched all meter readings.");
                return Ok(meterReadingViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving meter readings.");
                return StatusCode(500, "An error occurred while retrieving meter readings.");
            }
        }

        // GET: api/meterreading/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MeterReadingViewModel>> GetMeterReading(int id)
        {
            try
            {
                _logger.LogInformation("Fetching meter reading with ID {Id}.", id);
                var meterReading = await _meterReadingService.GetMeterReadingById(id);
                if (meterReading == null)
                {
                    _logger.LogWarning("Meter reading with ID {Id} not found.", id);
                    return NotFound();
                }

                var meterReadingViewModel = _mapper.Map<MeterReadingViewModel>(meterReading);
                _logger.LogInformation("Successfully fetched meter reading with ID {Id}.", id);
                return Ok(meterReadingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving meter reading with ID {Id}.", id);
                return StatusCode(500, "An error occurred while retrieving the meter reading.");
            }
        }

        // POST: api/meterreading
        [HttpPost]
        public async Task<ActionResult<MeterReadingViewModel>> CreateMeterReading([FromBody] MeterReadingViewModel meterReadingViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state while creating a new meter reading.");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating a new meter reading.");
                var meterReadingEntity = _mapper.Map<MeterReading>(meterReadingViewModel);
                await _meterReadingService.AddMeterReading(meterReadingEntity);
                _logger.LogInformation("Successfully created a new meter reading.");

                return CreatedAtAction(nameof(GetMeterReading), new { id = meterReadingEntity.Id }, meterReadingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new meter reading.");
                return StatusCode(500, "An error occurred while creating the meter reading.");
            }
        }

        // PUT: api/meterreading/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeterReading(int id, [FromBody] MeterReadingViewModel meterReadingViewModel)
        {
            try
            {
                if (id != meterReadingViewModel.Id)
                {
                    _logger.LogWarning("Meter reading ID mismatch. URL ID {Id} does not match model ID {ModelId}.", id, meterReadingViewModel.Id);
                    return BadRequest("Meter reading ID mismatch.");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state while updating meter reading with ID {Id}.", id);
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Updating meter reading with ID {Id}.", id);
                var meterReadingEntity = _mapper.Map<MeterReading>(meterReadingViewModel);
                await _meterReadingService.UpdateMeterReading(meterReadingEntity);
                _logger.LogInformation("Successfully updated meter reading with ID {Id}.", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating meter reading with ID {Id}.", id);
                return StatusCode(500, "An error occurred while updating the meter reading.");
            }
        }

        // DELETE: api/meterreading/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeterReading(int id)
        {
            try
            {
                _logger.LogInformation("Fetching meter reading with ID {Id} for deletion.", id);
                var meterReadingExists = await _meterReadingService.MeterReadingExists(id);
                if (!meterReadingExists)
                {
                    _logger.LogWarning("Meter reading with ID {Id} not found for deletion.", id);
                    return NotFound();
                }

                _logger.LogInformation("Deleting meter reading with ID {Id}.", id);
                await _meterReadingService.DeleteMeterReading(id);
                _logger.LogInformation("Successfully deleted meter reading with ID {Id}.", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting meter reading with ID {Id}.", id);
                return StatusCode(500, "An error occurred while deleting the meter reading.");
            }
        }
    }
}

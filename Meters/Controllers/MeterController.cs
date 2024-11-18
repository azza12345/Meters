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
    [Route("api/Meter")]
    [ApiController]
    public class MeterController : ControllerBase
    {
        private readonly IMeterService _meterService;
        private readonly IMapper _mapper;
        private readonly ILogger<MeterController> _logger;

        public MeterController(IMeterService meterService, IMapper mapper, ILogger<MeterController> logger)
        {
            _meterService = meterService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/meter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeterViewModel>>> GetMeters()
        {
            try
            {
                _logger.LogInformation("Fetching all meters.");
            var meters = await _meterService.GetAllMeters();
            var meterViewModels = _mapper.Map<IEnumerable<MeterViewModel>>(meters);
                _logger.LogInformation("Successfully fetched all meters.");
            return Ok(meterViewModels);
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving meters.");
                return StatusCode(500, "An error occurred while retrieving meters.");
            }
        }

        // GET: api/meter
        [HttpGet]
        [Route("GetMetersList")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetMetersList()
        {
            try
            {
                _logger.LogInformation("Fetching all meters.");
                var meters = await _meterService.GetAllMeters();
                var meterViewModels = _mapper.Map<IEnumerable<MeterViewModel>>(meters);
                _logger.LogInformation("Successfully fetched all meters.");
                return Ok(meterViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving meters.");
                return StatusCode(500, "An error occurred while retrieving meters.");
            }
        }

        // GET: api/meter/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MeterViewModel>> GetMeter(int id)
        {
            try
            {
                _logger.LogInformation("Fetching meter with ID {Id}.", id);
            var meter = await _meterService.GetMeterById(id);
            if (meter == null)
            {
                    _logger.LogWarning("Meter with ID {Id} not found.", id);
                return NotFound();
            }

            var meterViewModel = _mapper.Map<MeterViewModel>(meter);
                _logger.LogInformation("Successfully fetched meter with ID {Id}.", id);
            return Ok(meterViewModel);
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving meter with ID {Id}.", id);
                return StatusCode(500, "An error occurred while retrieving the meter.");
            }
        }

        // POST: api/meter
        [HttpPost]
        public async Task<ActionResult<MeterViewModel>> CreateMeter([FromBody] MeterViewModel meterViewModel)
        {
            try
            {
            if (!ModelState.IsValid)
            {
                    _logger.LogWarning("Invalid model state while creating a new meter.");
                return BadRequest(ModelState);
            }

                _logger.LogInformation("Creating a new meter.");
            var meterEntity = _mapper.Map<Meter>(meterViewModel);
            await _meterService.AddMeter(meterEntity);
                _logger.LogInformation("Successfully created a new meter.");

            return CreatedAtAction(nameof(GetMeter), new { id = meterEntity.Id }, meterViewModel);
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new meter.");
                return StatusCode(500, "An error occurred while creating the meter.");
            }
        }



        // PUT: api/meter/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeter(int id, [FromBody] MeterViewModel meterViewModel)
        {
            try
            {
            if (id != meterViewModel.Id)
            {
                    _logger.LogWarning("Meter ID mismatch. URL ID {Id} does not match model ID {ModelId}.", id, meterViewModel.Id);
                    return BadRequest("Meter ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                    _logger.LogWarning("Invalid model state while updating meter with ID {Id}.", id);
                return BadRequest(ModelState);
            }

                _logger.LogInformation("Updating meter with ID {Id}.", id);
            var meterEntity = _mapper.Map<Meter>(meterViewModel);
            await _meterService.UpdateMeter(meterEntity);
                _logger.LogInformation("Successfully updated meter with ID {Id}.", id);

            return NoContent();
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating meter with ID {Id}.", id);
                return StatusCode(500, "An error occurred while updating the meter.");
            }
        }

        // DELETE: api/meter/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeter(int id)
        {
            try
            {
                _logger.LogInformation("Fetching meter with ID {Id} for deletion.", id);
            var meterExists = await _meterService.MeterExists(id);
            if (!meterExists)
            {
                    _logger.LogWarning("Meter with ID {Id} not found for deletion.", id);
                return NotFound();
            }

                _logger.LogInformation("Deleting meter with ID {Id}.", id);
            await _meterService.DeleteMeter(id);
                _logger.LogInformation("Successfully deleted meter with ID {Id}.", id);

            return NoContent();
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting meter with ID {Id}.", id);
                return StatusCode(500, "An error occurred while deleting the meter.");
            }
        }
    }
}

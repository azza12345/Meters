
using AutoMapper;
using Business;
using Core.Models; 
using Meters.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Meters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        private readonly IMeterReadingService _meterReadingService;
        private readonly IMapper _mapper;

        public MeterReadingController(IMeterReadingService meterReadingService, IMapper mapper)
        {
            _meterReadingService = meterReadingService;
            _mapper = mapper;
        }

        // GET: api/meterreading
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeterReadingViewModel>>> GetMeterReadings()
        {
            var meterReadings = await _meterReadingService.GetAllMeterReadings();
            var meterReadingViewModels = _mapper.Map<IEnumerable<MeterReadingViewModel>>(meterReadings);
            return Ok(meterReadingViewModels);
        }

        // GET: api/meterreading/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MeterReadingViewModel>> GetMeterReading(int id)
        {
            var meterReading = await _meterReadingService.GetMeterReadingById(id);
            if (meterReading == null)
            {
                return NotFound();
            }

            var meterReadingViewModel = _mapper.Map<MeterReadingViewModel>(meterReading);
            return Ok(meterReadingViewModel);
        }

        // POST: api/meterreading
        [HttpPost]
        public async Task<ActionResult<MeterReadingViewModel>> CreateMeterReading([FromBody] MeterReadingViewModel meterReadingViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meterReadingEntity = _mapper.Map<MeterReading>(meterReadingViewModel);
            await _meterReadingService.AddMeterReading(meterReadingEntity);

            return CreatedAtAction(nameof(GetMeterReading), new { id = meterReadingEntity.Id }, meterReadingViewModel);
        }

        // PUT: api/meterreading/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeterReading(int id, [FromBody] MeterReadingViewModel meterReadingViewModel)
        {
            if (id != meterReadingViewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meterReadingEntity = _mapper.Map<MeterReading>(meterReadingViewModel);
            await _meterReadingService.UpdateMeterReading(meterReadingEntity);

            return NoContent();
        }

        // DELETE: api/meterreading/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeterReading(int id)
        {
            var meterReadingExists = await _meterReadingService.MeterReadingExists(id);
            if (!meterReadingExists)
            {
                return NotFound();
            }

            await _meterReadingService.DeleteMeterReading(id);
            return NoContent();
        }
    }
}

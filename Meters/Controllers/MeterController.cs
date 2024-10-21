using AutoMapper;
using Business;
using Core.Models; 
using Meters.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Meters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterController : ControllerBase
    {
        private readonly IMeterService _meterService;
        private readonly IMapper _mapper;

        public MeterController(IMeterService meterService, IMapper mapper)
        {
            _meterService = meterService;
            _mapper = mapper;
        }

        // GET: api/meter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeterViewModel>>> GetMeters()
        {
            var meters = await _meterService.GetAllMeters();
            var meterViewModels = _mapper.Map<IEnumerable<MeterViewModel>>(meters);
            return Ok(meterViewModels);
        }

        // GET: api/meter/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MeterViewModel>> GetMeter(int id)
        {
            var meter = await _meterService.GetMeterById(id);
            if (meter == null)
            {
                return NotFound();
            }

            var meterViewModel = _mapper.Map<MeterViewModel>(meter);
            return Ok(meterViewModel);
        }

        // POST: api/meter
        [HttpPost]
        public async Task<ActionResult<MeterViewModel>> CreateMeter([FromBody] MeterViewModel meterViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meterEntity = _mapper.Map<Meter>(meterViewModel);
            await _meterService.AddMeter(meterEntity);

            return CreatedAtAction(nameof(GetMeter), new { id = meterEntity.Id }, meterViewModel);
        }



        // PUT: api/meter/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeter(int id, [FromBody] MeterViewModel meterViewModel)
        {
            if (id != meterViewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meterEntity = _mapper.Map<Meter>(meterViewModel);
            await _meterService.UpdateMeter(meterEntity);

            return NoContent();
        }

        // DELETE: api/meter/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeter(int id)
        {
            var meterExists = await _meterService.MeterExists(id);
            if (!meterExists)
            {
                return NotFound();
            }

            await _meterService.DeleteMeter(id);
            return NoContent();
        }
    }
}

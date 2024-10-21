
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;

namespace Business
{
    public class MeterService : IMeterService
    {
        private readonly IMeterRepository _meterRepository;

        public MeterService(IMeterRepository meterRepository)
        {
            _meterRepository = meterRepository;
        }

        public async Task<IEnumerable<Meter>> GetAllMeters()
        {
            return await _meterRepository.GetAllMeters();
        }

        public async Task<Meter> GetMeterById(int id)
        {
            return await _meterRepository.GetMeterById(id);
        }

        public async Task AddMeter(Meter meter)
        {
            await _meterRepository.AddMeter(meter);
        }

        public async Task UpdateMeter(Meter meter)
        {
            await _meterRepository.UpdateMeter(meter);
        }

        public async Task DeleteMeter(int id)
        {
            await _meterRepository.DeleteMeter(id);
        }

        public async Task<bool> MeterExists(int id) 
        {
            var meter = await _meterRepository.GetMeterById(id);
            return meter != null; 
        }
    }
}


using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;

namespace Business
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingRepository _meterReadingRepository;

        public MeterReadingService(IMeterReadingRepository meterReadingRepository)
        {
            _meterReadingRepository = meterReadingRepository;
        }

        public async Task<IEnumerable<MeterReading>> GetAllMeterReadings()
        {
            return await _meterReadingRepository.GetAllMeterReadings();
        }

        public async Task<MeterReading> GetMeterReadingById(int id)
        {
            return await _meterReadingRepository.GetMeterReadingById(id);
        }

        public async Task AddMeterReading(MeterReading meterReading)
        {
            await _meterReadingRepository.AddMeterReading(meterReading);
        }

        public async Task UpdateMeterReading(MeterReading meterReading)
        {
            await _meterReadingRepository.UpdateMeterReading(meterReading);
        }

        public async Task DeleteMeterReading(int id)
        {
            await _meterReadingRepository.DeleteMeterReading(id);
        }
        public async Task<bool> MeterReadingExists(int id)
        {
            var meterReading = await _meterReadingRepository.GetMeterReadingById(id);
            return meterReading != null;
        }
    }
}


using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IMeterReadingService
    {
        Task<IEnumerable<MeterReading>> GetAllMeterReadings(); // Just the signature, no body
        Task<MeterReading> GetMeterReadingById(int id);
        Task AddMeterReading(MeterReading meterReading);
        Task UpdateMeterReading(MeterReading meterReading);
        Task DeleteMeterReading(int id);
        Task<bool> MeterReadingExists(int id);
    }
}

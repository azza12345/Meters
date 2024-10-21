
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IMeterReadingRepository
    {
        Task<IEnumerable<MeterReading>> GetAllMeterReadings(); // Ensure this is public
        Task<MeterReading> GetMeterReadingById(int id);
        Task AddMeterReading(MeterReading meterReading);
        Task UpdateMeterReading(MeterReading meterReading);
        Task DeleteMeterReading(int id);
    }
}

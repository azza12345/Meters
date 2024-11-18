// Business/IMeterService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IMeterService
    {
        Task<IEnumerable<Meter>> GetAllMeters(); 
        Task<Meter> GetMeterById(int id);
        Task AddMeter(Meter meter);
        Task UpdateMeter(Meter meter);
        Task DeleteMeter(int id);
        Task<bool> MeterExists(int id);
    }
}

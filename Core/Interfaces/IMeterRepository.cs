// Core/Interfaces/IMeterRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IMeterRepository
    {
        Task<IEnumerable<Meter>> GetAllMeters(); 
        Task<Meter> GetMeterById(int id);
        Task AddMeter(Meter meter);
        Task UpdateMeter(Meter meter);
        Task DeleteMeter(int id);
    }
}

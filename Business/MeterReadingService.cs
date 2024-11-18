using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Business
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingRepository _meterReadingRepository;
        private readonly ILogger<MeterReadingService> _logger;

        public MeterReadingService(IMeterReadingRepository meterReadingRepository, ILogger<MeterReadingService> logger)
        {
            _meterReadingRepository = meterReadingRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<MeterReading>> GetAllMeterReadings()
        {
            try
            {
                _logger.LogInformation("Fetching all meter readings.");
                var meterReadings = await _meterReadingRepository.GetAllMeterReadings();
                _logger.LogInformation("Successfully fetched all meter readings.");
                return meterReadings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all meter readings.");
                throw; // Rethrow the exception to be handled by the calling method
            }
        }

        public async Task<MeterReading> GetMeterReadingById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching meter reading with ID {Id}.", id);
                var meterReading = await _meterReadingRepository.GetMeterReadingById(id);
                if (meterReading == null)
                {
                    _logger.LogWarning("Meter reading with ID {Id} not found.", id);
                }
                return meterReading;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching meter reading with ID {Id}.", id);
                throw; // Rethrow the exception to be handled by the calling method
            }
        }

        public async Task AddMeterReading(MeterReading meterReading)
        {
            try
            {
                _logger.LogInformation("Adding a new meter reading.");
                await _meterReadingRepository.AddMeterReading(meterReading);
                _logger.LogInformation("Successfully added a new meter reading.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new meter reading.");
                throw; // Rethrow the exception to be handled by the calling method
            }
        }

        public async Task UpdateMeterReading(MeterReading meterReading)
        {
            try
            {
                _logger.LogInformation("Updating meter reading with ID {Id}.", meterReading.Id);
                await _meterReadingRepository.UpdateMeterReading(meterReading);
                _logger.LogInformation("Successfully updated meter reading with ID {Id}.", meterReading.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating meter reading with ID {Id}.", meterReading.Id);
                throw; // Rethrow the exception to be handled by the calling method
            }
        }

        public async Task DeleteMeterReading(int id)
        {
            try
            {
                _logger.LogInformation("Deleting meter reading with ID {Id}.", id);
                await _meterReadingRepository.DeleteMeterReading(id);
                _logger.LogInformation("Successfully deleted meter reading with ID {Id}.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting meter reading with ID {Id}.", id);
                throw; // Rethrow the exception to be handled by the calling method
            }
        }

        public async Task<bool> MeterReadingExists(int id)
        {
            try
            {
                _logger.LogInformation("Checking if meter reading with ID {Id} exists.", id);
                var meterReading = await _meterReadingRepository.GetMeterReadingById(id);
                bool exists = meterReading != null;
                if (exists)
                {
                    _logger.LogInformation("Meter reading with ID {Id} exists.", id);
                }
                else
                {
                    _logger.LogWarning("Meter reading with ID {Id} does not exist.", id);
                }
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if meter reading with ID {Id} exists.", id);
                throw; // Rethrow the exception to be handled by the calling method
            }
        }
    }
}

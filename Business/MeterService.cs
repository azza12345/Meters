using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Business
{
    public class MeterService : IMeterService
    {
        private readonly IMeterRepository _meterRepository;
        private readonly ILogger<MeterService> _logger;
        private readonly HttpClient _httpClient;

        public MeterService(IMeterRepository meterRepository, ILogger<MeterService> logger, HttpClient httpClient)
        {
            _meterRepository = meterRepository;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Meter>> GetAllMeters()
        {
            try
            {
                _logger.LogInformation("Fetching all meters.");
                var meters = await _meterRepository.GetAllMeters();
                _logger.LogInformation("Successfully fetched all meters.");
                return meters;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all meters.");
                throw;
            }
        }

        //public async Task<IEnumerable<Meter>> GetAllMeters()
        //{
        //    try
        //    {
        //        _logger.LogInformation("Fetching all meters from the API.");

        //        var response = await _httpClient.GetAsync("https://localhost:7182/api/Meter");
        //        response.EnsureSuccessStatusCode();


        //        _logger.LogInformation("Successfully fetched all meters from the API.");


        //        var meters = await response.Content.ReadFromJsonAsync<IEnumerable<Meter>>();

        //        return meters ?? Enumerable.Empty<Meter>();
        //    }
        //    catch (Exception ex)
        //    {

        //        _logger.LogError(ex, "An error occurred while fetching all meters from the API.");
        //        throw;
        //    }
        //}


        public async Task<Meter> GetMeterById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching meter with ID {Id}.", id);
                var meter = await _meterRepository.GetMeterById(id);
                if (meter == null)
                {
                    _logger.LogWarning("Meter with ID {Id} not found.", id);
                }
                return meter;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching meter with ID {Id}.", id);
                throw;
            }
        }

        public async Task AddMeter(Meter meter)
        {
            try
            {
                _logger.LogInformation("Adding a new meter.");
            await _meterRepository.AddMeter(meter);
                _logger.LogInformation("Successfully added a new meter.");
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new meter.");
                throw; // Rethrow the exception to be handled by the calling method
            }
        }

        public async Task UpdateMeter(Meter meter)
        {
            try
            {
                _logger.LogInformation("Updating meter with ID {Id}.", meter.Id);
            await _meterRepository.UpdateMeter(meter);
                _logger.LogInformation("Successfully updated meter with ID {Id}.", meter.Id);
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating meter with ID {Id}.", meter.Id);
                throw; // Rethrow the exception to be handled by the calling method
            }
        }

        public async Task DeleteMeter(int id)
        {
            try
            {
                _logger.LogInformation("Deleting meter with ID {Id}.", id);
            await _meterRepository.DeleteMeter(id);
                _logger.LogInformation("Successfully deleted meter with ID {Id}.", id);
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting meter with ID {Id}.", id);
                throw; // Rethrow the exception to be handled by the calling method
            }
        }

        public async Task<bool> MeterExists(int id) 
        {
            try
            {
                _logger.LogInformation("Checking if meter with ID {Id} exists.", id);
            var meter = await _meterRepository.GetMeterById(id);
                bool exists = meter != null;
                if (exists)
                {
                    _logger.LogInformation("Meter with ID {Id} exists.", id);
                }
                else
                {
                    _logger.LogWarning("Meter with ID {Id} does not exist.", id);
                }
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if meter with ID {Id} exists.", id);
                throw; // Rethrow the exception to be handled by the calling method
            }
        }
    }
}

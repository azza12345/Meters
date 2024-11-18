using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
   
        public class MeterRepository : IMeterRepository
        {
            private readonly MeterDbContext _context;
            private readonly IMapper _mapper;
        private readonly ILogger<MeterRepository> _logger;


        public MeterRepository(MeterDbContext context, IMapper mapper, ILogger<MeterRepository> logger)
            {
                _context = context;
                _mapper = mapper;
            _logger = logger;
         
            }

            public async Task<IEnumerable<Core.Models.Meter>> GetAllMeters()
            {
            try
            {
                _logger.LogInformation("Fetching all meters.");
                var meters = await _context.Meters.ToListAsync();
                var mappedMeters = _mapper.Map<IEnumerable<Core.Models.Meter>>(meters);
                _logger.LogInformation("Successfully fetched all meters.");
                return mappedMeters;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all meters.");
                throw; // Rethrow to handle at a higher level
            }
        }
               

        //public async Task<IEnumerable<Core.Models.Meter>> GetAllMeters()
        //{
        //    try
        //    {
        //        _logger.LogInformation("Fetching all meters from the API.");

        //        // Send an HTTP GET request to the API
        //        var response = await _httpClient.GetAsync("Meter"); // Ensure this endpoint matches your API
        //        response.EnsureSuccessStatusCode(); // Will throw an exception if the status code is not successful

        //        _logger.LogInformation("Successfully fetched all meters from the API.");

        //        // Deserialize the response content into a list of Meter objects
        //        var meters = await response.Content.ReadFromJsonAsync<IEnumerable<Core.Models.Meter>>();

        //        return meters ?? Enumerable.Empty<Core.Models.Meter>(); // Return an empty list if null
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log any errors that occur during data retrieval
        //        _logger.LogError(ex, "An error occurred while fetching all meters from the API.");
        //        throw; // Rethrow the exception to be handled by the calling method
        //    }
        //}

            public async Task<Core.Models.Meter> GetMeterById(int id)
            {
            try
            {
                _logger.LogInformation("Fetching meter with ID {Id}.", id);
                var meterEntity = await _context.Meters.FindAsync(id);
                if (meterEntity == null)
                {
                    _logger.LogWarning("Meter with ID {Id} not found.", id);
                }
                return _mapper.Map<Core.Models.Meter>(meterEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching meter with ID {Id}.", id);
                throw; // Rethrow to handle at a higher level
            }
        }

            public async Task AddMeter(Core.Models.Meter meter)
            {
            try
            {
                _logger.LogInformation("Adding a new meter.");
                var meterEntity = _mapper.Map<Infrastructure.Entities.MeterEntity>(meter);
                await _context.Meters.AddAsync(meterEntity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully added a new meter.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new meter.");
                throw; // Rethrow to handle at a higher level
            }
            }

            public async Task UpdateMeter(Core.Models.Meter meter)
            {
            try
            {
                _logger.LogInformation("Updating meter with ID {Id}.", meter.Id);
                var meterEntity = _mapper.Map<Infrastructure.Entities.MeterEntity>(meter);
                _context.Meters.Update(meterEntity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully updated meter with ID {Id}.", meter.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating meter with ID {Id}.", meter.Id);
                throw; // Rethrow to handle at a higher level
            }
            }

            public async Task DeleteMeter(int id)
            {
            try
            {
                _logger.LogInformation("Deleting meter with ID {Id}.", id);
                var meterEntity = await _context.Meters.FindAsync(id);
                if (meterEntity != null)
                {
                    _context.Meters.Remove(meterEntity);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Successfully deleted meter with ID {Id}.", id);
                }
                else
                {
                    _logger.LogWarning("Meter with ID {Id} not found for deletion.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting meter with ID {Id}.", id);
                throw; // Rethrow to handle at a higher level
                }
            }
        }

    }


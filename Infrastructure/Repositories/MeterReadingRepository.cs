using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly MeterDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<MeterReadingRepository> _logger;

        public MeterReadingRepository(MeterDbContext context, IMapper mapper, ILogger<MeterReadingRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MeterReading>> GetAllMeterReadings()
        {
            try
            {
                _logger.LogInformation("Fetching all meter readings.");
                var meterReadings = await _context.MeterReadings.Include(mr => mr.Meter).ToListAsync();
                var mappedReadings = _mapper.Map<IEnumerable<MeterReading>>(meterReadings);
                _logger.LogInformation("Successfully fetched all meter readings.");
                return mappedReadings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all meter readings.");
                throw; // Rethrow to handle at a higher level
            }
        }

        public async Task<MeterReading> GetMeterReadingById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching meter reading with ID {Id}.", id);
                var meterReading = await _context.MeterReadings.Include(mr => mr.Meter)
                    .FirstOrDefaultAsync(mr => mr.Id == id);
                if (meterReading == null)
                {
                    _logger.LogWarning("Meter reading with ID {Id} not found.", id);
                }
                return _mapper.Map<MeterReading>(meterReading);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching meter reading with ID {Id}.", id);
                throw; // Rethrow to handle at a higher level
            }
        }

        public async Task AddMeterReading(MeterReading meterReading)
        {
            try
            {
                _logger.LogInformation("Adding a new meter reading.");
                var meterReadingEntity = _mapper.Map<MeterReadingEntity>(meterReading);
                await _context.MeterReadings.AddAsync(meterReadingEntity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully added a new meter reading.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new meter reading.");
                throw; // Rethrow to handle at a higher level
            }
        }

        public async Task UpdateMeterReading(MeterReading meterReading)
        {
            try
            {
                _logger.LogInformation("Updating meter reading with ID {Id}.", meterReading.Id);
                var meterReadingEntity = _mapper.Map<MeterReadingEntity>(meterReading);
                _context.MeterReadings.Update(meterReadingEntity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully updated meter reading with ID {Id}.", meterReading.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating meter reading with ID {Id}.", meterReading.Id);
                throw; // Rethrow to handle at a higher level
            }
        }

        public async Task DeleteMeterReading(int id)
        {
            try
            {
                _logger.LogInformation("Deleting meter reading with ID {Id}.", id);
                var meterReadingEntity = await _context.MeterReadings.FindAsync(id);
                if (meterReadingEntity != null)
                {
                    _context.MeterReadings.Remove(meterReadingEntity);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Successfully deleted meter reading with ID {Id}.", id);
                }
                else
                {
                    _logger.LogWarning("Meter reading with ID {Id} not found for deletion.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting meter reading with ID {Id}.", id);
                throw; // Rethrow to handle at a higher level
            }
        }
    }
}

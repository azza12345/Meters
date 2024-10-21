using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MeterReadingRepository :IMeterReadingRepository
    {
        private readonly MeterDbContext _context;
        private readonly IMapper _mapper;

        public MeterReadingRepository(MeterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MeterReading>> GetAllMeterReadings()
        {
            var meterReadings = await _context.MeterReadings.Include(mr => mr.Meter).ToListAsync();
            return _mapper.Map<IEnumerable<MeterReading>>(meterReadings);
        }

        public async Task<MeterReading> GetMeterReadingById(int id)
        {
            var meterReading = await _context.MeterReadings.Include(mr => mr.Meter)
                .FirstOrDefaultAsync(mr => mr.Id == id);
            return _mapper.Map<MeterReading>(meterReading);
        }

        public async Task AddMeterReading(MeterReading meterReading)
        {
            var meterReadingEntity = _mapper.Map<MeterReadingEntity>(meterReading);
            _context.MeterReadings.Add(meterReadingEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMeterReading(MeterReading meterReading)
        {
            var meterReadingEntity = _mapper.Map<MeterReadingEntity>(meterReading);
            _context.MeterReadings.Update(meterReadingEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMeterReading(int id)
        {
            var meterReadingEntity = await _context.MeterReadings.FindAsync(id);
            if (meterReadingEntity != null)
            {
                _context.MeterReadings.Remove(meterReadingEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}


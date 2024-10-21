using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
   
        public class MeterRepository : IMeterRepository
        {
            private readonly MeterDbContext _context;
            private readonly IMapper _mapper;

            public MeterRepository(MeterDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<Core.Models.Meter>> GetAllMeters()
            {
                var meters = await _context.Meters.ToListAsync();
               
                return _mapper.Map<IEnumerable<Core.Models.Meter>>(meters);
            }

            public async Task<Core.Models.Meter> GetMeterById(int id)
            {
                var meterEntity = await _context.Meters.FindAsync(id);
                return _mapper.Map<Core.Models.Meter>(meterEntity);
            }

            public async Task AddMeter(Core.Models.Meter meter)
            {
                
                var meterEntity = _mapper.Map<Infrastructure.Entities.MeterEntity>(meter);
                await _context.Meters.AddAsync(meterEntity);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateMeter(Core.Models.Meter meter)
            {
                var meterEntity = _mapper.Map<Infrastructure.Entities.MeterEntity>(meter);
                _context.Meters.Update(meterEntity);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteMeter(int id)
            {
                var meterEntity = await _context.Meters.FindAsync(id);
                if (meterEntity != null)
                {
                    _context.Meters.Remove(meterEntity);
                    await _context.SaveChangesAsync();
                }
            }
        }

    }


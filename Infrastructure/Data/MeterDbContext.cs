using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class MeterDbContext :DbContext
    {
        public MeterDbContext(DbContextOptions<MeterDbContext> options)
        : base(options)
        {
        }

        public DbSet<MeterEntity> Meters { get; set; }
        public DbSet<MeterReadingEntity> MeterReadings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<MeterEntity>()
                .HasMany(m => m.MeterReadings)
                .WithOne(mr => mr.Meter)
                .HasForeignKey(mr => mr.MeterId);

            base.OnModelCreating(modelBuilder);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class MeterEntity
    {

        public int Id { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public ICollection<MeterReadingEntity> MeterReadings { get; set; }
    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
   public class Meter
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public ICollection<MeterReading>? MeterReadings { get; set; } 
    }
}


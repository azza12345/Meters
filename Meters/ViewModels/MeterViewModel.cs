namespace Meters.ViewModels
{
    public class MeterViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public DateTime? CreateDate { get; set; }
        public ICollection<MeterReadingViewModel>? MeterReadings { get; set; }
    }
}

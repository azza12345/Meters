namespace MetersMVC.ViewModels
{
    public class MeterReadingViewModel
    {
        public int Id { get; set; }
        public string Reading { get; set; }
        public DateTime ReadingDate { get; set; } = DateTime.Now;
        public int? MeterId { get; set; }  // Foreign key
        public MeterViewModel? Meter { get; set; }
    }
}

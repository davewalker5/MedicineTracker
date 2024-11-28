namespace MedicineTracker.Entities.Configuration
{
    public class ApplicationSettings
    {
        public int WarningDays { get; set; }
        public int CriticalDays { get; set; }
        public string DataFile { get; set; }
    }
}
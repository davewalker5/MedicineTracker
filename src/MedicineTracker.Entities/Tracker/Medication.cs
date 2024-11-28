namespace MedicineTracker.Entities.Tracker
{
    public class Medication
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public DateTime LastTaken { get; set; }
        public int DailyDose { get; set; }

        public int DaysRemaining()
            => Stock / DailyDose;
        
        public DateTime LastDay()
            => new DateTime(LastTaken.Year, LastTaken.Month, LastTaken.Day, 0, 0, 0).AddDays(DaysRemaining());
    }
}
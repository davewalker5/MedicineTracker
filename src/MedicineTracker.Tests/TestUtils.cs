namespace MedicineTracker.Tests
{
    internal static class TestUtils
    {
        public static DateTime DateWithoutTime(DateTime date)
        {
            return new(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTime NowWithoutTime()
            => DateWithoutTime(DateTime.Now);
    }
}

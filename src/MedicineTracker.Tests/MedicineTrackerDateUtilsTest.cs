using MedicineTracker.BusinessLogic.Stock;

namespace MedicineTracker.Tests
{
    [TestClass]
    public class MedicineTrackerDateUtilsTest
    {
        [TestMethod]
        public void DateWithoutTimeTest()
        {
            var date = MedicineTrackerDateUtils.DateWithoutTime(new(2023, 4, 6, 13, 56, 22));
            Assert.AreEqual(2023, date.Year);
            Assert.AreEqual(4, date.Month);
            Assert.AreEqual(6, date.Day);
            Assert.AreEqual(0, date.Hour);
            Assert.AreEqual(0, date.Minute);
            Assert.AreEqual(0, date.Second);
        }

        [TestMethod]
        public void TodayWithoutTimeTest()
        {
            var today = DateTime.Now;
            var date = MedicineTrackerDateUtils.TodayWithoutTime();
            Assert.AreEqual(today.Year, date.Year);
            Assert.AreEqual(today.Month, date.Month);
            Assert.AreEqual(today.Day, date.Day);
            Assert.AreEqual(0, date.Hour);
            Assert.AreEqual(0, date.Minute);
            Assert.AreEqual(0, date.Second);
        }
    }
}
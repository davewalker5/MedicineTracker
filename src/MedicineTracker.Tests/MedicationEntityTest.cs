using MedicineTracker.BusinessLogic.Stock;
using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Tests
{
    [TestClass]
    public class MedicationEntityTest
    {
        private Medication _medication;
        private readonly DateTime _initialStockDate = MedicineTrackerDateUtils.TodayWithoutTime();

        [TestInitialize]
        public void TestInitialise()
        {
            _medication = new Medication
            {
                Name = "Some Medication",
                DailyDose = 2,
                Stock = 6,
                LastTaken = _initialStockDate,
            };
        }

        [TestMethod]
        public void DaysRemainingTest()
        {
            Assert.AreEqual(3, _medication.DaysRemaining());
        }

        [TestMethod]
        public void LastDayTest()
        {
            var expectedLastDay = _initialStockDate.AddDays(3);
            Assert.AreEqual(expectedLastDay, _medication.LastDay());
        }
    }
}

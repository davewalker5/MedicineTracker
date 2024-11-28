using MedicineTracker.BusinessLogic.Stock;
using MedicineTracker.Entities.Interfaces;
using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Tests
{
    [TestClass]
    public class DoseUpdaterTest
    {
        private IList<Medication> _medications = [];
        private IDoseUpdater _updater;

        [TestInitialize]
        public void TestInitialise()
        {
            _medications.Add(new Medication
            {
                Name = "Some Medication",
                DailyDose = 1,
                Stock = 28,
                LastTaken = DateTime.Now,
            });

            _updater = new DoseUpdater();
        }

        [TestMethod]
        public void SetDoseTest()
        {
            Assert.AreEqual(1, _medications[0].DailyDose);
            _updater.SetDose(_medications, 0, 2);
            Assert.AreEqual(2, _medications[0].DailyDose);
        }
    }
}

using MedicineTracker.BusinessLogic.Stock;
using MedicineTracker.Entities.Exceptions;
using MedicineTracker.Entities.Interfaces;
using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Tests
{
    [TestClass]
    public class MedicationUpdaterTest
    {
        private const string MedicationName = "Some Medication";

        private IMedicationUpdater _updater;

        [TestInitialize]
        public void Initialise()
        {
            _updater = new MedicationUpdater();
        }

        [TestMethod]
        public void AddMedicationTest()
        {
            var medications = new List<Medication>();
            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _updater.AddMedication(medications, MedicationName);

            Assert.AreEqual(1, medications.Count);
            Assert.AreEqual(MedicationName, medications[0].Name);
            Assert.AreEqual(1, medications[0].DailyDose);
            Assert.AreEqual(0, medications[0].Stock);
            Assert.AreEqual(expectedDate, medications[0].LastTaken);
        }

        [TestMethod]
        [ExpectedException(typeof(MedicationAlreadyExistsException))]
        public void CannotAddExistingMedicationtest()
        {
            var medications = new List<Medication>
            {
                new() { Name = MedicationName }
            };
            _updater.AddMedication(medications, MedicationName);
        }

        [TestMethod]
        public void DeleteMedicationTest()
        {
            var medications = new List<Medication>();
            _updater.AddMedication(medications, MedicationName);
            Assert.AreEqual(1, medications.Count);
            _updater.DeleteMedication(medications, 0);
            Assert.AreEqual(0, medications.Count);
        }
    }
}

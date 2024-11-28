using MedicineTracker.BusinessLogic.Storage;

namespace MedicineTracker.Tests
{
    [TestClass]
    public class MedicationReaderTest
    {
        [TestMethod]
        public void ReadMedicationsTest()
        {
            var medications = new MedicationReader().Read("medications.json");

            var expectedDate = new DateTime(2024, 11, 27, 0, 0, 0);
            Assert.IsNotNull(medications);
            Assert.AreEqual(1, medications.Count);
            Assert.AreEqual("A Medication", medications[0].Name);
            Assert.AreEqual(24, medications[0].Stock);
            Assert.AreEqual(expectedDate, medications[0].LastTaken);
            Assert.AreEqual(2, medications[0].DailyDose);
        }
    }
}

using MedicineTracker.BusinessLogic.Storage;
using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Tests
{
    [TestClass]
    public class MedicationWriterTest
    {
        private const string MedicationName = "Some Medication";
        private const int DailyDose = 2;
        private const int Stock = 36;
        private readonly DateTime _initialStockDate = TestUtils.NowWithoutTime().AddDays(-1);

        [TestMethod]
        public void WriteMedicationsTest()
        {
            IList<Medication> medications =
            [
                new() {
                    Name = MedicationName,
                    DailyDose =DailyDose,
                    Stock = Stock,
                    LastTaken = _initialStockDate,
                }
            ];

            var filepath = Path.ChangeExtension(Path.GetTempFileName(), "json");
            new MedicationWriter().Write(medications, filepath);
            var read = new MedicationReader().Read(filepath);
            File.Delete(filepath);

            Assert.AreEqual(1, read.Count);
            Assert.AreEqual(MedicationName, read[0].Name);
            Assert.AreEqual(DailyDose, read[0].DailyDose);
            Assert.AreEqual(Stock, read[0].Stock);
            Assert.AreEqual(_initialStockDate, read[0].LastTaken);
        }
    }
}

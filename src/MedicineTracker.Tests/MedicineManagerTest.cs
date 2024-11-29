using MedicineTracker.BusinessLogic;
using MedicineTracker.BusinessLogic.Stock;
using MedicineTracker.BusinessLogic.Storage;
using MedicineTracker.Entities.Configuration;
using MedicineTracker.Entities.Interfaces;
using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Tests
{
    [TestClass]
    public class MedicineManagerTest
    {
        private const string MedicationName = "Some Medication";
        private const string SecondMedicationName = "Another Medication";
        private const int DailyDose = 2;
        private const int Stock = 36;
        private readonly DateTime _initialStockDate = MedicineTrackerDateUtils.TodayWithoutTime().AddDays(-1);

        private string _dataFilePath;
        private IMedicineManager _manager;
        private Random _random = new Random();

        [TestInitialize]
        public void TestInitialise()
        {
            // Create application settings with a temporary data file
            _dataFilePath = Path.ChangeExtension(Path.GetTempFileName(), "json");

            var settings = new ApplicationSettings
            {
                WarningDays = 14,
                CriticalDays = 7,
                DataFile = _dataFilePath
            };

            // Write some medications to the temporary data file
            IList<Medication> medications =
            [
                new() {
                    Name = MedicationName,
                    DailyDose = DailyDose,
                    Stock = Stock,
                    LastTaken = _initialStockDate,
                }
            ];

            new MedicationWriter().Write(medications, _dataFilePath);

            // Configure the management class
            _manager = new MedicineManager(
                settings,
                new MedicationReader(),
                new MedicationWriter(),
                new StockUpdater(),
                new DoseUpdater(),
                new MedicationUpdater(),
                null);
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete( _dataFilePath );
        }

        [TestMethod]
        public void ReadTest()
        {
            Assert.IsNull(_manager.Medications);
            _manager.Read();

            Assert.AreEqual(1, _manager.Medications.Count);
            Assert.AreEqual(MedicationName, _manager.Medications.ElementAt(0).Name);
            Assert.AreEqual(DailyDose, _manager.Medications.ElementAt(0).DailyDose);
            Assert.AreEqual(Stock, _manager.Medications.ElementAt(0).Stock);
            Assert.AreEqual(_initialStockDate, _manager.Medications.ElementAt(0).LastTaken);
        }

        [TestMethod]
        public void WriteTest()
        {
            // Read and delete the data file
            _manager.Read();
            File.Delete(_dataFilePath);
            Assert.IsFalse(File.Exists(_dataFilePath));

            // Write and re-read the data file
            _manager.Write();
            _manager.Read();

            Assert.AreEqual(1, _manager.Medications.Count);
            Assert.AreEqual(MedicationName, _manager.Medications.ElementAt(0).Name);
            Assert.AreEqual(DailyDose, _manager.Medications.ElementAt(0).DailyDose);
            Assert.AreEqual(Stock, _manager.Medications.ElementAt(0).Stock);
            Assert.AreEqual(_initialStockDate, _manager.Medications.ElementAt(0).LastTaken);
        }

        [TestMethod]
        public void AddStockTest()
        {
            _manager.Read();
            var tablets = _random.Next(1, 100);
            _manager.AddStock(0, tablets);

            Assert.AreEqual(Stock + tablets, _manager.Medications.ElementAt(0).Stock);
            Assert.AreEqual(_initialStockDate, _manager.Medications.ElementAt(0).LastTaken);
        }

        [TestMethod]
        public void SetStockTest()
        {
            _manager.Read();
            var tablets = _random.Next(1, 100);
            _manager.SetStock(0, tablets);

            Assert.AreEqual(tablets, _manager.Medications.ElementAt(0).Stock);
            Assert.AreEqual(_initialStockDate, _manager.Medications.ElementAt(0).LastTaken);
        }

        [TestMethod]
        public void TakeDoseForSingleMedicationTest()
        {
            _manager.Read();
            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _manager.TakeDose("1");

            Assert.AreEqual(Stock - DailyDose, _manager.Medications.ElementAt(0).Stock);
            Assert.AreEqual(expectedDate, _manager.Medications.ElementAt(0).LastTaken);
        }

        [TestMethod]
        public void TakeDoseForAllMedicationsTest()
        {
            _manager.Read();
            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _manager.TakeDose("*");

            Assert.AreEqual(Stock - DailyDose, _manager.Medications.ElementAt(0).Stock);
            Assert.AreEqual(expectedDate, _manager.Medications.ElementAt(0).LastTaken);
        }

        [TestMethod]
        public void UntakeDoseForSingleMedicationTest()
        {
            _manager.Read();
            var expectedDate = _manager.Medications.ElementAt(0).LastTaken.AddDays(-1);
            _manager.UntakeDose("1");

            Assert.AreEqual(Stock + DailyDose, _manager.Medications.ElementAt(0).Stock);
            Assert.AreEqual(expectedDate, _manager.Medications.ElementAt(0).LastTaken);
        }

        [TestMethod]
        public void UntakeDoseForAllMedicationsTest()
        {
            _manager.Read();
            var expectedDate = _manager.Medications.ElementAt(0).LastTaken.AddDays(-1);
            _manager.UntakeDose("*");

            Assert.AreEqual(Stock + DailyDose, _manager.Medications.ElementAt(0).Stock);
            Assert.AreEqual(expectedDate, _manager.Medications.ElementAt(0).LastTaken);
        }

        [TestMethod]
        public void FastForwardForSingleMedicationTest()
        {
            _manager.Read();
            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _manager.FastForward("1");

            Assert.AreEqual(Stock - DailyDose, _manager.Medications.ElementAt(0).Stock);
            Assert.AreEqual(expectedDate, _manager.Medications.ElementAt(0).LastTaken);
        }

        [TestMethod]
        public void FastForwardForAllMedicationsTest()
        {
            _manager.Read();
            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _manager.FastForward("*");

            Assert.AreEqual(Stock - DailyDose, _manager.Medications.ElementAt(0).Stock);
            Assert.AreEqual(expectedDate, _manager.Medications.ElementAt(0).LastTaken);
        }

        [TestMethod]
        public void SetDoseTest()
        {
            _manager.Read();
            Assert.AreEqual(DailyDose, _manager.Medications.ElementAt(0).DailyDose);
            _manager.SetDose(0, DailyDose + 1);
            Assert.AreEqual(DailyDose + 1, _manager.Medications.ElementAt(0).DailyDose);
        }

        [TestMethod]
        public void AddMedicationTest()
        {
            _manager.Read();
            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _manager.AddMedication(SecondMedicationName);
            Assert.AreEqual(2, _manager.Medications.Count);

            Assert.AreEqual(MedicationName, _manager.Medications.ElementAt(0).Name);
            Assert.AreEqual(DailyDose, _manager.Medications.ElementAt(0).DailyDose);
            Assert.AreEqual(Stock, _manager.Medications.ElementAt(0).Stock);
            Assert.AreEqual(_initialStockDate, _manager.Medications.ElementAt(0).LastTaken);

            Assert.AreEqual(SecondMedicationName, _manager.Medications.ElementAt(1).Name);
            Assert.AreEqual(1, _manager.Medications.ElementAt(1).DailyDose);
            Assert.AreEqual(0, _manager.Medications.ElementAt(1).Stock);
            Assert.AreEqual(expectedDate, _manager.Medications.ElementAt(1).LastTaken);
        }

        [TestMethod]
        public void DeleteMedicationTest()
        {
            _manager.Read();
            _manager.DeleteMedication(0);
            Assert.AreEqual(0, _manager.Medications.Count);
        }
    }
}

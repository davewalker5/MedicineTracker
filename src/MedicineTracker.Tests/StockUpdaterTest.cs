using MedicineTracker.BusinessLogic.Stock;
using MedicineTracker.Entities.Exceptions;
using MedicineTracker.Entities.Interfaces;
using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Tests
{
    [TestClass]
    public class StockUpdaterTest
    {
        private readonly DateTime _initialStockDate = MedicineTrackerDateUtils.TodayWithoutTime().AddDays(-1);
        private IList<Medication> _medications = [];
        private IStockUpdater _updater;

        [TestInitialize]
        public void TestInitialise()
        {
            _medications.Add(new Medication
            {
                Name = "Some Medication",
                DailyDose = 2,
                Stock = 0,
                LastTaken = _initialStockDate,
            });

            _medications.Add(new Medication
            {
                Name = "Another Medication",
                DailyDose = 1,
                Stock = 0,
                LastTaken = _initialStockDate,
            });

            _updater = new StockUpdater();

            Assert.AreEqual(0, _medications[0].Stock);
            Assert.AreEqual(_initialStockDate, _medications[0].LastTaken);
        }

        [TestMethod]
        public void SetStockTest()
        {
            _updater.SetStock(_medications, 0, 28);

            Assert.AreEqual(28, _medications[0].Stock);
            Assert.AreEqual(_initialStockDate, _medications[0].LastTaken);
        }

        [TestMethod]
        public void AddStockTest()
        {
            _medications[0].Stock = 5;
            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _updater.AddStock(_medications, 0, 3);

            Assert.AreEqual(8, _medications[0].Stock);
            Assert.AreEqual(_initialStockDate, _medications[0].LastTaken);
        }

        [TestMethod]
        public void DecrementSingleMedicationStockTest()
        {
            _medications[0].Stock = 5;
            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _updater.Decrement(_medications, 0, 1);

            Assert.AreEqual(3, _medications[0].Stock);
            Assert.AreEqual(expectedDate, _medications[0].LastTaken);
        }

        [TestMethod]
        public void DecrementAllMedicationsStockTest()
        {
            _medications[0].Stock = 9;
            _medications[1].Stock = 7;

            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _updater.Decrement(_medications, 1);

            Assert.AreEqual(7, _medications[0].Stock);
            Assert.AreEqual(expectedDate, _medications[0].LastTaken);

            Assert.AreEqual(6, _medications[1].Stock);
            Assert.AreEqual(expectedDate, _medications[1].LastTaken);
        }

        [TestMethod]
        public void IncrementSingleMedicationStockTest()
        {
            _medications[0].Stock = 5;
            var expectedDate = _initialStockDate.AddDays(-1);
            _updater.Increment(_medications, 0, 1);

            Assert.AreEqual(7, _medications[0].Stock);
            Assert.AreEqual(expectedDate, _medications[0].LastTaken);
        }

        [TestMethod]
        public void IncrementAllMedicationsStockTest()
        {
            _medications[0].Stock = 9;
            _medications[1].Stock = 7;
            var expectedDate = _initialStockDate.AddDays(-1);
            _updater.Increment(_medications, 1);

            Assert.AreEqual(11, _medications[0].Stock);
            Assert.AreEqual(expectedDate, _medications[0].LastTaken);

            Assert.AreEqual(8, _medications[1].Stock);
            Assert.AreEqual(expectedDate, _medications[1].LastTaken);
        }

        [TestMethod]
        public void FastForwardSingleMedicationStockTest()
        {
            _medications[0].Stock = 5;
            _medications[0].LastTaken = _initialStockDate;

            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _updater.FastForward(_medications, 0);

            Assert.AreEqual(3, _medications[0].Stock);
            Assert.AreEqual(expectedDate, _medications[0].LastTaken);
        }

        [TestMethod]
        public void FastForwardAllMedicationsStockTest()
        {
            _medications[0].Stock = 9;
            _medications[0].LastTaken = _initialStockDate;

            _medications[1].Stock = 7;
            _medications[1].LastTaken = _initialStockDate;

            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _updater.FastForward(_medications);

            Assert.AreEqual(7, _medications[0].Stock);
            Assert.AreEqual(expectedDate, _medications[0].LastTaken);

            Assert.AreEqual(6, _medications[1].Stock);
            Assert.AreEqual(expectedDate, _medications[1].LastTaken);
        }

        [TestMethod]
        public void SkipSingleMedicationTest()
        {
            _medications[0].Stock = 5;
            _medications[0].LastTaken = _initialStockDate;

            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _updater.Skip(_medications, 0);

            Assert.AreEqual(5, _medications[0].Stock);
            Assert.AreEqual(expectedDate, _medications[0].LastTaken);
        }

        [TestMethod]
        public void SkipAllMedicationsTest()
        {
            _medications[0].Stock = 5;
            _medications[0].LastTaken = _initialStockDate;

            var expectedDate = MedicineTrackerDateUtils.TodayWithoutTime();
            _updater.Skip(_medications);

            Assert.AreEqual(5, _medications[0].Stock);
            Assert.AreEqual(expectedDate, _medications[0].LastTaken);
        }
        
        [TestMethod]
        public void TakeDoseForAllMedicationsIgnoresUpToDateTest()
        {
            _medications[0].Stock = 5;
            _medications[0].LastTaken = MedicineTrackerDateUtils.TodayWithoutTime();
            _updater.Decrement(_medications, 1);
            Assert.AreEqual(5, _medications[0].Stock);
        }

        [TestMethod]
        [ExpectedException(typeof(StockDateOutOfRangeException))]
        public void CannotTakeFutureDoseForSingleMedicationTest()
        {
            _medications[0].Stock = 5;
            _medications[0].LastTaken = MedicineTrackerDateUtils.TodayWithoutTime();
            _updater.Decrement(_medications, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(StockDateOutOfRangeException))]
        public void CannotSkipFutureDoseForSingleMedicationTest()
        {
            _medications[0].Stock = 5;
            _medications[0].LastTaken = MedicineTrackerDateUtils.TodayWithoutTime();
            _updater.Skip(_medications, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(StockDateOutOfRangeException))]
        public void CannotSkipFutureDoseForAllMedicationsTest()
        {
            _medications[0].Stock = 5;
            _medications[0].LastTaken = MedicineTrackerDateUtils.TodayWithoutTime();
            _updater.Skip(_medications);
        }
    }
}

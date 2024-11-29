using MedicineTracker.BusinessLogic.Stock;
using MedicineTracker.Entities.Configuration;
using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Tests
{
    [TestClass]
    public class ActionGeneratorTest
    {
        private const int WarningDays = 14;
        private const string TakeDoseAction = "Take dose";
        private const string OrderMoreAction = "Order more";

        private ActionGenerator _generator;

        [TestInitialize]
        public void Initialise()
        {
            _generator = new(new ApplicationSettings{WarningDays = WarningDays});
        }

        [TestMethod]
        public void NoRelevantActionsTest()
        {
            var medication = new Medication
            {
                LastTaken = MedicineTrackerDateUtils.TodayWithoutTime(),
                DailyDose = 1,
                Stock = WarningDays + 1
            };

            var actions = _generator.GetActions(medication);
            Assert.AreEqual(0, actions.Count);
        }

        [TestMethod]
        public void TakeDoseTest()
        {
            var medication = new Medication
            {
                LastTaken = MedicineTrackerDateUtils.TodayWithoutTime().AddDays(-1),
                DailyDose = 1,
                Stock = WarningDays + 1
            };

            var actions = _generator.GetActions(medication);
            Assert.AreEqual(1, actions.Count);
            Assert.IsTrue(actions.Contains(TakeDoseAction, StringComparer.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void OrderMoreTest()
        {
            var medication = new Medication
            {
                LastTaken = MedicineTrackerDateUtils.TodayWithoutTime(),
                DailyDose = 1,
                Stock = WarningDays
            };

            var actions = _generator.GetActions(medication);
            Assert.AreEqual(1, actions.Count);
            Assert.IsTrue(actions.Contains(OrderMoreAction, StringComparer.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void MultipleActionTest()
        {
            var medication = new Medication
            {
                LastTaken = MedicineTrackerDateUtils.TodayWithoutTime().AddDays(-1),
                DailyDose = 1,
                Stock = WarningDays
            };

            var actions = _generator.GetActions(medication);
            Assert.AreEqual(2, actions.Count);
            Assert.IsTrue(actions.Contains(TakeDoseAction, StringComparer.OrdinalIgnoreCase));
            Assert.IsTrue(actions.Contains(OrderMoreAction, StringComparer.OrdinalIgnoreCase));
        }
    }
}
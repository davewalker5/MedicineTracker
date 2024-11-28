using MedicineTracker.BusinessLogic.Configuration;
using MedicineTracker.Entities.Configuration;

namespace MedicineTracker.Tests
{
    [TestClass]
    public class ConfigReaderTest
    {
        [TestMethod]
        public void ReadAppSettingsTest()
        {
            var settings = new ConfigReader<ApplicationSettings>().Read();

            Assert.AreEqual(14, settings.WarningDays);
            Assert.AreEqual(7, settings.CriticalDays);
            Assert.AreEqual("medications.json", settings.DataFile);
        }
    }
}

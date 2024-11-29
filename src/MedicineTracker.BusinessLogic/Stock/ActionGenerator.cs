using MedicineTracker.Entities.Tracker;
using MedicineTracker.Entities.Interfaces;
using MedicineTracker.Entities.Configuration;
using System.Resources;
using System.Reflection;

namespace MedicineTracker.BusinessLogic.Stock
{
    public class ActionGenerator : IActionGenerator
    {
        private readonly ResourceManager _resources = new("MedicineTracker.BusinessLogic.Properties.Resources", Assembly.GetExecutingAssembly());
        private readonly ApplicationSettings _settings;

        public ActionGenerator(ApplicationSettings settings)
            => _settings = settings;

        /// <summary>
        /// Return a list of actions for a medication
        /// </summary>
        /// <param name="medication"></param>
        /// <returns></returns>
        public IList<string> GetActions(Medication medication)
        {
            var actions = new List<string>();

            if (medication.LastTaken < MedicineTrackerDateUtils.TodayWithoutTime())
            {
                string action = _resources.GetString("TakeDoseAction");
                actions.Add(action);
            }

            if (medication.DaysRemaining() <= _settings.WarningDays)
            {
                string action = _resources.GetString("OrderMoreAction");
                actions.Add(action);
            }

            return actions;
        }
    }
}






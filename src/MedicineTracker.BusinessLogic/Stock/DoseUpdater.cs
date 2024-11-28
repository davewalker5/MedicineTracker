using MedicineTracker.Entities.Tracker;
using MedicineTracker.Entities.Interfaces;

namespace MedicineTracker.BusinessLogic.Stock
{
    public class DoseUpdater : IDoseUpdater
    {
        /// <summary>
        /// Set the stock level for the medication at the specified index
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="index"></param>
        /// <param name="tablets"></param>
        public void SetDose(IEnumerable<Medication> medications, int index, int tablets)
        {
            var medication = medications.ElementAt(index);
            medication.DailyDose = tablets;
        }
    }
}
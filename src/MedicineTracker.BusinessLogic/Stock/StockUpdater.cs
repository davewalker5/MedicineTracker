using MedicineTracker.Entities.Tracker;
using MedicineTracker.Entities.Interfaces;
using MedicineTracker.Entities.Exceptions;

namespace MedicineTracker.BusinessLogic.Stock
{
    public class StockUpdater : IStockUpdater
    {
        /// <summary>
        /// Add a number of tablets to the medication at the specified index
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="index"></param>
        /// <param name="tablets"></param>
        public void AddStock(IEnumerable<Medication> medications, int index, int tablets)
        {
            var medication = medications.ElementAt(index);
            medication.Stock += tablets;
        }

        /// <summary>
        /// Set the stock level for the medication at the specified index
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="index"></param>
        /// <param name="tablets"></param>
        public void SetStock(IEnumerable<Medication> medications, int index, int tablets)
        {
            var medication = medications.ElementAt(index);
            medication.Stock = tablets;
        }

        /// <summary>
        /// Decrement stock of a medication by the specified number of doses
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="index"></param>
        /// <param name="doses"></param>
        public void Decrement(IEnumerable<Medication> medications, int index, int doses)
        {
            UpdateStock(medications, index, -doses);
        }

        /// <summary>
        /// Decrement the stock of all medications by the specified number of doses
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="doses"></param>
        public void Decrement(IEnumerable<Medication> medications, int doses)
        {
            for (int i = 0; i < medications.Count(); i++)
            {
                UpdateStock(medications, i, -doses);
            }
        }

        /// <summary>
        /// Increment stock of a medication by the specified number of doses
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="index"></param>
        /// <param name="doses"></param>
        public void Increment(IEnumerable<Medication> medications, int index, int doses)
        {
            UpdateStock(medications, index, doses);
        }

        /// <summary>
        /// Increment the stock of all medications by the specified number of doses
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="doses"></param>
        public void Increment(IEnumerable<Medication> medications, int doses)
        {
            for (int i = 0; i < medications.Count(); i++)
            {
                UpdateStock(medications, i, doses);
            }
        }

        /// <summary>
        /// Fast forward medication stock to the current date for a single medication
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="index"></param>
        public void FastForward(IEnumerable<Medication> medications, int index)
        {
            var medication = medications.ElementAt(index);
            var now = MedicineTrackerDateUtils.TodayWithoutTime();
            var stockDate = MedicineTrackerDateUtils.DateWithoutTime(medication.LastTaken);
            var doses = (int)(now - stockDate).TotalDays;
            if (doses > 0)
            {
                UpdateStock(medications, index, -doses);
            }
        }

        /// <summary>
        /// Fast forward medication stock to the current date for all medications
        /// </summary>
        /// <param name="medications"></param>
        public void FastForward(IEnumerable<Medication> medications)
        {
            for (int i = 0; i < medications.Count(); i++)
            {
                FastForward(medications, i);
            }
        }

        /// <summary>
        /// Skip a dose for a specified medication, just updating the stock date
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="index"></param>
        public void Skip(IEnumerable<Medication> medications, int index)
        {
            UpdateStock(medications, index, 0);
        }

        /// <summary>
        /// Skip a dose for all medications, just updating the stock date
        /// </summary>
        /// <param name="medications"></param>
        public void Skip(IEnumerable<Medication> medications)
        {
            for (int i = 0; i < medications.Count(); i++)
            {
                UpdateStock(medications, i, 0);
            }
        }

        /// <summary>
        /// Update the stock level for a medication
        /// </summary>
        /// <param name="index"></param>
        /// <param name="doses"></param>
        private static void UpdateStock(IEnumerable<Medication> medications, int index, int doses)
        {
            var medication = medications.ElementAt(index);
            var updatedStock = medication.Stock + doses * medication.DailyDose;
            medication.Stock = updatedStock > 0 ? updatedStock : 0;
            medication.LastTaken = CalculateStockDate(medication.LastTaken, doses);
        }

        /// <summary>
        /// Calculate the updated stock date based on the current date and the number of doses
        /// </summary>
        /// <param name="stockDate"></param>
        /// <param name="doses"></param>
        /// <returns></returns>
        private static DateTime CalculateStockDate(DateTime stockDate, int doses)
        {
            // Scenarios:
            //
            // 1: "doses" is > 0, so we're adding tablets back onto stock i.e. "un-taking" and need to roll back the date
            // 2: "doses" is < 0, so we're subtracting tablets from the stock i.e. "taking" and need to roll forward the date
            // 3: "doses" is 0, so we're skipping and need to roll forward the date by one day
            int daysToAdvance = doses != 0 ? -doses : 1;
            var date = MedicineTrackerDateUtils.DateWithoutTime(stockDate.AddDays(daysToAdvance));

            // If the resulting date is greater than now, then this is attempting to take doses beyond
            // today, which isn't allowed
            if (date > DateTime.Now)
            {
                var message = $"Stock date {date.ToShortDateString()} is in the future";
                throw new StockDateOutOfRangeException(message);
            }

            return date;
        }
    }
}
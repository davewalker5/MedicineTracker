using MedicineTracker.Entities.Exceptions;
using MedicineTracker.Entities.Tracker;
using MedicineTracker.Entities.Interfaces;

namespace MedicineTracker.BusinessLogic.Stock
{
    public class MedicationUpdater : IMedicationUpdater
    {
        /// <summary>
        /// Add a new medication to the specified collection of medications, with 0 stock and default dose
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="name"></param>
        /// <exception cref="MedicationAlreadyExistsException"></exception>
        public void AddMedication(IList<Medication> medications, string name)
        {
            // Check the medication doesn't exist, first
            var medication = medications.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (medication != null)
            {
                var message = $"Medication with name {name} already exists";
                throw new MedicationAlreadyExistsException(message);
            }

            // Create the new medication, with 0 stock and defaul dose
            medication = new Medication
            {
                Name = name,
                Stock = 0,
                LastTaken = DateTime.Today,
                DailyDose = 1
            };

            // Add it to the collection
            medications.Add(medication);
        }

        /// <summary>
        /// Delete the medication at the specified index
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="index"></param>
        public void DeleteMedication(IList<Medication> medications, int index)
            => medications.RemoveAt(index);
    }
}
using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Entities.Interfaces
{
    public interface IMedicationUpdater
    {
        void AddMedication(IList<Medication> medications, string name);
        void DeleteMedication(IList<Medication> medications, int index);
    }
}
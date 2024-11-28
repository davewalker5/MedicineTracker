using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Entities.Interfaces
{
    public interface IDoseUpdater
    {
        void SetDose(IEnumerable<Medication> medications, int index, int tablets);
    }
}
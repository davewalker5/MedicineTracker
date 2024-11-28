using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Entities.Interfaces
{
    public interface IMedicationWriter
    {
        void Write(IEnumerable<Medication> medications, string path);
    }
}
using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Entities.Interfaces
{
    public interface IMedicationReader
    {
        IList<Medication> Read(string path);
    }
}
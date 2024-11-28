using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Entities.Interfaces
{
    public interface IMedicationTabulator
    {
        void Tabulate(IEnumerable<Medication> medications, string title);
    }
}
using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Entities.Interfaces
{
    public interface IActionGenerator
    {
        IList<string> GetActions(Medication medication);
    }
}

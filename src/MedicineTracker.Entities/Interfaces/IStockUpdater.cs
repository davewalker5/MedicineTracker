using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Entities.Interfaces
{
    public interface IStockUpdater
    {
        void AddStock(IEnumerable<Medication> medications, int index, int tablets);
        void Decrement(IEnumerable<Medication> medications, int doses);
        void Decrement(IEnumerable<Medication> medications, int index, int doses);
        void FastForward(IEnumerable<Medication> medications);
        void FastForward(IEnumerable<Medication> medications, int index);
        void Increment(IEnumerable<Medication> medications, int doses);
        void Increment(IEnumerable<Medication> medications, int index, int doses);
        void SetStock(IEnumerable<Medication> medications, int index, int tablets);
        void Skip(IEnumerable<Medication> medications, int index);
        void Skip(IEnumerable<Medication> medications);
    }
}
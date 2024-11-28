using MedicineTracker.Entities.Tracker;

namespace MedicineTracker.Entities.Interfaces
{
    public interface IMedicineManager
    {
        IReadOnlyCollection<Medication> Medications { get; }

        void AddMedication(string name);
        void AddStock(int index, int tablets);
        void DeleteMedication(int index);
        void FastForward(string medication);
        void Read();
        void SetDose(int index, int tablets);
        void SetStock(int index, int tablets);
        void Tabulate(string title);
        void TakeDose(string medication);
        void UntakeDose(string medication);
        void Write();
    }
}
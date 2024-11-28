using MedicineTracker.Entities.CommandLine;

namespace MedicineTracker.Entities.Interfaces
{
    public interface IHelpGenerator
    {
        void Generate(IEnumerable<CommandLineOption> options);
    }
}
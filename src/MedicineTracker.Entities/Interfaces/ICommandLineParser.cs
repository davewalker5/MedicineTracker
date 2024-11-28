using MedicineTracker.Entities.CommandLine;

namespace MedicineTracker.Entities.Interfaces
{
    public interface ICommandLineParser
    {
        void Add(CommandLineOptionType optionType, bool isOperation, string name, string shortName, string description, int minimumNumberOfValues, int maximumNumberOfValues);
        List<string> GetValues(CommandLineOptionType optionType);
        void Help();
        bool IsPresent(CommandLineOptionType optionType);
        void Parse(IEnumerable<string> args);
    }
}
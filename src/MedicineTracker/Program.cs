using MedicineTracker.BusinessLogic;
using MedicineTracker.BusinessLogic.CommandLine;
using MedicineTracker.BusinessLogic.Configuration;
using MedicineTracker.BusinessLogic.Stock;
using MedicineTracker.BusinessLogic.Storage;
using MedicineTracker.ConsoleUtils;
using MedicineTracker.Entities.CommandLine;
using MedicineTracker.Entities.Configuration;
using MedicineTracker.Entities.Interfaces;
using System.Diagnostics;
using System.Reflection;

namespace MedicineTracker
{
    public static class Program
    {
        private static IMedicineManager _manager;
        private static readonly ICommandLineParser _parser = new CommandLineParser(new HelpTabulator());

        public static void Main(string[] args)
        { 
            // Get the version number and application title
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            var title = $"Medication Tracking Tool v{info.FileVersion}";

            try
            {
                // Parse the command line
                _parser.Add(CommandLineOptionType.Help, true, "--help", "-h", "Display command line option help", 0, 0);
                _parser.Add(CommandLineOptionType.Add, true, "--add", "-a", "Add tablets to the stock level for a given medication", 2, 2);
                _parser.Add(CommandLineOptionType.Set, true, "--set", "-s", "Set the stock level for a given medication", 2, 2);
                _parser.Add(CommandLineOptionType.Take, true, "--take", "-t", "Take a dose of a medication", 1, 1);
                _parser.Add(CommandLineOptionType.Untake, true, "--untake", "-u", "Un-take a dose of a medication", 1, 1);
                _parser.Add(CommandLineOptionType.FastForward, true, "--fast-forward", "-ff", "Fast forward stock levels to the current date", 1, 1);
                _parser.Add(CommandLineOptionType.Skip, true, "--skip", "-sk", "Skip a dose of a medication", 1, 1);
                _parser.Add(CommandLineOptionType.Dose, true, "--dose", "-d", "Set the daily dose for a given medication", 2, 2);
                _parser.Add(CommandLineOptionType.New, true, "--new", "-n", "Add a new, named medication", 1, 1);
                _parser.Add(CommandLineOptionType.Delete, true, "--delete", "-de", "Delete a medication", 1, 1);
                _parser.Parse(args);

                // Show help, if requested, or manipulate/display stock levels
                if (_parser.IsPresent(CommandLineOptionType.Help))
                {
                    _parser.Help();
                }
                else
                {
                    // Read the application settings
                    var settings = new ConfigReader<ApplicationSettings>().Read();

                    // Configure a new medication manager
                    _manager = new MedicineManager(
                        settings,
                        new MedicationReader(),
                        new MedicationWriter(),
                        new StockUpdater(),
                        new DoseUpdater(),
                        new MedicationUpdater(),
                        new MedicationTabulator(settings, new ActionGenerator(settings)));

                    _manager.Read();

                    // Manage the available medications
                    AddNewMedication();
                    DeleteMedication();

                    // Manage stock levels
                    AddStock();
                    SetStock();

                    // Manage medication dosage
                    SetDose();
                    TakeDose();
                    UntakeDose();
                    FastForward();

                    // Tabulate the medications
                    _manager.Tabulate(title);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Add tablets to the stock level for a medication
        /// </summary>
        /// <param name="parser"></param>
        private static void AddStock()
        {
            if (_parser.IsPresent(CommandLineOptionType.Add))
            {
                var values = _parser.GetValues(CommandLineOptionType.Add);
                var index = int.Parse(values[0]) - 1;
                var tablets = int.Parse(values[1]);
                _manager.AddStock(index, tablets);
                _manager.Write();
            }
        }

        /// <summary>
        /// Set the stock level for a medication
        /// </summary>
        private static void SetStock()
        {
            if (_parser.IsPresent(CommandLineOptionType.Set))
            {
                var values = _parser.GetValues(CommandLineOptionType.Set);
                var index = int.Parse(values[0]) - 1;
                var tablets = int.Parse(values[1]);
                _manager.SetStock(index, tablets);
                _manager.Write();
            }
        }

        /// <summary>
        /// Take a daily dose of all medications or a specified medication
        /// </summary>
        private static void TakeDose()
        {
            if (_parser.IsPresent(CommandLineOptionType.Take))
            {
                var values = _parser.GetValues(CommandLineOptionType.Take);
                _manager.TakeDose(values[0]);
                _manager.Write();
            }
        }

        /// <summary>
        /// Un-take a daily dose of all medications or a specified medication
        /// </summary>
        private static void UntakeDose()
        {
            if (_parser.IsPresent(CommandLineOptionType.Untake))
            {
                var values = _parser.GetValues(CommandLineOptionType.Untake);
                _manager.UntakeDose(values[0]);
                _manager.Write();
            }
        }

        /// <summary>
        /// Fast forward stock levels for one or all medications to the current date
        /// </summary>
        private static void FastForward()
        {
            if (_parser.IsPresent(CommandLineOptionType.FastForward))
            {
                var values = _parser.GetValues(CommandLineOptionType.FastForward);
                _manager.FastForward(values[0]);
                _manager.Write();
            }
        }

        /// <summary>
        /// Set the dose for a medication
        /// </summary>
        private static void SetDose()
        {
            if (_parser.IsPresent(CommandLineOptionType.Dose))
            {
                var values = _parser.GetValues(CommandLineOptionType.Dose);
                var index = int.Parse(values[0]) - 1;
                var tablets = int.Parse(values[1]);
                _manager.SetDose(index, tablets);
                _manager.Write();
            }
        }

        /// <summary>
        /// Add a new medication
        /// </summary>
        private static void AddNewMedication()
        {
            if (_parser.IsPresent(CommandLineOptionType.New))
            {
                var values = _parser.GetValues(CommandLineOptionType.New);
                _manager.AddMedication(values[0]);
                _manager.Write();
            }
        }

        /// <summary>
        /// Delete a medication
        /// </summary>
        private static void DeleteMedication()
        {
            if (_parser.IsPresent(CommandLineOptionType.Delete))
            {
                var values = _parser.GetValues(CommandLineOptionType.Delete);
                var index = int.Parse(values[0]) - 1;
                _manager.DeleteMedication(index);
                _manager.Write();
            }
        }
    }
}

using MedicineTracker.Entities.Configuration;
using MedicineTracker.Entities.Tracker;
using MedicineTracker.Entities.Interfaces;
using Spectre.Console;

namespace MedicineTracker.ConsoleUtils
{
    public class MedicationTabulator : IMedicationTabulator
    {
        private readonly ApplicationSettings _settings;
        private readonly IActionGenerator _generator;

        public MedicationTabulator(ApplicationSettings settings, IActionGenerator generator)
        {
            _settings = settings;
            _generator = generator;
        }

        /// <summary>
        /// Tabulate a collection of medications with the remaining days and last day
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="title"></param>
        public void Tabulate(IEnumerable<Medication> medications, string title)
        {
            var table = new Table();
            var counter = 0;

            table.Title(title);
            table.AddColumn("#");
            table.AddColumn("Medication");
            table.AddColumn("Stock");
            table.AddColumn("Last Taken");
            table.AddColumn("Daily Dose");
            table.AddColumn("Days Left");
            table.AddColumn("Last Day");
            table.AddColumn("Actions");

            foreach (var medication in medications)
            {
                var colour = GetRowColour(medication.DaysRemaining());

                var actions = string.Join(", ", _generator.GetActions(medication));

                var rowData = new string[] {
                    GetCellData(colour, (++counter).ToString()),
                    GetCellData(colour, medication.Name),
                    GetCellData(colour, medication.Stock.ToString()),
                    GetCellData(colour, medication.LastTaken.ToShortDateString()),
                    GetCellData(colour, medication.DailyDose.ToString()),
                    GetCellData(colour, medication.DaysRemaining().ToString()),
                    GetCellData(colour, medication.LastDay().ToShortDateString()),
                    GetCellData(colour, actions)
                };

                table.AddRow(rowData);
            }

            AnsiConsole.Write(table);
        }

        private string GetRowColour(int daysRemaining)
        {
            if (daysRemaining <= _settings.CriticalDays)
            {
                return "red";
            }
            else if (daysRemaining <= _settings.WarningDays)
            {
                return "yellow";
            }
            else
            {
                return "white";
            }
        }

        private string GetCellData(string colour, string value)
            => $"[{colour}]{value}[/]";
    }
}
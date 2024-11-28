using MedicineTracker.Entities.Configuration;
using MedicineTracker.Entities.Interfaces;
using MedicineTracker.Entities.Tracker;
using System.Diagnostics.CodeAnalysis;

namespace MedicineTracker.BusinessLogic
{
    public class MedicineManager : IMedicineManager
    {
        private readonly ApplicationSettings _settings;
        private readonly IMedicationReader _reader;
        private readonly IMedicationWriter _writer;
        private readonly IStockUpdater _stockUpdater;
        private readonly IDoseUpdater _doseUpdater;
        private readonly IMedicationUpdater _medicationUpdater;
        private readonly IMedicationTabulator _tabulator;

        private IList<Medication> _medications;

        public IReadOnlyCollection<Medication> Medications { get { return _medications?.AsReadOnly(); } }

        public MedicineManager(
            ApplicationSettings settings,
            IMedicationReader reader,
            IMedicationWriter writer,
            IStockUpdater stockUpdater,
            IDoseUpdater doseUpdater,
            IMedicationUpdater medicationUpdater,
            IMedicationTabulator tabulator)
        {
            _settings = settings;
            _reader = reader;
            _writer = writer;
            _stockUpdater = stockUpdater;
            _doseUpdater = doseUpdater;
            _medicationUpdater = medicationUpdater;
            _tabulator = tabulator;
        }

        /// <summary>
        /// Read the medications data file
        /// </summary>
        public void Read()
            => _medications = _reader.Read(_settings.DataFile);

        /// <summary>
        /// Write the medications to the data file
        /// </summary>
        public void Write()
            => _writer.Write(_medications, _settings.DataFile);

        /// <summary>
        /// Tabulate the medications
        /// </summary>
        /// <param name="title"></param>
        [ExcludeFromCodeCoverage]
        public void Tabulate(string title)
            => _tabulator.Tabulate(_medications, title);

        /// <summary>
        /// Update the stock of a medication
        /// </summary>
        /// <param name="index"></param>
        /// <param name="tablets"></param>
        public void AddStock(int index, int tablets)
            => _stockUpdater.AddStock(_medications, index, tablets);

        /// <summary>
        /// Set stock of a medication
        /// </summary>
        /// <param name="index"></param>
        /// <param name="tablets"></param>
        public void SetStock(int index, int tablets)
            => _stockUpdater.SetStock(_medications, index, tablets);

        /// <summary>
        /// Take a dose of one or all medications
        /// </summary>
        /// <param name="medication"></param>
        public void TakeDose(string medication)
        {
            if (medication == "*")
            {
                _stockUpdater.Decrement(_medications, 1);
            }
            else
            {
                var index = int.Parse(medication) - 1;
                _stockUpdater.Decrement(_medications, index, 1);
            }
        }

        /// <summary>
        /// Un-take a dose of one or all medications
        /// </summary>
        /// <param name="medication"></param>
        public void UntakeDose(string medication)
        {
            if (medication == "*")
            {
                _stockUpdater.Increment(_medications, 1);
            }
            else
            {
                var index = int.Parse(medication) - 1;
                _stockUpdater.Increment(_medications, index, 1);
            }
        }

        /// <summary>
        /// Fast-forward stock levels for one or all medications
        /// </summary>
        /// <param name="medication"></param>
        public void FastForward(string medication)
        {
            if (medication == "*")
            {
                _stockUpdater.FastForward(_medications);
            }
            else
            {
                var index = int.Parse(medication) - 1;
                _stockUpdater.FastForward(_medications, index);
            }
        }

        /// <summary>
        /// Set the daily dose for a medication
        /// </summary>
        /// <param name="index"></param>
        /// <param name="tablets"></param>
        public void SetDose(int index, int tablets)
            => _doseUpdater.SetDose(_medications, index, tablets);

        /// <summary>
        /// Add a new medication with default stock and dose levels
        /// </summary>
        /// <param name="name"></param>
        public void AddMedication(string name)
            => _medicationUpdater.AddMedication(_medications, name);

        /// <summary>
        /// Delete a medication
        /// </summary>
        /// <param name="index"></param>
        public void DeleteMedication(int index)
            => _medicationUpdater.DeleteMedication(_medications, index);
    }
}

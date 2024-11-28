using MedicineTracker.Entities.Tracker;
using MedicineTracker.Entities.Interfaces;
using System.Text.Json;

namespace MedicineTracker.BusinessLogic.Storage
{
    public class MedicationWriter : IMedicationWriter
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };

        /// <summary>
        /// Write a collection of medications to a file in JSON format
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="path"></param>
        public void Write(IEnumerable<Medication> medications, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                var json = JsonSerializer.Serialize(medications, _options);
                writer.Write(json);
            }
        }
    }
}
using MedicineTracker.Entities.Tracker;
using MedicineTracker.Entities.Interfaces;
using System.Text.Json;

namespace MedicineTracker.BusinessLogic.Storage
{
    public class MedicationReader : IMedicationReader
    {
        /// <summary>
        /// Read a collection of medications from a JSON formatted file 
        /// </summary>
        /// <param name="medications"></param>
        /// <param name="path"></param>
        public IList<Medication> Read(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var content = reader.ReadToEnd();
                return JsonSerializer.Deserialize<IEnumerable<Medication>>(content)
                                     .OrderBy(x => x.Name).ToList();
            }
        }
    }
}
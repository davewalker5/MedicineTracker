using System.Diagnostics.CodeAnalysis;

namespace MedicineTracker.Entities.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MedicationAlreadyExistsException : Exception
    {
        public MedicationAlreadyExistsException()
        {
        }

        public MedicationAlreadyExistsException(string message) : base(message)
        {
        }

        public MedicationAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
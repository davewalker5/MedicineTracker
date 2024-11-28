using System.Diagnostics.CodeAnalysis;

namespace MedicineTracker.Entities.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class TooManyValuesException : Exception
    {
        public TooManyValuesException()
        {
        }

        public TooManyValuesException(string message) : base(message)
        {
        }

        public TooManyValuesException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
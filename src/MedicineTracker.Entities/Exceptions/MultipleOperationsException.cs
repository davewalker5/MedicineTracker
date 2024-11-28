using System.Diagnostics.CodeAnalysis;

namespace MedicineTracker.Entities.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MultipleOperationsException : Exception
    {
        public MultipleOperationsException()
        {
        }

        public MultipleOperationsException(string message) : base(message)
        {
        }

        public MultipleOperationsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}


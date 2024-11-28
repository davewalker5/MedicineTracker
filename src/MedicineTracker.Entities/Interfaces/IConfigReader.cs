namespace MedicineTracker.Entities.Interfaces
{
    public interface IConfigReader<T> where T : class, new()
    {
        T Read();
    }
}
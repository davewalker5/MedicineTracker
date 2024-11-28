using MedicineTracker.Entities.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MedicineTracker.BusinessLogic.Configuration
{
    /// <summary>
    /// Read the application settings into an object of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigReader<T> : IConfigReader<T> where T : class, new()
    {
        public T Read()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false);

            IConfigurationRoot configuration = builder.Build();
            T settings = new();
            configuration.GetSection("ApplicationSettings").Bind(settings);

            return settings;
        }
    }
}
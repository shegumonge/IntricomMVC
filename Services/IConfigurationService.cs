using IntricomMVC.DTO;
using IntricomMVC.Models;

namespace IntricomMVC.Services
{
    public interface IConfigurationService
    {
        Task<Configuration> GetConfigurationAsync(int id);
        Task<IEnumerable<Configuration>> GetConfigurationsAsync();
        Task<Configuration> PostConfigurationAsync(Configuration configuration);
        Task<bool> PutConfigurationAsync(Configuration configuration);

    }
}

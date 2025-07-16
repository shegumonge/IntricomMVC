using IntricomMVC.DTO;
using IntricomMVC.Models;

namespace IntricomMVC.Services
{
    public interface IClientService
    {
        Task<Client> GetClientAsync(int id);
        Task<IEnumerable<Client>> GetClientsAsync();
        Task<Client> PostClientAsync(ClientDTO itemDTO);
        Task<bool> PutClientAsync(int id, ClientDTO itemDTO);
        Task<bool> DeleteClientAsync(int id);


    }
}

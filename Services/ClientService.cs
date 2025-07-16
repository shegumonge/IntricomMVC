using AutoMapper;
using IntricomMVC.Data;
using IntricomMVC.DTO;
using IntricomMVC.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IntricomMVC.Services
{

    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IConfigurationService _configurationService;

        public ClientService(
                            ApplicationDbContext context,
                            IHttpContextAccessor httpContextAccessor,
                            IConfigurationService configurationService,
                            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configurationService = configurationService;
            _mapper = mapper;
        }

        
        public async Task<Client> GetClientAsync(int id)
        {
            try
            {
                var item = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id);
                //var itemDTO = _mapper.Map<ClientDTO>(item);
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            try
            {
                var items = await _context.Clients.ToListAsync();
                //var itemListDTO = _mapper.Map<IEnumerable<ClientDTO>>(items);
                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public async Task<Client> PostClientAsync(ClientDTO itemDTO)
        {
            try
            {
                //aquí validamos el tipo de repositorio system file o DAtabase
                var config = await _context.Configurations.FirstOrDefaultAsync();
                if (config != null)
                {
                    itemDTO.CreateDate = DateTime.UtcNow;
                    var item = _mapper.Map<Client>(itemDTO);
                    if (config.DataType == "DB")
                    {
                        //En caso de database
                        await _context.Clients.AddAsync(item);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //En caso de guardado en file
                        string basePath = @"C:\IntricomDB";
                        FileSystemRepository.SaveEntityToFileSystem(item, basePath);
                    }
                    return item;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> PutClientAsync(int id, ClientDTO itemDTO)
        {
            try
            {
                //En caso de database
                var itemExist = await _context.Clients.AnyAsync(x => x.Id == id);
                if (!itemExist) return false;

                var item = _mapper.Map<Client>(itemDTO);
                item.Id = id;
                _context.Update(item);
                await _context.SaveChangesAsync();


                //En caso de guardado en file
                string basePath = @"C:\IntricomDB";
                FileSystemRepository.SaveEntityToFileSystem(item, basePath);


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            try
            {
                var item = await _context.Clients.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (item == null) return false;

                _context.Clients.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}

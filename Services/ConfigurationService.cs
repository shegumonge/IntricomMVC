using AutoMapper;
using IntricomMVC.Data;
using IntricomMVC.DTO;
using IntricomMVC.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IntricomMVC.Services
{

    public class ConfigurationService : IConfigurationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        
        public ConfigurationService(
                            ApplicationDbContext context,
                            IHttpContextAccessor httpContextAccessor,
                            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        
        public async Task<Configuration> GetConfigurationAsync(int id)
        {
            try
            {
                var item = await _context.Configurations.FirstOrDefaultAsync(x => x.Id == id);
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<Configuration>> GetConfigurationsAsync()
        {
            try
            {
                var items = await _context.Configurations.ToListAsync();
                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Configuration> PostConfigurationAsync(Configuration configuration)
        {
            try
            {
                //itemDTO.CreateDate = DateTime.UtcNow;
                //var item = _mapper.Map<Client>(itemDTO);
                await _context.Configurations.AddAsync(configuration);
                await _context.SaveChangesAsync();

                //string basePath = @"C:\IntricomDB";
                //FileSystemRepository.SaveEntityToFileSystem(item, basePath);

                return configuration;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> PutConfigurationAsync(Configuration configuration)
        {
            try
            {
                var itemExist = await _context.Clients.AnyAsync(x => x.Id == configuration.Id);
                if (!itemExist) return false;

                _context.Update(configuration);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       

    }
}

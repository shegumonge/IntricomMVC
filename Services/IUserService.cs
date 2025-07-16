
using Microsoft.AspNetCore.Identity;

namespace IntricomMVC.Services
{
    public interface IUserService
    {
        Task<IdentityUser?> GetUser();
    }
}
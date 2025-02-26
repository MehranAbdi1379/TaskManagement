using TaskManagement.Domain.Models;

namespace TaskManagement.Service.Services
{
    public interface IJwtService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
using TaskManagement.Domain.Models;

namespace TaskManagement.Shared.ServiceInterfaces
{
    public interface IJwtService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
using System.Security.Claims;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.API.Services
{
    public class UserContextWeb : IUserContext
    {
        public int UserId { get; }

        public UserContextWeb(IHttpContextAccessor httpContextAccessor)
        {
            UserId = int.Parse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}

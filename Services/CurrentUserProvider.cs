using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebPulse_WebManager.Models;

namespace WebPulse_WebManager.Services
{
    public interface ICurrentUserProvider
    {
        ApplicationUser? CurrentUser { get; }
    }

    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public ApplicationUser? CurrentUser
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return !string.IsNullOrEmpty(userId) ? _userManager.FindByIdAsync(userId).Result : null;
            }
        }
    }
}

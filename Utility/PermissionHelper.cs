using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WebPulse_WebManager.Data;
using WebPulse_WebManager.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WebPulse_WebManager.Utility
{
    public class PermissionHelper
    {
        private UserManager<ApplicationUser>? _userManager;
        private IHttpContextAccessor? _httpContextAccessor;

        // Constructor to initialize the UserManager and IHttpContextAccessor
        public PermissionHelper(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> IsUserGlobalAdminOrOwnerAsync()
        {
            if (_userManager == null || _httpContextAccessor == null)
            {
                return false;
            }

            var u = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByIdAsync(u);

            if (user == null)
            {
                return false;
            }

            return await _userManager.IsInRoleAsync(user, "GlobalAdmin") || await _userManager.IsInRoleAsync(user, "SystemOwner");
        }

        public async Task<bool> IsGodModeOn()
        {
            if (_userManager == null || _httpContextAccessor == null)
            {
                return false;
            }

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {

                // Check if GodView is true
                return user.GodView;
            }

            return false;
        }
    }
}

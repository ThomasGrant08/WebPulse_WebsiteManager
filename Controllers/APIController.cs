using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebPulse_WebManager.Data;
using WebPulse_WebManager.Models;

namespace WebPulse_WebManager.Controllers
{
    [ApiController]
    [Route("api")] // Updated the route to a more standard format
    public class APIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public APIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("ToggleGodMode")]
        [Authorize(Roles = "SystemOwner, GlobalAdmin")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ToggleGodMode()
        {
            // Get the current user
            ApplicationUser user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            // Toggle GodView
            user.GodView = !user.GodView;
            _context.Update(user);
            await _context.SaveChangesAsync();

            // Update the user in the database
            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await _context.SaveChangesAsync();
                return Accepted();
            }
            else
            {
                return BadRequest("Failed to update user");
            }
        }
    }


}

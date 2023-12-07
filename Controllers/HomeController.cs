using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using WebPulse_WebManager.Data;
using WebPulse_WebManager.Models;
using WebPulse_WebManager.Repositories;
using WebPulse_WebManager.Utility;
using WebPulse_WebManager.ViewModels;

namespace WebPulse_WebManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly GroupRepository _groupRepository;
        private readonly PermissionHelper _permissionHelper;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, PermissionHelper permissionHelper)
        {
            _logger = logger;
            _context = context;
            _groupRepository = new GroupRepository(_context);
            _permissionHelper = permissionHelper;
        }

        public async Task<IActionResult> Index()
        {
            DashboardIndexViewModel model = new DashboardIndexViewModel();
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (await _permissionHelper.IsUserGlobalAdminOrOwnerAsync() && await _permissionHelper.IsGodModeOn())
            {
                model.Groups = _groupRepository.FindAll().ToList();
            }
            else
            {
                Func<Group, bool> userAssignedFilter = group => group.AssignedUsers.Any(user => user.Id == currentUserId);
                model.Groups = _groupRepository.FindAll(filter: userAssignedFilter).ToList();
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebPulse_WebManager.Data;
using WebPulse_WebManager.Models;
using WebPulse_WebManager.Repositories;
using WebPulse_WebManager.Utility;
using WebPulse_WebManager.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebPulse_WebManager.Controllers
{
    public class WebsitesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly WebsiteRepository _websiteRepository;
        private readonly GroupRepository _groupRepository;
        private readonly PermissionHelper _permissionHelper;

        public WebsitesController(ApplicationDbContext context, PermissionHelper permissionHelper)
        {
            _context = context;
            _websiteRepository = new WebsiteRepository(_context);
            _groupRepository = new GroupRepository(_context);
            _permissionHelper = permissionHelper;
        }

        // GET: Websites
        public async Task<IActionResult> Index()
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var viewModel = new WebsiteIndexViewMode();

            if (await _permissionHelper.IsUserGlobalAdminOrOwnerAsync() && await _permissionHelper.IsGodModeOn())
            {
                viewModel.Websites = _websiteRepository.FindAll().ToList();
            }
            else
            {
                Func<Website, bool> userAssignedFilter = website => website.Users.Any(user => user.Id == currentUserId);
                viewModel.Websites = _websiteRepository.FindAll(filter: userAssignedFilter).ToList();
            }

            return View(viewModel);
        }

        // GET: Websites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Website == null)
            {
                return NotFound();
            }

            var website = await _websiteRepository.FindById(id.Value);
            if (website == null)
            {
                return NotFound();
            }

            return View(website);
        }

        // GET: Websites/Create
        public IActionResult Create()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ViewData["GroupId"] = new SelectList(_groupRepository.FindAll(filter: group => group.AssignedUsers.Any(user => user.Id == currentUserId)), "Id", "Name");
            return View();
        }

        // POST: Websites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Website website)
        {

            ModelState.Remove<Website>(w => w.CreatedAt);
            ModelState.Remove<Website>(w => w.LastUpdatedAt);
            ModelState.Remove<Website>(w => w.Group);
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (ModelState.IsValid)
            {

                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail);

                if(user != null)
                {
                    website.Users.Add(user);
                } else
                {
                    return BadRequest();
                }
                website = await _websiteRepository.Insert(website);
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_groupRepository.FindAll(filter: group => group.AssignedUsers.Any(user => user.Id == currentUserId)), "Id", "Name");
            return View(website);
        }

        // GET: Websites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null || _context.Website == null)
            {
                return NotFound();
            }

            var website = await _websiteRepository.FindById(id.Value);
            if (website == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_groupRepository.FindAll(filter: group => group.AssignedUsers.Any(user => user.Id == currentUserId)), "Id", "Name");
            return View(website);
        }

        // POST: Websites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Website website)
        {
            if (id != website.Id)
            {
                return NotFound();
            }

            ModelState.Remove<Website>(w => w.CreatedAt);
            ModelState.Remove<Website>(w => w.LastUpdatedAt);
            ModelState.Remove<Website>(w => w.Group);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(website);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WebsiteExists(website.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["GroupId"] = new SelectList(_groupRepository.FindAll(filter: group => group.AssignedUsers.Any(user => user.Id == currentUserId)), "Id", "Name");
            return View(website);
        }

        // GET: Websites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Website == null)
            {
                return NotFound();
            }

            var website = await _websiteRepository.FindById(id.Value);
            if (website == null)
            {
                return NotFound();
            }

            return View(website);
        }

        // POST: Websites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Website == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Website'  is null.");
            }
            var website = await _websiteRepository.FindById(id);
            if (website != null)
            {
                await _websiteRepository.Delete(website);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool WebsiteExists(int id)
        {
          return (_context.Website?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

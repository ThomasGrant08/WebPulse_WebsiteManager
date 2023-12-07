using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebPulse_WebManager.Data;
using WebPulse_WebManager.Models;
using WebPulse_WebManager.Repositories;
using WebPulse_WebManager.Utility;
using WebPulse_WebManager.ViewModels;

namespace WebPulse_WebManager.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GroupRepository _groupRepository;
        private readonly PermissionHelper _permissionHelper; // Add PermissionHelper as a dependency

        public GroupsController(ApplicationDbContext context, PermissionHelper permissionHelper)
        {
            _context = context;
            _groupRepository = new GroupRepository(_context);
            _permissionHelper = permissionHelper;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            GroupIndexViewModel model = new GroupIndexViewModel();

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

        //GET: Groups/Dashboard/5
        public async Task<IActionResult> Dashboard(int? id)
        {
            if (id == null || _context.Group == null)
            {
                return NotFound();
            }

            var group = await _groupRepository.FindById(id.Value);
            if (group == null)
            {
                return NotFound();
            }

            GroupViewModel model = new GroupViewModel()
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                AssignedUsers = group.AssignedUsers,
                Websites = group.Websites
            };

            return View(model);
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Group == null)
            {
                return NotFound();
            }

            var group = await _context.Group
                .FirstOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Group group)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail);

                if (user != null)
                {
                    group.AssignedUsers.Add(user);
                }
                else
                {
                    return BadRequest();
                }
                group = await _groupRepository.Insert(group);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                Debug.WriteLine("ModelState is invalid.");
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var errors = ModelState[modelStateKey].Errors;
                    foreach (var error in errors)
                    {
                        Debug.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
            }
            return View();
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Group == null)
            {
                return NotFound();
            }

            var group = await _context.Group.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Id,CreatedAt,LastUpdatedAt,DeletedAt")] Group group)
        {
            if (id != group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.Id))
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
            return View(group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Group == null)
            {
                return NotFound();
            }

            var group = await _context.Group
                .FirstOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Group == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Group' is null.");
            }
            var group = await _context.Group.FindAsync(id);
            if (group != null)
            {
                group.DeletedAt = DateTime.Now;
                _context.Update(group);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return (_context.Group?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InviteUser(string email, int id)
        {
            if (email == null || id == 0)
            {
                return BadRequest();
            }

            var group = await _groupRepository.FindById(id);

            if (group == null)
            {
                return BadRequest();
            }

            var user = await _context.Users.Where(u => u.Email == email)?.FirstOrDefaultAsync();

            if (user != null)
            {
                group.AssignedUsers.Add(user);
                _context.Update(group);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Dashboard), new { id = id });
        }
    }
}

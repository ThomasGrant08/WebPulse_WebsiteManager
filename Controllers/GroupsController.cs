using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        private readonly PermissionHelper _permissionHelper;

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
                Websites = group.Websites,
                Banner = group.Image != null ? Convert.ToBase64String(group.Image) : null
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

            var group = await _groupRepository.FindById(id.Value);
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
        public async Task<IActionResult> Create(GroupFormViewModel newGroup)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail);

                var group = new Group()
                {
                    Name = newGroup.Name,
                    Description = newGroup.Description,
                    Image = ImageUtilities.EncodeImageToBytes(newGroup.Image)
                };

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
            return View(newGroup);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            GroupFormViewModel viewModel = new GroupFormViewModel()
            {
                Name = group.Name,
                Description = group.Description,
            };

            ViewBag.Image = group.Image != null ? Convert.ToBase64String(group.Image) : "";

            return View(viewModel);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GroupFormViewModel newGroup)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var updatedGroup = await _groupRepository.FindById(id);

                    updatedGroup.Name = newGroup.Name;
                    updatedGroup.Description = newGroup.Description;

                    if(newGroup.Image != null)
                    {
                        updatedGroup.Image = ImageUtilities.EncodeImageToBytes(newGroup.Image);
                    }

                    

                    if(await _groupRepository.Update(updatedGroup))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return Problem("Failed to update group.");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(errors);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
            var group = await _groupRepository.FindById(id);
            if (group != null)
            {
                if(await _groupRepository.Delete(group))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Problem("Failed to delete group.");
                }
            }
            return BadRequest();
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
                if(await _groupRepository.Update(group))
                {
                    return RedirectToAction(nameof(Dashboard), new { id = id });
                }
                else
                {
                    return Problem("Failed to update group.");
                }
            }
            return BadRequest();
        }
    }
}

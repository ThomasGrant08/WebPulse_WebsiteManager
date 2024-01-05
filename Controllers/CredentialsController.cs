using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WebPulse_WebManager.Controllers
{
    public class CredentialsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CredentialRepository _credentialRepository;
        private readonly WebsiteRepository _websiteRepository;
        private readonly PermissionHelper _permissionHelper;

        public CredentialsController(ApplicationDbContext context, PermissionHelper permissionHelper)
        {
            _context = context;
            _credentialRepository = new CredentialRepository(_context);
            _websiteRepository = new WebsiteRepository(_context);
            _permissionHelper = permissionHelper;
        }

        // GET: Credentials
        public async Task<IActionResult> Index()
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            CredentialsIndexViewModel model = new CredentialsIndexViewModel();

            if (await _permissionHelper.IsUserGlobalAdminOrOwnerAsync() && await _permissionHelper.IsGodModeOn())
            {
                model.Credentials = _credentialRepository.FindAll().ToList();
            }
            else
            {
                Func<Credential, bool> userAssignedFilter = credential => credential.AssignedUsers.Any(user => user.Id == currentUserId);
                model.Credentials = _credentialRepository.FindAll(filter: userAssignedFilter).ToList();
            }

            return View(model);
        }

        // GET: Credentials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Credential == null)
            {
                return NotFound();
            }

            var credential = await _credentialRepository.FindById(id.Value);
            if (credential == null)
            {
                return NotFound();
            }

            return View(credential);
        }

        // GET: Credentials/Create
        public IActionResult Create(int? group)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Func<Website, bool> assignedFilter = website => website.Users.Any(user => user.Id == currentUserId);
            if (group != null)
            {
                assignedFilter = website => website.Users.Any(user => user.Id == currentUserId) && website.Group.Id == group;
            }

            ViewData["WebsiteId"] = new SelectList(_websiteRepository.FindAll(filter: assignedFilter), "Id", "Name");
            return View();
        }

        // POST: Credentials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Credential credential)
        {
            ModelState.Remove<Credential>(c => c.CreatedAt);
            ModelState.Remove<Credential>(c => c.LastUpdatedAt);
            ModelState.Remove<Credential>(c => c.Website);
            ModelState.Remove<Credential>(c => c.Valid);

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail);

                if (user != null)
                {
                    credential.AssignedUsers.Add(user);
                }
                else
                {
                    return BadRequest();
                }
                await _credentialRepository.Insert(credential);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Debug.WriteLine("ModelState is invalid.");
                foreach (var modelStateKey in ModelState.Keys)
                {
                    foreach (var error in ModelState[modelStateKey].Errors)
                    {
                        Debug.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
            }

            Func<Website, bool> assignedFilter = website => website.Users.Any(user => user.Id == currentUserId);
            ViewData["WebsiteId"] = new SelectList(_websiteRepository.FindAll(filter: assignedFilter), "Id", "Name", credential.WebsiteId);


            return View(credential);
        }

        // GET: Credentials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Credential == null)
            {
                return NotFound();
            }

            var credential = await _credentialRepository.FindById(id.Value);
            if (credential == null)
            {
                return NotFound();
            }
            ViewData["WebsiteId"] = new SelectList(_context.Website, "Id", "Name", credential.WebsiteId);
            return View(credential);
        }

        // POST: Credentials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Credential credential)
        {
            if (id != credential.Id)
            {
                return NotFound();
            }

            ModelState.Remove<Credential>(c => c.CreatedAt);
            ModelState.Remove<Credential>(c => c.LastUpdatedAt);
            ModelState.Remove<Credential>(c => c.Website);
            ModelState.Remove<Credential>(c => c.Valid);

            if (ModelState.IsValid)
            {
                try
                {
                    var updateCredential = await _credentialRepository.FindById(id);

                    if(updateCredential != null)
                    {
                        updateCredential.Title = credential.Title;
                        updateCredential.Username = credential.Username;
                        updateCredential.Password = credential.Password;
                        updateCredential.Description = credential.Description;
                        updateCredential.WebsiteId = credential.WebsiteId;
                        updateCredential.Valid = true;
                        
                    } else
                    {
                        return BadRequest();
                    }

                    

                    if (await _credentialRepository.Update(updateCredential))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return Problem("Failed to update credential.");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CredentialExists(credential.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            else
            {
                Debug.WriteLine("ModelState is invalid.");
                foreach (var modelStateKey in ModelState.Keys)
                {
                    foreach (var error in ModelState[modelStateKey].Errors)
                    {
                        Debug.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
            }
            ViewData["WebsiteId"] = new SelectList(_context.Website, "Id", "Name", credential.WebsiteId);
            return View(credential);
        }

        // GET: Credentials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Credential == null)
            {
                return NotFound();
            }

            var credential = await _credentialRepository.FindById(id.Value);
            if (credential == null)
            {
                return NotFound();
            }

            return View(credential);
        }

        // POST: Credentials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Credential == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Credential'  is null.");
            }
            var credential = await _credentialRepository.FindById(id);
            if (credential != null)
            {
                if (await _credentialRepository.Delete(credential))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Problem("Failed to delete credential.");
                }
            }
            return BadRequest();
        }

        public async Task<IActionResult> Validity(int id)
        {
            if (_context.Credential == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Credential'  is null.");
            }

            var credential = await _credentialRepository.FindById(id);

            if (credential == null)
            {
                return NotFound();
            }

            return View(credential);
        }

        [HttpPost, ActionName("MarkInvalid")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidityConfirmed(int id, bool valid = false)
        {
            if (_context.Credential == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Credential'  is null.");
            }

            var updateCredential = await _credentialRepository.FindById(id);

            if (updateCredential != null)
            {
                updateCredential.Valid = valid;

                if (await _credentialRepository.Update(updateCredential))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Problem("Failed to update credential validity.");
                }

            }
            return BadRequest();

        }

        private bool CredentialExists(int id)
        {
            return (_context.Credential?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

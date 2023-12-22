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
using WebPulse_WebManager.ViewModels;

namespace WebPulse_WebManager.Controllers
{
    public class CredentialsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CredentialRepository _credentialRepository;

        public CredentialsController(ApplicationDbContext context)
        {
            _context = context;
            _credentialRepository = new CredentialRepository(_context);
        }

        // GET: Credentials
        public async Task<IActionResult> Index()
        {
            CredentialsIndexViewModel model = new CredentialsIndexViewModel()
            {
                Credentials = _credentialRepository.FindAll().ToList()
            };

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
        public IActionResult Create()
        {
            ViewData["WebsiteId"] = new SelectList(_context.Website, "Id", "Id");
            return View();
        }

        // POST: Credentials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Password,WebsiteId,Id,CreatedAt,LastUpdatedAt,DeletedAt")] Credential credential)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail);

                if(user != null)
                {
                    credential.AssignedUsers.Add(user);
                }
                else
                {
                    return BadRequest();
                }
                await _credentialRepository.Insert(credential);
                return RedirectToAction(nameof(Index));
            } else
            {
                Debug.WriteLine("ModelState is invalid.");
                foreach (var modelStateKey in ModelState.Keys)
                {
                    foreach(var error in ModelState[modelStateKey].Errors)
                    {
                        Debug.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
            }
            ViewData["WebsiteId"] = new SelectList(_context.Website, "Id", "Id", credential.WebsiteId);
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
            ViewData["WebsiteId"] = new SelectList(_context.Website, "Id", "Id", credential.WebsiteId);
            return View(credential);
        }

        // POST: Credentials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Username,Password,WebsiteId,Id,CreatedAt,LastUpdatedAt,DeletedAt")] Credential credential)
        {
            if (id != credential.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(await _credentialRepository.Update(credential))
                    {
                        return RedirectToAction(nameof(Index));
                    } else
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
            ViewData["WebsiteId"] = new SelectList(_context.Website, "Id", "Id", credential.WebsiteId);
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
                if(await _credentialRepository.Delete(credential))
                {
                    return RedirectToAction(nameof(Index));
                } else
                {
                    return Problem("Failed to delete credential.");
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

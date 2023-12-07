using System;
using System.Collections.Generic;
using System.Linq;
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

            var credential = await _context.Credential
                .Include(c => c.Website)
                .FirstOrDefaultAsync(m => m.Id == id);
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
                _context.Add(credential);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

            var credential = await _context.Credential.FindAsync(id);
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
                    _context.Update(credential);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
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

            var credential = await _context.Credential
                .Include(c => c.Website)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var credential = await _context.Credential.FindAsync(id);
            if (credential != null)
            {
                _context.Credential.Remove(credential);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CredentialExists(int id)
        {
          return (_context.Credential?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

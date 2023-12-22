using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebPulse_WebManager.Data;
using WebPulse_WebManager.Models;
using WebPulse_WebManager.Repositories;

namespace WebPulse_WebManager.Controllers
{
    public class WebsitesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly WebsiteRepository _websiteRepository;

        public WebsitesController(ApplicationDbContext context)
        {
            _context = context;
            _websiteRepository = new WebsiteRepository(_context);
        }

        // GET: Websites
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Website.Include(w => w.Group);
            return View(await applicationDbContext.ToListAsync());
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
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "Id");
            return View();
        }

        // POST: Websites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Url,GroupId,Id,CreatedAt,LastUpdatedAt,DeletedAt")] Website website)
        {
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
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "Id", website.GroupId);
            return View(website);
        }

        // GET: Websites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Website == null)
            {
                return NotFound();
            }

            var website = await _context.Website.FindAsync(id);
            if (website == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "Id", website.GroupId);
            return View(website);
        }

        // POST: Websites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Url,GroupId,Id,CreatedAt,LastUpdatedAt,DeletedAt")] Website website)
        {
            if (id != website.Id)
            {
                return NotFound();
            }

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
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "Id", website.GroupId);
            return View(website);
        }

        // GET: Websites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Website == null)
            {
                return NotFound();
            }

            var website = await _context.Website
                .Include(w => w.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var website = await _context.Website.FindAsync(id);
            if (website != null)
            {
                _context.Website.Remove(website);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WebsiteExists(int id)
        {
          return (_context.Website?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

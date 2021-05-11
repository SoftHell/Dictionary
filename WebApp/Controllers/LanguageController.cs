using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Controllers
{
    public class LanguageController : Controller
    {
        private readonly AppDbContext _context;

        public LanguageController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Language
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Languages
                .Include(l => l.Name)
                .ThenInclude(x => x.Translations);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Language/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Languages
                .Include(l => l.Name)
                .ThenInclude(x => x.Translations)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // GET: Language/Create
        public IActionResult Create()
        {
            ViewData["NameId"] = new SelectList(_context.LangStrings, "Id", "Id");
            return View();
        }

        // POST: Language/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameId")] Language language)
        {
            if (ModelState.IsValid)
            {
                language.Id = Guid.NewGuid();
                _context.Add(language);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NameId"] = new SelectList(_context.LangStrings, "Id", "Id", language.NameId);
            return View(language);
        }

        // GET: Language/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Languages
                .FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }
            ViewData["NameId"] = new SelectList(_context.LangStrings, "Id", "Id", language.Name);
            return View(language);
        }

        // POST: Language/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,NameId")] Language language)
        {
            if (id != language.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(language);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanguageExists(language.Id))
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
            ViewData["NameId"] = new SelectList(_context.LangStrings, "Id", "Id", language.Name);
            return View(language);
        }

        // GET: Language/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Languages
                .Include(l => l.Name)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // POST: Language/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var language = await _context.Languages.FindAsync(id);
            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LanguageExists(Guid id)
        {
            return _context.Languages.Any(e => e.Id == id);
        }
    }
}

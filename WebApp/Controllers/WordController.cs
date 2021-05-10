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
    public class WordController : Controller
    {
        private readonly AppDbContext _context;

        public WordController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Word
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Words.Include(w => w.QueryWord);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Word/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await _context.Words
                .Include(w => w.QueryWord)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }

        // GET: Word/Create
        public IActionResult Create()
        {
            ViewData["QueryWordId"] = new SelectList(_context.Words, "Id", "Value");
            return View();
        }

        // POST: Word/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Value,Example,Explanation,Pronunciation,QueryWordId")] Word word)
        {
            if (ModelState.IsValid)
            {
                word.Id = Guid.NewGuid();
                _context.Add(word);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QueryWordId"] = new SelectList(_context.Words, "Id", "Value", word.QueryWordId);
            return View(word);
        }

        // GET: Word/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await _context.Words.FindAsync(id);
            if (word == null)
            {
                return NotFound();
            }
            ViewData["QueryWordId"] = new SelectList(_context.Words, "Id", "Value", word.QueryWordId);
            return View(word);
        }

        // POST: Word/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Value,Example,Explanation,Pronunciation,QueryWordId")] Word word)
        {
            if (id != word.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(word);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WordExists(word.Id))
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
            ViewData["QueryWordId"] = new SelectList(_context.Words, "Id", "Value", word.QueryWordId);
            return View(word);
        }

        // GET: Word/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await _context.Words
                .Include(w => w.QueryWord)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }

        // POST: Word/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var word = await _context.Words.FindAsync(id);
            _context.Words.Remove(word);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WordExists(Guid id)
        {
            return _context.Words.Any(e => e.Id == id);
        }
    }
}

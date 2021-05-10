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
    public class PartOfSpeechController : Controller
    {
        private readonly AppDbContext _context;

        public PartOfSpeechController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PartOfSpeech
        public async Task<IActionResult> Index()
        {
            return View(await _context.PartsOfSpeech.ToListAsync());
        }

        // GET: PartOfSpeech/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partOfSpeech = await _context.PartsOfSpeech
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partOfSpeech == null)
            {
                return NotFound();
            }

            return View(partOfSpeech);
        }

        // GET: PartOfSpeech/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PartOfSpeech/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] PartOfSpeech partOfSpeech)
        {
            if (ModelState.IsValid)
            {
                partOfSpeech.Id = Guid.NewGuid();
                _context.Add(partOfSpeech);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partOfSpeech);
        }

        // GET: PartOfSpeech/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partOfSpeech = await _context.PartsOfSpeech.FindAsync(id);
            if (partOfSpeech == null)
            {
                return NotFound();
            }
            return View(partOfSpeech);
        }

        // POST: PartOfSpeech/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] PartOfSpeech partOfSpeech)
        {
            if (id != partOfSpeech.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partOfSpeech);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartOfSpeechExists(partOfSpeech.Id))
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
            return View(partOfSpeech);
        }

        // GET: PartOfSpeech/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partOfSpeech = await _context.PartsOfSpeech
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partOfSpeech == null)
            {
                return NotFound();
            }

            return View(partOfSpeech);
        }

        // POST: PartOfSpeech/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var partOfSpeech = await _context.PartsOfSpeech.FindAsync(id);
            _context.PartsOfSpeech.Remove(partOfSpeech);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartOfSpeechExists(Guid id)
        {
            return _context.PartsOfSpeech.Any(e => e.Id == id);
        }
    }
}

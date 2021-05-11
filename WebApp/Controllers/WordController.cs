using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using WebApp.ViewModels;

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
        public async Task<IActionResult> Create(WordViewModel? viewModel)
        {
            if (viewModel == null)
            {
                var vm = new WordViewModel()
                {
                    TopicSelectList =
                        new SelectList(await _context.Topics
                                .Include(t => t.Name)
                                .ThenInclude(n => n.Translations)
                                .ToListAsync(),
                            nameof(Topic.Id), nameof(Topic.Name)),

                    PartOfSpeechSelectList =
                        new SelectList(await _context.PartsOfSpeech
                                .Include(t => t.Name)
                                .ThenInclude(n => n.Translations)
                                .ToListAsync(),
                            nameof(PartOfSpeech.Id), nameof(PartOfSpeech.Name)),
                };
                return View(vm);
            }

            viewModel.TopicSelectList =
                new SelectList(await _context.Topics
                        .Include(t => t.Name)
                        .ThenInclude(n => n.Translations)
                        .ToListAsync(),
                    nameof(Topic.Id), nameof(Topic.Name));

            viewModel.PartOfSpeechSelectList =
                new SelectList(await _context.PartsOfSpeech
                        .Include(t => t.Name)
                        .ThenInclude(n => n.Translations)
                        .ToListAsync(),
                    nameof(PartOfSpeech.Id), nameof(PartOfSpeech.Name));
            
            return View(viewModel);
        }

        // GET: Word/AddEquivalent
        public async Task<IActionResult> AddEquivalent(WordViewModel vm, string equivalent)
        {
            vm.Equivalents!.Add(equivalent);
            
            vm.TopicSelectList =
                new SelectList(await _context.Topics
                        .Include(t => t.Name)
                        .ThenInclude(n => n.Translations)
                        .ToListAsync(),
                    nameof(Topic.Id), nameof(Topic.Name), nameof(vm.Word.TopicId));

            vm.PartOfSpeechSelectList =
                new SelectList(await _context.PartsOfSpeech
                        .Include(t => t.Name)
                        .ThenInclude(n => n.Translations)
                        .ToListAsync(),
                    nameof(PartOfSpeech.Id), nameof(PartOfSpeech.Name), nameof(vm.Word.PartOfSpeechId));

            return RedirectToAction(nameof(Create), new {viewModel = vm});
            
        }
       

        // POST: Word/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWord(WordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                
                /*if (vm.LanguageName == (int) ELanguage.En)
                {
                    var equivalent = new Word()
                    {
                        Value = vm.EstonianValue,
                        Language = ELanguage.Et,
                        PartOfSpeechId = vm.Word!.PartOfSpeechId,
                        TopicId = vm.Word!.TopicId
                    };
                    var savedEquivalent = await _context.Words.AddAsync(equivalent);
                    
                    var word = vm.Word;
                    word!.Value = vm.Value;
                    word.QueryWordId = savedEquivalent.Entity.Id;

                }*/
                
                _context.Add(vm.Word);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.TopicSelectList =
                new SelectList(await _context.Topics
                        .Include(t => t.Name)
                        .ThenInclude(n => n.Translations)
                        .ToListAsync(),
                    nameof(Topic.Id), nameof(Topic.Name));

            vm.PartOfSpeechSelectList =
                new SelectList(await _context.PartsOfSpeech
                        .Include(t => t.Name)
                        .ThenInclude(n => n.Translations)
                        .ToListAsync(),
                    nameof(PartOfSpeech.Id), nameof(PartOfSpeech.Name));

            vm.LanguageSelectList =
                new SelectList(await _context.Languages
                        .Include(t => t.Name)
                        .ThenInclude(n => n.Translations)
                        .ToListAsync(),
                    nameof(Language.Id), nameof(Language.Name));
            
            return RedirectToAction(nameof(Create), new {viewModel = vm});
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

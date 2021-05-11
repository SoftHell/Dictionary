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
        public async Task<IActionResult> Create()
        {
            var vm = new WordViewModel()
            {
                TopicSelectList =
                    new SelectList(await _context.Topics.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                        nameof(Topic.Id), nameof(Topic.Name)),

                PartOfSpeechSelectList =
                    new SelectList(await _context.PartsOfSpeech.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                        nameof(PartOfSpeech.Id), nameof(PartOfSpeech.Name)),
                LanguageSelectList =
                    new SelectList(
                        await _context.Languages.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                        nameof(Language.Id), nameof(Language.Name))
            };
            
            return View(vm);
            
        }
        
        // POST: Word/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                
                if (vm.Word.Value != string.Empty)
                {
                    var word = _context.Words.Add(vm.Word);
                    var id = word.Entity.Id;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(AddEquivalent), new {wordId = id});
                }
            }

            vm.TopicSelectList =
                new SelectList(
                    await _context.Topics.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                    nameof(Topic.Id), nameof(Topic.Name));

            vm.PartOfSpeechSelectList =
                new SelectList(
                    await _context.PartsOfSpeech.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                    nameof(PartOfSpeech.Id), nameof(PartOfSpeech.Name));
            vm.LanguageSelectList =
                new SelectList(
                    await _context.Languages.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                    nameof(Language.Id), nameof(Language.Name));
            
            return View(vm);
        }

        // GET: Word/AddEquivalent
        public async Task<IActionResult> AddEquivalent(Guid wordId)
        {
            var vm = new WordViewModel
            {
                TopicSelectList =
                    new SelectList(
                        await _context.Topics.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                        nameof(Topic.Id), nameof(Topic.Name)),
                PartOfSpeechSelectList =
                    new SelectList(
                        await _context.PartsOfSpeech.Include(x => x.Name).ThenInclude(n => n.Translations)
                            .ToListAsync(),
                        nameof(PartOfSpeech.Id), nameof(PartOfSpeech.Name)),
                Word = {QueryWordId = wordId},
            };


            return View(vm);

        }
        
        // POST: Word/AddEquivalent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEquivalent(WordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Word.Value != string.Empty)
                {
                    if (vm.Word.QueryWordId != null)
                    {
                        var parentWord = await _context.Words.FirstOrDefaultAsync(x => x.Id == vm.Word.QueryWordId);
                        
                        vm.Word.Topic = parentWord.Topic;
                        vm.Word.PartOfSpeech = parentWord.PartOfSpeech;
                        var lang = await _context.Languages.FirstOrDefaultAsync(x => x.Id == parentWord.LanguageId);
                        
                        Console.WriteLine("Parent language:" + lang.Id);
                        
                        if (lang.Abbreviation == ELanguage.En)
                        {
                            var equivalentLang =
                                await _context.Languages.FirstOrDefaultAsync(x => x.Abbreviation == ELanguage.Et);
                            vm.Word.LanguageId = equivalentLang.Id;
                        }
                        else
                        {
                            var equivalentLang =
                                await _context.Languages.FirstOrDefaultAsync(x => x.Abbreviation == ELanguage.En);
                            vm.Word.LanguageId = equivalentLang.Id;
                        }
                    }
                    var word = await _context.Words.AddAsync(vm.Word);
                    await _context.SaveChangesAsync();

                    if (vm.Word.QueryWordId == null)
                        
                        return RedirectToAction(nameof(Details), new {id = vm.Word.QueryWordId});
                    {
                        var parentWord = await _context.Words.FirstOrDefaultAsync(x => x.Id == vm.Word.QueryWordId);
                        if (parentWord.Equivalents != null)
                        {
                            parentWord.Equivalents.Add(word.Entity);
                        }
                        else
                        {
                            parentWord.Equivalents = new List<Word> {word.Entity};
                        }
                    }
                    return RedirectToAction(nameof(Details), new {id = vm.Word.QueryWordId});
                }
            }

            vm.TopicSelectList =
                new SelectList(
                    await _context.Topics.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                    nameof(Topic.Id), nameof(Topic.Name));

            vm.PartOfSpeechSelectList =
                new SelectList(
                    await _context.PartsOfSpeech.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                    nameof(PartOfSpeech.Id), nameof(PartOfSpeech.Name));
            
            return View(vm);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class WordController : Controller
    {
        private readonly AppDbContext _context;

        public WordController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Word
        public async Task<IActionResult> Index()
        {
            var words = await _context.Words
                .Include(w => w.QueryWord)
                .ToListAsync();

            words = words.OrderBy(x => x.Value).ToList();
            return View(words);
        }

        [AllowAnonymous]
        // GET: Word/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await _context.Words
                .Include(w => w.QueryWord)
                .Include(x => x.Equivalents)
                .Include(w => w.PartOfSpeech)
                .ThenInclude(x => x!.Name)
                .ThenInclude(n => n.Translations)
                .Include(w => w.Topic)
                .ThenInclude(x => x!.Name)
                .ThenInclude(n => n.Translations)
                .Include(w => w.Language)
                .ThenInclude(x => x!.Name)
                .ThenInclude(n => n.Translations)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (word == null)
            {
                return NotFound();
            }

            var vm = new WordViewModel()
            {
                Word = word,
                Value = word.Value,
            };

            return View(vm);
        }

        
        // GET: Word/Create
        public async Task<IActionResult> Create()
        {
            var vm = new WordViewModel()
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
                    await _context.SaveChangesAsync();

                    if (vm.Equivalent == string.Empty)
                        return RedirectToAction(nameof(Details), new {id = word.Entity.Id});
                    
                    var equivalent = await CreateEquivalent(word.Entity, vm.Equivalent!);
                    word.Entity.Equivalents = new List<Word> {equivalent};
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new {id = word.Entity.Id});
                }
            }

            vm.TopicSelectList =
                new SelectList(
                    await _context.Topics.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                    nameof(Topic.Id), nameof(Topic.Name), nameof(vm.Word.TopicId));

            vm.PartOfSpeechSelectList =
                new SelectList(
                    await _context.PartsOfSpeech.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                    nameof(PartOfSpeech.Id), nameof(PartOfSpeech.Name), nameof(vm.Word.PartOfSpeechId));
            vm.LanguageSelectList =
                new SelectList(
                    await _context.Languages.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                    nameof(Language.Id), nameof(Language.Name));

            return View(vm);
        }
        

        // GET: Word/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await _context.Words
                .Include(w => w.QueryWord)
                .Include(x => x.Equivalents)
                .Include(w => w.PartOfSpeech)
                .ThenInclude(x => x!.Name)
                .ThenInclude(n => n.Translations)
                .Include(w => w.Topic)
                .ThenInclude(x => x!.Name)
                .ThenInclude(n => n.Translations)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (word == null)
            {
                return NotFound();
            }

            var vm = new WordViewModel()
            {
                Word = word,
                Value = word.Value,

                TopicSelectList =
                    new SelectList(
                        await _context.Topics.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                        nameof(Topic.Id), nameof(Topic.Name), nameof(word.TopicId)),
                PartOfSpeechSelectList =
                    new SelectList(
                        await _context.PartsOfSpeech.Include(x => x.Name).ThenInclude(n => n.Translations)
                            .ToListAsync(),
                        nameof(PartOfSpeech.Id), nameof(PartOfSpeech.Name), nameof(word.PartOfSpeechId))
            };
            return View(vm);
        }

        // POST: Word/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, WordViewModel vm)
        {
            if (id != vm.Word.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vm.Word);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WordExists(vm.Word.Id))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Details), new {id = vm.Word.Id});
            }
            vm.TopicSelectList =
                new SelectList(
                    await _context.Topics.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                    nameof(Topic.Id), nameof(Topic.Name), nameof(vm.Word.TopicId));

            vm.PartOfSpeechSelectList =
                new SelectList(
                    await _context.PartsOfSpeech.Include(x => x.Name).ThenInclude(n => n.Translations).ToListAsync(),
                    nameof(PartOfSpeech.Id), nameof(PartOfSpeech.Name), nameof(vm.Word.PartOfSpeechId));

            return View(vm);
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
                .Include(w => w.Equivalents)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (word == null)
            {
                return NotFound();
            }

            var vm = new WordDeleteViewModel
            {
                Word = word,
            };

            return View(vm);
        }

        // POST: Word/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(WordDeleteViewModel vm)
        {
            var word = await _context.Words.FirstOrDefaultAsync(x => x.Id == vm.Word.Id);
            var equivalents = await _context.Words.Where(x => x.QueryWordId == vm.Word.Id).ToListAsync();
            if (equivalents != null && equivalents.Count > 0)
            {
                if (vm.KeepEquivalents)
                {
                    foreach (var equivalent in equivalents)
                    {
                        equivalent.QueryWordId = null;
                        equivalent.QueryWord = null;
                    }
                }
                else
                {
                    foreach (var equivalent in equivalents)
                    {
                        _context.Words.Remove(equivalent);
                    }
                }
                
                await _context.SaveChangesAsync();
            }
            _context.Words.Remove(word);
            await _context.SaveChangesAsync();
            
            return word.QueryWordId != null ? RedirectToAction(nameof(Details), new {id = word.QueryWordId}) : RedirectToAction(nameof(Index));
        }

        private bool WordExists(Guid id)
        {
            return _context.Words.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddEquivalent(WordViewModel vm)
        {
            if (vm.Equivalent == null || vm.Word.Id == default)
                return RedirectToAction(nameof(Details), new {id = vm.Word.Id});
            
            var parentWord = await _context.Words.FirstOrDefaultAsync(x => x.Id == vm.Word.Id);
            var equivalent = await CreateEquivalent(parentWord, vm.Equivalent!);
            
            if (parentWord.Equivalents == null)
            {
                parentWord.Equivalents = new List<Word> {equivalent};
            }
            else
            {
                parentWord.Equivalents.Add(equivalent);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {id = vm.Word.Id});
        }
        
        public async Task<Guid> FindEquivalentLanguage(Guid parentWordLangId)
        {
            
            var lang = await _context.Languages.FirstOrDefaultAsync(x => x.Id == parentWordLangId);
            if (lang.Abbreviation == ELanguage.En)
            {
                var equivalentLang =
                    await _context.Languages.FirstOrDefaultAsync(x => x.Abbreviation == ELanguage.Et);
                return equivalentLang.Id;
            }
            else
            {
                var equivalentLang =
                    await _context.Languages.FirstOrDefaultAsync(x => x.Abbreviation == ELanguage.En);
                return equivalentLang.Id;
            }
        }

        public async Task<Word> CreateEquivalent(Word parentWord, string equivalentValue)
        {
            var equivalent = new Word()
            {
                Value = equivalentValue,
                LanguageId = await FindEquivalentLanguage(parentWord.LanguageId),
                TopicId = parentWord.TopicId,
                PartOfSpeechId = parentWord.PartOfSpeechId,
                QueryWordId = parentWord.Id
            };
            var res = _context.Words.Add(equivalent);
            return res.Entity;
        }
    }
}
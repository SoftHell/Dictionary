using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using DAL;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new SearchViewModel
            {
                LanguageSelectList =
                    new SelectList(
                        await _context.Languages
                            .Include(x => x.Name)
                            .ThenInclude(n => n.Translations)
                            .ToListAsync(),
                        nameof(Language.Id), nameof(Language.Name))
            };
            
            return View(vm);
        }
        
        // POST: Word/Search
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(SearchViewModel vm)
        {
            if (vm.KeyWord.Equals(default) || vm.KeyWord.Equals(string.Empty))
            {
                var result = new List<Word>();
                return View(result);
            }

            var word = vm.KeyWord!.Trim().ToLower();
            
            if (vm.MatchExactness.Equals(EMatchExactness.Exact.ToString()))
            {
                var result = await _context.Words
                    .Include(w => w.QueryWord)
                    .Include(w => w.Equivalents)
                    .Where(x => x.LanguageId.Equals(vm.LanguageId))
                    .Where(x => x.Value.ToLower().Equals(word))
                    .ToListAsync();
                return View(result);
            }

            if (vm.MatchExactness.Equals(EMatchExactness.Fuzzy.ToString()))
            {
                var result = await _context.Words
                    .Include(w => w.QueryWord)
                    .Include(w => w.Equivalents)
                    .Where(x => x.LanguageId.Equals(vm.LanguageId))
                    .ToListAsync();

                var fuzzyFilteredRes = result.Where(res => LogicHelp.LevenshteinDistance(res.Value.Trim().ToLower(), vm.KeyWord) < 3).ToList();

                return View(fuzzyFilteredRes);
                
            }

            if (!vm.MatchExactness.Equals(EMatchExactness.Partial.ToString())) return View();
            {
                var result = await _context.Words
                    .Include(w => w.QueryWord)
                    .Include(w => w.Equivalents)
                    .Where(x => x.LanguageId.Equals(vm.LanguageId))
                    .Where(x => x.Value.ToLower().Contains(vm.KeyWord))
                    .ToListAsync();
                
                return View(result);
            }
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
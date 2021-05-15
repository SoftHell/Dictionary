using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.ViewModels;
using WebApp.ViewModels.Test;

namespace WebApp.Controllers
{
    public class TestController : Controller
    {

        private readonly ILogger<TestController> _logger;
        private readonly AppDbContext _ctx;
        
        public TestController(ILogger<TestController> logger, AppDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }
        
        // GET
        public async Task<IActionResult> TestIndex()
        {
            _logger.LogInformation("Test method");
            
            var vm = new TestViewModel()
            {
                Words = await _ctx.Words
                    .Include(w => w.QueryWord)
                    .Include(w => w.Equivalents)
                    .ToListAsync()
            };
            
            _logger.LogInformation("Test method pre-return");
            
            return View(vm);
        }
        
        public async Task<IActionResult>? TestDetails(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await _ctx.Words
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }
        
    }
}
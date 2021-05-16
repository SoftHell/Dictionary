using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Repositories;
using Domain;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Word Controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly WordRepository _wordRepo;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public WordController(AppDbContext context)
        {
            _context = context;
            _wordRepo = new WordRepository(context);
        }

        /// <summary>
        /// Get all the Words from DB
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.Word>>> GetWords(ELanguage lang)
        {
            var res = await _wordRepo.GetAllByLanguageAsync(lang);
            
            return Ok(res);
        }

        /// <summary>
        /// Get one word by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.Word>> GetWord(Guid id)
        {
            var word = await _wordRepo.FirstOrDefaultAsync(id);

            if (word == null)
            {
                return NotFound();
            }

            return word;
        }

        /// <summary>
        /// Update a Word
        /// </summary>
        /// <param name="id"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWord(Guid id, Word word)
        {
            if (id != word.Id)
            {
                return BadRequest();
            }

            _context.Entry(word).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WordExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Create a new Word
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Word>> PostWord(Word word)
        {
            _context.Words.Add(word);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWord", new { id = word.Id }, word);
        }

        /// <summary>
        /// Delete a Word by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWord(Guid id)
        {
            var word = await _context.Words.FindAsync(id);
            if (word == null)
            {
                return NotFound();
            }

            _context.Words.Remove(word);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WordExists(Guid id)
        {
            return _context.Words.Any(e => e.Id == id);
        }
    }
}

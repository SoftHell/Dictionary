using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartOfSpeechController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PartOfSpeechController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PartOfSpeech
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartOfSpeech>>> GetPartsOfSpeech()
        {
            return await _context.PartsOfSpeech.ToListAsync();
        }

        // GET: api/PartOfSpeech/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PartOfSpeech>> GetPartOfSpeech(Guid id)
        {
            var partOfSpeech = await _context.PartsOfSpeech.FindAsync(id);

            if (partOfSpeech == null)
            {
                return NotFound();
            }

            return partOfSpeech;
        }

        // PUT: api/PartOfSpeech/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartOfSpeech(Guid id, PartOfSpeech partOfSpeech)
        {
            if (id != partOfSpeech.Id)
            {
                return BadRequest();
            }

            _context.Entry(partOfSpeech).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartOfSpeechExists(id))
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

        // POST: api/PartOfSpeech
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PartOfSpeech>> PostPartOfSpeech(PartOfSpeech partOfSpeech)
        {
            _context.PartsOfSpeech.Add(partOfSpeech);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPartOfSpeech", new { id = partOfSpeech.Id }, partOfSpeech);
        }

        // DELETE: api/PartOfSpeech/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartOfSpeech(Guid id)
        {
            var partOfSpeech = await _context.PartsOfSpeech.FindAsync(id);
            if (partOfSpeech == null)
            {
                return NotFound();
            }

            _context.PartsOfSpeech.Remove(partOfSpeech);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PartOfSpeechExists(Guid id)
        {
            return _context.PartsOfSpeech.Any(e => e.Id == id);
        }
    }
}

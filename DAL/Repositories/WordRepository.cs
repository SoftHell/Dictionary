using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class WordRepository
    {
        private AppDbContext _context { get; set; }
        
        public WordRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<DTO.Word>> GetAllAsync()
        {
            var words = await _context.Words
                .Include(w => w.QueryWord)
                .Include(w => w.Equivalents)
                .Select(w => WordMapper.Map(w))
                .ToListAsync();

            words = words
                .OrderBy(x => x.Value)
                .ToList();
            
            return words;
        }
        
        public async Task<DTO.Word> FirstOrDefaultAsync(Guid id)
        {
            var word = await _context.Words
                .Include(w => w.QueryWord)
                .Include(w => w.Equivalents)
                .FirstOrDefaultAsync(w => w.Id == id);
            
            return WordMapper.Map(word);
        }
        
        

        
    }
}
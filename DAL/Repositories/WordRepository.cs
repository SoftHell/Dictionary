using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Domain;
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
        
        public async Task<IEnumerable<DTO.Word>> GetAllByLanguageAsync(ELanguage lang)
        {
            var fromLanguage = await _context.Languages
                .Include(x => x.Name)
                .ThenInclude(n => n.Translations)
                .Where(l => l.Abbreviation == lang).FirstOrDefaultAsync();

            var words = await _context.Words
                .Include(w => w.QueryWord)
                .Include(w => w.Equivalents)
                .Where(w => w.LanguageId == fromLanguage.Id)
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
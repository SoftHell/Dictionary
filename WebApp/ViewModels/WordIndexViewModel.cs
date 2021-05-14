using System;
using System.Collections.Generic;
using Domain;

namespace WebApp.ViewModels
{
    public class WordIndexViewModel
    {
        public List<Word> Words { get; set; } = default!;
        
        public Guid FromLanguageId { get; set; }
        
        public string FromLanguage { get; set; } = default!;
        public string ToLanguage { get; set; } = default!;
        
        public ELanguage ToELanguage { get; set; }
        
        public Guid ToLanguageId { get; set; }
    }
}
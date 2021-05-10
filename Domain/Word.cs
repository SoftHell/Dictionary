using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Word
    {
        public Guid Id { get; set; }

        [MaxLength(64)] public string Value { get; set; } = default!;
        
        [MaxLength(512)] public string? Example { get; set; } = default!;
        
        [MaxLength(1024)] public string? Explanation { get; set; } = default!;
        
        [MaxLength(1024)] public string? Pronunciation { get; set; } = default!;
        
        [MaxLength(64)] public Language Language { get; set; } = default!;
        
        [MaxLength(64)] public PartOfSpeech PartOfSpeech { get; set; } = default!;
        
        [MaxLength(64)] public Topic? Topic { get; set; } = default!;
        
        public Guid? QueryWordId { get; set; }
        
        public Word? QueryWord { get; set; } = default!;
        
        public ICollection<Word>? Equivalents { get; set; } = default!;

    }
}
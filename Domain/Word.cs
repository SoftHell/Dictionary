using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Word
    {
        public Guid Id { get; set; }

        [MaxLength(64)] public string Value { get; set; } = default!;
        
        [MaxLength(64)] public Language Language { get; set; } = default!;
        
        [MaxLength(64)] public PartOfSpeech PartOfSpeech { get; set; } = default!;
        
        [MaxLength(64)] public Category? Category { get; set; } = default!;
        
        
    }
}
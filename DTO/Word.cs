using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain;

namespace DTO
{
    public class Word
    {
        public Guid Id { get; set; }
        
        [MinLength(2)] [MaxLength(64)] public string Value { get; set; } = default!;

        public Guid LanguageId { get; set; }
        
        public Guid? PartOfSpeechId { get; set; }
        
        public Guid? TopicId { get; set; }
        
        [MaxLength(512)] public string? Example { get; set; } = default!;
        
        [MaxLength(1024)] public string? Explanation { get; set; } = default!;
        
        [MaxLength(32)] public string? Pronunciation { get; set; } = default!;
        
        public Guid? QueryWordId { get; set; }
        
        public List<string>? Equivalents { get; set; } = default!;
        
        public string? EquivalentString { get; set; } = default!;
    }
}
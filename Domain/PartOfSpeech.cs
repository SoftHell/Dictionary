using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class PartOfSpeech
    {
        public Guid Id { get; set; }
        
        public Guid NameId { get; set; }
        [MaxLength(32)] public LangString Name { get; set; } = default!;
        
        public Guid? AbbreviationId { get; set; }
        
        [MaxLength(6)] public LangString? Abbreviation { get; set; } = default!;
    }
}
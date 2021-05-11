using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Language
    {
        public Guid Id { get; set; }
        
        public Guid NameId { get; set; }
        [MaxLength(32)] public LangString Name { get; set; } = default!;
        
        public ELanguage Abbreviation { get; set; }
    }
}
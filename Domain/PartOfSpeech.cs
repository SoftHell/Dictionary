using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class PartOfSpeech
    {
        public Guid Id { get; set; }
        
        [MaxLength(32)] public string Name { get; set; } = default!;
    }
}
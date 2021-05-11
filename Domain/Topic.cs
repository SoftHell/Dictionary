using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Topic
    {
        public Guid Id { get; set; }
        
        public Guid NameId { get; set; }
        [MaxLength(32)] public LangString Name { get; set; } = default!;
    }
}
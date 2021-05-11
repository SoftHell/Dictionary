using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Translation
    {
        [MaxLength(5)]
        public virtual string Culture { get; set; } = default!;

        [MaxLength(10240)]
        public virtual string Value { get; set; } = "";

        public Guid LangStringId { get; set; } = default!;
        public LangString? LangString { get; set; }
    }
}
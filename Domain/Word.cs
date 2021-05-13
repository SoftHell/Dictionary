using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Word
    {
        public Guid Id { get; set; }
        
        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = nameof(Value))]
        [MinLength(2, ErrorMessageResourceName = "ErrorMessage_MinLength", ErrorMessageResourceType = typeof(Resources.Common))]
        [MaxLength(64, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Common))]
        public string Value { get; set; } = default!;

        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = nameof(Language))]
        public Guid LanguageId { get; set; }
        
        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = nameof(Language))]
        public Language? Language { get; set; } = default!;
        
        public Guid? PartOfSpeechId { get; set; }
        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = nameof(PartOfSpeech))]
        public PartOfSpeech? PartOfSpeech { get; set; } = default!;
        
        public Guid? TopicId { get; set; }
        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = nameof(Topic))]
        public Topic? Topic { get; set; } = default!;
        
        
        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = nameof(Example))]
        [MaxLength(512)] public string? Example { get; set; } = default!;
        
        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = nameof(Explanation))]
        [MaxLength(1024)] public string? Explanation { get; set; } = default!;
        
        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = nameof(Pronunciation))]
        [MaxLength(32)] public string? Pronunciation { get; set; } = default!;
        
        
        public Guid? QueryWordId { get; set; }
        
        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = "Equivalent")]
        public Word? QueryWord { get; set; } = default!;
        
        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = nameof(Equivalents))]
        
        public ICollection<Word>? Equivalents { get; set; } = default!;

    }
}
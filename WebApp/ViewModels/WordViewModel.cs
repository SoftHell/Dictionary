using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels
{
    public class WordViewModel
    {

        public Word Word { get; set; } = new();
        
        public Guid? QueryWordId { get; set; }

        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = "InsertNewWord")]
        [MinLength(1, ErrorMessageResourceName = "ErrorMessage_MinLength", ErrorMessageResourceType = typeof(Resources.Common))]
        [MaxLength(64, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Common))]
        public string? Value { get; set; } = default!;
        
        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = nameof(Explanation))]
        [MaxLength(1024, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Common))]
        public string? Explanation { get; set; } = default!;
        
        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = nameof(Example))]
        [MaxLength(512, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Common))]
        public string? Example { get; set; } = default!;
        
        [Display(ResourceType = typeof(Resources.Views.Word.Create), Name = nameof(Pronunciation))]
        [MaxLength(32, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Common))]
        public string? Pronunciation { get; set; } = default!;
        
        public SelectList? TopicSelectList { get; set; }
        public SelectList? PartOfSpeechSelectList { get; set; }
        public SelectList? LanguageSelectList { get; set; }
        
    }
}
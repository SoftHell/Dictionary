using System;
using System.ComponentModel.DataAnnotations;
using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels
{
    public class SearchViewModel
    {
        [MinLength(1, ErrorMessageResourceName = "ErrorMessage_MinLength", ErrorMessageResourceType = typeof(Resources.Common))]
        [MaxLength(64, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Common))]
        public string KeyWord { get; set; } = default!;
        
        public string MatchExactness { get; set; } = default!;
        
        public Guid LanguageId { get; set; }
        
        public SelectList? LanguageSelectList { get; set; }
    }
}
using System;
using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels
{
    public class SearchViewModel
    {
        public string KeyWord { get; set; } = default!;
        
        public bool ExactMatch { get; set; }
        
        public Guid LanguageId { get; set; }
        
        public SelectList? LanguageSelectList { get; set; }
    }
}
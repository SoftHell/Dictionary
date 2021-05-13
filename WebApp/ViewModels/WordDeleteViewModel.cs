using System;
using Domain;

namespace WebApp.ViewModels
{
    public class WordDeleteViewModel
    {
        public Word Word { get; set; } = default!;
        
        public bool KeepEquivalents { get; set; }
    }
}
using Domain;

namespace WebApp.ViewModels
{
    public class SearchViewModel
    {
        public string KeyWord { get; set; } = default!;
         
        public string LanguageName { get; set; } = default!;
        
        public bool ExactMatch { get; set; }
    }
}
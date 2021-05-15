using System.Collections;
using System.Collections.Generic;
using Domain;

namespace WebApp.ViewModels.Test
{
    public class TestViewModel
    {
        public ICollection<Word> Words { get; set; } = default!;
    }
}
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public static class WordMapper
    {
        public static DTO.Word Map(Domain.Word w)
        {
            var word = MapEntity(w);
            word.EquivalentString = MapEquivalentsAsString(w);
            word.Equivalents = MapEquivalents(w);
            return word;
        }
        
        private static DTO.Word MapEntity(Domain.Word w)
        {
            var word = new DTO.Word()
            {
                Id = w.Id,
                Value = w.Value,
                LanguageId = w.LanguageId,
                PartOfSpeechId = w.PartOfSpeechId,
                TopicId = w.TopicId,
                Example = w.Example,
                Explanation = w.Explanation,
                Pronunciation = w.Pronunciation,
                QueryWordId = w.QueryWordId,
            };
            return word;
        }

        private static List<string> MapEquivalents(Domain.Word w)
        {
            var equivalents = new List<string>();
            
            if (w.QueryWord != null) equivalents.Add(w.QueryWord.Value);

            if (w.Equivalents == null) return equivalents;

            equivalents.AddRange(w.Equivalents.Select(eq => eq.Value));

            return equivalents;
        }
        
        private static string MapEquivalentsAsString(Domain.Word w)
        {
            var equivalents = MapEquivalents(w);
            var result = "";

            foreach (var eq in equivalents)
            {
                if (result.Equals(string.Empty))
                {
                    result += eq.ToLower();
                }
                else
                {
                    result += ", " + eq.ToLower();
                } 
            }
            return result;
        }
    }
}
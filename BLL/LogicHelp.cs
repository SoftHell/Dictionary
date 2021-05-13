using Domain;

namespace BLL
{
    public static class LogicHelp
    {
        public static string DisplayEquivalents(Word word)
        {
            if (word.Equivalents == null)
            {
                return string.Empty;
            }
            var equivalents = "";

            foreach (var eq in word.Equivalents)
            {
                if (!equivalents.Equals(""))
                {
                    equivalents += ", " + eq.Value;
                }
                else
                {
                    equivalents += eq.Value;
                }
            }
            return equivalents;
        }
    }
}
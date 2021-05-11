using System.Collections.Generic;
using Domain;

namespace DAL.DataInit
{
    public static class AppDataInit
    {
        public static void SeedAppData(AppDbContext ctx)
        {

            AddPartsOfSpeech(ctx);
            
            AddLanguages(ctx);
            
            AddTopics(ctx);
            
            //AddWords(ctx);
            
            ctx.SaveChanges();
        }

        private static void AddPartsOfSpeech(AppDbContext ctx)
        {

            var partsOfSpeechEn = new[]
            {
                "noun",
                "verb",
                "adjective",
                "pronoun",
                "adverb",
                "preposition",
                "conjunction",
                "interjection"
            };
            
            var partsOfSpeechEt = new[]
            {
                "nimisõna",
                "tegusõna",
                "omadussõna",
                "asesõna",
                "määrsõna",
                "kaassõna",
                "sidesõna",
                "hüüdsõna"
            };
            
            for (var i = 0; i < partsOfSpeechEn.Length; i++)
            {
                var langString = new LangString {Translations = new List<Translation>()};
                var translationEn = new Translation
                {
                    Value = partsOfSpeechEn[i],
                    Culture = "en"
                };
                var translationEt = new Translation
                {
                    Value = partsOfSpeechEt[i],
                    Culture = "et"
                };
                langString.Translations.Add(translationEn);
                langString.Translations.Add(translationEt);

                var partOfSpeech = new PartOfSpeech()
                {
                    Name = langString
                };
                
                ctx.PartsOfSpeech.Add(partOfSpeech);
            }
            

        }
        
        private static void AddLanguages(AppDbContext ctx)
        {
            var languagesEn = new[]
            {
                "English",
                "Estonian"

            };
            
            var languagesEt = new[]
            {
                "inglise keel",
                "eesti keel"
            };

            for (var i = 0; i < languagesEn.Length; i++)
            {
                var langString = new LangString {Translations = new List<Translation>()};
                var translationEn = new Translation
                {
                    Value = languagesEn[i],
                    Culture = "en"
                };
                var translationEt = new Translation
                {
                    Value = languagesEt[i],
                    Culture = "et"
                };
                langString.Translations.Add(translationEn);
                langString.Translations.Add(translationEt);

                var language = new Language
                {
                    Name = langString,
                    Abbreviation = (ELanguage) i
                };
                ctx.Languages.Add(language);
            }
        }
        
        private static void AddTopics(AppDbContext ctx)
        {
            var topicsEn = new[]
            {
                "Food",
                "Sports",
                "Music",
                "Movies",
                "Time",
                "Numbers",
                "Buildings",
                "Family",
                "Travel",
                "Courtesy",
                "Pets",
                "Politics",
                "Science",
                "Religion",
                "Slang",
                "History",
                "Nature",
                "Money",
                "Hobbies",
                "Entertainment",
                "Employment",
                "Transportation",

            };
            
            var topicsEt = new[]
            {
                "Toit",
                "Sport",
                "Muusika",
                "Filmid",
                "Aeg",
                "Arvud",
                "Hooned",
                "Perekond",
                "Reisimine",
                "Viisakusväljendid",
                "Lemmikloomad",
                "Poliitika",
                "Teadus",
                "Religioon",
                "Släng",
                "Ajalugu",
                "Loodus",
                "Raha",
                "Hobid",
                "Meelelahutus",
                "Töö",
                "Transport",
            };

            for (var i = 0; i < topicsEn.Length; i++)
            {
                var langString = new LangString {Translations = new List<Translation>()};
                var translationEn = new Translation
                {
                    Value = topicsEn[i],
                    Culture = "en"
                };
                var translationEt = new Translation
                {
                    Value = topicsEt[i],
                    Culture = "et"
                };
                langString.Translations.Add(translationEn);
                langString.Translations.Add(translationEt);

                var topic = new Topic()
                {
                    Name = langString
                };
                ctx.Topics.Add(topic);
            }
        }
        
        private static void AddWords(AppDbContext ctx)
        {
            var wordsEn = new[]
            {
                "English",
                "Estonian",
                

            };
            
            var wordsEt = new[]
            {
                "inglise keel",
                "eesti keel",
                
            };

            for (var i = 0; i < wordsEn.Length; i++)
            {
                var langString = new LangString {Translations = new List<Translation>()};
                var translationEn = new Translation
                {
                    Value = wordsEn[i],
                    Culture = "en"
                };
                var translationEt = new Translation
                {
                    Value = wordsEt[i],
                    Culture = "et"
                };
                langString.Translations.Add(translationEn);
                langString.Translations.Add(translationEt);

                var word = new Word()
                {
                    Value = langString
                };
                ctx.Words.Add(word);
            }
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataInit
{
    public static class AppDataInit
    {
        public static async Task SeedAppData(AppDbContext ctx)
        {

            /*AddPartsOfSpeech(ctx);
            
            AddLanguages(ctx);
            
            AddTopics(ctx);*/
            
            await AddWords(ctx);
            
            await ctx.SaveChangesAsync();
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
                "inglise",
                "eesti"
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
        
        private static async Task AddWords(AppDbContext ctx)
        {
            var eng = await ctx.Languages.Where(x => x.Abbreviation.Equals(ELanguage.En)).FirstOrDefaultAsync();
            var est = await ctx.Languages.Where(x => x.Abbreviation.Equals(ELanguage.Et)).FirstOrDefaultAsync();

            void Translator(string et, string en)
            {
                var wordEn = new Word
                {
                    Value = en,
                    LanguageId = eng.Id
                };
                var dbWordEn = ctx.Words.Add(wordEn);
            
                var wordEt = new Word
                {
                    Value = et,
                    LanguageId = est.Id,
                    QueryWordId = dbWordEn.Entity.Id
                };
                var dbWordEt = ctx.Words.Add(wordEt);
                dbWordEn.Entity.Equivalents = new List<Word> {dbWordEt.Entity};
            }
            
            Translator("Ema", "Mother");
            Translator("Isa", "Father");
            Translator("Õde", "Sister");
            Translator("Vend", "Brother");
            Translator("Vanaisa", "Grandfather");
            Translator("Vanaema", "Grandmother");
            Translator("Tädi", "Aunt");
            Translator("Onu", "Uncle");
            Translator("Nõbu", "Cousin");
            Translator("Õepoeg", "Nephew");
            Translator("Õetütar", "Niece");
            Translator("Vennapoeg", "Nephew");
            Translator("Vennatütar", "Niece");
            Translator("Koer", "Dog");
            Translator("Kass", "Cat");
            Translator("Katus", "Roof");
            Translator("Sõnaosav", "Eloquent");
            Translator("Mitmedimensioonilie", "Multidimensional");
            Translator("Saladuslik", "Mysterious");
            Translator("Loomulik", "Natural");
            Translator("Kallis", "Expensive");
            Translator("Tõetruu", "Believable");
            Translator("Ettevõtlik", "Entrepreneurial");
            Translator("Karakter", "Character");
            Translator("Valmisolek", "Willingness");
            Translator("Maraton", "Marathon");
            Translator("Hägune", "Fuzzy");
            Translator("Osaline", "Partial");
            Translator("Täpne", "Exact");
            Translator("Ratastool", "Wheelchair");
            Translator("Reisimine", "Travelling");
            Translator("Puhkus", "Vacation");
            Translator("Suvi", "Summer");
            Translator("Sügis", "Autumn");
            Translator("Talv", "Winter");
            Translator("Kevad", "Spring");
            Translator("Esmaspäev", "Monday");
            Translator("Teisipäev", "Tuesday");
            Translator("Kolmapäev", "Wednesday");
            Translator("Neljapäev", "Thursday");
            Translator("Reede", "Friday");
            Translator("Laupäev", "Saturday");
            Translator("Pühapäev", "Sunday");
            Translator("Kohvi", "Coffee");
            Translator("Kohvik", "Cafe");
            Translator("Restoran", "Restaurant");
            Translator("Piknik", "Picnic");
            Translator("Pitsa", "Pizza");
            Translator("Kino", "Cinema");
            Translator("Paismais", "Popcorn");
            Translator("Paraku", "Alas");
            Translator("Armastama", "To love");
            Translator("Metsik", "Wild");
            Translator("Mets", "Forest");
            Translator("Vihmamets", "Rainforest");
            Translator("Vihm", "Rain");
            Translator("Päike", "Sun");
            Translator("Äike", "Thunder");
            Translator("Püksid", "Pants");
            Translator("Kampsun", "Sweater");
            Translator("T-särk", "T-shirt");
            Translator("Sokid", "Socks");
            Translator("Laev", "Ship");
            Translator("Rong", "Train");
            Translator("Auto", "Car");
            Translator("Vesi", "Water");
            Translator("Õhk", "Air");
            Translator("Majakas", "Beacon");
            Translator("Laine", "Wave");
            Translator("Ujumine", "Swimming");
            Translator("Tantsimine", "Dancing");
            Translator("Sõudmine", "Rowing");
            Translator("Jooksmine", "Running");
            Translator("Sörkimine", "Jogging");
            Translator("Suusatamine", "Skiing");
            Translator("Kelgutamine", "Sledding");
            Translator("Uisutamine", "Skating");
            Translator("Toit", "Food");
            Translator("Jook", "Drink");
            Translator("Söömine", "Eating");
            Translator("Joomine", "Drinking");
            Translator("Tarbimine", "Consuming");
            Translator("Teater", "Theater");
            Translator("Kontsert", "Concert");
            Translator("Film", "Movie");
            Translator("Orkester", "Orchestra");
            Translator("Sümfoonia", "Symphony");
            Translator("Klaver", "Piano");
            Translator("Viiul", "Violin");
            Translator("Tšello", "Cello");
            Translator("Oboe", "Oboe");
            Translator("Akordion", "Accordion");
            Translator("Võlukunst", "Magic");
            Translator("Võlur", "Wizard");

        }

    }
}
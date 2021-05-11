using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Domain
{
    
    public class LangString
    {
        public Guid Id { get; set; }
        
        private static string _defaultCulture = "en";
        
        public virtual ICollection<Translation>? Translations { get; set; }


        public LangString()
        {
            
        }

        public LangString(string value, string? culture = null)
        {
            SetTranslation(value, culture);
        }
        
        

        public virtual void SetTranslation(string value, string? culture = null)
        {
            //Translations ??= new List<Translation>();
            culture ??= Thread.CurrentThread.CurrentUICulture.Name;
            
            if (Translations == null)
            {
                Translations = new List<Translation>();
            }

            var translation = Translations.FirstOrDefault(t => t.Culture == culture);
            if (translation == null)
            {
                Translations.Add(new Translation()
                {
                    Value = value,
                    Culture = culture,
                });
            }
            else
            {
                translation.Value = value;
            }
        }

        public string? Translate(string? culture = null)
        {
            if (Translations == null)
            {
                throw new NullReferenceException("Translations cannot be null. Did you forget the .Include?");
            }

            culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;
            
            /*
             cultures in db
             en, en-GB
             in query
             ru, en, en-US, en-GB
             */

            // do we have exact match
            var translation = Translations.FirstOrDefault(t => t.Culture == culture);
            if (translation != null)
            {
                return translation.Value;
            }
            
            // do we have match without region - match en-XX or en
            translation = Translations.FirstOrDefault(t => culture.StartsWith(t.Culture));
            if (translation != null)
            {
                return translation.Value;
            }
            
            // do we have the default culture string
            // exact match
            translation = Translations.FirstOrDefault(t => t.Culture == _defaultCulture);
            if (translation != null)
            {
                return translation.Value;
            }
            // starts with
            translation = Translations.FirstOrDefault(t => _defaultCulture.StartsWith(t.Culture));
            if (translation != null)
            {
                return translation.Value;
            }
            
            // just return the first one or null
            return Translations.FirstOrDefault()?.Value;
        }

        public override string ToString()
        {
            return Translate() ?? "??????";
        }
        
        // var x = "Foo" + new LangString(Bar)
        // x will be string FooBar
        public static implicit operator string(LangString? l) => l?.ToString() ?? "null";

        // LangString s = "Foo"
        public static implicit operator LangString(string s) => new LangString(s);

    }
    
}
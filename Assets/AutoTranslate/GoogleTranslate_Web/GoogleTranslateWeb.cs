using GoodTime.Tools.FactoryTranslate;
using GoodTime.Tools.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoodTime.Tools.GoogleTranslate.Web
{
    public class GoogleTranslateWeb : GenericTranslateApi
    {
        private Dictionary<string, string> _ignoreLocale;

        public GoogleTranslateWeb()
        {
            _ignoreLocale = new Dictionary<string, string>();
            //_ignoreLocale.Add("en-gb", "en-gb");
            //_ignoreLocale.Add("pt-pt", "pt-pt");
            //_ignoreLocale.Add("pt-br", "pt-br");
            _ignoreLocale.Add("zh-cn", "zh-cn");
            _ignoreLocale.Add("zh-tw", "zh-tw");
        }

        public bool CheckService()
        {
            return WebInformation.CheckService("https://translate.googleapis.com");
        }

        public string GetNameService()
        {
            return "GoogleApiWeb";
        }

        public string MappingLocale(string namelocale)
        {
            namelocale = namelocale.ToLower();
            if (namelocale.Contains("-"))
            {
                if (_ignoreLocale.ContainsKey(namelocale)) return _ignoreLocale[namelocale];
                int index = namelocale.IndexOf('-');
                return namelocale.Remove(index, namelocale.Length - index);
            }
            if (namelocale.Length > 2) throw new Exception("Locale Code invalide");
            return namelocale;
        }

        public string Translate(string word, string sourceLanguage, string targetLanguage, string key = null)
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<string, string> Translate(Dictionary<string, string> words, string sourceLanguage, string targetLanguage, string key = null)
        {
            throw new System.NotImplementedException();
        }

        public Sprite Translate(Sprite sprite, string sourceLanguage, string targetLanguage)
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<string, Sprite> Translate(Dictionary<string, Sprite> sprites, string sourceLanguage, string targetLanguage)
        {
            throw new System.NotImplementedException();
        }

        public Texture Translate(Texture texture, string sourceLanguage, string targetLanguage)
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<string, Texture> Translate(Dictionary<string, Texture> textures, string sourceLanguage, string targetLanguage)
        {
            throw new System.NotImplementedException();
        }

        public bool ValidateLocale(string namelocale)
        {
            namelocale = namelocale.ToLower();
            if (namelocale.Contains("-") == true)
            {
                return _ignoreLocale.ContainsKey(namelocale);
            }
            return namelocale.Length < 3;
        }

        //private RespontTranslateGoogle DeserializeRespont(string json)
        //{
        //    RespontTranslateGoogle respontTranslateGoogle = new RespontTranslateGoogle();

        //    return respontTranslateGoogle;
        //}

        private string BuildRequest(string sourceText, string sourceLanguage, string targetLanguage, string key = null)
        {
            string url = BuildBaseRequest(sourceLanguage, targetLanguage, key);
            //url += string.Format("&dt=t");
            url += string.Format("&op=translate");
            url += string.Format("&text={0}", sourceText);
            return url;
        }

        private string BuildBaseRequest(string sourceLanguage, string targetLanguage, string key = null)
        {
            string url = string.Format(@"https://translate.google.com/?hl={1}&sl={0}&tl={1}",
                                                sourceLanguage, targetLanguage);
            if (!string.IsNullOrEmpty(key))
            {
                url += string.Format("&key={0}", key);
            }
            return url;
        }
    }
}
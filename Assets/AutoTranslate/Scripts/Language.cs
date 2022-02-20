using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodTime.Tools.InterfaceTranslate
{
    public class Language
    {
        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
            }
        }

        public string ShortName
        {
            get
            {
                return _shortname;
            }
            private set
            {
                _shortname = value;
            }
        }

        private string _name;
        private string _shortname;

        public static Language Arabic = new Language() { Name = "Arabic", ShortName = "sq" };
        public static Language Armenian = new Language() { Name = "Armenian", ShortName = "ar" };
        public static Language Danish = new Language() { Name = "Danish", ShortName = "da" };
        public static Language Chinese = new Language() { Name = "Chinese", ShortName = "zh-CN" };

        private static Dictionary<string, string> _languages;

        public static Language GetLanguage(string name)
        {
            string shortname = string.Empty;
            Initialized();
            _languages.TryGetValue(name, out shortname);
            return new Language() { Name = name, ShortName = shortname };
        }

        private static void Initialized()
        {
            if (_languages == null)
            {
                _languages = new Dictionary<string, string>();
                _languages.Add("Albanian", "sq");
                _languages.Add("Arabic", "ar");
                _languages.Add("Armenian", "hy");
                _languages.Add("Azerbaijani", "az");
                _languages.Add("Basque", "eu");
                _languages.Add("Belarusian", "be");
                _languages.Add("Bengali", "bn");
                _languages.Add("Bulgarian", "bg");
                _languages.Add("Catalan", "ca");
                _languages.Add("Chinese", "zh-CN");
                _languages.Add("Croatian", "hr");
                _languages.Add("Czech", "cs");
                _languages.Add("Danish", "da");
                _languages.Add("Dutch", "nl");
                _languages.Add("English", "en");
                _languages.Add("Esperanto", "eo");
                _languages.Add("Estonian", "et");
                _languages.Add("Filipino", "tl");
                _languages.Add("Finnish", "fi");
                _languages.Add("French", "fr");
                _languages.Add("Galician", "gl");
                _languages.Add("German", "de");
                _languages.Add("Georgian", "ka");
                _languages.Add("Greek", "el");
                _languages.Add("Haitian Creole", "ht");
                _languages.Add("Hebrew", "iw");
                _languages.Add("Hindi", "hi");
                _languages.Add("Hungarian", "hu");
                _languages.Add("Icelandic", "is");
                _languages.Add("Indonesian", "id");
                _languages.Add("Irish", "ga");
                _languages.Add("Italian", "it");
                _languages.Add("Japanese", "ja");
                _languages.Add("Korean", "ko");
                _languages.Add("Lao", "lo");
                _languages.Add("Latin", "la");
                _languages.Add("Latvian", "lv");
                _languages.Add("Lithuanian", "lt");
                _languages.Add("Macedonian", "mk");
                _languages.Add("Malay", "ms");
                _languages.Add("Maltese", "mt");
                _languages.Add("Norwegian", "no");
                _languages.Add("Persian", "fa");
                _languages.Add("Polish", "pl");
                _languages.Add("Portuguese", "pt");
                _languages.Add("Romanian", "ro");
                _languages.Add("Russian", "ru");
                _languages.Add("Serbian", "sr");
                _languages.Add("Slovak", "sk");
                _languages.Add("Slovenian", "sl");
                _languages.Add("Spanish", "es");
                _languages.Add("Swahili", "sw");
                _languages.Add("Swedish", "sv");
                _languages.Add("Tamil", "ta");
                _languages.Add("Telugu", "te");
                _languages.Add("Thai", "th");
                _languages.Add("Turkish", "tr");
                _languages.Add("Ukrainian", "uk");
                _languages.Add("Urdu", "ur");
                _languages.Add("Vietnamese", "vi");
                _languages.Add("Welsh", "cy");
                _languages.Add("Yiddish", "yi");
            }
        }
    }
}

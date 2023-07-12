using GoodTime.Tools.FactoryTranslate;
using GoodTime.Tools.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

namespace GoodTime.Tools.GoogleTranslate.Apis
{
    public class GoogleTranslateApis : GenericTranslateApi
    {
        private const int MAXCHARS_FORREQUST = 5000;
        private const String SEPARATE_STRING = "[$]";
        private const String SEPARATE_STRING2 = "[$ ]";
        private const String SEPARATE_STRING3 = "[ $]";
        private const String SEPARATE_STRING4 = "[[ $]";
        private const String SEPARATE_STRING5 = "[ $]]";

        private Dictionary<string, string> _ignoreLocale;

        public GoogleTranslateApis() 
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
            return "GoogleApiFree";
        }

        public string MappingLocale(string namelocale)
        {
            namelocale = namelocale.ToLower();
            if ( namelocale.Contains("-") )
            {
                if (_ignoreLocale.ContainsKey(namelocale)) return _ignoreLocale[namelocale];
                int index = namelocale.IndexOf('-');
                return namelocale.Remove(index, namelocale.Length - index);
            }
            if (namelocale.Length > 2) throw new Exception("Locale Code invalide");
            return namelocale;
        }

        public string Translate(string sourceText, string sourceLanguage, string targetLanguage, string key = null)
        {
            string url = BuildRequest(sourceText.ToString(), sourceLanguage, targetLanguage);
            string translationFromGoogle = RequestToGoogleApi(url);

            RespontTranslateGoogle respontTranslateGoogle = DeserializeRespont(translationFromGoogle);

            return respontTranslateGoogle.FullRespont;
        }

        public Dictionary<string, string> Translate(Dictionary<string, string> words, string sourceLanguage, string targetLanguage, string key = null)
        {
            List<string> listRespontWords = new List<string>();
            Dictionary<string, string> targetWords = new Dictionary<string, string>();

            StringBuilder sourceText = new StringBuilder(MAXCHARS_FORREQUST + 100);
            StringBuilder sourceTryText = new StringBuilder(MAXCHARS_FORREQUST + 100);

            string translationFromGoogle = string.Empty;
            RespontTranslateGoogle respontTranslateGoogle;

            sourceLanguage = MappingLocale(sourceLanguage);
            targetLanguage = MappingLocale(targetLanguage);

            foreach (var item in words)
            {
                string temp = item.Value + SEPARATE_STRING;
                sourceTryText.Append(temp);
                if (sourceTryText.Length > MAXCHARS_FORREQUST)
                {
                    string url2 = BuildRequest(sourceText.ToString(), sourceLanguage, targetLanguage);
                    translationFromGoogle = RequestToGoogleApi(url2);
                    respontTranslateGoogle = DeserializeRespont(translationFromGoogle);
                    listRespontWords.AddRange(respontTranslateGoogle.FullRespont.Split(SEPARATE_STRING.ToCharArray()).ToList());
                    sourceTryText.Clear();
                    sourceText.Clear();
                }
                else
                {
                    sourceText.Append(temp);
                }
            }

            string url = BuildRequest(sourceText.ToString(), sourceLanguage, targetLanguage);
            translationFromGoogle = RequestToGoogleApi(url);

            respontTranslateGoogle = DeserializeRespont(translationFromGoogle);
            string response = respontTranslateGoogle.FullRespont;
            String[] mass = new String[5];
            mass[0] = SEPARATE_STRING;
            mass[1] = SEPARATE_STRING4;
            mass[2] = SEPARATE_STRING5;
            mass[3] = SEPARATE_STRING2;
            mass[4] = SEPARATE_STRING3;
            
            listRespontWords.AddRange(response.Split(mass, StringSplitOptions.None).ToList());

            int index = 0;
            foreach (var item in words)
            {
                targetWords.Add(item.Key, listRespontWords[index]);
                ++index;
            }

            return targetWords;
        }

        public Sprite Translate(Sprite sprite, string sourceLanguage, string targetLanguage)
        {
            //string url = BuildBaseRequest2(sourceLanguage, targetLanguage);
            ////url += "&q=apple";
            ////url += "&dt=i";
            //string translationFromGoogle = RequestToGoogleApi(url);
            return null;
        }

        public Dictionary<string, Sprite> Translate(Dictionary<string, Sprite> sprites, string sourceLanguage, string targetLanguage)
        {
            throw new NotImplementedException();
        }

        public Texture Translate(Texture texture, string sourceLanguage, string targetLanguage)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, Texture> Translate(Dictionary<string, Texture> textures, string sourceLanguage, string targetLanguage)
        {
            throw new NotImplementedException();
        }

        public bool ValidateLocale(string namelocale)
        {
            namelocale = namelocale.ToLower();
            if ( namelocale.Contains("-") == true)
            {
                return _ignoreLocale.ContainsKey(namelocale);
            }
            return namelocale.Length < 3;
        }

        private RespontTranslateGoogle DeserializeRespont(string json)
        {
            RespontTranslateGoogle respontTranslateGoogle = new RespontTranslateGoogle();

            object respontObject = JsonConvert.DeserializeObject(json);
            JArray arrayGeneric = respontObject as JArray;
            JArray arrayWords = arrayGeneric.First() as JArray;
            StringBuilder fullrespont = new StringBuilder(MAXCHARS_FORREQUST);

            foreach (var arrayWord in arrayWords)
            {
                JArray array = arrayWord as JArray;
                JValue translatedWord = array[0] as JValue;
                JValue sourceWord = array[1] as JValue;
                respontTranslateGoogle.TargetWords.Add((string)translatedWord);
                respontTranslateGoogle.SourceWords.Add((string)sourceWord);

                fullrespont.Append(translatedWord);
            }
            respontTranslateGoogle.FullRespont = fullrespont.ToString();
            return respontTranslateGoogle;
        }

        private string BuildRequest(string sourceText, string sourceLanguage, string targetLanguage, string key = null)
        {
            string url = BuildBaseRequest(sourceLanguage, targetLanguage, key);
            url += string.Format("&dt=t");
            url += string.Format("&q={0}", sourceText);
            return url;
        }

        private string BuildBaseRequest(string sourceLanguage, string targetLanguage, string key = null)
        {
            string url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}",
                                                sourceLanguage, targetLanguage);
            if (!string.IsNullOrEmpty(key))
            {
                url += string.Format("&key={0}", key);
            }
            return url;
        }

        private string RequestToGoogleApi(string url)
        {
            string translationFromGoogle = "";

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");
                wc.Encoding = System.Text.Encoding.UTF8;
                translationFromGoogle = wc.DownloadString(url);
            }

            return translationFromGoogle;
        }
    }

    public class RespontTranslateGoogle
    {
        public List<string> SourceWords = new List<string>();
        public List<string> TargetWords = new List<string>();
        public string FullRespont;
    }
}

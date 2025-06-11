using EqualchanceGames.Tools.Helpers;
using EqualchanceGames.Tools.InterfaceTranslate;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using UnityEngine.TestTools;

namespace EqualchanceGames.Tools.InterfaceTranslate
{
    public class GoogleApiFree : ITranslateApi
    {
        private const int MAXCHARS_FORREQUST = 5000;
        private const String SEPARATE_STRING = " [𓀀] ";
        private const String SEPARATE_STRING2 = " [⨉] ";
        private const String SEPARATE_STRING3 = "[№]";
        private const String SEPARATE_STRING4 = "[⨉]";
        private const String SEPARATE_STRING5 = "[𓀀]";

		private Dictionary<string, string> _ignoreLocale;

        public GoogleApiFree() 
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
            string translationFromGoogle = RequestToGoogleApi(sourceText, sourceLanguage, targetLanguage, key);

            RespontTranslateGoogle respontTranslateGoogle = DeserializeRespont(translationFromGoogle);

            return respontTranslateGoogle.FullRespont;
        }

        public Dictionary<string, string> Translate(Dictionary<string, string> words, string sourceLanguage, string targetLanguage, bool singleWordTranslation = false, string key = null)
        {
            List<string> listRespontWords = new List<string>();
            Dictionary<string, string> targetWords = new Dictionary<string, string>();

            StringBuilder sourceText = new StringBuilder(MAXCHARS_FORREQUST + 100);
            StringBuilder sourceTryText = new StringBuilder(MAXCHARS_FORREQUST + 100);

            string translationFromGoogle = string.Empty;
            RespontTranslateGoogle respontTranslateGoogle;

            sourceLanguage = MappingLocale(sourceLanguage);
            targetLanguage = MappingLocale(targetLanguage);

            if (singleWordTranslation)
            {
				foreach (var item in words)
				{
					translationFromGoogle = RequestToGoogleApi(item.Value, sourceLanguage, targetLanguage, key);
					respontTranslateGoogle = DeserializeRespont(translationFromGoogle);
					if (string.IsNullOrEmpty(respontTranslateGoogle.FullRespont) == true) listRespontWords.Add("");
					else listRespontWords.Add(respontTranslateGoogle.FullRespont);
				}
			}
            else
            {
				foreach (var item in words)
				{
					string temp = item.Value + SEPARATE_STRING;
					sourceTryText.Append(temp);
					if (sourceTryText.Length > MAXCHARS_FORREQUST)
					{
						translationFromGoogle = RequestToGoogleApi(sourceTryText.ToString(), sourceLanguage, targetLanguage, key);
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

				translationFromGoogle = RequestToGoogleApi(sourceText.ToString(), sourceLanguage, targetLanguage);

				respontTranslateGoogle = DeserializeRespont(translationFromGoogle);
				string response = respontTranslateGoogle.FullRespont;
				String[] mass = new String[5];
				mass[0] = SEPARATE_STRING;
				mass[1] = SEPARATE_STRING4;
				mass[2] = SEPARATE_STRING5;
				mass[3] = SEPARATE_STRING2;
				mass[4] = SEPARATE_STRING3;

				listRespontWords.AddRange(response.Split(mass, StringSplitOptions.None).ToList());
			}

            if (listRespontWords.Count != 0 && String.IsNullOrEmpty(listRespontWords[listRespontWords.Count - 1]))
            {
                listRespontWords.RemoveAt(listRespontWords.Count - 1);
            }

            if (listRespontWords.Count != words.Count)
            {
				foreach (var item in words)
				{
					targetWords.Add(item.Key, "");
				}
			}
            else
            {
				int index = 0;
				foreach (var item in words)
				{
     //               string responseWord = listRespontWords[index];
					//if (item.Value.Length != 0 && responseWord.Length != 0 && )
					targetWords.Add(item.Key, listRespontWords[index]);
					++index;
				}
			}
                
            return targetWords;
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

        private string RequestToGoogleApi(string sourceText, string sourceLanguage, string targetLanguage, string key = null)
        {
            string translationFromGoogle = "";

            string url = string.Empty;
            if ( key == null )
			{
                url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                                                sourceLanguage,
                                                targetLanguage,
                                                sourceText);
            }
            else
			{
                url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}&key={3}",
                                                sourceLanguage,
                                                targetLanguage,
                                                sourceText,
                                                key);
            }

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

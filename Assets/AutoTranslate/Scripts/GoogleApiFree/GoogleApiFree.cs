using GoodTime.Tools.InterfaceTranslate;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GoodTime.Tools.InterfaceTranslate
{
    public class GoogleApiFree : ITranslateApi
    {
        private const int MAXCHARS_FORREQUST = 5000;
        private const String SEPARATE_STRING = "[$]";
        private const String SEPARATE_STRING2 = "[$ ]";
        private const String SEPARATE_STRING3 = "[ $]";

        public string Translate(string sourceText, string sourceLanguage, string targetLanguage)
        {
            string translationFromGoogle = RequestToGoogleApi(sourceText, sourceLanguage, targetLanguage);

            RespontTranslateGoogle respontTranslateGoogle = DeserializeRespont(translationFromGoogle);

            return respontTranslateGoogle.FullRespont;
        }

        public Dictionary<string, string> Translate(Dictionary<string, string> words, string sourceLanguage, string targetLanguage)
        {
            List<string> listRespontWords = new List<string>();
            Dictionary<string, string> targetWords = new Dictionary<string, string>();

            StringBuilder sourceText = new StringBuilder(MAXCHARS_FORREQUST + 100);
            StringBuilder sourceTryText = new StringBuilder(MAXCHARS_FORREQUST + 100);

            string translationFromGoogle = string.Empty;
            RespontTranslateGoogle respontTranslateGoogle;

            foreach (var item in words)
            {
                string temp = item.Value + SEPARATE_STRING;
                sourceTryText.Append(temp);
                if (sourceTryText.Length > MAXCHARS_FORREQUST)
                {
                    translationFromGoogle = RequestToGoogleApi(sourceTryText.ToString(), sourceLanguage, targetLanguage);
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
            String[] mass = new String[3];
            mass[0] = SEPARATE_STRING;
            mass[1] = SEPARATE_STRING2;
            mass[2] = SEPARATE_STRING3;
            listRespontWords.AddRange(response.Split(mass, StringSplitOptions.None).ToList());

            int index = 0;
            foreach (var item in words)
            {
                targetWords.Add(item.Key, listRespontWords[index]);
                ++index;
            }

            return targetWords;
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

        private string RequestToGoogleApi(string sourceText, string sourceLanguage, string targetLanguage)
        {
            string translationFromGoogle = "";

            string url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                                                sourceLanguage,
                                                targetLanguage,
                                                sourceText);

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

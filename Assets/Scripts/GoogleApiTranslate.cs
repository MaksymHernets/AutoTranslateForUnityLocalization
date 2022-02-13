using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GoodTime.Tools.GoogleApiTranslate
{
    public class GoogleApiTranslate
    {
        public string Translate(string word, string sourceLanguage, string targetLanguage)
        {
            string translation = string.Empty;

            string translationFromGoogle = RequestToGoogleApi(word, sourceLanguage, targetLanguage);

            var json = JsonConvert.DeserializeObject(translationFromGoogle);
            var json1 = json as JArray;
            var json2 = json1.First() as JArray;
            var json3 = json2.First() as JArray;
            var json4 = json3.First() as JValue;
            translation = json4.Value.ToString();

            return translation;
        }

        public Dictionary<string, string> Translate(List<string> words, string sourceLanguage, string targetLanguage)
        {
            Dictionary<string, string> translation = new Dictionary<string, string>();

            string sourceText = string.Empty;
            foreach (var item in words)
            {
                sourceText += item + " . ";
            }

            string translationFromGoogle = RequestToGoogleApi(sourceText, sourceLanguage, targetLanguage);

            var json = JsonConvert.DeserializeObject(translationFromGoogle);
            var arrayGeneric = json as JArray;
            var arrayWords = arrayGeneric.First() as JArray;

            foreach (var arrayWord in arrayWords)
            {
                var array = arrayWord as JArray;
                var translatedWord = array[0] as JValue;
                var sourceWord = array[1] as JValue;

                translation.Add(sourceWord.Value.ToString(), translatedWord.Value.ToString() );
            }
           
            return translation;
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
}

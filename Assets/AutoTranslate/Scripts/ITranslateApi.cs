using System.Collections.Generic;

namespace GoodTime.Tools.InterfaceTranslate
{
    public interface ITranslateApi
    {
        public string Translate(string word, string sourceLanguage, string targetLanguage);
        public Dictionary<string, string> Translate(Dictionary<string, string> words, string sourceLanguage, string targetLanguage);
    }
}

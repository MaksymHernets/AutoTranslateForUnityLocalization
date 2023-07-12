using System.Collections.Generic;

namespace GoodTime.Tools.InterfaceTranslate
{
    public interface TextTranslateApi
    {
        string Translate(string word, string sourceLanguage, string targetLanguage, string key = null);
        Dictionary<string, string> Translate(Dictionary<string, string> words, string sourceLanguage, string targetLanguage, string key = null);
        
    }
}

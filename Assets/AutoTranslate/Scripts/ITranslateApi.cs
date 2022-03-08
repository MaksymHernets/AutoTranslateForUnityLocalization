using System.Collections.Generic;

namespace GoodTime.Tools.InterfaceTranslate
{
    public interface ITranslateApi
    {
        string Translate(string word, string sourceLanguage, string targetLanguage);
        Dictionary<string, string> Translate(Dictionary<string, string> words, string sourceLanguage, string targetLanguage);
    }
}

using System.Collections.Generic;

namespace EqualchanceGames.Tools.InterfaceTranslate
{
    public interface ITranslateApi
    {
        string Translate(string word, string sourceLanguage, string targetLanguage, string key = null);
        Dictionary<string, string> Translate(Dictionary<string, string> words, string sourceLanguage, string targetLanguage, bool singleWordTranslation = false, string key = null);
        bool CheckService();
        bool ValidateLocale(string nameLocale);
        string MappingLocale(string nameLocale);
        string GetNameService();
    }
}

namespace GoodTime.Tools.InterfaceTranslate
{
    public interface ITranslateApi
    {
        public string Translate(string word, string sourceLanguage, string targetLanguage);
    }
}

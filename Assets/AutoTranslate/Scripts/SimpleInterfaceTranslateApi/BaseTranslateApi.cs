namespace GoodTime.Tools.InterfaceTranslate
{
    public interface BaseTranslateApi
    {
        bool ValidateLocale(string nameLocale);
        string MappingLocale(string nameLocale);
    }
}

using GoodTime.Tools.InterfaceTranslate;

namespace GoodTime.Tools.FactoryTranslate
{
    public class FactoryTranslateApi
    {
        public static ITranslateApi GetTranslateApi()
        {
            AutoTranslateSetting setting = AutoTranslateSetting.GetOrCreateSettings();

            if (setting.PlatformForTranslate == TypePlatformTranslate.GoogleApisCustom)
            {
                return new GoogleApiTranslate();
            }
            else if (setting.PlatformForTranslate == TypePlatformTranslate.GoogleApisOffical)
            {

            }
            return null;
        }
    }
}
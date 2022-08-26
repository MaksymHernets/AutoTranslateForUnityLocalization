using GoodTime.Tools.InterfaceTranslate;
using System;

namespace GoodTime.Tools.FactoryTranslate
{
    public class FactoryTranslateApi
    {
        public static ITranslateApi GetTranslateApi()
        {
            AutoTranslateSetting setting = AutoTranslateSetting.GetOrCreateSettings();

            if (setting.PlatformForTranslate == TypePlatformTranslate.GoogleApiFree)
            {
                return new GoogleApiTranslate();
            }
            else if (setting.PlatformForTranslate == TypePlatformTranslate.GoogleApi)
            {
                new Exception("not work yet");
            }
            else if (setting.PlatformForTranslate == TypePlatformTranslate.BingApi)
            {
                new Exception("not work yet");
            }
            return null;
        }
    }
}
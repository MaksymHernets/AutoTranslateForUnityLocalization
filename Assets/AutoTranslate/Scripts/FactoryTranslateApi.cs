using GoodTime.Tools.GoogleTranslate.Apis;
using GoodTime.Tools.GoogleTranslate.Web;
using GoodTime.Tools.InterfaceTranslate;
using System;

namespace GoodTime.Tools.FactoryTranslate
{
    public class FactoryTranslateApi
    {
        public static GenericTranslateApi GetTranslateApi()
        {
            AutoTranslateSetting setting = AutoTranslateSetting.GetOrCreateSettings();
            return GetTranslateApi(setting.CurrentServiceTranslate);
        }

        public static GenericTranslateApi GetTranslateApi(TypeServiceTranslate typePlatformTranslate)
        {
            if (typePlatformTranslate == TypeServiceTranslate.GoogleTranslateApis)
            {
                return new GoogleTranslateApis();
            }
            else if (typePlatformTranslate == TypeServiceTranslate.GoogleTranslateWeb)
            {
                return new GoogleTranslateWeb();
            }
            else if (typePlatformTranslate == TypeServiceTranslate.GoogleTranslateCloud)
            {
                new Exception("not work yet");
            }
            else if (typePlatformTranslate == TypeServiceTranslate.BingApi)
            {
                new Exception("not work yet");
            }
            return null;
        }
    }
}
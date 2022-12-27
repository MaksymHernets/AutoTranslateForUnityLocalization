using GoodTime.Tools.InterfaceTranslate;
using System;

namespace GoodTime.Tools.FactoryTranslate
{
    public class FactoryTranslateApi
    {
        public static ITranslateApi GetTranslateApi(TypePlatformTranslate typePlatformTranslate)
        {
            if (typePlatformTranslate == TypePlatformTranslate.GoogleApiFree)
            {
                return new GoogleApiFree();
            }
            else if (typePlatformTranslate == TypePlatformTranslate.GoogleApi)
            {
                new Exception("not work yet");
            }
            else if (typePlatformTranslate == TypePlatformTranslate.BingApi)
            {
                new Exception("not work yet");
            }
            return null;
        }
    }
}
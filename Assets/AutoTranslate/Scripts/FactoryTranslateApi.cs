using EqualchanceGames.Tools.InterfaceTranslate;
using System;

namespace EqualchanceGames.Tools.FactoryTranslate
{
    public class FactoryTranslateApi
    {
        public static ITranslateApi GetTranslateApi(TypeServiceTranslate typePlatformTranslate)
        {
            if (typePlatformTranslate == TypeServiceTranslate.GoogleApiFree)
            {
                return new GoogleApiFree();
            }
            else if (typePlatformTranslate == TypeServiceTranslate.GoogleApi)
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
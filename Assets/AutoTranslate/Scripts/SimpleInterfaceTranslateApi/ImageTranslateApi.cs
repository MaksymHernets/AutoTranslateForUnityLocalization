using System.Collections.Generic;
using UnityEngine;

namespace GoodTime.Tools.InterfaceTranslate
{
    public interface ImageTranslateApi
    {
        Sprite Translate(Sprite sprite, string sourceLanguage, string targetLanguage);
        Dictionary<string, Sprite> Translate(Dictionary<string, Sprite> sprites, string sourceLanguage, string targetLanguage);
        Texture Translate(Texture texture, string sourceLanguage, string targetLanguage);
        Dictionary<string, Texture> Translate(Dictionary<string, Texture> textures, string sourceLanguage, string targetLanguage);
    }
}
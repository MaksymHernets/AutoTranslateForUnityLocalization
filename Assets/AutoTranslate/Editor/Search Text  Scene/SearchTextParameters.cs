using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

namespace GoodTime.HernetsMaksym.AutoTranslate.Editor
{
    public class SearchTextParameters
    {
        public Scene Scene;
        public string NameTable;
        public bool SkipPrefab;
        public Locale SourceLocale;
    }
}

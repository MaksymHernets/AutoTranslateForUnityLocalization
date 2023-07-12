using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using GoodTime.Tools.FactoryTranslate;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
    public class AutoTranslate_AssetTable_EditorWindow
    {
        [MenuItem("Window/Auto Localization/Auto Translate for Asset Tables", false, MyProjectSettings_AutoTranslate.BaseIndex + 2)]
        public static void ShowWindow()
        {
            //Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            //AutoTranslate_StringTable_EditorWindow window = GetWindow<AutoTranslate_StringTable_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            //window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.AutoTranslate);
            //window.Show();

            GenericTranslateApi googleTranslateApiFree = FactoryTranslateApi.GetTranslateApi();

            //Sprite sprite = AssetDatabase.GetAssetPath<Sprite>("asd");
            Sprite sprite = default(Sprite);

            Debug.Log(googleTranslateApiFree.Translate("apple", "en", "ru"));
            
            //googleTranslateApiFree.Translate(sprite, "en", "ru");
        }
    }
}
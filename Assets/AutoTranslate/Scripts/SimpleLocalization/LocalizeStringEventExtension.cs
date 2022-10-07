using System;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
	public static class LocalizeStringEventExtension
    {
        public static void Clear_OnUpdateString(this LocalizeStringEvent localizeStringEvent)
        {
            while (localizeStringEvent.OnUpdateString.GetPersistentEventCount() != 0)
            {
                UnityEventTools.RemovePersistentListener(localizeStringEvent.OnUpdateString, 0);
            }
        }

        public static void Sign_ReferenceTable(this LocalizeStringEvent localizeStringEvent, string TableCollectionName, string Key)
        {
            LocalizedString localizedString = new LocalizedString();
            localizedString.SetReference(TableCollectionName, Key);
            localizeStringEvent.StringReference = localizedString;
        }

        public static void Sign_OnUpdateString_TextLegacy(this LocalizeStringEvent localizeStringEvent, Text text)
        {
            var targetinfo = UnityEvent.GetValidMethodInfo(text, "set_text", new Type[] { typeof(string) });
            if (targetinfo != null)
            {
                UnityAction<string> action = Delegate.CreateDelegate(typeof(UnityAction<string>), text, targetinfo, false) as UnityAction<string>;
                UnityEventTools.AddPersistentListener(localizeStringEvent.OnUpdateString, action);
                EditorUtility.SetDirty(text.gameObject);
            }
        }

        public static void Sign_OnUpdateString_TextMeshPro(this LocalizeStringEvent localizeStringEvent, TextMeshProUGUI text)
        {
            var targetinfo = UnityEvent.GetValidMethodInfo(text, "set_text", new Type[] { typeof(string) });
            if (targetinfo != null)
            {
                UnityAction<string> action = Delegate.CreateDelegate(typeof(UnityAction<string>), text, targetinfo, false) as UnityAction<string>;
                UnityEventTools.AddPersistentListener(localizeStringEvent.OnUpdateString, action);
                EditorUtility.SetDirty(text.gameObject);
            }
        }

        public static void Sign_OnUpdateString_DropdownLegacy(this LocalizeStringEvent localizeStringEvent, Dropdown dropdown)
        {
            var targetinfo = UnityEvent.GetValidMethodInfo(dropdown, "set_text", new Type[] { typeof(string) });
            if (targetinfo != null)
            {
                UnityAction<string> action = Delegate.CreateDelegate(typeof(UnityAction<string>), dropdown, targetinfo, false) as UnityAction<string>;
                UnityEventTools.AddPersistentListener(localizeStringEvent.OnUpdateString, action);
                EditorUtility.SetDirty(dropdown.gameObject);
            }
        }

        public static LocalizeStringEvent GetOrAdd_LocalizeStringEventComponent(GameObject gameObject)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            if (gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent))
                return gameObject.GetComponent<LocalizeStringEvent>();
            else
                return gameObject.AddComponent<LocalizeStringEvent>();
        }
    }
}

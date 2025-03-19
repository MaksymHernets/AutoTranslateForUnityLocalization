using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EqualchanceGames.Tools.InterfaceTranslate
{
    public class AutoTranslateSetting : ScriptableObject
    {
        public const string k_MyCustomSettingsPath = "Assets/AutoTranslate/ProjectSettings/AutoTranslateSetting.asset";

        [SerializeField] public TypeServiceTranslate CurrentServiceTranslate;
        [SerializeField] public ServiceApi[] ServiceApis;

        [NonSerialized] public string TempKeyForService = string.Empty;

        public static AutoTranslateSetting GetOrCreateSettings()
        {
            string[] guids = AssetDatabase.FindAssets("AutoTranslateSetting t:AutoTranslateSetting", null);
            string path = string.Empty;

            if (guids.Length != 0) path = AssetDatabase.GUIDToAssetPath(guids[0]);
            else return null;

            AutoTranslateSetting settings = AssetDatabase.LoadAssetAtPath<AutoTranslateSetting>(path);

            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<AutoTranslateSetting>();
                
                AssetDatabase.CreateAsset(settings, k_MyCustomSettingsPath);
                AssetDatabase.SaveAssets();
            }
            if(settings.ServiceApis == null )
			{
                settings.ServiceApis = GetNamePlatformTranslate();
            }
            if (settings.ServiceApis.Length != 0)
            {
                settings.CurrentServiceTranslate = TypeServiceTranslate.GoogleApiFree;
                settings.ServiceApis[0].HasLimit = true;
                settings.ServiceApis[0].IsActive = true;
                settings.ServiceApis[0].Description = "Free api service from google for text translation.";
            }

            return settings;
        }

        public static ServiceApi[] GetNamePlatformTranslate()
		{
            List<ServiceApi> services = new List<ServiceApi>();

            string[] names = Enum.GetNames(typeof(TypeServiceTranslate));
			foreach (string name in names)
			{
                ServiceApi service = new ServiceApi(name);
                services.Add(service);
            }

            return services.ToArray();
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }

        public void SetCurrentKey(string key)
		{
            ServiceApis[GetServiceIndex()].SetKey(key);
        }

        public string GetCurrentKey()
		{
            return ServiceApis[GetServiceIndex()].Key;
        }

        private int GetServiceIndex()
		{
            string nameservice = CurrentServiceTranslate.ToString();
            for (int i = 0; i < ServiceApis.Length; ++i)
            {
                if (ServiceApis[i].Name == nameservice)
                {
                    return i;
                }
            }
            return -1;
        }

        public ServiceApi GetCurrentService()
		{
            return ServiceApis[GetServiceIndex()];
        }
    }

    public class ServiceApi
	{
        public string Name;
        public string Key;
        public DateTime KeyEnter;
        public bool IsActive;
        public bool HasLimit;
        public int IsOnline;
        public string Description;

        public ServiceApi(string name, string key = null)
		{
            Name = name;
            if( key != null )
			{
                Key = key;
                KeyEnter = DateTime.Now;
            }
            IsActive = false;
            HasLimit = false;
            IsOnline = -2;
        }

        public void SetKey(string key)
		{
            Key = key;
            KeyEnter = DateTime.Now;
        }
    }
}

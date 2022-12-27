using GoodTime.Tools.FactoryTranslate;
using GoodTime.Tools.InterfaceTranslate;
using System.Collections.Generic;
using UnityEngine.Localization.Tables;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
    public class TranslateLocalization
    {
        private ITranslateApi translator;

        public TranslateLocalization()
        {
            AutoTranslateSetting setting = AutoTranslateSetting.GetOrCreateSettings();
            translator = FactoryTranslateApi.GetTranslateApi(setting.PlatformForTranslate);
        }

        public IEnumerable<TranslateStatus> Make(TranslateParameters translateParameters, TranslateData translateData)
        {
            float progressRate = 0.9f / translateData.stringTables.Count;
            int indexTable = 0;

            foreach (var sharedtable in translateData.sharedtables)
            {
                if (translateParameters.IsTranslateStringTables.ContainsKey(sharedtable.TableCollectionName) == false) continue;
                if (translateParameters.IsTranslateStringTables[sharedtable.TableCollectionName] == false) continue;

                StringTable sourceLanguageTable = default(StringTable);
                List<StringTable> tablesForTranslate = new List<StringTable>();

                foreach (var table in translateData.stringTables)
                {
                    if (table.TableCollectionName == sharedtable.TableCollectionName)
                    {
                        if (table.LocaleIdentifier != translateData.selectedLocale.Identifier)
                        {
                            tablesForTranslate.Add(table);
                        }
                        else
                        {
                            sourceLanguageTable = table;
                        }
                    }
                }

                Dictionary<string,string> lists = new Dictionary<string, string>();

                foreach (StringTable targetLanguageTable in tablesForTranslate)
                {
                    ++indexTable;
                    float progress = 0.1f + indexTable * progressRate;

                    yield return new TranslateStatus(progress, sharedtable.TableCollectionName, targetLanguageTable.LocaleIdentifier.CultureInfo.DisplayName);

                    foreach (var entry in sharedtable.Entries)
                    {
                        StringTableEntry sourceWord;
                        if (!sourceLanguageTable.TryGetValue(entry.Id, out sourceWord))
                        {
                            continue;
                        }
                        if (sourceWord == null)
                        {
                            continue;
                        }
                        if (sourceWord.IsSmart == true && translateParameters.canTranslateSmartWords == false)
                        {
                            continue;
                        }

                        StringTableEntry targetWord;
                        if (targetLanguageTable.TryGetValue(entry.Id, out targetWord))
                        {
                            if (translateParameters.canOverrideWords == false)
                            {
                                continue;
                            }
                        }

                        lists.Add(entry.Key, sourceWord.Value);
                    }

                    Dictionary<string, string> result = translator.Translate(lists, sourceLanguageTable.LocaleIdentifier.Code, targetLanguageTable.LocaleIdentifier.Code);

                    foreach (var item in result)
                    {
                        targetLanguageTable.AddEntry(item.Key, item.Value);
                    }

                    lists.Clear();
                }
            }

            yield return new TranslateStatus(1, string.Empty, string.Empty);
        }
    }
}
using EqualchanceGames.Tools.FactoryTranslate;
using EqualchanceGames.Tools.InterfaceTranslate;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Localization.Tables;

namespace EqualchanceGames.Tools.AutoTranslate
{
    public class TranslateLocalization
    {
        private ITranslateApi translator;

        public TranslateLocalization()
        {
            AutoTranslateSetting setting = AutoTranslateSetting.GetOrCreateSettings();
            translator = FactoryTranslateApi.GetTranslateApi(setting.CurrentServiceTranslate);
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
                            foreach (var locale in translateData.locales)
                            {
                                if (locale.Identifier == table.LocaleIdentifier)
                                {
                                    tablesForTranslate.Add(table);
                                }
                            }
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


                    Dictionary<string,string> keyValuePairs = new Dictionary<string,string>();
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
                            if (String.IsNullOrEmpty(targetWord.Value) == false && translateParameters.canOverrideWords == false)
                            {
                                continue;
                            }
                        }

                        string word = sourceWord.Value;

                        if (targetWord != null && sourceWord.IsSmart == true)
                        {
                            targetWord.IsSmart = true;

                            
                            bool key = false;
                            int start = 0; int end = 0;
                            for (int i = 0; i < word.Length; ++i)
                            {
                                if ( key == false && word[i] == '{')
                                {
                                    start = i+1;
                                    key = true;
                                }
                                if ( key == true && word[i] == '}')
                                {
                                    end = i;
                                    key = false;
                                    if (start - end >= 1) continue;
                                    string subkey = word.Substring(start, end - start);
                                    int tempkey = 0;
                                    if( int.TryParse(subkey, out tempkey) == false)
                                    {
                                        keyValuePairs.Add(keyValuePairs.Count.ToString(), subkey);
                                    }
                                    else
                                    {
                                        if ( keyValuePairs.ContainsKey(subkey) )
                                        {
                                            keyValuePairs.Add(keyValuePairs.Count.ToString(), keyValuePairs[subkey]);
                                            keyValuePairs[subkey] = subkey;
                                        }
                                        else
                                        {
                                            keyValuePairs.Add(subkey, subkey);
                                        }
                                    }
                                }
                            }

                            foreach (var keyValuePair in keyValuePairs)
                            {
                                word = word.Replace("{" + keyValuePair.Value + "}", "{" + keyValuePair.Key + "}");
                            }
                        }

                        lists.Add(entry.Key, word);
                    }

                    if ( lists.Count != 0 ) 
                    {
                        Dictionary<string, string> result = translator.Translate(lists, sourceLanguageTable.LocaleIdentifier.Code, targetLanguageTable.LocaleIdentifier.Code, translateParameters.canTranslateEmptyWords);

                        foreach (var item in result)
                        {
                            string word = item.Value;
                            foreach (var keyValuePair in keyValuePairs)
                            {
                                word = word.Replace("{" + keyValuePair.Key + "}", "{" + keyValuePair.Value + "}");
                            }
                            Undo.RecordObject(targetLanguageTable, "AddEntry for StringTable (AutoTranslate)");
                            targetLanguageTable.AddEntry(item.Key, word);
                        }
                    }

                    lists.Clear();
                }
            }

            yield return new TranslateStatus(1, string.Empty, string.Empty);
        }
    }
}
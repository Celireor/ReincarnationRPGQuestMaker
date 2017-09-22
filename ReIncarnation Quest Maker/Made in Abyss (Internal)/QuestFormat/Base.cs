using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Utility;

namespace ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat
{
    public static class QuestVariableExtensions
    {
        public static void GenerateFromKV_Iterate(this object ThisObject, KVPairFolder ThisFolder, Action<KVPair, FieldInfo> IterationAction = null)
        {
            ThisFolder.Items.ForEach(obj =>
            {
                FieldInfo ThisFieldInfo = ThisObject.GetType().GetField(obj.Key);
                if (ThisFieldInfo != null)
                {
                    if (typeof(IConvertible).IsAssignableFrom(ThisFieldInfo.FieldType))
                    {
                        ThisFieldInfo.SetValue(ThisObject, Convert.ChangeType(obj.Value, ThisFieldInfo.FieldType));
                    }
                }
                IterationAction?.Invoke(obj, ThisFieldInfo);
            });
        }

        public static string ConvertToKVText_Iterate(this object ThisObject, int TabCount, bool ConvertEmptyFields)
        {
            string ReturnValue = "";
            FieldInfo[] AllFields = ThisObject.GetType().GetFields();
            for (int x = 0; x < AllFields.Length; x++)
            {
                if (typeof(IConvertible).IsAssignableFrom(AllFields[x].FieldType))
                {
                    string Value = Convert.ToString(AllFields[x].GetValue(ThisObject)); ;
                    if (AllFields[x].FieldType == typeof(bool))
                    {
                        Value = Value.ToLower();
                    }
                    if (ConvertEmptyFields || Value != "")
                    {
                        ReturnValue += QuestVariable.PrintKeyValue(AllFields[x].Name, Value, TabCount);
                    }
                }
            }
            return ReturnValue;
        }
    }

    public abstract class QuestVariable
    {
        public QuestVariableOptionalFields OptionalFields = new QuestVariableOptionalFields();

        public static T GenerateFromKeyValue<T>(KVPair ThisKV) where T : QuestVariable, new()
        {
            T ReturnValue = new T();
            ReturnValue.GenerateFromKV(ThisKV);
            return ReturnValue;
        }

        public static List<T> FillFromKeyValueFolder<T>(KVPairFolder Thisfolder) where T : QuestVariable, new()
        {
            List<T> ReturnValue = new List<T>();
            Thisfolder.Items.ForEach(obj =>
            {
                ReturnValue.Add(GenerateFromKeyValue<T>(obj));
            });
            return ReturnValue;
        }

        public void GenerateFromKeyValue_Iterate(KVPairFolder ThisKV, Action<KVPair, FieldInfo> ThisInfo = null, Action<KVPair, FieldInfo> ThisOptionalInfo = null)
        {
            this.GenerateFromKV_Iterate(ThisKV, ThisInfo);
            OptionalFields.GenerateFromKeyValue_Iterate(ThisKV, ThisOptionalInfo);
        }

        public abstract void GenerateFromKV(KVPair ThisKV);

        public abstract string ConvertToText(int TabCount = 0);

        public string ConvertToText_Iterate(int TabCount)
        {
            return this.ConvertToKVText_Iterate(TabCount, true) + OptionalFields.ConvertToKVText_Iterate(TabCount, false);
        }

        public static string PrintEncapsulation(string StringToEncapsulate, int TabCount, string DesiredListName = "", bool PrintListName = false)
        {
            string ReturnValue = new string('\t', TabCount) + "{" + Environment.NewLine + StringToEncapsulate + new string('\t', TabCount) + "}" + Environment.NewLine;
            if (PrintListName)
            {
                ReturnValue = new string('\t', TabCount) + "\"" + DesiredListName + "\"" + Environment.NewLine + ReturnValue;
            }
            return ReturnValue;
        }

        public static string PrintListVariables<T>(List<T> ThisList, string DesiredListName, int TabCount = 0, bool AlwaysPrint = false) where T : QuestVariable
        {
            if (AlwaysPrint || ThisList.Count > 0)
            {
                string StringToEncapsulate = "";
                ThisList.ForEach(obj => StringToEncapsulate += obj.ConvertToText(TabCount + 1));
                return PrintEncapsulation(StringToEncapsulate, TabCount, DesiredListName, true);
            }
            return "";
        }
        public static string PrintKeyValue(string Key, string Value, int TabCount = 0)
        {

            string ReturnValue = new string('\t', TabCount) + "\"" + Key + "\"\t\"" + Value + "\"" + Environment.NewLine;
            return ReturnValue;
        }
    }

    public class QuestVariableOptionalFields
    {
        public void GenerateFromKeyValue_Iterate(KVPairFolder ThisKV, Action<KVPair, FieldInfo> ThisInfo = null)
        {
            this.GenerateFromKV_Iterate(ThisKV, ThisInfo);
        }
    }

    public abstract class QuestVariableEditorExternal
    {
        public List<Action> OnUpdateList = new List<Action>();
        public void OnUpdate()
        {
            OnUpdateList.ForEach(obj => obj());
        }
    }

    public class KVPair : QuestVariable
    {
        public string Key;
        public string Value;
        public KVPairFolder FolderValue;

        public KVPairEditorExternal ThisEditorExternal = new KVPairEditorExternal();

        public KVPair() { /*Generates an empty KVPair*/ }

        public KVPair(string Key, string Value)
        {
            this.Key = Key;
            this.Value = Value;
        }

        public KVPair(string Key, KVPairFolder FolderValue)
        {
            this.Key = Key;
            this.FolderValue = FolderValue;
        }

        public void Delete()
        {
            Key = null;
            Value = null;
            if (FolderValue != null)
            {
                FolderValue.Delete();
                FolderValue = null;
            }
        }

        public void Trash() {
            if (ThisEditorExternal.EncapsulatingList != null) {
                ThisEditorExternal.EncapsulatingList.Remove(this);
            }
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            Key = ThisKV.Key;
            Value = ThisKV.Value;
        }

        public override string ConvertToText(int TabCount = 0)
        {
            return PrintKeyValue(Key, Value, TabCount);
        }

        public class KVPairEditorExternal : QuestVariableEditorExternal {
            public List<KVPair> EncapsulatingList;
        }
    }

    public class KVList : ListeningList<KVPair> {
        public KVList(List<KVPair> ThisList) : base()
        {
            ThisList.ForEach(obj => Add(obj));
        }

        public KVList() {
            AddListener(SetEncapsulatingList, false, true, false);
        }

        void SetEncapsulatingList(ListeningList<KVPair> Useless, KVPair NewPair) {
            NewPair.ThisEditorExternal.EncapsulatingList = this;
        }
    }

    public class ListeningList<T> : List<T>
    {
        public delegate void Listener(ListeningList<T> ThisList, T NewItem = default(T));
        List<Listener> AddListners = new List<Listener>();
        List<Listener> RemoveListeners = new List<Listener>();

        public new void Remove(T Item)
        {
            base.Remove(Item);
            RemoveListeners.ForEach(obj => obj(this, Item));

        }

        public new void Add(T Item)
        {
            base.Add(Item);
            AddListners.ForEach(obj => obj(this, Item));
        }

        public void AddListener(Listener NewListener, bool InstantlyRun = true, bool OnAdd = true, bool OnRemove = true)
        {
            if (InstantlyRun) {
                NewListener(this, default(T));
            }
            if (OnAdd)
            {
                AddListners.Add(NewListener);
            }
            if (OnRemove)
            {
                RemoveListeners.Add(NewListener);
            }
        }
    }

    public abstract class IndexableVariable : QuestVariable {
        public abstract string ConvertToText_Full(int Index, int TabCount = 0);
    }
}

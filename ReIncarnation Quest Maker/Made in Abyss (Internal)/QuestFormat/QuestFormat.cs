using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Parser;
using System;
using System.Collections.Generic;
using System.Reflection;
using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Utility;

namespace ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat
{
    public static class QuestVariableExtensions {
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
                    string Value;
                    if (AllFields[x].FieldType == typeof(bool))
                    {
                        if ((bool)AllFields[x].GetValue(ThisObject))
                        {
                            Value = "true";
                        }
                        else
                        {
                            Value = "false";
                        }
                    }
                    else
                    {
                        Value = Convert.ToString(AllFields[x].GetValue(ThisObject));
                    }
                    if(ConvertEmptyFields || Value != "")
                    {
                        ReturnValue += new string('\t', TabCount) + "\"" + AllFields[x].Name + "\"\t\"" + Value + "\"" + Environment.NewLine;
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

        public void GenerateFromKeyValue_Iterate(KVPairFolder ThisKV, Action<KVPair, FieldInfo> ThisInfo = null, Action<KVPair, FieldInfo> ThisOptionalInfo = null) {
            this.GenerateFromKV_Iterate(ThisKV, ThisInfo);
            OptionalFields.GenerateFromKeyValue_Iterate(ThisKV, ThisOptionalInfo);
        }

        public abstract void GenerateFromKV(KVPair ThisKV);

        public abstract string ConvertToText(int TabCount = 0);

        public string ConvertToText_Iterate(int TabCount) {
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

        public override void GenerateFromKV(KVPair ThisKV)
        {
            Key = ThisKV.Key;
            Value = ThisKV.Value;
        }

        public override string ConvertToText(int TabCount = 0)
        {
            return new string('\t', TabCount) + "\"" + Key + "\"\t\"" + Value + "\"" + Environment.NewLine;
        }
    }

    public class UniqueList<T> : List<T>
    {
        public Func<T, T, bool> Predicate;
        public List<Action<UniqueList<T>>> Listeners = new List<Action<UniqueList<T>>>();

        public UniqueList(Func<T, T, bool> Predicate)
        {
            this.Predicate = Predicate;
        }

        public void Remove(T Item) {
            base.Remove(Item);
            Listeners.ForEach(obj => obj(this));

        }

        public void Add(T Item)
        {
            base.Add(Item);
            Listeners.ForEach(obj => obj(this));
        }

        public void AddListener (Action<UniqueList<T>> Listener)
        {
            Listener(this);
            Listeners.Add(Listener);
        }
    }

    public class QuestList : QuestVariable
    {
        public QuestList_EditorExternal ThisEditorExternal = new QuestList_EditorExternal();

        public List<Quest> Quests = new List<Quest>();

        public override string ConvertToText(int TabCount = 0)
        {
            string ReturnString = "";
            Quests.ForEach(obj => { ReturnString += obj.ConvertToText(); });
            return ReturnString;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            KVPairFolder ThisKVPairFolder = ThisKV.FolderValue;
            ThisKVPairFolder.Items.ForEach(obj =>
            {
                Quest NewQuest = GenerateFromKeyValue<Quest>(obj);
                Quests.Add(NewQuest);
                ThisEditorExternal.PossibleTypeIcons.Add(NewQuest.typeIcon);
            });

        }

        public void SortQuests()
        {
            Quests.Sort(Quest.SortByQuestID());
        }

        public class QuestList_EditorExternal : QuestVariableEditorExternal
        {

            public string FilePath;
            public int LargestQuestID;
            public UniqueList<string> PossibleTypeIcons = new UniqueList<string>((a, b) => a == b);
            public UniqueList<string> PossibleQuestPrerequisites = new UniqueList<string>((a, b) => a == b);
            public UniqueList<string> PossibleQuestOptionSelectImage = new UniqueList<string>((a, b) => a == b);

            public QuestList_EditorExternal()
            {
                PossibleTypeIcons.Add("story");
                PossibleTypeIcons.Add("class");
                PossibleTypeIcons.Add("repeatable");

                PossibleQuestPrerequisites.Add("level");

                PossibleQuestOptionSelectImage.Add("story");
                PossibleQuestOptionSelectImage.Add("story_repeatable");
                PossibleQuestOptionSelectImage.Add("check");
                PossibleQuestOptionSelectImage.Add("chat");
            }
        }
    }

    public class Quest : QuestVariable
    {

        public string full_name;
        public string name;
        public string description;
        public bool repeatable;
        public int questID;
        public string portrait;
        public string typeIcon;
        public bool cantAbandon;

        public List<KVPair> prerequisites = new List<KVPair>();

        public List<QuestStage> stages = new List<QuestStage>();

        public List<QuestDialogue> injections = new List<QuestDialogue>();

        public Quest_EditorExternal ThisEditorExternal = new Quest_EditorExternal();

        public static Quest GenerateDefaultQuest(int questID)
        {
            Quest ReturnValue = new Quest();
            ReturnValue.questID = questID;
            ReturnValue.typeIcon = "story";
            QuestStage.Generate(0, ReturnValue);

            return ReturnValue;
        }

        public override string ConvertToText(int TabCount = 0)
        {
            string ReturnValue = "\"" + questID + "\"" + Environment.NewLine;

            /* Contents of Quest */
            string StringToEncapsulate = ConvertToText_Iterate(TabCount + 1);

            //string injectionsString = QuestDialogue.co

            /* Contents of Prerequisites */
            StringToEncapsulate += PrintListVariables(prerequisites, "prerequisites", TabCount + 1);


            string affectedNPCsString;
            /* Contents of Stages */
            StringToEncapsulate += QuestStage.ConvertToText_Full(stages, out affectedNPCsString, TabCount + 1);

            /* Contents of injections */
            string injectionsString = "";
            injections.ForEach(obj => {
                injectionsString += obj.ConvertToText(TabCount + 2);
            });
            StringToEncapsulate += PrintEncapsulation(injectionsString, TabCount + 1, "injections", true);

            /* Encapsulate everything */
            ReturnValue += PrintEncapsulation(StringToEncapsulate, TabCount);

            return ReturnValue;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            GenerateFromKeyValue_Iterate(ThisKV.FolderValue, (KVPair IterationKV, FieldInfo ThisFieldInfo) =>
            {

                switch (IterationKV.Key)
                {
                    case "prerequisites":
                        {
                            prerequisites = FillFromKeyValueFolder<KVPair>(IterationKV.FolderValue);
                        }
                        break;
                    /*case "affectedNpcs":
                        {
                            IterationKV.FolderValue.Items.ForEach((obj) =>
                            {
                                //QuestInjectDialogue newAffectedNPC = QuestInjectDialogue.GenerateFromKV_Parent(obj, this);
                            });
                        }
                        break;*/
                    case "stages":
                        {
                            IterationKV.FolderValue.Items.ForEach((obj) =>
                            {
                                QuestStage.KVGenerate(obj, this);
                            });
                        }
                        break;
                    case "injections":
                        {
                            IterationKV.FolderValue.Items.ForEach((obj) =>
                            {
                                QuestDialogue.KVGenerate(obj, this);
                            });
                        }
                        break;
                }
            });
        }

        public static IComparer<Quest> SortByQuestID()
        {
            return new QuestIDComparer();
        }

        private class QuestIDComparer : IComparer<Quest>
        {
            int IComparer<Quest>.Compare(Quest x, Quest y)
            {
                return x.questID.CompareTo(y.questID);
            }
        }

        public class Quest_EditorExternal : QuestVariableEditorExternal
        {
            public UniqueList<String> PossibleDialogueInjections = new UniqueList<string>((a, b) => (a == b));
        }
    }

    public class QuestStage : QuestVariable
    {
        public string stageName;
        public string description;

        //public List<QuestInjectDialogue> affectedNPCs = new List<QuestInjectDialogue>();

        public List<QuestTask> tasks = new List<QuestTask>();
        public List<KVPair> dialogue = new List<KVPair>();
        public List<KVPair> particles = new List<KVPair>();

        public QuestStage_EditorExternal ThisEditorExternal = new QuestStage_EditorExternal();

        public void Trash()
        {
            ThisEditorExternal.Parent.stages.Remove(this);
            ThisEditorExternal.Parent.stages.AssignIndexValuesToListItems((obj, index) =>
            {
                obj.ThisEditorExternal.StageNum = index;
                obj.ThisEditorExternal.OnUpdate();
            });
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            //dontuse.
            throw new NotImplementedException();
        }

        public static void MassGenerate<T>(QuestStage ReturnValue, KVPair obj, string typeString) where T : QuestTask, new()
        {
            if (obj.Key == typeString)
            {
                obj.FolderValue.Items.ForEach(obj2 =>
                {
                    QuestTask.KVGenerate<T>(ReturnValue, obj2);
                });
            }
        }

        public static QuestStage KVGenerate(KVPair ThisKV, Quest Parent)
        {
            int Num = Convert.ToInt32(ThisKV.Key);
            QuestStage ReturnValue = Insert(Num, Parent);
            ReturnValue.GenerateFromKeyValue_Iterate(ThisKV.FolderValue, (KVPair IterationKV, FieldInfo ThisField) =>
            {
                switch (IterationKV.Key)
                {
                    //opens the tasks folder
                    case "tasks":
                        {
                            IterationKV.FolderValue.Items.ForEach(obj =>
                            {
                                MassGenerate<QuestTask_location>(ReturnValue, obj, "location");
                                MassGenerate<QuestTask_gather>(ReturnValue, obj, "gather");
                                MassGenerate<QuestTask_kill>(ReturnValue, obj, "kill");
                                MassGenerate<QuestTask_killType>(ReturnValue, obj, "killType");
                                MassGenerate<QuestTask_talkto>(ReturnValue, obj, "talkto");
                            });
                        }
                        break;
                }
            });
            return ReturnValue;
        }

        public static QuestStage Generate(int StageNum, Quest Parent)
        {
            QuestStage ReturnValue = new QuestStage();
            ReturnValue.ThisEditorExternal.StageNum = StageNum;
            ReturnValue.ThisEditorExternal.Parent = Parent;
            Parent.stages.Add(ReturnValue);
            return ReturnValue;
        }

        class QuestTaskStringConverter<T> where T : QuestTask
        {
            List<T> TaskList = new List<T>();
            string TaskType;

            public QuestTaskStringConverter(string TaskType) { this.TaskType = TaskType; }

            public void Add(QuestTask obj)
            {
                Type ObjType = obj.GetType();
                if (ObjType == typeof(T))
                {
                    TaskList.Add(obj as T);
                }
            }

            public string Print(int TabCount)
            {

                if (TaskList.Count > 0)
                {
                    int index = 0;
                    string taskstring = "";
                    TaskList.ForEach(obj =>
                    {
                        taskstring += obj.ConvertToText_Full(index, TabCount + 2);
                        index++;
                    });
                    return PrintEncapsulation(taskstring, TabCount + 1, TaskType, true);
                }
                return "";
            }
        }

        public override string ConvertToText(int TabCount = 0)
        {
            string stringtoencapsulate = ConvertToText_Iterate(TabCount + 1);

            if (tasks.Count > 0)
            {
                string TaskString = "";

                QuestTaskStringConverter<QuestTask_gather> GatherTaskList = new QuestTaskStringConverter<QuestTask_gather>("gather");
                QuestTaskStringConverter<QuestTask_location> LocationTasksList = new QuestTaskStringConverter<QuestTask_location>("location");
                QuestTaskStringConverter<QuestTask_talkto> TalkToTaskList = new QuestTaskStringConverter<QuestTask_talkto>("talkto");
                QuestTaskStringConverter<QuestTask_kill> KillTaskList = new QuestTaskStringConverter<QuestTask_kill>("kill");
                QuestTaskStringConverter<QuestTask_killType> KillTypeTaskList = new QuestTaskStringConverter<QuestTask_killType>("killType");

                tasks.ForEach(obj =>
                {
                    GatherTaskList.Add(obj);
                    LocationTasksList.Add(obj);
                    TalkToTaskList.Add(obj);
                    KillTaskList.Add(obj);
                    KillTypeTaskList.Add(obj);
                });

                TaskString += GatherTaskList.Print(TabCount);
                TaskString += LocationTasksList.Print(TabCount);
                TaskString += TalkToTaskList.Print(TabCount);
                TaskString += KillTypeTaskList.Print(TabCount);
                TaskString += KillTaskList.Print(TabCount);

                stringtoencapsulate += PrintEncapsulation(TaskString, TabCount, "tasks", true);
            }

            return PrintEncapsulation(stringtoencapsulate, TabCount, Convert.ToString(ThisEditorExternal.StageNum), true);
        }

        public static string ConvertToText_Full(List<QuestStage> Group, out string affectedNPCsString, int TabCount = 0)
        {
            string ReturnValue = "";

            string affectedNPCsString_ReturnValue = "";

            Group.ForEach(obj =>
            {
                /*obj.affectedNPCs.ForEach(obj2 => {
                    affectedNPCsString_ReturnValue += obj2.ConvertToText(TabCount + 1);
                });*/

                ReturnValue += obj.ConvertToText(TabCount + 1);
            });

            affectedNPCsString = PrintEncapsulation(affectedNPCsString_ReturnValue, TabCount, "affectedNpcs", true);

            return PrintEncapsulation(ReturnValue, TabCount, "stages", true);
            //AffectedNPCsText = PrintEncapsulation(AffectedNPCsText_ReturnValue, TabCount, "affectedNPCs", true);
        }

        public static QuestStage Insert(int StageNum, Quest Parent)
        {
            QuestStage ReturnValue = Parent.stages.Find(a => a.ThisEditorExternal.StageNum == StageNum); ;
            if (ReturnValue == null)
            {
                ReturnValue = Generate(StageNum, Parent);
            }
            return ReturnValue;
        }

        public class QuestStage_EditorExternal : QuestVariableEditorExternal
        {
            public Quest Parent;
            public int StageNum;
        }
    }

    public class QuestDialogue : QuestVariable
    {
        public List<QuestDialogueOption> options = new List<QuestDialogueOption>();

        public QuestDialogue_EditorExternal ThisEditorExternal = new QuestDialogue_EditorExternal();

        public override string ConvertToText(int TabCount = 0)
        {
            string ReturnValue = "";
            int x = 0;
            options.ForEach(obj =>
            {
                x++;
                ReturnValue += PrintEncapsulation(obj.ConvertToText(TabCount + 2), TabCount + 1, x.ToString(), true);
            });
            if (ReturnValue != "") {
                ReturnValue = PrintEncapsulation(ReturnValue, TabCount, ThisEditorExternal.DialogueName, true);
            }
            return ReturnValue;
        }

        public static QuestDialogue Generate(Quest Parent) {

            QuestDialogue ReturnValue = new QuestDialogue();
            ReturnValue.ThisEditorExternal.ThisQuest = Parent;

            Parent.injections.Add(ReturnValue);
            return ReturnValue;
        }

        public static QuestDialogue KVGenerate(KVPair ThisKV, Quest Parent) {
            QuestDialogue ReturnValue = Generate(Parent);
            ReturnValue.ThisEditorExternal.ChangeName(ThisKV.Key);
            ThisKV.FolderValue.Items.ForEach(obj => QuestDialogueOption.KVGenerate(obj, Parent, ReturnValue.options));

            return ReturnValue;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            throw new NotImplementedException();
        }

        public class QuestDialogue_EditorExternal : QuestVariableEditorExternal
        {
            public string DialogueName = "";
            public Quest ThisQuest;
            bool IsAdded = false;
            public void ChangeName(string NewName) {
                ThisQuest.ThisEditorExternal.PossibleDialogueInjections.Remove(DialogueName);
                DialogueName = NewName;
                ThisQuest.ThisEditorExternal.PossibleDialogueInjections.Add(DialogueName);
            }
        }
    }

    public class QuestDialogueOption : QuestVariable
    {
        public string selectText;
        public string selectImg;

        public QuestDialogueResponse Response;

        public QuestDialogueOption_OptionalFields ThisOptionalFields {
            get { return (QuestDialogueOption_OptionalFields)OptionalFields; }
        }

        public QuestDialogueOption() {
            OptionalFields = new QuestDialogueOption_OptionalFields();
        }

        public QuestDialogueOption_EditorExternal ThisEditorExternal = new QuestDialogueOption_EditorExternal();

        public override string ConvertToText(int TabCount = 0)
        {
            string ReturnValue = "";

            bool HasConverted = false;
            if (ThisEditorExternal.AutoGenerateListenStringID && ThisOptionalFields.sendListenString != "") {
                ThisOptionalFields.sendListenString = Convert.ToString(ThisEditorExternal.Parent.questID) + "_" + ThisOptionalFields.sendListenString;
                HasConverted = true;
            }

            ReturnValue += ConvertToText_Iterate(TabCount);

            /*if (HasConverted)
            {
                ReturnValue += new string('\t', TabCount) + "\"" + "listeningQuest" + "\"\t\"" + ThisEditorExternal.listeningQuest + "\"" + Environment.NewLine; ;
            }*/

            if (Response != null) {
                ReturnValue += PrintEncapsulation(Response.ConvertToText(TabCount + 1), TabCount, "goto", true);
            }

            if (HasConverted) {
                int NumberToRemove = ThisEditorExternal.Parent.questID.ToString().Length + 1;
                ThisOptionalFields.sendListenString.Remove(0, NumberToRemove);
            }

            return ReturnValue;
        }

        public static QuestDialogueOption Generate(Quest Parent, List<QuestDialogueOption> ParentList) {
            QuestDialogueOption ReturnValue = new QuestDialogueOption();
            ReturnValue.ThisEditorExternal.Parent = Parent;
            ParentList.Add(ReturnValue);
            return ReturnValue;
        }

        public static QuestDialogueOption KVGenerate(KVPair ThisKV, Quest Parent, List<QuestDialogueOption> ParentList) {
            QuestDialogueOption ReturnValue = Generate(Parent, ParentList);

            ReturnValue.GenerateFromKeyValue_Iterate(ThisKV.FolderValue, (obj, objType) => {
                if (obj.Key == "goto")
                {
                    ReturnValue.Response = QuestDialogueResponse.KVGenerate(obj, Parent);
                }
                /*if (obj.Key == "listeningQuest")
                {
                    ReturnValue.ThisEditorExternal.listeningQuest = Convert.ToInt32(obj.Value);
                }*/
            });
            if (ReturnValue.ThisOptionalFields.sendListenString != "") {
                int NumberToRemove = ReturnValue.ThisEditorExternal.Parent.questID.ToString().Length + 1;
                ReturnValue.ThisOptionalFields.sendListenString.Remove(0, NumberToRemove);
            }

            return ReturnValue;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            throw new NotImplementedException();
        }

        public class QuestDialogueOption_EditorExternal : QuestVariableEditorExternal
        {
            public bool AutoGenerateListenStringID = true;
            public Quest Parent;

            //public int listeningQuest;
        }

        public class QuestDialogueOption_OptionalFields : QuestVariableOptionalFields
        {
            //public string questMarker = "";
            //public string selectBackground = "";
            public string sendListenString = "";
        }
    }

    public class QuestDialogueResponse : QuestVariable
    {
        public string responseText = "";
        public List<QuestDialogueOption> options = new List<QuestDialogueOption>();
        public List<KVPair> QuestsToGive = new List<KVPair>();
        public QuestDialogueResponse_OptionalFields ThisOptionalFields {
            get
            {
                return OptionalFields as QuestDialogueResponse_OptionalFields;
            }
        }

        public QuestDialogueResponse() {
            OptionalFields = new QuestDialogueResponse_OptionalFields();
        }

        public override string ConvertToText(int TabCount = 0)
        {
            string ReturnValue = "";

            ReturnValue += ConvertToText_Iterate(TabCount);

            string OptionsString = "";
            int x = 0;

            options.ForEach(obj =>
            {
                x++;
                OptionsString += PrintEncapsulation(obj.ConvertToText(TabCount + 2), TabCount + 1, x.ToString(), true);
            });

            ReturnValue += PrintEncapsulation(OptionsString, TabCount, "options", true);

            return ReturnValue;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            throw new NotImplementedException();
        }

        public static QuestDialogueResponse KVGenerate(KVPair ThisKV, Quest Parent)
        {
            QuestDialogueResponse ReturnValue = new QuestDialogueResponse();

            ReturnValue.GenerateFromKeyValue_Iterate(ThisKV.FolderValue, (obj, objType) => {
                if (obj.Key == "options")
                {
                    obj.FolderValue.Items.ForEach(obj2 => {
                        QuestDialogueOption.KVGenerate(obj2, Parent, ReturnValue.options);
                    });
                }
            });

            return ReturnValue;
        }

        public class QuestDialogueResponse_OptionalFields : QuestVariableOptionalFields
        {
            public string responsePortait = "";
            public string playSound = "";
        }
    }
}

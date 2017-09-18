using ReIncarnation_Quest_Maker.Made_in_Abyss.Parser;
using System;
using System.Collections.Generic;
using System.Reflection;
using ReIncarnation_Quest_Maker.Made_in_Abyss.Utility;

namespace ReIncarnation_Quest_Maker.Made_in_Abyss.QuestFormat
{
    public abstract class QuestVariable {
        public static T GenerateFromKeyValue<T>(KVPair ThisKV) where T : QuestVariable, new()
        {
            T ReturnValue = new T();
            ReturnValue.GenerateFromKV(ThisKV);
            return ReturnValue;
        }

        public static List<T> FillFromKeyValueFolder<T>(KVPairFolder Thisfolder) where T : QuestVariable, new()
        {
            List<T> ReturnValue = new List<T>();
            Thisfolder.Items.ForEach(obj => {
                ReturnValue.Add(GenerateFromKeyValue<T>(obj));
            });
            return ReturnValue;
        }

        public abstract void GenerateFromKV(KVPair ThisKV);

        public void GenerateFromKeyValue_Iterate(KVPairFolder ThisFolder, Action<KVPair, FieldInfo> IterationAction = null)
        {
            ThisFolder.Items.ForEach(obj => {
                FieldInfo ThisFieldInfo = GetType().GetField(obj.Key);
                if (ThisFieldInfo != null) {
                    if (typeof(IConvertible).IsAssignableFrom(ThisFieldInfo.FieldType))
                    {
                        ThisFieldInfo.SetValue(this, Convert.ChangeType(obj.Value, ThisFieldInfo.FieldType));
                    }
                }
                IterationAction?.Invoke(obj, ThisFieldInfo);
            });
        }

        public abstract string ConvertToText(int TabCount = 0);

        public string ConvertToText_Iterate(int TabCount)
        {
            string ReturnValue = "";
            FieldInfo[] AllFields = GetType().GetFields();
            for (int x = 0; x < AllFields.Length; x++) {
                if (typeof(IConvertible).IsAssignableFrom(AllFields[x].FieldType)) {
                    string Value;
                    if (AllFields[x].FieldType == typeof(bool))
                    {
                        if ((bool)AllFields[x].GetValue(this))
                        {
                            Value = "true";
                        }
                        else
                        {
                            Value = "false";
                        }
                    }
                    else {
                        Value = Convert.ToString(AllFields[x].GetValue(this));
                    }
                    ReturnValue += new string('\t', TabCount) + "\"" + AllFields[x].Name + "\"\t\"" + Value + "\"" + Environment.NewLine;
                }
            }
            return ReturnValue;
        }

        public static string PrintEncapsulation(string StringToEncapsulate, int TabCount, string DesiredListName = "", bool PrintListName = false) {
            string ReturnValue = new string('\t', TabCount) + "{" + Environment.NewLine + StringToEncapsulate + new string('\t', TabCount) + "}" + Environment.NewLine;
            if (PrintListName) {
                ReturnValue = new string('\t', TabCount) + "\"" + DesiredListName + "\"" + Environment.NewLine + ReturnValue;
            }
            return ReturnValue;
        }

        public static string PrintListVariables<T>(List<T> ThisList, string DesiredListName, int TabCount = 0, bool AlwaysPrint = false) where T : QuestVariable {
            if (AlwaysPrint || ThisList.Count > 0)
            {
                string StringToEncapsulate = "";
                ThisList.ForEach(obj => StringToEncapsulate += obj.ConvertToText(TabCount + 1));
                return PrintEncapsulation(StringToEncapsulate, TabCount, DesiredListName, true);
            }
            return "";
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

    public class UniqueList<T> : List<T> {
        public Func<T, T, bool> Predicate;

        public UniqueList(Func<T, T, bool> Predicate) {
            this.Predicate = Predicate;
        }

        public T TryAdd(T Item) {
            T ReturnValue = Find(a => Predicate(a, Item));
            if (ReturnValue == null) {
                ReturnValue = Item;
                Add(Item);
            }
            return ReturnValue;
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
            ThisKVPairFolder.Items.ForEach(obj => {
                Quest NewQuest = GenerateFromKeyValue<Quest>(obj);
                Quests.Add(NewQuest);
                ThisEditorExternal.PossibleTypeIcons.TryAdd(NewQuest.typeIcon);
            });

        }

        public void SortQuests() {
            Quests.Sort(Quest.SortByQuestID());
        }

        public class QuestList_EditorExternal : QuestVariableEditorExternal
        {

            public string FilePath;
            public int LargestQuestID;
            public UniqueList<string> PossibleTypeIcons = new UniqueList<string>((a, b) => a == b);

            public QuestList_EditorExternal()
            {
                PossibleTypeIcons.TryAdd("story");
                PossibleTypeIcons.TryAdd("class");
                PossibleTypeIcons.TryAdd("repeatable");
            }
        }
    }

    public class Quest : QuestVariable {

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

        public Quest_EditorExternal ThisEditorExternal = new Quest_EditorExternal();

        public static Quest GenerateDefaultQuest(int questID) {
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



            string affectedNPCsString;
			
            /* Contents of Stages */
            string stagesString = QuestStage.ConvertToText_Full(stages, out affectedNPCsString, TabCount + 1);


            StringToEncapsulate += affectedNPCsString;
            /* Contents of Prerequisites */
            StringToEncapsulate += PrintListVariables(prerequisites, "prerequisites", TabCount + 1);

            StringToEncapsulate += stagesString;
            /* Encapsulate everything */
            ReturnValue += PrintEncapsulation(StringToEncapsulate, TabCount);

            return ReturnValue;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            GenerateFromKeyValue_Iterate(ThisKV.FolderValue, (KVPair IterationKV, FieldInfo ThisFieldInfo) => {

                switch (IterationKV.Key)
                {
                    case "prerequisites":
                        {
                            prerequisites = FillFromKeyValueFolder<KVPair>(IterationKV.FolderValue);
                        }
                        break;
                    case "affectedNpcs":
                        {
                            IterationKV.FolderValue.Items.ForEach((obj) => {
                                //QuestInjectDialogue newAffectedNPC = QuestInjectDialogue.GenerateFromKV_Parent(obj, this);
                            });
                        }
                        break;
                    case "stages":
                        {
                            IterationKV.FolderValue.Items.ForEach((obj) => {
                                QuestStage.KVGenerate(obj, this);
                            });
                        }
                        break;
                }
            });
        }

        public static IComparer<Quest> SortByQuestID() {
            return new QuestIDComparer();
        }

        private class QuestIDComparer : IComparer<Quest> {
            int IComparer<Quest>.Compare(Quest x, Quest y) {
                return x.questID.CompareTo(y.questID);
            }
        }

        public class Quest_EditorExternal : QuestVariableEditorExternal
        {
        }
    }

    public class QuestStage : QuestVariable
    {
        public string stageName;
        public string description;

        //public List<QuestInjectDialogue> affectedNPCs = new List<QuestInjectDialogue>();

        public List<QuestTask> tasks = new List<QuestTask>();

        public QuestStage_EditorExternal ThisEditorExternal = new QuestStage_EditorExternal();

        public void Trash()
        {
            ThisEditorExternal.Parent.stages.Remove(this);
            ThisEditorExternal.Parent.stages.AssignIndexValuesToListItems((obj, index) => {
                obj.ThisEditorExternal.StageNum = index;
                obj.ThisEditorExternal.OnUpdate();
            });
        }

        public static QuestStage KVGenerate(KVPair ThisPair, Quest Parent)
        {
            int Num = Convert.ToInt32(ThisPair.Key);
            QuestStage ReturnValue = Insert(Num, Parent);
            ReturnValue.GenerateFromKeyValue_Iterate(ThisPair.FolderValue, (KVPair IterationKV, FieldInfo ThisField) => {
                switch (IterationKV.Key)
                {
                    //opens the tasks folder
                    case "tasks":
                        {
                            IterationKV.FolderValue.Items.ForEach(obj =>
                            {
                                switch (obj.Key)
                                {
                                    //opens the task type folder
                                    case "location":
                                        {
                                            obj.FolderValue.Items.ForEach(obj2 =>
                                            {
                                                QuestTask.KVGenerate<QuestTask_location>(ReturnValue, obj2);
                                            });
                                        }
                                        break;
                                }
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

        class QuestTaskStringConverter<T> where T : QuestTask {
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

            public string Print (int TabCount) {

                if (TaskList.Count > 0)
                {
                    int index = 0;
                    string taskstring = "";
                    TaskList.ForEach(obj => {
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

            if (tasks.Count > 0) {
                string TaskString = "";

                QuestTaskStringConverter<QuestTask_gather> GatherTaskList = new QuestTaskStringConverter<QuestTask_gather>("gather");

                QuestTaskStringConverter<QuestTask_location> LocationTasksList = new QuestTaskStringConverter<QuestTask_location>("location");

                QuestTaskStringConverter<QuestTask_talkto> TalkToTaskList = new QuestTaskStringConverter<QuestTask_talkto>("talkto");

                QuestTaskStringConverter<QuestTask_kill> KillTaskList = new QuestTaskStringConverter<QuestTask_kill>("kill");

                tasks.ForEach(obj =>
                {
                    GatherTaskList.Add(obj);
                    LocationTasksList.Add(obj);
                    TalkToTaskList.Add(obj);
                    KillTaskList.Add(obj);
                });

                TaskString += GatherTaskList.Print(TabCount);
                TaskString += LocationTasksList.Print(TabCount);
                TaskString += TalkToTaskList.Print(TabCount);
                TaskString += KillTaskList.Print(TabCount);

                stringtoencapsulate += PrintEncapsulation(TaskString, TabCount, "tasks", true);
            }

            return PrintEncapsulation(stringtoencapsulate, TabCount, Convert.ToString(ThisEditorExternal.StageNum), true);
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            //dontuse.
            throw new NotImplementedException();
        }

        public static string ConvertToText_Full(List<QuestStage> Group, out string affectedNPCsString, int TabCount = 0)
        {
            string ReturnValue = "";

            string affectedNPCsString_ReturnValue = "";

            Group.ForEach(obj => {
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

    public abstract class QuestTask : QuestVariable
    {
        public static UniqueList<string> PossibleTaskTypes = new UniqueList<string>((a, b) => a == b);

        public static Dictionary<string, Type> TaskTypeDictionary = new Dictionary<string, Type>();
        public QuestTask_EditorExternal ThisEditorExternal = new QuestTask_EditorExternal();

        public bool Trash()
        {
            ThisEditorExternal.ParentStage.tasks.Remove(this);
            return true;
        }

        static QuestTask()
        {
            AddPossibleTaskType("location", typeof(QuestTask_location));
            AddPossibleTaskType("talkto", typeof(QuestTask_talkto));
            AddPossibleTaskType("kill", typeof(QuestTask_kill));
            AddPossibleTaskType("gather", typeof(QuestTask_gather));
        }
        public QuestTask() { }

        public static T KVGenerate <T> (QuestStage Parent, KVPair ThisKV) where T : QuestTask, new() {
            T ReturnValue = new T();
            GenerateEmpty(ReturnValue, Parent);
            ReturnValue.GenerateFromKV(ThisKV);
            return ReturnValue;
        }

        public static QuestTask Generate(string TypeString, QuestStage Parent)
        {
            Type NewType;
            bool Exists = TaskTypeDictionary.TryGetValue(TypeString, out NewType);
            if (Exists && NewType.BaseType == typeof(QuestTask)) {
                QuestTask ReturnValue = GenerateEmpty(Activator.CreateInstance(NewType) as QuestTask, Parent);
                return ReturnValue;
            }
#if DEBUG

            if (Exists && !(NewType.BaseType == typeof(QuestTask)))
            {
                throw new ArgumentException(NewType.ToString() + " is not a QuestTask");
            }
            throw new ArgumentException(NewType.ToString() + " is not listed.");
#else
            return null;
#endif
        }

        static QuestTask GenerateEmpty(QuestTask InputValue, QuestStage Parent)
        {
            QuestTask ReturnValue = InputValue;
            ReturnValue.ThisEditorExternal.ParentStage = Parent;
            ReturnValue.ThisEditorExternal.GenerateDefaultValues?.Invoke();
            Parent.tasks.Add(ReturnValue);
            return ReturnValue;
        }

        public static void AddPossibleTaskType(string TypeString, Type TaskType) {
            PossibleTaskTypes.Add(TypeString);
            TaskTypeDictionary.Add(TypeString, TaskType);
        }

        public override string ConvertToText(int TabCount = 0)
        {
            string StringToEncapsulate = ConvertToText_Iterate(TabCount + 1);
            return StringToEncapsulate;
        }

        public abstract string ConvertToText_Full(int Index, int TabCount = 0);

        public override void GenerateFromKV(KVPair ThisKV)
        {
            GenerateFromKeyValue_Iterate(ThisKV.FolderValue);
        }
    }

    public class QuestTask_EditorExternal : QuestVariableEditorExternal
    {
        public QuestStage ParentStage;
        public Action GenerateDefaultValues; 
    }

    public class QuestTask_location : QuestTask
    {
        public string name;
        public string locationString;
        public int radius;
        public new QuestTaskLocation_EditorExternal ThisEditorExternal = new QuestTaskLocation_EditorExternal();

        public QuestTask_location() { }

        public override string ConvertToText_Full(int Index, int TabCount)
        {
            string StringToEncapsulate = ConvertToText(TabCount);
            return PrintEncapsulation(StringToEncapsulate, TabCount, Convert.ToString(Index), true);
        }

        public class QuestTaskLocation_EditorExternal
        {
        }
    }

    public class QuestTask_talkto : QuestTask
    {
        public string description;
        public string listenString;
        public string completionString;

        public QuestTask_talkto() { }

        public override string ConvertToText_Full(int Index, int TabCount)
        {
            string StringToEncapsulate = ConvertToText(TabCount);
            return PrintEncapsulation(StringToEncapsulate, TabCount, Convert.ToString(Index), true);
        }

        public class QuestTaskLocation_EditorExternal
        {
        }
    }

    public class QuestTask_kill : QuestTask
    {
        public int amount;
        public new QuestTaskLocation_EditorExternal ThisEditorExternal = new QuestTaskLocation_EditorExternal();

        public QuestTask_kill() { }

        public new string ConvertToText(int TabCount)
        {
            return PrintEncapsulation(base.ConvertToText(TabCount + 1), TabCount, ThisEditorExternal.TargetName, true);
        }

        public class QuestTaskLocation_EditorExternal : QuestTask_EditorExternal
        {
            public string TargetName;
        }
        public override string ConvertToText_Full(int Index, int TabCount = 0)
        {
            return ConvertToText(TabCount);
        }

    }
    public class QuestTask_gather : QuestTask
    {
        public int required;
        public new QuestTaskLocation_EditorExternal ThisEditorExternal = new QuestTaskLocation_EditorExternal();

        public QuestTask_gather() { }

        public new string ConvertToText(int TabCount)
        {
            return PrintEncapsulation(base.ConvertToText(TabCount + 1), TabCount, ThisEditorExternal.ItemName, true);
        }

        public class QuestTaskLocation_EditorExternal : QuestTask_EditorExternal
        {
            public string ItemName;
        }
        public override string ConvertToText_Full(int Index, int TabCount = 0)
        {
            return ConvertToText(TabCount);
        }
    }
    public class QuestTask_killType : QuestTask
    {
        public string customName;
        public int amount;
        public new QuestTaskLocation_EditorExternal ThisEditorExternal = new QuestTaskLocation_EditorExternal();

        public QuestTask_gather() { }

        public new string ConvertToText(int TabCount)
        {
            return PrintEncapsulation(base.ConvertToText(TabCount + 1), TabCount, ThisEditorExternal.ItemName, true);
        }

        public class QuestTaskLocation_EditorExternal : QuestTask_EditorExternal
        {
            public string ItemName;
        }
        public override string ConvertToText_Full(int Index, int TabCount = 0)
        {
            return ConvertToText(TabCount);
        }
    }

    /*public class QuestTask_talkto : QuestTask
    {
        public string description;
        public string listenString;
        public string completionString;

        public QuestTaskLocation_EditorExternal ThisEditorExternal = new QuestTaskLocation_EditorExternal();

        public override string ConvertToText(int TabCount = 0)
        {
            string StringToEncapsulate = ConvertToText_Iterate(TabCount + 1);
            return PrintEncapsulation(StringToEncapsulate, TabCount, Convert.ToString(ThisEditorExternal.ID), true);
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            GenerateFromKeyValue_Iterate(ThisKV.FolderValue);
            ThisEditorExternal.ID = Convert.ToInt32(ThisKV.Key);
        }

        public class QuestTaskLocation_EditorExternal : QuestTask_EditorExternal
        {
            public int ID;
        }
    }*/



    /*public class QuestTask_talkto : QuestTask {
        public string description;
        public string listenString;
        public string completionString;
    }*/

    /*public class QuestInjectDialogue : QuestVariable {
        public string NPCAffected;
        public string Dialogue;
        public QuestStage ThisStage;
		
        public override string ConvertToText(int TabCount = 0)
        {
            //return new string('\t', TabCount) + "\"" + Convert.ToString(ThisStage.ThisEditorExternal.StageNum) + "\"\t\"" + NPCAffected + "\"" + Environment.NewLine;
            return new string('\t', TabCount) + "\"" + NPCAffected + "\"\t\"" + Dialogue + "\"" + Environment.NewLine;
        }

        public static QuestInjectDialogue Generate(QuestStage Parent)
        {
            QuestInjectDialogue returnValue = new QuestInjectDialogue();
            returnValue.ThisStage = Parent;
            Parent.affectedNPCs.Add(returnValue);
            return returnValue;
        }

        /*public static QuestInjectDialogue GenerateFromKV_Parent(KVPair ThisKV, Quest Parent)
        {
            QuestInjectDialogue returnValue = new QuestInjectDialogue();
            returnValue.ThisStage = QuestStage.Insert(0, Parent);
            returnValue.GenerateFromKV(ThisKV);
            return returnValue;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            /*int StageNum = Convert.ToInt32(ThisKV.Key);
            Insert(StageNum);
            NPCAffected = ThisKV.Value;

            NPCAffected = ThisKV.Key;
            Dialogue = ThisKV.Value;
        }

        private void AddToGroup(QuestStage NewGroup)
        {
            ThisStage = NewGroup;
            ThisStage.affectedNPCs.Add(this);
        }
        public void RemoveFromGroup()
        {
            ThisStage.affectedNPCs.Remove(this);
            ThisStage = null;
        }

        public void Insert (int StageNum)
        {
            QuestStage NewStage = QuestStage.Insert(StageNum, ThisStage.ThisEditorExternal.Parent);
           
            AddToGroup(NewStage);
        }

        public void ModifyStageNum(int NewStageNum) {
            RemoveFromGroup();
            Insert(NewStageNum);
        }
    }

    /*public class QuestTask : QuestVariable {

    }

    public class */
}

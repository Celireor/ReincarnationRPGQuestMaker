using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat
{
    public abstract class QuestTask : IndexableVariable
    {
        public static ListeningList<string> PossibleTaskTypes = new ListeningList<string>();

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
            AddPossibleTaskType("killType", typeof(QuestTask_killType));
        }
        public QuestTask() { }

        public static T KVGenerate<T>(QuestStage Parent, KVPair ThisKV) where T : QuestTask, new()
        {
            T ReturnValue = new T();
            GenerateEmpty(ReturnValue, Parent);
            ReturnValue.GenerateFromKV(ThisKV);
            return ReturnValue;
        }

        public static QuestTask Generate(string TypeString, QuestStage Parent)
        {
            Type NewType;
            bool Exists = TaskTypeDictionary.TryGetValue(TypeString, out NewType);
            if (Exists && NewType.BaseType == typeof(QuestTask))
            {
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

        public static void AddPossibleTaskType(string TypeString, Type TaskType)
        {
            PossibleTaskTypes.Add(TypeString);
            TaskTypeDictionary.Add(TypeString, TaskType);
        }

        public override string ConvertToText(int TabCount = 0)
        {
            string StringToEncapsulate = ConvertToText_Iterate(TabCount + 1);
            return StringToEncapsulate;
        }

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

        public override void GenerateFromKV(KVPair ThisKV)
        {
            GenerateFromKeyValue_Iterate(ThisKV.FolderValue);
            ThisEditorExternal.TargetName = ThisKV.Key;
        }

        public new string ConvertToText(int TabCount)
        {
            return PrintEncapsulation(base.ConvertToText(TabCount), TabCount, ThisEditorExternal.TargetName, true);
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

        public override void GenerateFromKV(KVPair ThisKV)
        {
            GenerateFromKeyValue_Iterate(ThisKV.FolderValue);
            ThisEditorExternal.ItemName = ThisKV.Key;
        }

        public class QuestTaskLocation_EditorExternal : QuestTask_EditorExternal
        {
            public string ItemName;
        }
        public override string ConvertToText_Full(int Index, int TabCount = 0)
        {
            return PrintEncapsulation(base.ConvertToText(TabCount), TabCount, ThisEditorExternal.ItemName, true);
        }
    }
    public class QuestTask_killType : QuestTask
    {
        public string customName;
        public int amount;
        public new QuestTaskLocation_EditorExternal ThisEditorExternal = new QuestTaskLocation_EditorExternal();


        public override void GenerateFromKV(KVPair ThisKV)
        {
            GenerateFromKeyValue_Iterate(ThisKV.FolderValue);
            ThisEditorExternal.EnemyGroupName = ThisKV.Key;
        }

        public QuestTask_killType() { }

        public class QuestTaskLocation_EditorExternal : QuestTask_EditorExternal
        {
            public string EnemyGroupName;
        }
        public override string ConvertToText_Full(int Index, int TabCount = 0)
        {
            return PrintEncapsulation(base.ConvertToText(TabCount), TabCount, ThisEditorExternal.EnemyGroupName, true);
        }
    }
}

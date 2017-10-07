using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat
{
    public abstract class QuestTask : MultiTypeVariable<QuestTask>
    {
        public static new ListeningList<string> PossibleTaskTypes = new ListeningList<string>();

        public QuestTask_EditorExternal ThisEditorExternal = new QuestTask_EditorExternal();

        public static void MassGenerate<T>(QuestStage Parent, KVPair obj, string typeString) where T : QuestTask, new()
        {
            if (obj.Key == typeString)
            {
                bool ShouldGenerateMultiple = true;
                obj.FolderValue.Items.ForEach(obj2 =>
                {
                    if (obj2.FolderValue == null) {
                        ShouldGenerateMultiple = false;
                    }
                });

                if (ShouldGenerateMultiple == false)
                {
                    KVGenerate<T>(Parent, obj);
                }
                else {

                    obj.FolderValue.Items.ForEach(obj2 =>
                    {
                        KVGenerate<T>(Parent, obj2);
                    });
                }
            }
        }

        public override string ConvertToText(int TabCount = 0)
        {
            string StringToEncapsulate = ConvertToText_Iterate(TabCount + 1);
            return StringToEncapsulate;
        }

        //public abstract string ConvertToText_Full(int Index, int TabCount = 0);

        public override bool Trash()
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
            AddPossibleTaskType("event", typeof(QuestTask_event));
            AddPossibleTaskType("useAbility", typeof(QuestTask_useAbility));
            AddPossibleTaskType("reachLevel", typeof(QuestTask_reachLevel));
        }
        public QuestTask() {
            OptionalFields = new QuestTask_OptionalFields();
        }

        public static T KVGenerate<T>(QuestStage Parent, KVPair ThisKV) where T : QuestTask, new()
        {
            T ReturnValue = new T();
            GenerateEmpty(ReturnValue, Parent);
            ReturnValue.GenerateFromKV(ThisKV);
            return ReturnValue;
        }

        public static QuestTask Generate(string TypeString, QuestStage Parent)
        {
            return GenerateEmpty(GetNewT(TypeString), Parent);
        }

        public static new void AddPossibleTaskType(string TypeString, Type TaskType)
        {
            MultiTypeVariable<QuestTask>.AddPossibleTaskType(TypeString, TaskType);
            PossibleTaskTypes.Add(TypeString);
        }

        static QuestTask GenerateEmpty(QuestTask InputValue, QuestStage Parent)
        {
            QuestTask ReturnValue = InputValue;
            ReturnValue.ThisEditorExternal.ParentStage = Parent;
            Parent.tasks.Add(ReturnValue);
            return ReturnValue;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            GenerateFromKeyValue_Iterate(ThisKV.FolderValue);
        }
    }

    public class QuestTask_EditorExternal : QuestVariableEditorExternal
    {
        public QuestStage ParentStage;
    }

    public class QuestTask_OptionalFields : QuestVariableOptionalFields
    {

    }

    public class QuestTask_location : QuestTask
    {
        public string name;
        public string locationString;
        public int radius;
        public new QuestTaskLocation_EditorExternal ThisEditorExternal = new QuestTaskLocation_EditorExternal();
        public QuestTaskLocation_OptionalFields ThisOptionalFields { get { return OptionalFields as QuestTaskLocation_OptionalFields; } }

        public QuestTask_location()  { OptionalFields = new QuestTaskLocation_OptionalFields(); }

        public override string ConvertToText_Full(int Index, int TabCount)
        {
            string StringToEncapsulate = ConvertToText(TabCount);
            return PrintEncapsulation(StringToEncapsulate, TabCount, Convert.ToString(Index), true);
        }

        public class QuestTaskLocation_EditorExternal
        {
        }

        public class QuestTaskLocation_OptionalFields : QuestTask_OptionalFields
        {
            public string completionString;
        }
    }

    public class QuestTask_talkto : QuestTask
    {
        public string description;
        public string listenString;

        public new QuestTaskTalkTo_EditorExternal ThisEditorExternal { get { return base.ThisEditorExternal as QuestTaskTalkTo_EditorExternal; } }
        public QuestTaskTalkTo_OptionalFields ThisOptionalFields { get { return OptionalFields as QuestTaskTalkTo_OptionalFields; } }

        public override bool Trash()
        {
            ThisEditorExternal.ParentStage.ThisEditorExternal.Parent.ThisEditorExternal.ParentQuestList.ThisEditorExternal.AllTalkTos.Remove(this);
            return base.Trash();
        }
        public QuestTask_talkto() {
            base.ThisEditorExternal = new QuestTaskTalkTo_EditorExternal();
            OptionalFields = new QuestTaskTalkTo_OptionalFields();
        }

        public override string ConvertToText_Full(int Index, int TabCount)
        {
            string StringToEncapsulate = ConvertToText(TabCount);
            return PrintEncapsulation(StringToEncapsulate, TabCount, Convert.ToString(Index), true);
        }

        public void UpdateListenString()
        {
            ThisEditorExternal.ListeningStringQuestLength = QuestDialogueOption_OptionalFields.GetQuestIDFromListenString(listenString) - 1;

            if (ThisEditorExternal.ListeningStringQuestLength > 0)
            {
                Quest OldListeningStringParent = ThisEditorExternal.ListenStringParent;
                int NewListenStringID = Convert.ToInt32(listenString.Remove(ThisEditorExternal.ListeningStringQuestLength));
                if (ThisEditorExternal.ParentStage.ThisEditorExternal.Parent.questID == NewListenStringID)
                {
                    ThisEditorExternal.ListenStringParent = ThisEditorExternal.ParentStage.ThisEditorExternal.Parent;
                }
                else
                {
                    ThisEditorExternal.ListenStringParent = ThisEditorExternal.ParentStage.ThisEditorExternal.Parent.ThisEditorExternal.ParentQuestList.GetQuest(NewListenStringID);
                }
                if (ThisEditorExternal.ListenStringParent != null)
                {
                    ThisEditorExternal.ListenStringParent.ThisEditorExternal.OnUpdateList.Add(UpdateListenStringID);
                }
                if (OldListeningStringParent != null)
                {
                    OldListeningStringParent.ThisEditorExternal.OnUpdateList.Remove(UpdateListenStringID);
                }
            }
            else
            {
                if (ThisEditorExternal.ListenStringParent != null)
                {
                    ThisEditorExternal.ListenStringParent.ThisEditorExternal.OnUpdateList.Remove(UpdateListenStringID);
                }
            }
        }

        public void UpdateListenStringID()
        {
            if (listenString != "")
            {
                listenString = listenString.Remove(0, ThisEditorExternal.ListeningStringQuestLength + 1);
                ThisEditorExternal.ListeningStringQuestLength = ThisEditorExternal.ListenStringParent.questID.ToString().Length;
                listenString = ThisEditorExternal.ListenStringParent.questID.ToString() + '_' + listenString;
                ThisEditorExternal.OnUpdate();
            }
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            base.GenerateFromKV(ThisKV);
            ThisEditorExternal.ParentStage.ThisEditorExternal.Parent.ThisEditorExternal.ParentQuestList.ThisEditorExternal.AllTalkTos.Add(this);
            UpdateListenString();
        }

        public class QuestTaskTalkTo_EditorExternal : QuestTask_EditorExternal
        {
            public int ListeningStringQuestLength;
            public Quest ListenStringParent;
        }

        public class QuestTaskTalkTo_OptionalFields : QuestTask_OptionalFields
        {
            public string completionString;
        }
    }

    public class QuestTask_kill : QuestTask
    {
        public int amount;
        public new QuestTaskLocation_EditorExternal ThisEditorExternal = new QuestTaskLocation_EditorExternal();

        public QuestTask_kill()  { }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            GenerateFromKeyValue_Iterate(ThisKV.FolderValue);
            ThisEditorExternal.TargetName = ThisKV.Key;
        }

        public class QuestTaskLocation_EditorExternal : QuestTask_EditorExternal
        {
            public string TargetName;
        }
        public override string ConvertToText_Full(int Index, int TabCount = 0)
        {
            return PrintEncapsulation(base.ConvertToText(TabCount), TabCount, ThisEditorExternal.TargetName, true);
        }

    }
    public class QuestTask_gather : QuestTask
    {
        public int required;
        public new QuestTaskLocation_EditorExternal ThisEditorExternal = new QuestTaskLocation_EditorExternal();
        public QuestTaskGather_OptionalFields ThisOptionalFields { get { return OptionalFields as QuestTaskGather_OptionalFields; } }
        //public 

        public QuestTask_gather()  {
            OptionalFields = new QuestTaskGather_OptionalFields();
        }

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

        public class QuestTaskGather_OptionalFields : QuestTask_OptionalFields {
           public bool consume;
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

        public QuestTask_killType()  { }

        public class QuestTaskLocation_EditorExternal : QuestTask_EditorExternal
        {
            public string EnemyGroupName;
        }

        public override string ConvertToText_Full(int Index, int TabCount = 0)
        {
            return PrintEncapsulation(base.ConvertToText(TabCount), TabCount, ThisEditorExternal.EnemyGroupName, true);
        }
    }

    public class QuestTask_event : QuestTask
    {
        public string eventName;
        public string description;
        public override string ConvertToText_Full(int Index, int TabCount = 0)
        {
            return PrintEncapsulation(ConvertToText_Iterate(TabCount + 1), TabCount, Index.ToString(), true);
        }
    }

    public class QuestTask_useAbility : QuestTask
    {
        public string description;
        public int amount;
        public new QuestTask_useAbility_EditorExternal ThisEditorExternal { get { return base.ThisEditorExternal as QuestTask_useAbility_EditorExternal; } }

        public QuestTask_useAbility() {
            base.ThisEditorExternal = new QuestTask_useAbility_EditorExternal();
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            GenerateFromKeyValue_Iterate(ThisKV.FolderValue);
            ThisEditorExternal.AbilityName = ThisKV.Key;
        }

        public override string ConvertToText_Full(int Index, int TabCount = 0)
        {
            return PrintEncapsulation(ConvertToText_Iterate(TabCount + 1), TabCount, ThisEditorExternal.AbilityName, true);
        }

        public class QuestTask_useAbility_EditorExternal : QuestTask_EditorExternal {
            public string AbilityName;
        }
    }

    public class QuestTask_reachLevel : QuestTask
    {
        public int level;
        public string description;

        public override string ConvertToText_Full(int Index, int TabCount = 0)
        {
            return PrintEncapsulation(ConvertToText_Iterate(TabCount + 1), TabCount, Index.ToString(), true);
        }
    }
}

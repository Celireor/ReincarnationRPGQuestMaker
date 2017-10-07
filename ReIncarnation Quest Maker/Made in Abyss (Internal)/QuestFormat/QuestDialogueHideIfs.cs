using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Utility;

namespace ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat
{
    public abstract class QuestDialogueOptionHideIf : MultiTypeVariable<QuestDialogueOptionHideIf>
    {
        public QuestDialogueOptionHideIf_EditorExternal ThisEditorExternal = new QuestDialogueOptionHideIf_EditorExternal();

        public KVPair ThisKV = new KVPair();

        public override bool Trash()
        {
            ThisEditorExternal.ParentOption.hideIf.Remove(this);
            return true;
        }

        static QuestDialogueOptionHideIf()
        {
            AddPossibleTaskType("quests", typeof(QuestDialogueOptionHideIf_Quest));
            AddPossibleTaskType("prereqs", typeof(QuestDialogueOptionHideIf_Prereqs));
            AddPossibleTaskType("items", typeof(QuestDialogueOptionHideIf_Item));
        }

        public static void MassGenerate<T>(QuestDialogueOption Parent, KVPair obj, string typeString) where T : QuestDialogueOptionHideIf, new()
        {
            if (obj.Key == typeString)
            {
                obj.FolderValue.Items.ForEach(obj2 =>
                {
                    if (obj2.FolderValue == null)
                    {
                        KVGenerate<T>(Parent, obj2);
                    }
                    else {
                        obj2.FolderValue.Items.ForEach(obj3 => {
                            KVGenerate<T>(Parent, obj3);
                        });
                    }
                });
            }
        }

        public static T KVGenerate<T>(QuestDialogueOption Parent, KVPair ThisKV) where T : QuestDialogueOptionHideIf, new()
        {
            T ReturnValue = new T();
            GenerateEmpty(ReturnValue, Parent);
            ReturnValue.GenerateFromKV(ThisKV);
            return ReturnValue;
        }

        public static QuestDialogueOptionHideIf Generate(string TypeString, QuestDialogueOption Parent)
        {
            QuestDialogueOptionHideIf ReturnValue = GenerateEmpty(GetNewT(TypeString), Parent);
            ReturnValue.ThisKV.Value = "active";
            return ReturnValue;
        }

        static QuestDialogueOptionHideIf GenerateEmpty(QuestDialogueOptionHideIf InputValue, QuestDialogueOption Parent)
        {
            QuestDialogueOptionHideIf ReturnValue = InputValue;
            ReturnValue.ThisEditorExternal.ParentOption = Parent;
            Parent.hideIf.Add(ReturnValue);
            return ReturnValue;
        }

        public override string ConvertToText_Full(int Index, int TabCount = 0)
        {
            return ThisKV.ConvertToText(TabCount);
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            this.ThisKV.GenerateFromKV(ThisKV);
        }

        public virtual void SetDefaultValues() { }
    }

    public class QuestDialogueOptionHideIf_EditorExternal : QuestVariableEditorExternal
    {
        public QuestDialogueOption ParentOption;
    }

    public class QuestDialogueOptionHideIf_Quest : QuestDialogueOptionHideIf
    {

        public static ListeningList<string> PossibleQuestStates = new ListeningList<string>();
        public static ListeningList<string> PossiblePrereqStates = new ListeningList<string>();

        static QuestDialogueOptionHideIf_Quest()
        {
            PossibleQuestStates.Add("complete");
            PossibleQuestStates.Add("incomplete");
            PossibleQuestStates.Add("active");

            PossiblePrereqStates.Add("belowLevel");
            PossiblePrereqStates.Add("aboveLevel");
            PossiblePrereqStates.Add("belowTier");
            PossiblePrereqStates.Add("aboveTier");
        }

        public override void SetDefaultValues()
        {
            ThisKV.Value = "active";
        }
    }

    public class QuestDialogueOptionHideIf_Prereqs : QuestDialogueOptionHideIf
    {
    }

    public class QuestDialogueOptionHideIf_Item : QuestDialogueOptionHideIf
    {
    }

    public class QuestDialogueOptionHideIf_Event : QuestDialogueOptionHideIf
    {
    }
}

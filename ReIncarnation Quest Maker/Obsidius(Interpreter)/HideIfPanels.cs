using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat;
using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Utility;

namespace ReIncarnation_Quest_Maker
{
    public class QuestDialogueHideIfPanel : MultiTypePanel<QuestDialogueHideIfPanel, QuestDialogueOptionHideIf>
    {
        public ModifyQuestVariableTable ThisTable;
        public override bool Trash_Addon()
        {
            return base.Trash_Addon();
        }

        static QuestDialogueHideIfPanel()
        {
            ItemToPanel.Add(typeof(QuestDialogueOptionHideIf_Quest), typeof(QuestDialogueHideIfPanel_Quest));
            ItemToPanel.Add(typeof(QuestDialogueOptionHideIf_Prereqs), typeof(QuestDialogueHideIfPanel_Prereqs));
            ItemToPanel.Add(typeof(QuestDialogueOptionHideIf_Item), typeof(QuestDialogueHideIfPanel_Item));
        }

        public QuestDialogueHideIfPanel(OrganizedControlList<QuestDialogueHideIfPanel, QuestDialogueOptionHideIf> Parent) : base(Parent)
        {
            ThisTable = new ModifyQuestVariableTable();
            AddControl(ThisTable);
        }

        public override void Move_Addon(QuestDialogueHideIfPanel OtherObject, int OtherPosition)
        {
            ThisQuestVariable.ThisEditorExternal.ParentOption.hideIf.Swap(ListPosition, OtherPosition);
        }

        public override void Generate_Addon(QuestDialogueOptionHideIf Item, OrganizedControlList<QuestDialogueHideIfPanel, QuestDialogueOptionHideIf> Parent)
        {
            throw new NotImplementedException();
        }
    }

    public class QuestDialogueHideIfPanel_Quest : QuestDialogueHideIfPanel
    {
        public QuestDialogueHideIfPanel_Quest(OrganizedControlList<QuestDialogueHideIfPanel, QuestDialogueOptionHideIf> Parent) : base(Parent) { }

        public new QuestDialogueOptionHideIf_Quest ThisQuestVariable
        {
            get
            {
                return (QuestDialogueOptionHideIf_Quest)base.ThisQuestVariable;
            }
        }

        public void ModifyQuestID(object sender, EventArgs e)
        {
            ThisQuestVariable.ThisKV.Key = ((int)MainForm.GetNumberFromNumericUpDown(sender)).ToString();
        }

        public void ModifyQuestState(object sender, EventArgs e)
        {
            ThisQuestVariable.ThisKV.Value = MainForm.GetTextFromComboBox(sender);
        }


        public override void Generate_Addon(QuestDialogueOptionHideIf Item_raw, OrganizedControlList<QuestDialogueHideIfPanel, QuestDialogueOptionHideIf> Parent)
        {
            QuestDialogueOptionHideIf_Quest Item = (QuestDialogueOptionHideIf_Quest)Item_raw;

            ThisTable.AddItem("Quest ID", new DefaultNumericUpDown(Convert.ToInt32(Item.ThisKV.Key)), ModifyQuestID);
            ThisTable.AddItem("Quest State", new DefaultDropDown(Item.ThisKV.Value, QuestDialogueOptionHideIf_Quest.PossibleQuestStates), ModifyQuestState);
        }
    }

    public class QuestDialogueHideIfPanel_Prereqs : QuestDialogueHideIfPanel
    {
        public QuestDialogueHideIfPanel_Prereqs(OrganizedControlList<QuestDialogueHideIfPanel, QuestDialogueOptionHideIf> Parent) : base(Parent) { }

        public new QuestDialogueOptionHideIf_Prereqs ThisQuestVariable
        {
            get
            {
                return (QuestDialogueOptionHideIf_Prereqs)base.ThisQuestVariable;
            }
        }

        public void ModifyPrereqValue(object sender, EventArgs e)
        {
            ThisQuestVariable.ThisKV.Value = ((int)MainForm.GetNumberFromNumericUpDown(sender)).ToString();
        }

        public void ModifyPrereqState(object sender, EventArgs e)
        {
            ThisQuestVariable.ThisKV.Key = MainForm.GetTextFromComboBox(sender);
        }


        public override void Generate_Addon(QuestDialogueOptionHideIf Item_raw, OrganizedControlList<QuestDialogueHideIfPanel, QuestDialogueOptionHideIf> Parent)
        {
            QuestDialogueOptionHideIf_Prereqs Item = (QuestDialogueOptionHideIf_Prereqs)Item_raw;

            ThisTable.AddItem("Prerequisite State", new DefaultDropDown(Item.ThisKV.Key, QuestDialogueOptionHideIf_Quest.PossiblePrereqStates), ModifyPrereqState);
            ThisTable.AddItem("Prerequisite Value", new DefaultNumericUpDown(Convert.ToInt32(Item.ThisKV.Value)), ModifyPrereqValue);
        }
    }

    public class QuestDialogueHideIfPanel_Item : QuestDialogueHideIfPanel
    {
        public QuestDialogueHideIfPanel_Item(OrganizedControlList<QuestDialogueHideIfPanel, QuestDialogueOptionHideIf> Parent) : base(Parent)
        {
        }

        public override void Generate_Addon(QuestDialogueOptionHideIf Item_raw, OrganizedControlList<QuestDialogueHideIfPanel, QuestDialogueOptionHideIf> Parent)
        {
            QuestDialogueOptionHideIf_Item Item = (QuestDialogueOptionHideIf_Item)Item_raw;

            ThisTable.AddItem("Item Name", new DefaultTextBox(Item.ThisKV.Key), (a, b) => Item.ThisKV.Key = MainForm.GetTextFromTextBox(a));
            ThisTable.AddItem("Item Count", new DefaultTextBox(Item.ThisKV.Value), (a, b) => Item.ThisKV.Value = ((int)MainForm.GetNumberFromNumericUpDown(a)).ToString());
        }
    }

    //public class QuestDialogueHideIfPanel_tasks
}

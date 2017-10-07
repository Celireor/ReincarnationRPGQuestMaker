using System;

using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat;
using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Utility;
using ReIncarnation_Quest_Maker.Obsidius;

namespace ReIncarnation_Quest_Maker
{
    public class QuestTaskPanel : MultiTypePanel<QuestTaskPanel, QuestTask>
    {
        public ModifyQuestVariableTable ThisTable;

        public override bool Trash_Addon()
        {
            return ThisQuestVariable.Trash();
        }

        public QuestTaskPanel(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent)
        {
            ThisTable = new ModifyQuestVariableTable();
            AddControl(ThisTable);
        }

        static QuestTaskPanel()
        {
            ItemToPanel.Add(typeof(QuestTask_location), typeof(QuestTaskPanel_location));
            ItemToPanel.Add(typeof(QuestTask_talkto), typeof(QuestTaskPanel_talkto));
            ItemToPanel.Add(typeof(QuestTask_kill), typeof(QuestTaskPanel_kill));
            ItemToPanel.Add(typeof(QuestTask_gather), typeof(QuestTaskPanel_gather));
            ItemToPanel.Add(typeof(QuestTask_killType), typeof(QuestTaskPanel_killType));
            ItemToPanel.Add(typeof(QuestTask_event), typeof(QuestTaskPanel_event));
            ItemToPanel.Add(typeof(QuestTask_useAbility), typeof(QuestTaskPanel_useAbility));
            ItemToPanel.Add(typeof(QuestTask_reachLevel), typeof(QuestTaskPanel_reachLevel));
        }

        public override void Move_Addon(QuestTaskPanel OtherObject, int OtherPosition)
        {
            ThisQuestVariable.ThisEditorExternal.ParentStage.tasks.Swap(ListPosition, OtherPosition);
        }
    }

    public class QuestTaskPanel_location : QuestTaskPanel
    {
        public QuestTask_location ThisItem;

        public void ModifyLocationName(object sender, EventArgs e)
        {
            ThisItem.name = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyLocationInternalName(object sender, EventArgs e)
        {
            ThisItem.locationString = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyLocationRadius(object sender, EventArgs e)
        {
            ThisItem.radius = (int)MainForm.GetNumberFromNumericUpDown(sender);
        }

        public void ModifyLocationCompletionString(object sender, EventArgs e)
        {
            ThisItem.ThisOptionalFields.completionString = MainForm.GetTextFromTextBox(sender);
        }

        public QuestTaskPanel_location(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public override void Generate_Addon(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_location Item = (QuestTask_location)Item_raw;

            ThisItem = Item;

            ThisTable.AddItem("Name", new DefaultTextBox(Item.name), ModifyLocationName);
            ThisTable.AddItem("Location Name", new DefaultTextBox(Item.locationString), ModifyLocationInternalName);
            ThisTable.AddItem("Radius", new DefaultNumericUpDown(Item.radius), ModifyLocationRadius);
            ThisTable.AddItem("Completion String", new DefaultTextBox(Item.ThisOptionalFields.completionString), ModifyLocationCompletionString);
        }
    }
    public class QuestTaskPanel_talkto : QuestTaskPanel
    {
        public QuestTask_talkto ThisItem;
        public DefaultDropDown ListenString;
        public Quest ListenStringParent;
        public int ListeningStringQuestLength;

        public void ModifyTalkToDescription(object sender, EventArgs e)
        {
            ThisItem.description = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyTalkToListenString(object sender, EventArgs e)
        {
            ThisItem.listenString = MainForm.GetTextFromComboBox(sender);

            ThisItem.UpdateListenString();
            ListenString.Text = ThisItem.listenString;
        }

        public void ModifyTalkToCompletionString(object sender, EventArgs e)
        {
            ThisItem.ThisOptionalFields.completionString = MainForm.GetTextFromTextBox(sender);
        }

        public QuestTaskPanel_talkto(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public override void Generate_Addon(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {

            QuestTask_talkto Item = (QuestTask_talkto)Item_raw;

            ThisItem = Item;

            ThisTable.AddItem("Description", new DefaultTextBox(Item.description), ModifyTalkToDescription);
            ListenString = new DefaultDropDown(Item.listenString, Interpreter.CurrentQuestList.ThisEditorExternal.PossibleListenStrings, true);
            ThisTable.AddItem("Listen String", ListenString, ModifyTalkToListenString);
            ThisTable.AddItem("Completion String", new DefaultTextBox(Item.ThisOptionalFields.completionString), ModifyTalkToCompletionString);

            ThisItem.ThisEditorExternal.OnUpdateList.Add(UpdateListenString);

            /*Interpreter.CurrentQuestList.ThisEditorExternal.PossibleListenStrings.AddListener((useless, item) => {

            }, false, false, true)*/
        }

        protected override void Dispose(bool disposing)
        {
            ThisItem.ThisEditorExternal.OnUpdateList.Remove(UpdateListenString);
            base.Dispose(disposing);
        }

        public void UpdateListenString()
        {
            ListenString.Text = ThisItem.listenString;
        }
    }
    public class QuestTaskPanel_kill : QuestTaskPanel
    {
        public QuestTask_kill ThisItem;

        public void ModifyKillTarget(object sender, EventArgs e)
        {
            ThisItem.ThisEditorExternal.TargetName = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyKillAmount(object sender, EventArgs e)
        {
            ThisItem.amount = (int)MainForm.GetNumberFromNumericUpDown(sender);
        }

        public QuestTaskPanel_kill(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public override void Generate_Addon(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_kill Item = (QuestTask_kill)Item_raw;

            ThisItem = Item;

            ThisTable.AddItem("Target Name", new DefaultTextBox(Item.ThisEditorExternal.TargetName), ModifyKillTarget);
            ThisTable.AddItem("Number To Kill", new DefaultNumericUpDown(Item.amount), ModifyKillAmount);
        }
    }
    public class QuestTaskPanel_gather : QuestTaskPanel
    {
        public QuestTask_gather ThisItem;

        public void ModifyGatherObject(object sender, EventArgs e)
        {
            ThisItem.ThisEditorExternal.ItemName = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyGatherAmount(object sender, EventArgs e)
        {
            ThisItem.required = (int)MainForm.GetNumberFromNumericUpDown(sender);
        }

        public void ModifyConsumedOnCompletion(object sender, EventArgs e)
        {
            ThisItem.ThisOptionalFields.consume = MainForm.GetStateFromCheckBox(sender);
        }

        public QuestTaskPanel_gather(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public override void Generate_Addon(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_gather Item = (QuestTask_gather)Item_raw;

            ThisItem = Item;

            ThisTable.AddItem("Item Name", new DefaultTextBox(Item.ThisEditorExternal.ItemName), ModifyGatherObject);
            ThisTable.AddItem("Number To Gather", new DefaultNumericUpDown(Item.required), ModifyGatherAmount);
            ThisTable.AddItem("Consume On Completion", new DefaultCheckBox(Item.ThisOptionalFields.consume), ModifyConsumedOnCompletion);
        }
    }
    public class QuestTaskPanel_killType : QuestTaskPanel
    {
        public QuestTask_killType ThisItem;

        public void ModifyKillTypeGroupName(object sender, EventArgs e)
        {
            ThisItem.ThisEditorExternal.EnemyGroupName = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyKillTypeCustomName(object sender, EventArgs e)
        {
            ThisItem.customName = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyKillTypeAmount(object sender, EventArgs e)
        {
            ThisItem.amount = (int)MainForm.GetNumberFromNumericUpDown(sender);
        }

        public QuestTaskPanel_killType(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public override void Generate_Addon(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_killType Item = (QuestTask_killType)Item_raw;

            ThisItem = Item;

            ThisTable.AddItem("Custom Name", new DefaultTextBox(Item.customName), ModifyKillTypeCustomName);
            ThisTable.AddItem("Enemy Type", new DefaultTextBox(Item.ThisEditorExternal.EnemyGroupName), ModifyKillTypeGroupName);
            ThisTable.AddItem("Number To Kill", new DefaultNumericUpDown(Item.amount), ModifyKillTypeAmount);
        }
    }

    public class QuestTaskPanel_event : QuestTaskPanel
    {
        public new QuestTask_event ThisQuestVariable { get { return base.ThisQuestVariable as QuestTask_event; } }

        public QuestTaskPanel_event(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent)
        {
        }

        private void ModifyEventName(object sender, EventArgs e)
        {
            ThisQuestVariable.eventName = MainForm.GetTextFromTextBox(sender);
        }

        private void ModifyEventDescription(object sender, EventArgs e)
        {
            ThisQuestVariable.description = MainForm.GetTextFromTextBox(sender);
        }

        public override void Generate_Addon(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_event Item = (QuestTask_event)Item_raw;
            ThisTable.AddItem("Event Name", new DefaultTextBox(Item.eventName), ModifyEventName);
            ThisTable.AddItem("Description", new DefaultTextBox(Item.description), ModifyEventDescription);
        }
    }

    public class QuestTaskPanel_useAbility : QuestTaskPanel
    {
        public new QuestTask_useAbility ThisQuestVariable { get { return base.ThisQuestVariable as QuestTask_useAbility; } }

        public QuestTaskPanel_useAbility(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { AddControl(ThisTable); }

        public override void Generate_Addon(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_useAbility Item = (QuestTask_useAbility)Item_raw;
            ThisTable.AddItem("Ability/Item", new DefaultTextBox(Item.ThisEditorExternal.AbilityName), ModifyUseAbilityItem);
            ThisTable.AddItem("Description", new DefaultTextBox(Item.description), ModifyUseAbilityDescription);
            ThisTable.AddItem("Amount", new DefaultNumericUpDown(Item.amount), ModifyUseAbilityAmount);
        }

        private void ModifyUseAbilityItem(object sender, EventArgs e)
        {
            ThisQuestVariable.ThisEditorExternal.AbilityName = MainForm.GetTextFromTextBox(sender);
        }

        private void ModifyUseAbilityAmount(object sender, EventArgs e)
        {
            ThisQuestVariable.amount = (int)MainForm.GetNumberFromNumericUpDown(sender);
        }

        private void ModifyUseAbilityDescription(object sender, EventArgs e)
        {
            ThisQuestVariable.description = MainForm.GetTextFromTextBox(sender);
        }
    }
    public class QuestTaskPanel_reachLevel : QuestTaskPanel
    {
        public new QuestTask_reachLevel ThisQuestVariable { get { return base.ThisQuestVariable as QuestTask_reachLevel; } }

        public QuestTaskPanel_reachLevel(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { AddControl(ThisTable); }

        public override void Generate_Addon(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_reachLevel Item = (QuestTask_reachLevel)Item_raw;
            ThisTable.AddItem("Level", new DefaultNumericUpDown(Item.level), ModifyReachLevelLevel);
            ThisTable.AddItem("Description", new DefaultTextBox(Item.description), ReachLevelDescription);
        }

        private void ModifyReachLevelLevel(object sender, EventArgs e)
        {
            ThisQuestVariable.level = (int)MainForm.GetNumberFromNumericUpDown(sender);
        }

        private void ReachLevelDescription(object sender, EventArgs e)
        {
            ThisQuestVariable.description = MainForm.GetTextFromTextBox(sender);
        }
    }
}
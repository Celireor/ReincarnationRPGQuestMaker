using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat;
using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Parser;
using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Utility;
using ReIncarnation_Quest_Maker.Obsidius;

namespace ReIncarnation_Quest_Maker
{

    public class QuestButton : SortablePanel<QuestButton, Quest>
    {

        private DefaultLabel QuestButtonQuestName;
        // this.newquestbutton = new System.Windows.Forms.Button();

        public void moveAddon(QuestButton Other, int OtherPos)
        {
            int temp = ThisQuestVariable.questID;
            ThisQuestVariable.SetQuestID(Other.ThisQuestVariable.questID);
            Other.ThisQuestVariable.SetQuestID(temp);
            UpdateValues();
            Other.UpdateValues();
            Interpreter.ThisForm.UpdateScreen();
        }

        public bool Trash()
        {
            if (ParentControlList.ThisList.Count > 1)
            {
                Interpreter.CurrentQuestList.Quests.Remove(ThisQuestVariable);
                Interpreter.GetLargestQuestID();
                Interpreter.SelectQuest(Interpreter.CurrentQuestList.Quests[0]);
                return true;
            }
            return false;
        }

        void OnClick(object sender, EventArgs e)
        {
            Interpreter.SelectQuest(ThisQuestVariable);
        }

        void UpdateValues()
        {
            QuestButtonQuestName.Text = "(ID: " + ThisQuestVariable.questID.ToString() + ") " + ThisQuestVariable.name;
        }

        public static QuestButton Generate_2(Quest NewQuest, OrganizedControlList<QuestButton, Quest> Parent)
        {
            QuestButton ReturnValue = QuestButton.NewQuestButton(NewQuest, Parent);
            return ReturnValue;
        }

        public QuestButton() { GenerateFunction = Generate_2; }
        public QuestButton(OrganizedControlList<QuestButton, Quest> Parent) : base(Parent) { ContentsPanel.Click += new EventHandler(OnClick); }

        public static QuestButton NewQuestButton(Quest NewQuest, OrganizedControlList<QuestButton, Quest> Parent)
        {
            QuestButton ReturnValue = new QuestButton(Parent);

            ReturnValue.Move_Addon = ReturnValue.moveAddon;
            ReturnValue.TrashFunction = ReturnValue.Trash;

            ReturnValue.QuestButtonQuestName = new DefaultLabel("(ID: " + NewQuest.questID.ToString() + ") " + NewQuest.name);

            NewQuest.ThisEditorExternal.OnUpdateList.Add(ReturnValue.UpdateValues);

            // 
            // QuestButton
            // 
            ReturnValue.AddControl(ReturnValue.QuestButtonQuestName);
            ReturnValue.Click += new System.EventHandler(ReturnValue.OnClick);
            // 
            // QuestButtonText
            // 

            ReturnValue.QuestButtonQuestName.Click += new System.EventHandler(ReturnValue.OnClick);

            return ReturnValue;
        }

        public override IComparer<QuestButton> SortComparer()
        {
            return new QuestButton_IComparer();
        }

        private class QuestButton_IComparer : IComparer<QuestButton>
        {
            public int Compare(QuestButton a, QuestButton b)
            {
                return a.ThisQuestVariable.questID.CompareTo(b.ThisQuestVariable.questID);
            }
        }
    }

    public class PrerequisitePanel : SortablePanel<PrerequisitePanel, KVPair>
    {
        ModifyQuestVariableTable ThisTable;

        public void moveAddon(PrerequisitePanel other, int OtherPos)
        {
            Interpreter.SelectedQuest.prerequisites.Swap(ListPosition, OtherPos);
        }

        public void UpdateType(object sender, EventArgs e)
        {
            ThisQuestVariable.Key = MainForm.GetTextFromComboBox(sender);
        }
        public void UpdateValue(object sender, EventArgs e)
        {
            ThisQuestVariable.Value = MainForm.GetTextFromTextBox(sender);
        }

        public bool Trash()
        {
            Interpreter.SelectedQuest.prerequisites.Remove(ThisQuestVariable);
            return true;
        }

        public PrerequisitePanel() { GenerateFunction = Generate_2; }

        public PrerequisitePanel(OrganizedControlList<PrerequisitePanel, KVPair> Parent) : base(Parent) { }

        public static PrerequisitePanel Generate_2(KVPair Item, OrganizedControlList<PrerequisitePanel, KVPair> Parent)
        {
            PrerequisitePanel ReturnValue = new PrerequisitePanel(Parent);
            ReturnValue.ThisTable = new ModifyQuestVariableTable();
            ReturnValue.AddControl(ReturnValue.ThisTable);

            ReturnValue.Move_Addon = ReturnValue.moveAddon;
            ReturnValue.TrashFunction = ReturnValue.Trash;

            ReturnValue.ThisTable.AddItem("Type: ", new DefaultDropDown(Item.Key, Interpreter.CurrentQuestList.ThisEditorExternal.PossibleQuestPrerequisites), ReturnValue.UpdateType);
            ReturnValue.ThisTable.AddItem("Value: ", new DefaultTextBox(Item.Value), ReturnValue.UpdateValue);

            return ReturnValue;
        }

        public override IComparer<PrerequisitePanel> SortComparer()
        {
            return new PrerequisitePanel_IComparer();
        }

        private class PrerequisitePanel_IComparer : IComparer<PrerequisitePanel>
        {
            public int Compare(PrerequisitePanel a, PrerequisitePanel b)
            {
                return a.ListPosition.CompareTo(b.ListPosition);
                //return string.Compare(a.ThisPrerequisite.PrerequisiteType, b.ThisPrerequisite.PrerequisiteType);
            }
        }
    }

    public class QuestStagePanel : SortablePanel<QuestStagePanel, QuestStage>
    {
        public TableLayoutPanel table1;
        public DefaultLabel stagenumlabel;
        public TextBox QuestStageNameTextBox;

        public bool Trash()
        {
            if (ParentControlList.ThisList.Count > 1)
            {
                ThisQuestVariable.Trash();
                Interpreter.SelectQuestStage(Interpreter.SelectedQuest.stages[0]);
                return true;
            }
            return false;
        }

        public void moveAddon(QuestStagePanel other, int otherpos)
        {
            ThisQuestVariable.ThisEditorExternal.Parent.stages.Swap(ListPosition, otherpos);
            UtilityFunctions.Swap2Variables(ref ThisQuestVariable.ThisEditorExternal.StageNum, ref other.ThisQuestVariable.ThisEditorExternal.StageNum);
            OnValuesUpdated();
            other.OnValuesUpdated();
        }

        public void OnValuesUpdated()
        {
            stagenumlabel.Text = ThisQuestVariable.ThisEditorExternal.StageNum.ToString();
            QuestStageNameTextBox.Text = ThisQuestVariable.stageName;
        }

        public void UpdateValue(object sender, EventArgs e)
        {
            ThisQuestVariable.stageName = MainForm.GetTextFromTextBox(sender);
        }

        public void SelectStage(object sender, EventArgs e)
        {
            Interpreter.SelectQuestStage(ThisQuestVariable);
        }

        public static QuestStagePanel Generate_2(QuestStage Item, OrganizedControlList<QuestStagePanel, QuestStage> Parent)
        {
            QuestStagePanel ReturnValue = new QuestStagePanel(Parent);

            ReturnValue.stagenumlabel = new DefaultLabel(Item.ThisEditorExternal.StageNum.ToString());
            ReturnValue.QuestStageNameTextBox = new DefaultTextBox(Item.stageName);

            ReturnValue.QuestStageNameTextBox.TextChanged += new System.EventHandler(ReturnValue.UpdateValue);

            ReturnValue.table1.Controls.Add(ReturnValue.QuestStageNameTextBox, 1, 0);
            ReturnValue.table1.Controls.Add(ReturnValue.stagenumlabel, 0, 0);
            ReturnValue.table1.Click += new System.EventHandler(ReturnValue.SelectStage);
            ReturnValue.stagenumlabel.Click += new System.EventHandler(ReturnValue.SelectStage);

            Item.ThisEditorExternal.OnUpdateList.Add(ReturnValue.OnValuesUpdated);

            return ReturnValue;
        }
        public QuestStagePanel() { GenerateFunction = Generate_2; }

        public QuestStagePanel(OrganizedControlList<QuestStagePanel, QuestStage> Parent) : base(Parent)
        {
            TrashFunction = Trash;
            Move_Addon = moveAddon;

            table1 = new DefaultTable(3, 1);

            this.Click += new System.EventHandler(SelectStage);

            this.table1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.table1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            this.table1.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            AddControl(table1);
        }

        public override IComparer<QuestStagePanel> SortComparer()
        {
            return new QuestStagePanel_IComparer();
        }

        private class QuestStagePanel_IComparer : IComparer<QuestStagePanel>
        {
            public int Compare(QuestStagePanel a, QuestStagePanel b)
            {
                return a.ThisQuestVariable.ThisEditorExternal.StageNum.CompareTo(b.ThisQuestVariable.ThisEditorExternal.StageNum);
            }
        }
    }

    public class QuestTaskPanel : SortablePanel<QuestTaskPanel, QuestTask>
    {
        public static Dictionary<Type, Type> TaskToPanel = new Dictionary<Type, Type>();
        public ModifyQuestVariableTable ThisTable;

        public QuestTaskPanel() { GenerateFunction = Generate_2; }

        public QuestTaskPanel(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent)
        {
            ThisTable = new ModifyQuestVariableTable();
            AddControl(ThisTable);
            Move_Addon = moveAddon;
        }

        static QuestTaskPanel()
        {
            TaskToPanel.Add(typeof(QuestTask_location), typeof(QuestTaskPanel_location));
            TaskToPanel.Add(typeof(QuestTask_talkto), typeof(QuestTaskPanel_talkto));
            TaskToPanel.Add(typeof(QuestTask_kill), typeof(QuestTaskPanel_kill));
            TaskToPanel.Add(typeof(QuestTask_gather), typeof(QuestTaskPanel_gather));
            TaskToPanel.Add(typeof(QuestTask_killType), typeof(QuestTaskPanel_killType));
        }

        public void moveAddon(QuestTaskPanel OtherObject, int OtherPosition)
        {
            ThisQuestVariable.ThisEditorExternal.ParentStage.tasks.Swap(ListPosition, OtherPosition);
        }

        public static QuestTaskPanel Generate_2(QuestTask Item, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            Type ReturnType;
            TaskToPanel.TryGetValue(Item.GetType(), out ReturnType);
#if DEBUG
            if (ReturnType.BaseType != typeof(QuestTaskPanel))
            {
                throw new ArgumentNullException("put returntype in wtf");
            }
#endif
            QuestTaskPanel ReturnValue = Activator.CreateInstance(ReturnType) as QuestTaskPanel;

            ReturnValue = ReturnValue.GenerateFunction(Item, Parent);
            ReturnValue.TrashFunction = Item.Trash;

            return ReturnValue;
        }

        public override IComparer<QuestTaskPanel> SortComparer()
        {
            return new SortablePanel_IComparer();
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

        public QuestTaskPanel_location() { GenerateFunction = Generate_2; }

        public QuestTaskPanel_location(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public static new QuestTaskPanel Generate_2(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_location Item = (QuestTask_location)Item_raw;
            QuestTaskPanel_location ReturnValue = new QuestTaskPanel_location(Parent);

            ReturnValue.ThisItem = Item;

            ReturnValue.ThisTable.AddItem("Name", new DefaultTextBox(Item.name), ReturnValue.ModifyLocationName);
            ReturnValue.ThisTable.AddItem("Location Name", new DefaultTextBox(Item.locationString), ReturnValue.ModifyLocationInternalName);
            ReturnValue.ThisTable.AddItem("Radius", new DefaultNumericUpDown(Item.radius), ReturnValue.ModifyLocationRadius);

            return ReturnValue;
        }
    }
    public class QuestTaskPanel_talkto : QuestTaskPanel
    {
        public QuestTask_talkto ThisItem;

        public void ModifyTalkToDescription(object sender, EventArgs e)
        {
            ThisItem.description = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyTalkToListenString(object sender, EventArgs e)
        {
            ThisItem.listenString = MainForm.GetTextFromComboBox(sender);
        }

        public void ModifyTalkToCompletionString(object sender, EventArgs e)
        {
            ThisItem.completionString = MainForm.GetTextFromTextBox(sender);
        }

        public QuestTaskPanel_talkto() { GenerateFunction = Generate_2; }

        public QuestTaskPanel_talkto(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public static new QuestTaskPanel Generate_2(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_talkto Item = (QuestTask_talkto)Item_raw;
            QuestTaskPanel_talkto ReturnValue = new QuestTaskPanel_talkto(Parent);

            ReturnValue.ThisItem = Item;

            ReturnValue.ThisTable.AddItem("Description", new DefaultTextBox(Item.description), ReturnValue.ModifyTalkToDescription);
            ReturnValue.ThisTable.AddItem("Listen String", new DefaultDropDown(Item.listenString, Interpreter.CurrentQuestList.ThisEditorExternal.PossibleListenStrings, true), ReturnValue.ModifyTalkToListenString);
            ReturnValue.ThisTable.AddItem("Completion String", new DefaultTextBox(Item.completionString), ReturnValue.ModifyTalkToCompletionString);

            return ReturnValue;
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

        public QuestTaskPanel_kill() { GenerateFunction = Generate_2; }

        public QuestTaskPanel_kill(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public static new QuestTaskPanel Generate_2(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_kill Item = (QuestTask_kill)Item_raw;

            QuestTaskPanel_kill ReturnValue = new QuestTaskPanel_kill(Parent);

            ReturnValue.ThisItem = Item;

            ReturnValue.ThisTable.AddItem("Target Name", new DefaultTextBox(Item.ThisEditorExternal.TargetName), ReturnValue.ModifyKillTarget);
            ReturnValue.ThisTable.AddItem("Number To Kill", new DefaultNumericUpDown(Item.amount), ReturnValue.ModifyKillAmount);

            return ReturnValue;
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

        public QuestTaskPanel_gather() { GenerateFunction = Generate_2; }

        public QuestTaskPanel_gather(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public static new QuestTaskPanel Generate_2(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_gather Item = (QuestTask_gather)Item_raw;

            QuestTaskPanel_gather ReturnValue = new QuestTaskPanel_gather(Parent);

            ReturnValue.ThisItem = Item;

            ReturnValue.ThisTable.AddItem("Item Name", new DefaultTextBox(Item.ThisEditorExternal.ItemName), ReturnValue.ModifyGatherObject);
            ReturnValue.ThisTable.AddItem("Number To Gather", new DefaultNumericUpDown(Item.required), ReturnValue.ModifyGatherAmount);

            return ReturnValue;
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

        public QuestTaskPanel_killType() { GenerateFunction = Generate_2; }

        public QuestTaskPanel_killType(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public static new QuestTaskPanel Generate_2(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_killType Item = (QuestTask_killType)Item_raw;

            QuestTaskPanel_killType ReturnValue = new QuestTaskPanel_killType(Parent);

            ReturnValue.ThisItem = Item;

            ReturnValue.ThisTable.AddItem("Custom Name", new DefaultTextBox(Item.customName), ReturnValue.ModifyKillTypeCustomName);
            ReturnValue.ThisTable.AddItem("Enemy Type", new DefaultTextBox(Item.ThisEditorExternal.EnemyGroupName), ReturnValue.ModifyKillTypeGroupName);
            ReturnValue.ThisTable.AddItem("Number To Kill", new DefaultNumericUpDown(Item.amount), ReturnValue.ModifyKillTypeAmount);

            return ReturnValue;
        }
    }

    public class QuestDialoguePanel : SortablePanel<QuestDialoguePanel, QuestDialogue>
    {
        public DefaultTable ThisTable;
        public DefaultTable NameTable;
        public DefaultTextBox NameBox;

        public ListPanel<QuestDialogueOptionsPanel, QuestDialogueOption> ThisOptionsList;

        public bool Trash()
        {
            return ThisQuestVariable.Trash();
        }

        public void ModifyDialogueName(object sender, EventArgs e)
        {
            ThisQuestVariable.ThisEditorExternal.ChangeName(MainForm.GetTextFromTextBox(sender));
        }

        public QuestDialoguePanel() { GenerateFunction = Generate_2; }

        public QuestDialoguePanel(OrganizedControlList<QuestDialoguePanel, QuestDialogue> Parent) : base(Parent)
        {
            TrashFunction = Trash;
            FinishUpFunction = FinishUpFunction_2;
        }

        public QuestDialogueOption NewQuestDialogueOption()
        {
            return QuestDialogueOption.Generate(Interpreter.SelectedQuest, ThisQuestVariable.options);
        }

        public static QuestDialoguePanel Generate_2(QuestDialogue Item, OrganizedControlList<QuestDialoguePanel, QuestDialogue> Parent)
        {
            QuestDialoguePanel ReturnValue = new QuestDialoguePanel(Parent);

            ReturnValue.ThisTable = new DefaultTable(1, 2);
            ReturnValue.NameBox = new DefaultTextBox(Item.ThisEditorExternal.DialogueName);
            ReturnValue.ThisOptionsList = new ListPanel<QuestDialogueOptionsPanel, QuestDialogueOption>("New Dialogue Option", ReturnValue.NewQuestDialogueOption, QuestDialogueOptionsPanel.Sort());

            ReturnValue.ThisTable.Controls.Add(ReturnValue.NameBox, 0, 0);
            ReturnValue.ThisTable.Controls.Add(ReturnValue.ThisOptionsList, 0, 1);

            ReturnValue.AddControl(ReturnValue.ThisTable);
            //ReturnValue.ThisTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            ReturnValue.NameBox.TextChanged += ReturnValue.ModifyDialogueName;

            return ReturnValue;
        }

        public void FinishUpFunction_2()
        {
            ThisOptionsList.ThisList.Refresh(ThisQuestVariable.options);
        }

        public override IComparer<QuestDialoguePanel> SortComparer()
        {
            return new PanelComparer();
        }

        public class PanelComparer : IComparer<QuestDialoguePanel>
        {
            public int Compare(QuestDialoguePanel x, QuestDialoguePanel y)
            {
                return x.ThisQuestVariable.ThisEditorExternal.DialogueName.CompareTo(y.ThisQuestVariable.ThisEditorExternal.DialogueName);
            }
        }
    }

    public class QuestDialogueOptionsPanel : SortablePanel<QuestDialogueOptionsPanel, QuestDialogueOption>
    {
        public DefaultTable ThisTable;
        public ModifyQuestVariableTable DialogueOptionsTable;
        public ModifyQuestVariableTable DialogueResponseTable;

        public ListPanel<QuestDialogueOptionsPanel, QuestDialogueOption> ResponseOptionsList;

        DefaultTextBox ResponseTextBox;
        DefaultTextBox ResponsePortraitBox;
        DefaultTextBox PlaySoundBox;

        public QuestDialogueOption NewQuestDialogueOption()
        {
            return QuestDialogueOption.Generate(Interpreter.SelectedQuest, ThisQuestVariable.Response.options);
        }

        public QuestDialogueOptionsPanel() { GenerateFunction = Generate_2; }

        public void OnSelectionTextChanged(object sender, EventArgs e)
        {
            ThisQuestVariable.selectText = MainForm.GetTextFromTextBox(sender);
        }

        public void OnSelectionImageChanged(object sender, EventArgs e)
        {
            ThisQuestVariable.selectImg = MainForm.GetTextFromComboBox(sender);
        }

        /*public void OnQuestMarkerChanged(object sender, EventArgs e)
        {
            ThisQuestVariable.ThisOptionalFields.questMarker = MainForm.GetTextFromComboBox(sender);
        }

        public void OnSelectBackgroundChanged(object sender, EventArgs e)
        {
            ThisQuestVariable.ThisOptionalFields.selectBackground = MainForm.GetTextFromComboBox(sender);
        }*/

        public void OnSendListenStringChanged(object sender, EventArgs e)
        {

            ThisQuestVariable.ThisOptionalFields.ChangeListenString(MainForm.GetTextFromTextBox(sender));
        }

        public void OnHasResponseChanged(object sender, EventArgs e)
        {
            bool State = MainForm.GetStateFromCheckBox(sender);

            if (State)
            {
                ThisQuestVariable.Response = new QuestDialogueResponse();
                ThisTable.Controls.Add(DialogueResponseTable, 0, 1);
                ThisTable.Controls.Add(ResponseOptionsList, 0, 2);
                return;
            }

            ThisQuestVariable.Response = null;
            ThisTable.Controls.Remove(DialogueResponseTable);
            ThisTable.Controls.Remove(ResponseOptionsList);
        }

        public void OnResponseTextChanged(object sender, EventArgs e)
        {
            ThisQuestVariable.Response.responseText = MainForm.GetTextFromTextBox(sender);
        }

        public void OnResponsePortraitChanged(object sender, EventArgs e)
        {
            ThisQuestVariable.Response.ThisOptionalFields.responsePortait = MainForm.GetTextFromTextBox(sender);
        }

        public void OnPlaySoundChanged(object sender, EventArgs e)
        {
            ThisQuestVariable.Response.ThisOptionalFields.playSound = MainForm.GetTextFromTextBox(sender);
        }

        public QuestDialogueOptionsPanel(OrganizedControlList<QuestDialogueOptionsPanel, QuestDialogueOption> Parent) : base(Parent)
        {

            FinishUpFunction = FinishUp_2;

            ThisTable = new DefaultTable(1, 3);

            DialogueOptionsTable = new ModifyQuestVariableTable();
            DialogueResponseTable = new ModifyQuestVariableTable();
            ResponseOptionsList = new ListPanel<QuestDialogueOptionsPanel, QuestDialogueOption>("New Option", NewQuestDialogueOption, QuestDialogueOptionsPanel.Sort());

            AddControl(ThisTable, true);
            ThisTable.Controls.Add(DialogueOptionsTable, 0, 0);
        }
        public static QuestDialogueOptionsPanel Generate_2(QuestDialogueOption Item, OrganizedControlList<QuestDialogueOptionsPanel, QuestDialogueOption> Parent)
        {
            QuestDialogueOptionsPanel ReturnValue = new QuestDialogueOptionsPanel(Parent);

            ReturnValue.TrashFunction = Item.Trash;

            ReturnValue.DialogueOptionsTable.AddItem("Selection Text", new DefaultTextBox(Item.selectText), ReturnValue.OnSelectionTextChanged);
            ReturnValue.DialogueOptionsTable.AddItem("Selection Image", new DefaultDropDown(Item.selectImg, Interpreter.CurrentQuestList.ThisEditorExternal.PossibleQuestOptionSelectImage), ReturnValue.OnSelectionImageChanged);
            //ReturnValue.DialogueOptionsTable.AddItem("Quest Marker", new DefaultDropDown(Item.ThisOptionalFields.questMarker, new List<string>()), ReturnValue.OnQuestMarkerChanged);
            //ReturnValue.DialogueOptionsTable.AddItem("Select Background", new DefaultDropDown(Item.ThisOptionalFields.selectBackground, new List<string>()), ReturnValue.OnSelectBackgroundChanged);
            ReturnValue.DialogueOptionsTable.AddItem("Sent Listen String", new DefaultTextBox(Item.ThisOptionalFields.sendListenString), ReturnValue.OnSendListenStringChanged);
            ReturnValue.DialogueOptionsTable.AddItem("Has Response", new DefaultCheckBox(Item.Response != null), ReturnValue.OnHasResponseChanged);

            ReturnValue.ResponseTextBox = new DefaultTextBox();
            ReturnValue.ResponsePortraitBox = new DefaultTextBox();
            ReturnValue.PlaySoundBox = new DefaultTextBox();

            ReturnValue.DialogueResponseTable.AddItem("Response Text", ReturnValue.ResponseTextBox, ReturnValue.OnResponseTextChanged);
            ReturnValue.DialogueResponseTable.AddItem("Response Portrait", ReturnValue.ResponsePortraitBox, ReturnValue.OnResponsePortraitChanged);
            ReturnValue.DialogueResponseTable.AddItem("Sound Played", ReturnValue.PlaySoundBox, ReturnValue.OnPlaySoundChanged);

            return ReturnValue;
        }

        public void FinishUp_2()
        {
            if (ThisQuestVariable.Response != null)
            {
                ResponseTextBox.Text = ThisQuestVariable.Response.responseText;
                ResponsePortraitBox.Text = ThisQuestVariable.Response.ThisOptionalFields.responsePortait;
                PlaySoundBox.Text = ThisQuestVariable.Response.ThisOptionalFields.playSound;

                ResponseOptionsList.ThisList.Refresh(ThisQuestVariable.Response.options);

                ThisTable.Controls.Add(DialogueResponseTable, 0, 1);
                ThisTable.Controls.Add(ResponseOptionsList, 0, 2);
            }
        }

        public override IComparer<QuestDialogueOptionsPanel> SortComparer()
        {
            return new ThisSortComparer();
        }

        public class ThisSortComparer : IComparer<QuestDialogueOptionsPanel>
        {
            public int Compare(QuestDialogueOptionsPanel x, QuestDialogueOptionsPanel y)
            {
                return 0;
            }
        }
    }

    public class QuestStageDialoguePanel : SortablePanel<QuestStageDialoguePanel, KVPair>
    {
        ModifyQuestVariableTable ThisTable;

        public bool Trash()
        {
            Interpreter.SelectedQuestStage.dialogue.Remove(ThisQuestVariable);
            return true;
        }

        public QuestStageDialoguePanel()
        {
            GenerateFunction = Generate_2;
        }

        public QuestStageDialoguePanel(OrganizedControlList<QuestStageDialoguePanel, KVPair> Parent) : base(Parent)
        {
            TrashFunction = Trash;
        }

        public void ModifyNPCName(object sender, EventArgs e)
        {
            ThisQuestVariable.Key = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyDialogue(object sender, EventArgs e)
        {
            ThisQuestVariable.Value = MainForm.GetTextFromComboBox(sender);
        }

        public static QuestStageDialoguePanel Generate_2(KVPair Item, OrganizedControlList<QuestStageDialoguePanel, KVPair> Parent)
        {
            QuestStageDialoguePanel ReturnValue = new QuestStageDialoguePanel(Parent);
            ReturnValue.ThisTable = new ModifyQuestVariableTable();
            ReturnValue.ThisTable.AddItem("NPC: ", new DefaultTextBox(Item.Key), ReturnValue.ModifyNPCName);
            ReturnValue.ThisTable.AddItem("Dialogue: ", new DefaultDropDown(Item.Value, Interpreter.SelectedQuest.ThisEditorExternal.PossibleDialogueInjections), ReturnValue.ModifyDialogue);
            //ReturnValue.ThisTable.AddItem("Text Box: ", new DefaultDropDown(Item.Key, , true), ReturnValue.ModifyNPCName);
            ReturnValue.AddControl(ReturnValue.ThisTable);
            return ReturnValue;
        }

        public override IComparer<QuestStageDialoguePanel> SortComparer()
        {
            return new SortablePanel_IComparer();
        }
    }

    public class QuestStageParticlePanel : SortablePanel<QuestStageParticlePanel, KVPair>
    {
        ModifyQuestVariableTable ThisTable;

        public bool Trash()
        {
            Interpreter.SelectedQuestStage.dialogue.Remove(ThisQuestVariable);
            return true;
        }

        public QuestStageParticlePanel()
        {
            GenerateFunction = Generate_2;
        }

        public QuestStageParticlePanel(OrganizedControlList<QuestStageParticlePanel, KVPair> Parent) : base(Parent)
        {
            TrashFunction = Trash;
        }

        public void ModifyNPCName(object sender, EventArgs e)
        {
            ThisQuestVariable.Key = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyDialogue(object sender, EventArgs e)
        {
            ThisQuestVariable.Value = MainForm.GetTextFromComboBox(sender);
        }

        public static QuestStageParticlePanel Generate_2(KVPair Item, OrganizedControlList<QuestStageParticlePanel, KVPair> Parent)
        {
            QuestStageParticlePanel ReturnValue = new QuestStageParticlePanel(Parent);
            ReturnValue.ThisTable = new ModifyQuestVariableTable();
            ReturnValue.ThisTable.AddItem("NPC: ", new DefaultTextBox(Item.Key), ReturnValue.ModifyNPCName);
            ReturnValue.ThisTable.AddItem("Particle: ", new DefaultDropDown(Item.Value, Interpreter.CurrentQuestList.ThisEditorExternal.PossibleParticles), ReturnValue.ModifyDialogue);
            ReturnValue.AddControl(ReturnValue.ThisTable);
            return ReturnValue;
        }

        public override IComparer<QuestStageParticlePanel> SortComparer()
        {
            return new SortablePanel_IComparer();
        }
    }
}
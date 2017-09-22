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

        public override void Move_Addon(QuestButton Other, int OtherPos)
        {
            int temp = ThisQuestVariable.questID;
            ThisQuestVariable.SetQuestID(Other.ThisQuestVariable.questID);
            Other.ThisQuestVariable.SetQuestID(temp);
            UpdateValues();
            Other.UpdateValues();
            Interpreter.ThisForm.UpdateScreen();
        }

        public override bool Trash_Addon()
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

        public override void Generate_Addon(Quest NewQuest, OrganizedControlList<QuestButton, Quest> Parent)
        {
            //QuestButton ReturnValue = new QuestButton(Parent);

            QuestButtonQuestName = new DefaultLabel("(ID: " + NewQuest.questID.ToString() + ") " + NewQuest.name);

            NewQuest.ThisEditorExternal.OnUpdateList.Add(UpdateValues);

            // 
            // QuestButton
            // 
            AddControl(QuestButtonQuestName);
            Click += new System.EventHandler(OnClick);
            // 
            // QuestButtonText
            // 

            QuestButtonQuestName.Click += new System.EventHandler(OnClick);
        }

        public QuestButton(OrganizedControlList<QuestButton, Quest> Parent) : base(Parent) { ContentsPanel.Click += new EventHandler(OnClick); }

        public new IComparer<QuestButton> SortComparer()
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

        public override void Move_Addon(PrerequisitePanel other, int OtherPos)
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

        public override bool Trash_Addon()
        {
            Interpreter.SelectedQuest.prerequisites.Remove(ThisQuestVariable);
            return true;
        }

        public PrerequisitePanel(OrganizedControlList<PrerequisitePanel, KVPair> Parent) : base(Parent) { }

        public override void Generate_Addon(KVPair Item, OrganizedControlList<PrerequisitePanel, KVPair> Parent)
        {
            //PrerequisitePanel ReturnValue = new PrerequisitePanel(Parent);
            ThisTable = new ModifyQuestVariableTable();
            AddControl(ThisTable);

            ThisTable.AddItem("Type: ", new DefaultDropDown(Item.Key, Interpreter.CurrentQuestList.ThisEditorExternal.PossibleQuestPrerequisites), UpdateType);
            ThisTable.AddItem("Value: ", new DefaultTextBox(Item.Value), UpdateValue);

            //return ReturnValue;
        }

        public new IComparer<PrerequisitePanel> SortComparer()
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

        public override bool Trash_Addon()
        {
            if (ParentControlList.ThisList.Count > 1)
            {
                ThisQuestVariable.Trash();
                Interpreter.SelectQuestStage(Interpreter.SelectedQuest.stages[0]);
                return true;
            }
            return false;
        }

        public override void Move_Addon(QuestStagePanel other, int otherpos)
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

        public override void Generate_Addon(QuestStage Item, OrganizedControlList<QuestStagePanel, QuestStage> Parent)
        {
            //QuestStagePanel ReturnValue = new QuestStagePanel(Parent);

            stagenumlabel = new DefaultLabel(Item.ThisEditorExternal.StageNum.ToString());
            QuestStageNameTextBox = new DefaultTextBox(Item.stageName);

            QuestStageNameTextBox.TextChanged += new System.EventHandler(UpdateValue);

            table1.Controls.Add(QuestStageNameTextBox, 1, 0);
            table1.Controls.Add(stagenumlabel, 0, 0);
            table1.Click += new System.EventHandler(SelectStage);
            stagenumlabel.Click += new System.EventHandler(SelectStage);

            Item.ThisEditorExternal.OnUpdateList.Add(OnValuesUpdated);

            //return ReturnValue;
        }

        public QuestStagePanel(OrganizedControlList<QuestStagePanel, QuestStage> Parent) : base(Parent)
        {

            table1 = new DefaultTable(3, 1);

            this.Click += new System.EventHandler(SelectStage);

            this.table1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.table1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            this.table1.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            AddControl(table1);
        }

        public new IComparer<QuestStagePanel> SortComparer()
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
        public static Dictionary<Type, Type> TaskToGenerateFunction = new Dictionary<Type, Type>();
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
            TaskToGenerateFunction.Add(typeof(QuestTask_location), typeof(QuestTaskPanel_location));
            TaskToGenerateFunction.Add(typeof(QuestTask_talkto), typeof(QuestTaskPanel_talkto));
            TaskToGenerateFunction.Add(typeof(QuestTask_kill), typeof(QuestTaskPanel_kill));
            TaskToGenerateFunction.Add(typeof(QuestTask_gather), typeof(QuestTaskPanel_gather));
            TaskToGenerateFunction.Add(typeof(QuestTask_killType), typeof(QuestTaskPanel_killType));
        }

        public override void Move_Addon(QuestTaskPanel OtherObject, int OtherPosition)
        {
            ThisQuestVariable.ThisEditorExternal.ParentStage.tasks.Swap(ListPosition, OtherPosition);
        }

        public override QuestTaskPanel CreateInstanceAddon(QuestTask Item, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            Type ReturnType;
            TaskToGenerateFunction.TryGetValue(Item.GetType(), out ReturnType);
#if DEBUG
            if (ReturnType.BaseType != typeof(QuestTaskPanel))
            {
                throw new ArgumentNullException("put returntype in wtf");
            }
#endif
            QuestTaskPanel ReturnValue = (QuestTaskPanel)Activator.CreateInstance(ReturnType, Parent);

            return ReturnValue;
        }

        public override void Generate_Addon(QuestTask Item, OrganizedControlList<QuestTaskPanel, QuestTask> Parent) {
            throw new NotImplementedException();
            //implement in inherited classes
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

        public QuestTaskPanel_location(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public override void Generate_Addon(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_location Item = (QuestTask_location)Item_raw;
            QuestTaskPanel_location ReturnValue = new QuestTaskPanel_location(Parent);

            ThisItem = Item;

            ThisTable.AddItem("Name", new DefaultTextBox(Item.name), ModifyLocationName);
            ThisTable.AddItem("Location Name", new DefaultTextBox(Item.locationString), ModifyLocationInternalName);
            ThisTable.AddItem("Radius", new DefaultNumericUpDown(Item.radius), ReturnValue.ModifyLocationRadius);
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

        public QuestTaskPanel_talkto(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public override void Generate_Addon(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_talkto Item = (QuestTask_talkto)Item_raw;

            ThisItem = Item;

            ThisTable.AddItem("Description", new DefaultTextBox(Item.description), ModifyTalkToDescription);
            ThisTable.AddItem("Listen String", new DefaultDropDown(Item.listenString, Interpreter.CurrentQuestList.ThisEditorExternal.PossibleListenStrings, true), ModifyTalkToListenString);
            ThisTable.AddItem("Completion String", new DefaultTextBox(Item.completionString), ModifyTalkToCompletionString);
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

        public QuestTaskPanel_gather(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent) { }

        public override void Generate_Addon(QuestTask Item_raw, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            QuestTask_gather Item = (QuestTask_gather)Item_raw;

            ThisItem = Item;

            ThisTable.AddItem("Item Name", new DefaultTextBox(Item.ThisEditorExternal.ItemName), ModifyGatherObject);
            ThisTable.AddItem("Number To Gather", new DefaultNumericUpDown(Item.required), ModifyGatherAmount);
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

    public class QuestDialoguePanel : SortablePanel<QuestDialoguePanel, QuestDialogue>
    {
        public DefaultTable ThisTable;
        public DefaultTable NameTable;
        public DefaultTextBox NameBox;

        public ListPanel<QuestDialogueOptionsPanel, QuestDialogueOption> ThisOptionsList;

        public override bool Trash_Addon()
        {
            return ThisQuestVariable.Trash();
        }

        public void ModifyDialogueName(object sender, EventArgs e)
        {
            ThisQuestVariable.ThisEditorExternal.ChangeName(MainForm.GetTextFromTextBox(sender));
        }

        public QuestDialoguePanel(OrganizedControlList<QuestDialoguePanel, QuestDialogue> Parent) : base(Parent)
        {
            FinishUpFunction = FinishUpFunction_2;
        }

        public QuestDialogueOption NewQuestDialogueOption()
        {
            return QuestDialogueOption.Generate(Interpreter.SelectedQuest, ThisQuestVariable.options);
        }

        public override void Generate_Addon(QuestDialogue Item, OrganizedControlList<QuestDialoguePanel, QuestDialogue> Parent)
        {
            //QuestDialoguePanel ReturnValue = new QuestDialoguePanel(Parent);

            ThisTable = new DefaultTable(1, 2);
            NameBox = new DefaultTextBox(Item.ThisEditorExternal.DialogueName);
            ThisOptionsList = new ListPanel<QuestDialogueOptionsPanel, QuestDialogueOption>("New Dialogue Option", NewQuestDialogueOption, QuestDialogueOptionsPanel.Sort());

            ThisTable.Controls.Add(NameBox, 0, 0);
            ThisTable.Controls.Add(ThisOptionsList, 0, 1);

            AddControl(ThisTable);
            //ThisTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            NameBox.TextChanged += ModifyDialogueName;

            //return ReturnValue;
        }

        public void FinishUpFunction_2()
        {
            ThisOptionsList.ThisList.Refresh(ThisQuestVariable.options);
        }

        public new IComparer<QuestDialoguePanel> SortComparer()
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

        public override bool Trash_Addon()
        {
            return ThisQuestVariable.Trash();
        }

        public QuestDialogueOption NewQuestDialogueOption()
        {
            return QuestDialogueOption.Generate(Interpreter.SelectedQuest, ThisQuestVariable.Response.options);
        }

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

        /*public void OnGivesQuestChanged(object sender, EventArgs e)
        {

            ThisQuestVariable.ThisOptionalFields.ThisEditorExternal.GivesQuest = MainForm.GetStateFromCheckBox(sender);
        }

        public void OnQuestGivenChanged(object sender, EventArgs e)
        {
            ThisQuestVariable.ThisOptionalFields.ThisEditorExternal.giveQuest = (int)MainForm.GetNumberFromNumericUpDown(sender);
        }*/

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
        public override void Generate_Addon (QuestDialogueOption Item, OrganizedControlList<QuestDialogueOptionsPanel, QuestDialogueOption> Parent)
        {
            //QuestDialogueOptionsPanel ReturnValue = new QuestDialogueOptionsPanel(Parent);

            DialogueOptionsTable.AddItem("Selection Text", new DefaultTextBox(Item.selectText), OnSelectionTextChanged);
            DialogueOptionsTable.AddItem("Selection Image", new DefaultDropDown(Item.selectImg, Interpreter.CurrentQuestList.ThisEditorExternal.PossibleQuestOptionSelectImage), OnSelectionImageChanged);
            //DialogueOptionsTable.AddItem("Quest Marker", new DefaultDropDown(Item.ThisOptionalFields.questMarker, new List<string>()), OnQuestMarkerChanged);
            //DialogueOptionsTable.AddItem("Select Background", new DefaultDropDown(Item.ThisOptionalFields.selectBackground, new List<string>()), OnSelectBackgroundChanged);
           // DialogueOptionsTable.AddItem("Gives Quest", new DefaultCheckBox(Item.ThisOptionalFields.ThisEditorExternal.GivesQuest), OnGivesQuestChanged);
           // DialogueOptionsTable.AddItem("Quest Given", new DefaultNumericUpDown(Item.ThisOptionalFields.ThisEditorExternal.giveQuest), OnQuestGivenChanged);
            DialogueOptionsTable.AddItem("Sent Listen String", new DefaultTextBox(Item.ThisOptionalFields.sendListenString), OnSendListenStringChanged);
            DialogueOptionsTable.AddItem("Has Response", new DefaultCheckBox(Item.Response != null), OnHasResponseChanged);

            ResponseTextBox = new DefaultTextBox();
            ResponsePortraitBox = new DefaultTextBox();
            PlaySoundBox = new DefaultTextBox();

            DialogueResponseTable.AddItem("Response Text", ResponseTextBox, OnResponseTextChanged);
            DialogueResponseTable.AddItem("Response Portrait", ResponsePortraitBox, OnResponsePortraitChanged);
            DialogueResponseTable.AddItem("Sound Played", PlaySoundBox, OnPlaySoundChanged);

            //return ReturnValue;
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
    }

    public class QuestDialogueGivesQuestPanel : SortablePanel<QuestDialogueGivesQuestPanel, KVPair>
    {
        public QuestDialogueGivesQuestPanel()
        {
        }

        public QuestDialogueGivesQuestPanel(OrganizedControlList<QuestDialogueGivesQuestPanel, KVPair> Parent) : base(Parent)
        {
        }

        public override void Generate_Addon(KVPair Item, OrganizedControlList<QuestDialogueGivesQuestPanel, KVPair> Parent)
        {
            throw new NotImplementedException();
        }
    }

    public class QuestStageDialoguePanel : SortablePanel<QuestStageDialoguePanel, KVPair>
    {
        ModifyQuestVariableTable ThisTable;

        public override bool Trash_Addon()
        {
            Interpreter.SelectedQuestStage.dialogue.Remove(ThisQuestVariable);
            return true;
        }

        public QuestStageDialoguePanel(OrganizedControlList<QuestStageDialoguePanel, KVPair> Parent) : base(Parent) { }

        public void ModifyNPCName(object sender, EventArgs e)
        {
            ThisQuestVariable.Key = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyDialogue(object sender, EventArgs e)
        {
            ThisQuestVariable.Value = MainForm.GetTextFromComboBox(sender);
        }

        public override void Generate_Addon (KVPair Item, OrganizedControlList<QuestStageDialoguePanel, KVPair> Parent)
        {
            //QuestStageDialoguePanel ReturnValue = new QuestStageDialoguePanel(Parent);
            ThisTable = new ModifyQuestVariableTable();
            ThisTable.AddItem("NPC: ", new DefaultTextBox(Item.Key), ModifyNPCName);
            ThisTable.AddItem("Dialogue: ", new DefaultDropDown(Item.Value, Interpreter.SelectedQuest.ThisEditorExternal.PossibleDialogueInjections), ModifyDialogue);
            //ThisTable.AddItem("Text Box: ", new DefaultDropDown(Item.Key, , true), ModifyNPCName);
            AddControl(ThisTable);
            //return ReturnValue;
        }
    }

    public class QuestStageParticlePanel : SortablePanel<QuestStageParticlePanel, KVPair>
    {
        ModifyQuestVariableTable ThisTable;

        public override bool Trash_Addon()
        {
            Interpreter.SelectedQuestStage.dialogue.Remove(ThisQuestVariable);
            return true;
        }

        public QuestStageParticlePanel(OrganizedControlList<QuestStageParticlePanel, KVPair> Parent) : base(Parent)
        {
        }

        public void ModifyNPCName(object sender, EventArgs e)
        {
            ThisQuestVariable.Key = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyDialogue(object sender, EventArgs e)
        {
            ThisQuestVariable.Value = MainForm.GetTextFromComboBox(sender);
        }

        public override void Generate_Addon(KVPair Item, OrganizedControlList<QuestStageParticlePanel, KVPair> Parent)
        {
            //QuestStageParticlePanel ReturnValue = new QuestStageParticlePanel(Parent);
            ThisTable = new ModifyQuestVariableTable();
            ThisTable.AddItem("NPC: ", new DefaultTextBox(Item.Key), ModifyNPCName);
            ThisTable.AddItem("Particle: ", new DefaultDropDown(Item.Value, Interpreter.CurrentQuestList.ThisEditorExternal.PossibleParticles), ModifyDialogue);
            AddControl(ThisTable);
            //return ReturnValue;
        }
    }
}
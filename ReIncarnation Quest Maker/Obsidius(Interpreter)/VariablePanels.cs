using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat;
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
            ThisQuestVariable.ForceSetQuestID(Other.ThisQuestVariable.questID);
            Other.ThisQuestVariable.ForceSetQuestID(temp);
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
            ParentControlList.SortControls();
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

    public class PrerequisitePanel : KVPanel<PrerequisitePanel>
    {

        public override void Move_Addon(PrerequisitePanel other, int OtherPos)
        {
            Interpreter.SelectedQuest.questprerequisites.Swap(ListPosition, OtherPos);
        }

        public void UpdateType(object sender, EventArgs e)
        {
            ThisQuestVariable.Key = MainForm.GetTextFromComboBox(sender);
        }
        public void UpdateValue(object sender, EventArgs e)
        {
            ThisQuestVariable.Value = ((int)MainForm.GetNumberFromNumericUpDown(sender)).ToString();
        }

        public PrerequisitePanel(OrganizedControlList<PrerequisitePanel, KVPair> Parent) : base(Parent) { }

        public override void Generate_Addon(KVPair Item, OrganizedControlList<PrerequisitePanel, KVPair> Parent)
        {
            //PrerequisitePanel ReturnValue = new PrerequisitePanel(Parent);

            ThisTable.AddItem("Quest:", new DefaultNumericUpDown(Convert.ToInt32(Item.Key)), UpdateValue);
            ThisTable.AddItem("State:", new DefaultDropDown(Item.Value, Interpreter.CurrentQuestList.ThisEditorExternal.PossibleQuestStates), UpdateType);

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
        public DefaultLabel QuestStageNameLabel;

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
        }

        public void SelectStage(object sender, EventArgs e)
        {
            Interpreter.SelectQuestStage(ThisQuestVariable);
        }

        public override void Generate_Addon(QuestStage Item, OrganizedControlList<QuestStagePanel, QuestStage> Parent)
        {
            //QuestStagePanel ReturnValue = new QuestStagePanel(Parent);

            stagenumlabel = new DefaultLabel(Item.ThisEditorExternal.StageNum.ToString());
            QuestStageNameLabel = new DefaultLabel(Item.ThisOptionalFields.description);

            QuestStageNameLabel.Click += new System.EventHandler(SelectStage);

            table1.Controls.Add(QuestStageNameLabel, 1, 0);
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

        public QuestDialogueOption NewQuestDialogueOption(string Useless)
        {
            return QuestDialogueOption.Generate(Interpreter.SelectedQuest, ThisQuestVariable.options);
        }

        public override void Generate_Addon(QuestDialogue Item, OrganizedControlList<QuestDialoguePanel, QuestDialogue> Parent)
        {
            //QuestDialoguePanel ReturnValue = new QuestDialoguePanel(Parent);

            ThisTable = new DefaultTable(1, 2);
            NameBox = new DefaultTextBox(Item.ThisEditorExternal.DialogueName);
            ThisOptionsList = new ListPanel<QuestDialogueOptionsPanel, QuestDialogueOption>("New Dialogue Option", NewQuestDialogueOption);

            ThisTable.Controls.Add(NameBox, 0, 0);
            ThisTable.Controls.Add(ThisOptionsList, 0, 1);

            AddControl(ThisTable);

            NameBox.TextChanged += ModifyDialogueName;
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
        public ListPanel<RunScriptPanel, QuestDialogueOptionRunScript> RunScriptList;

        public ListPanel<QuestDialogueOptionsPanel, QuestDialogueOption> ResponseOptionsList;
        public ListPanel<GivesQuestPanel, KVPair> giveQuestsList;
        public ListPanel<QuestDialogueHideIfPanel, QuestDialogueOptionHideIf> hideIfList;

        DefaultTextBox ResponseTextBox;
        DefaultTextBox ResponsePortraitBox;
        DefaultTextBox PlaySoundBox;

        public override bool Trash_Addon()
        {
            return ThisQuestVariable.Trash();
        }

        public QuestDialogueOption NewQuestDialogueOption(string useless)
        {
            return QuestDialogueOption.Generate(Interpreter.SelectedQuest, ThisQuestVariable.Response.options);
        }

        public KVPair NewQuestDialogueGivesQuestPanel(string HideIfTypeStringHideIfTypeString) {
            KVPair ReturnValue = new KVPair();
            ThisQuestVariable.giveQuests.Add(ReturnValue);
            return ReturnValue;
        }

        public QuestDialogueOptionHideIf NewHideIf(string InString) {
            return QuestDialogueOptionHideIf.Generate(InString, ThisQuestVariable);
        }

        public void OnSelectionTextChanged(object sender, EventArgs e)
        {
            ThisQuestVariable.selectText = MainForm.GetTextFromTextBox(sender);
        }

        public void OnSelectionImageChanged(object sender, EventArgs e)
        {
            ThisQuestVariable.selectImg = MainForm.GetTextFromComboBox(sender);
        }

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
                ThisTable.Controls.Add(DialogueResponseTable, 0, 4);
                ThisTable.Controls.Add(ResponseOptionsList, 0, 5);
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

            ThisTable = new DefaultTable(1, 6);

            DialogueOptionsTable = new ModifyQuestVariableTable();
            DialogueResponseTable = new ModifyQuestVariableTable();
            ResponseOptionsList = new ListPanel<QuestDialogueOptionsPanel, QuestDialogueOption>("New Option", NewQuestDialogueOption);
            giveQuestsList = new ListPanel<GivesQuestPanel, KVPair>("New Quest Given", NewQuestDialogueGivesQuestPanel);
            hideIfList = new ListPanel<QuestDialogueHideIfPanel, QuestDialogueOptionHideIf>("New Hide If Condition", NewHideIf, new DefaultDropDown("quests", Interpreter.CurrentQuestList.ThisEditorExternal.PossibleHideIfStrings));
            RunScriptList = new ListPanel<RunScriptPanel, QuestDialogueOptionRunScript>("New Script", obj => { return new QuestDialogueOptionRunScript(); });

            AddControl(ThisTable, true);

            ThisTable.Controls.Add(hideIfList, 0, 0);
            ThisTable.Controls.Add(giveQuestsList, 0, 1);
            ThisTable.Controls.Add(DialogueOptionsTable, 0, 2);
            ThisTable.Controls.Add(RunScriptList, 0, 3);
        }
        public override void Generate_Addon(QuestDialogueOption Item, OrganizedControlList<QuestDialogueOptionsPanel, QuestDialogueOption> Parent)
        {
            //QuestDialogueOptionsPanel ReturnValue = new QuestDialogueOptionsPanel(Parent);

            DialogueOptionsTable.AddItem("Selection Text", new DefaultTextBox(Item.selectText), OnSelectionTextChanged);
            DialogueOptionsTable.AddItem("Selection Image", new DefaultDropDown(Item.selectImg, Interpreter.CurrentQuestList.ThisEditorExternal.PossibleQuestOptionSelectImage), OnSelectionImageChanged);
            //DialogueOptionsTable.AddItem("Quest Marker", new DefaultDropDown(Item.ThisOptionalFields.questMarker, new List<string>()), OnQuestMarkerChanged);
            //DialogueOptionsTable.AddItem("Select Background", new DefaultDropDown(Item.ThisOptionalFields.selectBackground, new List<string>()), OnSelectBackgroundChanged);
            DialogueOptionsTable.AddItem("Sent Listen String", new DefaultTextBox(Item.ThisOptionalFields.sendListenString), OnSendListenStringChanged);
            DialogueOptionsTable.AddItem("Has Response", new DefaultCheckBox(Item.Response != null), OnHasResponseChanged);

            ResponseTextBox = new DefaultTextBox("", true);
            ResponseTextBox.MinimumSize = new Size(0, ResponseTextBox.Height * 5);
            ResponsePortraitBox = new DefaultTextBox();
            PlaySoundBox = new DefaultTextBox();

            DialogueResponseTable.AddItem("Response Text", ResponseTextBox, OnResponseTextChanged);
            DialogueResponseTable.AddItem("Response Portrait", ResponsePortraitBox, OnResponsePortraitChanged);
            DialogueResponseTable.AddItem("Sound Played", PlaySoundBox, OnPlaySoundChanged);

            //return ReturnValue;
        }

        public void FinishUp_2()
        {
            hideIfList.ThisList.Refresh(ThisQuestVariable.hideIf);
            giveQuestsList.ThisList.Refresh(ThisQuestVariable.giveQuests);
            RunScriptList.ThisList.Refresh(ThisQuestVariable.runScript);
            if (ThisQuestVariable.Response != null)
            {
                ResponseTextBox.Text = ThisQuestVariable.Response.responseText;
                ResponsePortraitBox.Text = ThisQuestVariable.Response.ThisOptionalFields.responsePortait;
                PlaySoundBox.Text = ThisQuestVariable.Response.ThisOptionalFields.playSound;

                ResponseOptionsList.ThisList.Refresh(ThisQuestVariable.Response.options);
                ThisTable.Controls.Add(DialogueResponseTable, 0, 4);
                ThisTable.Controls.Add(ResponseOptionsList, 0, 3);
            }
        }
    }

    public class GivesQuestPanel : KVPanel<GivesQuestPanel>
    {

        public void QuestIDOnChanged(object sender, EventArgs e)
        {
            ThisQuestVariable.Key = ((int)MainForm.GetNumberFromNumericUpDown(sender)).ToString();
        }
        public void ForceGiveQuestChanged(object sender, EventArgs e)
        {
            ThisQuestVariable.Value = MainForm.GetStateFromCheckBox(sender).ToString().ToLower();
        }

        public GivesQuestPanel(OrganizedControlList<GivesQuestPanel, KVPair> Parent) : base(Parent)
        {
        }

        public override void Generate_Addon(KVPair Item, OrganizedControlList<GivesQuestPanel, KVPair> Parent)
        {
            ThisTable.AddItem("Quest ID", new DefaultNumericUpDown(Convert.ToInt32(Item.Key)), QuestIDOnChanged);
            ThisTable.AddItem("Force Give", new DefaultCheckBox(Convert.ToBoolean(Item.Value)), ForceGiveQuestChanged);
        }
    }

   

    public class QuestStageDialoguePanel : KVPanel<QuestStageDialoguePanel>
    {
        public QuestStageDialoguePanel(OrganizedControlList<QuestStageDialoguePanel, KVPair> Parent) : base(Parent) { }

        public void ModifyNPCName(object sender, EventArgs e)
        {
            ThisQuestVariable.Key = MainForm.GetTextFromTextBox(sender);
        }

        public void ModifyDialogue(object sender, EventArgs e)
        {
            ThisQuestVariable.Value = MainForm.GetTextFromComboBox(sender);
        }

        public override void Generate_Addon(KVPair Item, OrganizedControlList<QuestStageDialoguePanel, KVPair> Parent)
        {
            //QuestStageDialoguePanel ReturnValue = new QuestStageDialoguePanel(Parent);
            ThisTable.AddItem("NPC: ", new DefaultTextBox(Item.Key), ModifyNPCName);
            ThisTable.AddItem("Dialogue: ", new DefaultDropDown(Item.Value, Interpreter.SelectedQuest.ThisEditorExternal.PossibleDialogueInjections), ModifyDialogue);
            //ThisTable.AddItem("Text Box: ", new DefaultDropDown(Item.Key, , true), ModifyNPCName);
            //return ReturnValue;
        }
    }

    public class QuestStageParticlePanel : KVPanel<QuestStageParticlePanel>
    {

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
            ThisTable.AddItem("NPC: ", new DefaultTextBox(Item.Key), ModifyNPCName);
            ThisTable.AddItem("Particle: ", new DefaultDropDown(Item.Value, Interpreter.CurrentQuestList.ThisEditorExternal.PossibleParticles), ModifyDialogue);
            //return ReturnValue;
        }
    }

    public class RunScriptPanel : SortablePanel<RunScriptPanel, QuestDialogueOptionRunScript>
    {
        ModifyQuestVariableTable ThisTable = new ModifyQuestVariableTable();
        ListPanel<RunScriptArgumentsPanel, KVPair> ArgumentsPanel = new ListPanel<RunScriptArgumentsPanel, KVPair>("New Argument", (a) => { return new KVPair(); });

        public RunScriptPanel(OrganizedControlList<RunScriptPanel, QuestDialogueOptionRunScript> Parent) : base(Parent)
        {
            AddControl(ThisTable);
            AddControl(ArgumentsPanel);
        }

        public override void Generate_Addon(QuestDialogueOptionRunScript Item, OrganizedControlList<RunScriptPanel, QuestDialogueOptionRunScript> Parent)
        {
            ThisTable.AddItem("Class Name", new DefaultTextBox(Item.className), (a, b) => Item.className = MainForm.GetTextFromTextBox(a));
            ThisTable.AddItem("Function Name", new DefaultTextBox(Item.functionName), (a, b) => Item.functionName = MainForm.GetTextFromTextBox(a));
            ArgumentsPanel.ThisList.Refresh(Item.arguments);
        }
    }

    public class RunScriptArgumentsPanel : KVPanel<RunScriptArgumentsPanel>
    {
        public RunScriptArgumentsPanel(OrganizedControlList<RunScriptArgumentsPanel, KVPair> Parent) : base(Parent)
        {
        }

        public override void Generate_Addon(KVPair Item, OrganizedControlList<RunScriptArgumentsPanel, KVPair> Parent)
        {
            ThisTable.AddItem("Name", new DefaultTextBox(Item.Key), (a, b) => Item.Key = MainForm.GetTextFromTextBox(a));
            ThisTable.AddItem("Value", new DefaultTextBox(Item.Value), (a, b) => Item.Value = MainForm.GetTextFromTextBox(a));
        }
    }

    public class QuestStageItemRewardPanel : KVPanel<QuestStageItemRewardPanel>
    {
        public QuestStageItemRewardPanel(OrganizedControlList<QuestStageItemRewardPanel, KVPair> Parent) : base(Parent)
        {
        }

        public void UpdateItemName(object sender, EventArgs e)
        {
            ThisQuestVariable.Key = MainForm.GetTextFromTextBox(sender);
        }

        public void UpdateItemNum(object sender, EventArgs e)
        {
            ThisQuestVariable.Value = ((int)MainForm.GetNumberFromNumericUpDown(sender)).ToString();
        }

        public override void Generate_Addon(KVPair Item, OrganizedControlList<QuestStageItemRewardPanel, KVPair> Parent)
        {
            ThisTable.AddItem("Item:", new DefaultTextBox(Item.Key), UpdateItemName);
            ThisTable.AddItem("Charges:", new DefaultNumericUpDown(Convert.ToInt32(Item.Value)), UpdateItemNum);
        }
    }
    public class QuestStageItemConsumedPanel : KVPanel<QuestStageItemConsumedPanel>
    {
        public QuestStageItemConsumedPanel(OrganizedControlList<QuestStageItemConsumedPanel, KVPair> Parent) : base(Parent)
        {
        }

        public void UpdateItemName(object sender, EventArgs e)
        {
            ThisQuestVariable.Key = MainForm.GetTextFromTextBox(sender);
        }

        public void UpdateItemNum(object sender, EventArgs e)
        {
            ThisQuestVariable.Value = ((int)MainForm.GetNumberFromNumericUpDown(sender)).ToString();
        }

        public override void Generate_Addon(KVPair Item, OrganizedControlList<QuestStageItemConsumedPanel, KVPair> Parent)
        {
            ThisTable.AddItem("Item:", new DefaultTextBox(Item.Key), UpdateItemName);
            ThisTable.AddItem("Amount:", new DefaultNumericUpDown(Convert.ToInt32(Item.Value)), UpdateItemNum);
        }
    }
}
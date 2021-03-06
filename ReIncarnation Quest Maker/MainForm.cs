﻿using System;
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
using System.Runtime.InteropServices;

namespace ReIncarnation_Quest_Maker
{
    /*public abstract class FreezableForm : Form
    {
        private const int WM_SETREDRAW = 0x000B;
        private const int WM_USER = 0x400;
        private const int EM_GETEVENTMASK = (WM_USER + 59);
        private const int EM_SETEVENTMASK = (WM_USER + 69);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        IntPtr eventMask = IntPtr.Zero;

        int drawStopCount = 0;

        public void StopDrawing()
        {
            if (drawStopCount == 0)
            {
                // Stop redrawing:
                SendMessage(this.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
                // Stop sending of events:
                eventMask = SendMessage(this.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
            }
            drawStopCount++;
        }

        public void StartDrawing()
        {
            drawStopCount--;
            if (drawStopCount == 0)
            {
                // turn on events
                SendMessage(this.Handle, EM_SETEVENTMASK, 0, eventMask);

                // turn on redrawing
                SendMessage(this.Handle, WM_SETREDRAW, 1, IntPtr.Zero);

                Invalidate();
                Refresh();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FreezableForm
            // 
            this.ClientSize = new System.Drawing.Size(820, 394);
            this.Name = "FreezableForm";
            this.ResumeLayout(false);

        }
    }*/

    public partial class MainForm : Form
    {
        public OrganizedControlList<QuestButton, Quest> QuestButtons;
        public OrganizedControlList<QuestStagePanel, QuestStage> QuestStagePanels;
        public OrganizedControlList<PrerequisitePanel, KVPair> PrerequisitePanels;
        public OrganizedControlList<QuestDialoguePanel, QuestDialogue> QuestDialoguePanels;
        public OrganizedControlList<QuestTaskPanel, QuestTask> QuestTaskPanels;
        public OrganizedControlList<QuestStageDialoguePanel, KVPair> QuestStageDialoguePanels;
        public OrganizedControlList<QuestStageParticlePanel, KVPair> QuestStageParticlePanels;
        public OrganizedControlList<QuestStageItemRewardPanel, KVPair> QuestStageRewardsPanel;
        public OrganizedControlList<GivesQuestPanel, KVPair> QuestStageGivesQuestPanel;
        public OrganizedControlList<QuestStageItemConsumedPanel, KVPair> QuestStageItemConsumedPanels;
        //public OrganizedControlList<QuestStageDialoguePanel, KVPair> QuestStageParticlePanels;
        //public OrganizedControlList<AffectedNPCsPanel, QuestInjectDialogue> AffectedNPCsPanels;
        //public 

        public MainForm()
        {
            InitializeComponent();
            QuestButtons = OrganizedControlList<QuestButton, Quest>.GenerateOrganizedControlList(QuestList);
            QuestStagePanels = OrganizedControlList<QuestStagePanel, QuestStage>.GenerateOrganizedControlList(QuestStageList);
            PrerequisitePanels = OrganizedControlList<PrerequisitePanel, KVPair>.GenerateOrganizedControlList(PrerequisiteList);
            QuestDialoguePanels = OrganizedControlList<QuestDialoguePanel, QuestDialogue>.GenerateOrganizedControlList(QuestDialogueList);
            QuestTaskPanels = OrganizedControlList<QuestTaskPanel, QuestTask>.GenerateOrganizedControlList(QuestStageTaskList);
            QuestStageDialoguePanels = OrganizedControlList<QuestStageDialoguePanel, KVPair>.GenerateOrganizedControlList(questStageDialogueList);
            QuestStageParticlePanels = OrganizedControlList<QuestStageParticlePanel, KVPair>.GenerateOrganizedControlList(questStageParticleList);
            QuestStageRewardsPanel = OrganizedControlList<QuestStageItemRewardPanel, KVPair>.GenerateOrganizedControlList(questrewarditemspanel);
            QuestStageGivesQuestPanel = OrganizedControlList<GivesQuestPanel, KVPair>.GenerateOrganizedControlList(questsGivenPanel);
            QuestStageItemConsumedPanels = OrganizedControlList<QuestStageItemConsumedPanel, KVPair>.GenerateOrganizedControlList(itemsConsumedPanel);

              //AffectedNPCsPanels = OrganizedControlList<AffectedNPCsPanel, QuestInjectDialogue>.GenerateOrganizedControlList(AffectedNPCsList, AffectedNPCsPanel.Sort());

              Interpreter.ThisForm = this;
            Interpreter.GenerateDefaultQuestList();
            InitialiseScreen();

        }

        public void InitialiseScreen() {
        }

        public void LoadValues()
        {
            Interpreter.CurrentQuestList.ThisEditorExternal.PossibleTypeIcons.AddListener((obj, useless) => QuestTypeIcon.UpdatePossibleValues(obj));
            QuestTask.PossibleTaskTypes.AddListener((obj, useless) => QuestStageTaskType.UpdatePossibleValues(obj));
            QuestStageTaskType.Text = "kill";
        }

        public void UpdateEverything ()
        {
            ClearScreen();
            //NewQuestList.ThisEditorExternal.FilePath = FilePath;

            LoadValues();
            UpdateScreen();

            QuestButtons.Refresh(Interpreter.CurrentQuestList.Quests);
        }

        public void UpdateScreen()
        {
           // StopDrawing();
            QuestName.Text = Interpreter.SelectedQuest.name;
            //QuestInternalName.Text = Interpreter.SelectedQuest.full_name;
            QuestDescriptionTextBox.Text = Interpreter.SelectedQuest.description;
            QuestPortrait.Text = Interpreter.SelectedQuest.portrait;
            QuestTypeIcon.Text = Interpreter.SelectedQuest.typeIcon;
            QuestID.Value = Interpreter.SelectedQuest.questID;
            QuestRepeatable.Checked = Interpreter.SelectedQuest.repeatable;
            QuestCantAbandon.Checked = Interpreter.SelectedQuest.cantAbandon;
          //  StartDrawing();
        }

        public void UpdateQuestData()
        {
            //StopDrawing();
            levelrequiredfield.Value = Interpreter.SelectedQuest.ThisEditorExternal.prereqlevel;
            classrequiredfield.Text = Interpreter.SelectedQuest.ThisEditorExternal.prereqclass;
            PrerequisitePanels.Refresh(Interpreter.SelectedQuest.questprerequisites);
            QuestStagePanels.Refresh(Interpreter.SelectedQuest.stages);
            QuestDialoguePanels.Refresh(Interpreter.SelectedQuest.injections);
            //

            UpdateStageData();
           // StartDrawing();
        }

        public void UpdateStageData()
        {
            //StopDrawing();
            QuestStageDescriptionBox.Text = Interpreter.SelectedQuestStage.ThisOptionalFields.description;
            QuestStageXPOnCompletion.Value = Interpreter.SelectedQuestStage.ThisEditorExternal.exp;
            QuestStageGoldOnCompletion.Value = Interpreter.SelectedQuestStage.ThisEditorExternal.gold;
            QuestTaskPanels.Refresh(Interpreter.SelectedQuestStage.tasks);
            QuestStageDialoguePanels.Refresh(Interpreter.SelectedQuestStage.dialogue);
            QuestStageParticlePanels.Refresh(Interpreter.SelectedQuestStage.particles);
            QuestStageRewardsPanel.Refresh(Interpreter.SelectedQuestStage.ThisEditorExternal.items);
            QuestStageGivesQuestPanel.Refresh(Interpreter.SelectedQuestStage.ThisEditorExternal.QuestsGiven);
            QuestStageItemConsumedPanels.Refresh(Interpreter.SelectedQuestStage.ThisEditorExternal.itemsConsumed);
            //AffectedNPCsPanel.MassGenerate(Interpreter.SelectedQuestStage.affectedNPCs.ToArray(), AffectedNPCsPanels);
            // SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
            //StartDrawing();
        }

        public void ClearScreen()
        {
            ClearQuestData();
            QuestButtons.Clear();
        }

        public void ClearQuestData()
        {
            PrerequisitePanels.Clear();
            QuestStagePanels.Clear();
        }

        public void ClearStageData()
        {
            //AffectedNPCsPanels.Clear();
        }

        public void OnNewQuest(Quest NewQuest)
        {
            QuestButton.Generate(NewQuest, QuestButtons);
        }

        public void OnNewQuestStage(QuestStage NewStage)
        {
            QuestStagePanel.Generate(NewStage, QuestStagePanels);
        }

        public void OnNewPrerequisite(KVPair NewPrequisite)
        {
            PrerequisitePanel.Generate(NewPrequisite, PrerequisitePanels);
        }

        public void OnNewQuestStageTask(QuestTask NewTask)
        {
            QuestTaskPanel.Generate(NewTask, QuestTaskPanels);
        }

        public void OnNewQuestDialogue(QuestDialogue NewDialogue)
        {
            QuestDialoguePanel.Generate(NewDialogue, QuestDialoguePanels);
        }

        public void OnNewQuestStageDialogue(KVPair NewStageDialogue) {
            QuestStageDialoguePanel.Generate(NewStageDialogue, QuestStageDialoguePanels);
        }
        

        public void OnNewQuestStageParticle(KVPair NewStageParticle)
        {
            QuestStageParticlePanel.Generate(NewStageParticle, QuestStageParticlePanels);
        }

        public void OnNewQuestStageItemReward(KVPair NewQuestStageItemReward) {
            QuestStageItemRewardPanel.Generate(NewQuestStageItemReward, QuestStageRewardsPanel);
        }

        public void OnNewQuestStageGiven(KVPair NewQuestStageGiven) {
            GivesQuestPanel.Generate(NewQuestStageGiven, QuestStageGivesQuestPanel);
        }

        /*public void OnNewAffectedNPC(QuestInjectDialogue NewAffectedNPC)
        {
            AffectedNPCsPanel.Generate(NewAffectedNPC, AffectedNPCsPanels);
        }*/

        //file/load/new

        private void toolStripDropDownCompileButton_Click(object sender, EventArgs e)
        {
            if (Interpreter.CurrentQuestList.ThisEditorExternal.FilePath == null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Text Files (.txt)|*.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    //Parse
                    Interpreter.Print(sfd.FileName);
                }
            }
            else
            {
                Interpreter.Print(Interpreter.CurrentQuestList.ThisEditorExternal.FilePath);
            }
        }

        private void fromTextToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (.txt)|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Parse
                Interpreter.LoadNewQuestListFromText(ofd.FileName);
            }
        }

        private void MergeButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (.txt)|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Parse
                Interpreter.MergeQuestListFromText(ofd.FileName);
            }
        }

        private void NewQuestList(object sender, EventArgs e)
        {
            Interpreter.GenerateDefaultQuestList();
            UpdateScreen();
        }

        //modify

        private void QuestDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            Interpreter.UpdateSelectedQuestDescription(GetTextFromTextBox(sender));
        }

        private void QuestName_TextChanged(object sender, EventArgs e)
        {
            Interpreter.UpdateSelectedQuestName(GetTextFromTextBox(sender));
        }

        private void InternalName_TextChanged(object sender, EventArgs e)
        {
            Interpreter.UpdateSelectedQuestInternalName(GetTextFromTextBox(sender));
        }

        private void QuestPortrait_TextChanged(object sender, EventArgs e)
        {
            Interpreter.UpdateSelectedQuestPortrait(GetTextFromTextBox(sender));

        }

        private void QuestCantAbandon_CheckedChanged(object sender, EventArgs e)
        {
            Interpreter.UpdateSelectedQuestCantAbandon(GetStateFromCheckBox(sender));
        }

        private void QuestRepeatable_CheckedChanged(object sender, EventArgs e)
        {
            Interpreter.UpdateSelectedQuestRepeatable(GetStateFromCheckBox(sender));
        }

        private void QuestTypeIcon_SelectedIndexChanged(object sender, EventArgs e)
        {
            Interpreter.UpdateSelectedQuestIcon(GetTextFromComboBox(sender));
        }

        private void QuestStageDescriptionBox_TextChanged(object sender, EventArgs e)
        {
            Interpreter.UpdateSelectedQuestStateDescription(GetTextFromTextBox(sender));
        }

        private void UpdateIDButton_Click(object sender, EventArgs e)
        {
            Interpreter.UpdateSelectedQuestID((int) GetNumberFromNumericUpDown(QuestID));
            QuestButtons.SortControls();
        }

        private void XPOnCompletion_ValueChanged(object sender, EventArgs e)
        {
            Interpreter.SelectedQuestStage.ThisEditorExternal.exp = (int)GetNumberFromNumericUpDown(sender);
        }

        private void GoldOnCompletion_ValueChanged(object sender, EventArgs e)
        {
            Interpreter.SelectedQuestStage.ThisEditorExternal.gold = (int)GetNumberFromNumericUpDown(sender);
        }

        //usefulfuncs

        public static string GetTextFromTextBox(object box)
        {
            TextBox senderbox = (TextBox)box;
            return senderbox.Text;
        }

        public static string GetTextFromComboBox(object box)
        {
            ComboBox senderbox = (ComboBox)box;
            return senderbox.Text;
        }

        public static bool GetStateFromCheckBox(object box)
        {
            CheckBox senderbox = (CheckBox)box;
            return senderbox.Checked;
        }

        public static decimal GetNumberFromNumericUpDown(object box)
        {
            NumericUpDown senderbox = (NumericUpDown)box;
            return senderbox.Value;
        }

        //add

        private void newQuestButton_Click(object sender, EventArgs e)
        {
            Interpreter.AddQuest();
        }

        private void newqueststagebutton_Click(object sender, EventArgs e)
        {
            Interpreter.AddQuestStage();
        }

        private void NewPrerequisiteButton_Click(object sender, EventArgs e)
        {
            Interpreter.AddPrerequisite();
        }

        private void newAffectedNPC_Click(object sender, EventArgs e)
        {
            Interpreter.AddAffectedNPC();
        }

        private void newqueststagetaskbutton_Click(object sender, EventArgs e)
        {
            Interpreter.AddQuestStageTask(GetTextFromComboBox(QuestStageTaskType));
        }

        private void NewQuestDialogueButton_Click(object sender, EventArgs e)
        {
            Interpreter.AddQuestDialogue();
        }

        private void NewStageDialogueButton_Click(object sender, EventArgs e)
        {
            Interpreter.AddQuestStageDialogue();
        }

        private void NewStageParticle_Click(object sender, EventArgs e)
        {
            Interpreter.AddQuestStageParticle();
        }

        private void newQuestGivenButton_Click(object sender, EventArgs e)
        {
            Interpreter.AddNewQuestGiven();
        }

        private void fixIDsButton_Click(object sender, EventArgs e)
        {
            Interpreter.fixIDs();
        }

        private void questrewarditemsbutton_Click(object sender, EventArgs e)
        {
            Interpreter.AddQuestStageReward();
        }

        private void levelrequiredfield_ValueChanged(object sender, EventArgs e)
        {
            Interpreter.SelectedQuest.ThisEditorExternal.prereqlevel = (int) GetNumberFromNumericUpDown(sender);
        }

        private void classrequiredfield_TextChanged(object sender, EventArgs e)
        {
            Interpreter.SelectedQuest.ThisEditorExternal.prereqclass = GetTextFromTextBox(sender);
        }

        private void newItemConsumedButton_Click(object sender, EventArgs e)
        {
            KVPair NewItemConsumed = new KVPair("", 1.ToString());
            Interpreter.SelectedQuestStage.ThisEditorExternal.itemsConsumed.Add(NewItemConsumed);
            QuestStageItemConsumedPanel.Generate(NewItemConsumed, QuestStageItemConsumedPanels);
        }

        private void DuplicateQuestButton_Click(object sender, EventArgs e)
        {
            Quest NewQuest = Interpreter.CurrentQuestList.CloneQuest(Interpreter.SelectedQuest);
            OnNewQuest(NewQuest);
        }
    }

    public class DefaultCheckBox : CheckBox {
        public DefaultCheckBox(bool Checked = false) {
            this.Checked = Checked;
        }
    }

    public class DefaultPanel : Panel {
        public DefaultPanel()
        {
            BackColor = System.Drawing.SystemColors.ControlDark;

            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Dock = System.Windows.Forms.DockStyle.Fill;
        }
    }

    public class DefaultLabel : Label
    {

        public DefaultLabel(string Text) : this()
        {
            this.Text = Text;
        }

        public DefaultLabel() : base()
        {
            Location = new System.Drawing.Point(0, 0);
            MaximumSize = new Size(0, 25);
            TabIndex = 1;
            TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ForeColor = SystemColors.ControlLight;
            Anchor = System.Windows.Forms.AnchorStyles.Top;
            Dock = DockStyle.Fill;
            AutoSize = true;
        }
    }
    public class DefaultTextBox : TextBox
    {
        public DefaultTextBox(string Text, bool Multiline = false) : this()
        {
            this.Text = Text;
            this.Multiline = Multiline;
            if (Multiline) {
                ScrollBars = ScrollBars.Vertical;
            }
        }

        public DefaultTextBox() : base() {
            Dock = System.Windows.Forms.DockStyle.Fill;
            Location = new System.Drawing.Point(3, 3);
            Size = new System.Drawing.Size(94, 20);
        }
    }

    public class DefaultTable : TableLayoutPanel
    {
        /*static int nextID = 0;

        int thisID;*/

        static short ChangeinitNum = 0;

        public static LinkedList<DefaultTable> AutoSizeStack = new LinkedList<DefaultTable>();

        public static void StartInit()
        {
            ChangeinitNum++;
        }
        public static void EndInit()
        {
            ChangeinitNum--;
            if (ChangeinitNum == 0) {
                ActivateAutosize();
            }
        }

        static void ActivateAutosize() {
            LinkedListNode<DefaultTable> NextTable = AutoSizeStack.First;
            for (int x = 0; x < AutoSizeStack.Count; x++) {
                NextTable.Value.AutoSize = true;
                NextTable.Value.ResumeLayout();
                NextTable = NextTable.Next;
            }
        }

        /*int numsizechanged = 0;

        public override string ToString()
        {
            return thisID + "asd";
        }*/

        public DefaultTable(int ColumnCount, int RowCount)
        {
            SuspendLayout();
            this.RowCount = RowCount;
            this.ColumnCount = ColumnCount;
            Dock = DockStyle.Fill;
            TabIndex = 5;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoSizeStack.AddFirst(this);
            /*SizeChanged += new System.EventHandler( (a, b) =>{
                numsizechanged++;
                if (numsizechanged >= 2) {
                }

            });
            thisID = nextID;
            nextID++;*/
        }
    }

    public class DefaultImagebutton : PictureBox
    {

        public DefaultImagebutton(Action<object, EventArgs> OnClick, Image ThisImage)
        {
            this.Image = ThisImage;
            Location = new System.Drawing.Point(0, 0);
            Margin = new Padding(0, 0, 0, 0);
            Click += new System.EventHandler(OnClick);
            Padding = new Padding(0, 0, 0, 0);
            this.Width = Image.Width;
            this.Height = Image.Height;
        }
    }

    public class DefaultButton : Button
    {
        public DefaultButton(Action<object, EventArgs> OnClick, string Text = "")
        {
            this.Text = Text;
            Location = new System.Drawing.Point(0, 0);
            UseVisualStyleBackColor = true;
            Margin = new Padding(0, 0, 0, 0);
            Click += new System.EventHandler(OnClick);
            Padding = new Padding(0, 0, 0, 0);
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
    }

    public class DefaultDropDown : ComboBox
    { 
        public DefaultDropDown(string Text, ListeningList<string> Options, bool IsSearchBox = false)
        {
            if (IsSearchBox) {
                DropDown += (sender, e) => {
                    LinqExtensions.UpdatePossibleValues(this, Options, true);
                };
                Options.AddListener((obj, useless) => LinqExtensions.UpdatePossibleValues(this, obj, true));
            }
            else
            {
                Options.AddListener((obj, useless) => LinqExtensions.UpdatePossibleValues(this, obj));
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            }
            FormattingEnabled = true;
            Location = new System.Drawing.Point(130, 78);
            Size = new System.Drawing.Size(10, 21);
            Dock = System.Windows.Forms.DockStyle.Fill;

            this.Text = Text;
        }
    }

    public class DefaultNumericUpDown : NumericUpDown
    {
        public DefaultNumericUpDown(int Value)
        {
            this.Maximum = 2147483647;
            this.Value = Value;
            this.Dock = DockStyle.Fill;
            this.AutoSize = true;
        }
    }
}

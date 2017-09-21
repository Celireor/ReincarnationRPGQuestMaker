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
    public partial class MainForm : Form
    {
        public OrganizedControlList<QuestButton, Quest> QuestButtons;
        public OrganizedControlList<QuestStagePanel, QuestStage> QuestStagePanels;
        public OrganizedControlList<PrerequisitePanel, KVPair> PrerequisitePanels;
        public OrganizedControlList<QuestDialoguePanel, QuestDialogue> QuestDialoguePanels;
        public OrganizedControlList<QuestTaskPanel, QuestTask> QuestTaskPanels;
        public OrganizedControlList<QuestStageDialoguePanel, KVPair> QuestStageDialoguePanels;
        //public OrganizedControlList<QuestStageDialoguePanel, KVPair> QuestStageParticlePanels;
        //public OrganizedControlList<AffectedNPCsPanel, QuestInjectDialogue> AffectedNPCsPanels;
        //public 

        public MainForm()
        {
            InitializeComponent();
            QuestButtons = OrganizedControlList<QuestButton, Quest>.GenerateOrganizedControlList(QuestList, QuestButton.Sort());
            QuestStagePanels = OrganizedControlList<QuestStagePanel, QuestStage>.GenerateOrganizedControlList(QuestStageList, QuestStagePanel.Sort());
            PrerequisitePanels = OrganizedControlList<PrerequisitePanel, KVPair>.GenerateOrganizedControlList(PrerequisiteList, PrerequisitePanel.Sort());
            QuestDialoguePanels = OrganizedControlList<QuestDialoguePanel, QuestDialogue>.GenerateOrganizedControlList(QuestDialogueList, QuestDialoguePanel.Sort());
            QuestTaskPanels = OrganizedControlList<QuestTaskPanel, QuestTask>.GenerateOrganizedControlList(QuestStageTaskList, QuestTaskPanel.Sort());
            QuestStageDialoguePanels = OrganizedControlList<QuestStageDialoguePanel, KVPair>.GenerateOrganizedControlList(questStageDialogueList, QuestStageDialoguePanel.Sort());

            //AffectedNPCsPanels = OrganizedControlList<AffectedNPCsPanel, QuestInjectDialogue>.GenerateOrganizedControlList(AffectedNPCsList, AffectedNPCsPanel.Sort());

            Interpreter.ThisForm = this;
            Interpreter.GenerateDefaultQuestList();
            InitialiseScreen();

        }

        public void InitialiseScreen() {
        }

        public void LoadValues()
        {
            Interpreter.CurrentQuestList.ThisEditorExternal.PossibleTypeIcons.AddListener(obj => QuestTypeIcon.UpdatePossibleValues(obj));
            QuestTask.PossibleTaskTypes.AddListener(obj => QuestStageTaskType.UpdatePossibleValues(obj));
            QuestStageTaskType.Text = "kill";
        }

        public void UpdateScreen()
        {
            QuestName.Text = Interpreter.SelectedQuest.name;
            QuestInternalName.Text = Interpreter.SelectedQuest.full_name;
            QuestDescriptionTextBox.Text = Interpreter.SelectedQuest.description;
            QuestPortrait.Text = Interpreter.SelectedQuest.portrait;
            QuestTypeIcon.Text = Interpreter.SelectedQuest.typeIcon;
            QuestID.Value = Interpreter.SelectedQuest.questID;
            QuestRepeatable.Checked = Interpreter.SelectedQuest.repeatable;
            QuestCantAbandon.Checked = Interpreter.SelectedQuest.cantAbandon;
        }

        public void UpdateQuestData()
        {
            PrerequisitePanels.Refresh(Interpreter.SelectedQuest.prerequisites);
            QuestStagePanels.Refresh(Interpreter.SelectedQuest.stages);
            QuestDialoguePanels.Refresh(Interpreter.SelectedQuest.injections);
            //

            UpdateStageData();
        }

        public void UpdateStageData()
        {
            QuestStageDescriptionBox.Text = Interpreter.SelectedQuestStage.description;
            QuestTaskPanels.Refresh(Interpreter.SelectedQuestStage.tasks);
            QuestStageDialoguePanels.Refresh(Interpreter.SelectedQuestStage.dialogue);
            //AffectedNPCsPanel.MassGenerate(Interpreter.SelectedQuestStage.affectedNPCs.ToArray(), AffectedNPCsPanels);
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

        /*public void OnNewAffectedNPC(QuestInjectDialogue NewAffectedNPC)
        {
            AffectedNPCsPanel.Generate(NewAffectedNPC, AffectedNPCsPanels);
        }*/

        //file/load/new

        private void fromReincQuestFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ReIncarnation Quest Maker Files (.ReincQuest)|*.ReincQuest";
        }

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
                QuestFormatParser.OpenTextFile(ofd.FileName);
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
    }

    /*
    public class AffectedNPCsPanel : SortablePanel<AffectedNPCsPanel, QuestInjectDialogue>
    {
        TableLayoutPanel seperator;
        DefaultLabel constlabel;
        TextBox npcbox;

        public void UpdateValue(object sender, EventArgs e)
        {
            ThisQuestVariable.NPCAffected = MainForm.GetTextFromTextBox(sender);
        }

        public AffectedNPCsPanel() { GenerateFunction = Generate_2; }

        public AffectedNPCsPanel (OrganizedControlList<AffectedNPCsPanel, QuestInjectDialogue> Parent) : base(Parent)
        {
            seperator = new DefaultTable(2, 1);

            AddControl(seperator);

            constlabel = new DefaultLabel("NPC:");
        }

        public static AffectedNPCsPanel Generate_2(QuestInjectDialogue newAffectedNPC, OrganizedControlList<AffectedNPCsPanel, QuestInjectDialogue> affectedNPCsPanels)
        {
            AffectedNPCsPanel ReturnValue = new AffectedNPCsPanel(affectedNPCsPanels);

            ReturnValue.npcbox = new DefaultTextBox(newAffectedNPC.NPCAffected);
            ReturnValue.npcbox.TextChanged += ReturnValue.UpdateValue;

            ReturnValue.seperator.Controls.Add(ReturnValue.constlabel, 0, 1);
            ReturnValue.seperator.Controls.Add(ReturnValue.npcbox, 1, 1);

            ReturnValue.FinishUp();

            return ReturnValue;
        }

        public override IComparer<AffectedNPCsPanel> SortComparer ()
        {
            return new SortableButton_IComparer();
        }
    }
    */
    /*public class DefaultPanelButton : Button
    {
        public DefaultPanelButton(string Text, Action<object, EventArgs> OnClick)
        {
            this.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Location = new System.Drawing.Point(3, 3);
            this.Name = "newqueststagetaskbutton";
            this.Size = new System.Drawing.Size(185, 32);
            this.TabIndex = 11;
            this.Text = "New Task";
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Click += new System.EventHandler(OnClick);
        }
    }*/

    public class DefaultCheckBox : CheckBox {
        public DefaultCheckBox(bool Checked = false) {
            this.Checked = Checked;
        }
    }

    public class DefaultPanel : Panel {
        public DefaultPanel()
        {
            BackColor = System.Drawing.SystemColors.ControlDark;

            AutoSize = true;
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
            AutoSize = true;
            MaximumSize = new Size(0, 25);
            TabIndex = 1;
            TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ForeColor = SystemColors.ControlLight;
            Anchor = System.Windows.Forms.AnchorStyles.Top;
            Dock = DockStyle.Fill;
        }
    }
    public class DefaultTextBox : TextBox {

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

    public class DefaultTable : TableLayoutPanel {
        public DefaultTable(int ColumnCount, int RowCount)
        {
            this.RowCount = RowCount;
            this.ColumnCount = ColumnCount;
            Dock = DockStyle.Fill;
            TabIndex = 5;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
    }

    public class DefaultImagebutton : PictureBox
    {

        public DefaultImagebutton(Action<object, EventArgs> OnClick, Image ThisImage)
        {
            this.Image = ThisImage;
            Location = new System.Drawing.Point(0, 0);
            Margin = new Padding(0, 0, 0, 0);
            AutoSize = true;
            Click += new System.EventHandler(OnClick);
            Padding = new Padding(0, 0, 0, 0);
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
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Click += new System.EventHandler(OnClick);
            Padding = new Padding(0, 0, 0, 0);
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
                Options.AddListener(obj => LinqExtensions.UpdatePossibleValues(this, obj, true));
            }
            else
            {
                Options.AddListener(obj => LinqExtensions.UpdatePossibleValues(this, obj));
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
        }
    }
}

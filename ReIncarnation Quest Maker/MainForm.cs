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
            Interpreter.UpdateSelectedQuestID((int) GetNumberFromNumericUpDown(sender));
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

    public class OrganizedControlList <T, U> where T : SortablePanel <T, U>, new() {
        public U ThisQuestVariable;

        public List<T> ThisList = new List<T>();

        public Panel BaseControl;

        public IComparer<T> OrderMetric;

        public int NextPos = 0;

        //public Func

        public static OrganizedControlList <T, U> GenerateOrganizedControlList (Panel BaseControl, IComparer<T> OrderMetric) {
            OrganizedControlList<T, U> ReturnValue = new OrganizedControlList<T, U> ();
            ReturnValue.OrderMetric = OrderMetric;
            ReturnValue.BaseControl = BaseControl;
            ReturnValue.BaseControl.SizeChanged += ReturnValue.ResetAllItemSizes;
            return ReturnValue;
        }

        public void ResetAllItemSizes(object sender, EventArgs e) {
            ThisList.ForEach(obj => obj.ResetSize(this));
        }

        public void Add(T Item) {
            ThisList.Add(Item);
            PositionNewItem(Item);
            BaseControl.Controls.Add(Item);
            //RepositionItems();
        }

        public void Remove(T Item) {
            ThisList.Remove(Item);
            BaseControl.Controls.Remove(Item);
            SortControls();
        }

        public void RepositionItems () {

            NextPos = 0;
            ThisList.ForEach(obj =>
            {
                PositionNewItem(obj);
            });
        }

        public void PositionNewItem (T Item)
        {
            Item.UpdateLocation(NextPos - BaseControl.VerticalScroll.Value);
            NextPos += Item.Height + 3;
        }

        public void SortControls() {
            ThisList.Sort(OrderMetric);
            RepositionItems();
        }

        public void Clear ()
        {
            ThisList.ForEach(obj =>
            {
                BaseControl.Controls.Remove(obj);
            });
            ThisList.Clear();
            NextPos = 0;
        }

        public void Refresh(List<U> RefreshList) {
            Clear();
            SortablePanel<T, U>.MassGenerate(RefreshList.ToArray(), this);
        }
    }
    

    public abstract class SortablePanel <T, U> : Panel where T : SortablePanel <T, U>, new()
    {
        DefaultTable ThisTable;
        public Panel DownContentsPanel;

        DefaultTable ContentsTable;
        public Panel ContentsPanel;

        DefaultImagebutton TrashButton;
        DefaultImagebutton UpButton;
        DefaultImagebutton DownButton;
        TableLayoutPanel UpDownButtonTable;
        public OrganizedControlList<T, U> ParentControlList;

        public int ListPosition;
        public Action<T, int> Move_Addon;
        public Func<bool> TrashFunction;

        public U ThisQuestVariable;

        public Func<U, OrganizedControlList<T, U>, T> GenerateFunction;
        public Action FinishUpFunction;

        public static IComparer<T> Sort() {
            return new T().SortComparer();
        }

        public void Trash (object sender, EventArgs e)
        {
            bool CanTrash = true;
            if (TrashFunction != null) {
                CanTrash = TrashFunction();
            }
            if (CanTrash)
            {
                ParentControlList.Remove((T)this);
                ParentControlList.ThisList.AssignIndexValuesToListItems((obj, index) => {
                    obj.ListPosition = index;
                });
            }
        }

        public void MoveUp (object sender, EventArgs e)
        {
            MovePos(-1);
        }

        public void MoveDown (object sender, EventArgs e)
        {
            MovePos(1);
        }

        public void MovePos(int Displacement) {
            if (ListPosition + Displacement < 0 || ListPosition + Displacement >= ParentControlList.ThisList.Count) {
                return;
            }
            T OtherItem = ParentControlList.ThisList.Swap(ListPosition, ListPosition + Displacement);
            Move_Addon?.Invoke(OtherItem, ListPosition + Displacement);
            ListPosition += Displacement;
            OtherItem.ListPosition -= Displacement;
            ParentControlList.SortControls();
        }

        public void UpdateLocation(int Offset)
        {
            Location = new System.Drawing.Point(3, 3 + Offset);
        }

        public SortablePanel() {
            //nothing in here. only use for sortcomparer
        }

        public void ResetSize(OrganizedControlList<T, U> Parent)
        {
            int ExtraVerticalScrollSpace = 0;
            if (Parent.BaseControl.VerticalScroll.Visible)
            {
                ExtraVerticalScrollSpace = SystemInformation.VerticalScrollBarWidth;
            }
            MinimumSize = new System.Drawing.Size(Parent.BaseControl.Size.Width - 6 - ExtraVerticalScrollSpace, 0);
            MaximumSize = new System.Drawing.Size(Parent.BaseControl.Size.Width - 6 - ExtraVerticalScrollSpace, int.MaxValue);
            ThisTable.MaximumSize = MaximumSize;
            ContentsTable.MaximumSize = MaximumSize;
            DownContentsPanel.MaximumSize = MaximumSize;
            ContentsPanel.MaximumSize = MaximumSize;
        }

        public void OnSizeChanged(object sender, EventArgs e) {
            ParentControlList.RepositionItems();
        }

        public SortablePanel (OrganizedControlList<T, U> Parent) {

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            ThisTable = new DefaultTable(1, 2);
            DownContentsPanel = new Panel();

            ContentsTable = new DefaultTable(3, 1);
            ContentsPanel = new Panel();
            TrashButton = new DefaultImagebutton(Trash, new Bitmap(Properties.Resources.Trash));
            UpDownButtonTable = new DefaultTable(1, 2);
            UpButton = new DefaultImagebutton(MoveUp, new Bitmap(ReIncarnation_Quest_Maker.Properties.Resources.UpArrow));
            DownButton = new DefaultImagebutton(MoveDown, new Bitmap(ReIncarnation_Quest_Maker.Properties.Resources.DownArrow));

            ContentsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            ContentsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ContentsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));

            TabIndex = 5;

            ParentControlList = Parent;

            BackColor = SystemColors.ControlDarkDark;

            ContentsPanel.AutoSize = true;
            ContentsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ContentsPanel.Margin = new Padding(0, 0, 0, 0);

            ContentsPanel.Dock = DockStyle.Fill;

            DownContentsPanel.AutoSize = true;
            DownContentsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            DownContentsPanel.Dock = DockStyle.Fill;
            DownContentsPanel.Margin = new Padding(0, 0, 0, 0);

            ContentsTable.Margin = new Padding(0, 0, 0, 0);
            ContentsTable.TabIndex = 5;

            UpDownButtonTable.Margin = new Padding(0, 0, 0, 0);

            UpDownButtonTable.Controls.Add(UpButton, 0, 0);
            UpDownButtonTable.Controls.Add(DownButton, 0, 1);

            ContentsTable.Controls.Add(UpDownButtonTable, 0, 0);
            ContentsTable.Controls.Add(ContentsPanel, 1, 0);
            ContentsTable.Controls.Add(TrashButton, 2, 0);
            //ContentsTable.Dock = DockStyle.Fill;

            ThisTable.Controls.Add(ContentsTable, 0, 0);
            ThisTable.Controls.Add(DownContentsPanel, 0, 1);
            //ThisTable.Dock = DockStyle.Fill;

            Controls.Add(ThisTable);
        }

        public static T[] MassGenerate(U[] Item, OrganizedControlList<T, U> Parent)
        {
            List<T> ReturnValue = new List<T>();
            for (int x = 0; x < Item.Length; x++)
            {
                ReturnValue.Add(Generate(Item[x], Parent));
            }
            return ReturnValue.ToArray();
        }

        public static T Generate(U Item, OrganizedControlList<T, U> Parent)
        {
            T ReturnValue = new T().GenerateFunction(Item, Parent);
            ReturnValue.ThisQuestVariable = Item;
            ReturnValue.FinishUp();
            return ReturnValue;
        }

        public void AddControl(Control Item, bool AddToDownContentsPanel = false)
        {
            if (AddToDownContentsPanel)
            {
                DownContentsPanel.Controls.Add(Item);
                return;
            }
            ContentsPanel.Controls.Add(Item);
        }

        public void RemoveControl(Control Item, bool RemoveFromDownContentsPanel = false)
        {
            if (RemoveFromDownContentsPanel)
            {
                DownContentsPanel.Controls.Remove(Item);
                return;
            }
            ContentsPanel.Controls.Remove(Item);
        }

        public void FinishUp()
        {
            Dock = DockStyle.Fill;
            Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            ListPosition = ParentControlList.ThisList.Count;

            FinishUpFunction?.Invoke();

            SizeChanged += OnSizeChanged;

            ParentControlList.Add((T)this);

            ResetSize(ParentControlList);
        }

        public abstract IComparer<T> SortComparer();

        public class SortablePanel_IComparer : IComparer<T>
        {
            public int Compare(T a, T b) //Compare TA to TB kappa ta loses
            {
                return a.ListPosition.CompareTo(b.ListPosition);
            }
        }
    }

    public class ListPanel<T, U> : Panel where T : SortablePanel<T, U>, new()
    {
        public DefaultTable ThisTable;
        public DefaultPanel ThisPanel;
        public DefaultButton ThisButton;
        public OrganizedControlList<T, U> ThisList;

        public Func<U> NewDefaultItem;

        public void OnClick(object sender, EventArgs e)
        {
            SortablePanel<T, U>.Generate(NewDefaultItem(), ThisList);
        }

        public void ResetSize (object sender, EventArgs e)
        {
            Size NewMaxSize = new System.Drawing.Size(Parent.Size.Width - 6, int.MaxValue);
            ThisTable.MaximumSize = NewMaxSize;
            NewMaxSize.Width -= 6;
            ThisPanel.MaximumSize = NewMaxSize;
            ThisButton.MaximumSize = NewMaxSize;
        }

        public ListPanel(string ThisButtonText, Func<U> NewDefaultItem, IComparer<T> SortMetric)
        {
            ThisTable = new DefaultTable(1, 2);
            ThisTable.Margin = new Padding(0, 0, 0, 0);
            ThisTable.Padding = new Padding(0, 0, 0, 0);

            this.NewDefaultItem = NewDefaultItem;
            ThisPanel = new DefaultPanel();
            ThisButton = new DefaultButton(OnClick, ThisButtonText);
            ThisList = OrganizedControlList<T, U>.GenerateOrganizedControlList(ThisPanel, SortMetric);

            //ThisPanel.AutoSize = true;

            ThisPanel.Dock = DockStyle.Top;
            ThisButton.Dock = System.Windows.Forms.DockStyle.Top;
            ThisTable.Dock = DockStyle.Top;

            ThisTable.Controls.Add(ThisButton, 0, 1);
            ThisTable.Controls.Add(ThisPanel, 0, 0);
            Controls.Add(ThisTable);

            //ThisTable.Dock = DockStyle.Top;

            //ThisPanel.AutoSize = true;
            //ThisPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            //Anchor = AnchorStyles.Left | AnchorStyles.Right;

            SizeChanged += ResetSize;
        }
    }

    public class ModifyQuestVariableTable : DefaultTable
    {
        public ModifyQuestVariableTable() : base(2, 1)
        {
            Dock = DockStyle.Fill;
        }

        public void AddItem(string VarString, Control VarControl, EventHandler OnValueChanged)
        {
            Controls.Add(new DefaultLabel(VarString), 0, RowCount - 1);
            Controls.Add(VarControl, 1, RowCount - 1);

            Type VarControlType = VarControl.GetType();
            if (VarControlType == typeof(DefaultTextBox))
            {
                TextBox temp = VarControl as TextBox;
                temp.TextChanged += OnValueChanged;
            }
            else if (VarControlType == typeof(DefaultDropDown))
            {

                ComboBox temp = VarControl as ComboBox;
                temp.TextChanged += OnValueChanged;
            }
            else if (VarControlType == typeof(DefaultNumericUpDown))
            {

                DefaultNumericUpDown temp = VarControl as DefaultNumericUpDown;
                temp.ValueChanged += OnValueChanged;
            }
            else if (VarControlType == typeof(DefaultCheckBox))
            {

                DefaultCheckBox temp = VarControl as DefaultCheckBox;
                temp.CheckedChanged += OnValueChanged;
            }

            RowCount++;
        }
    }

    public class QuestButton : SortablePanel<QuestButton, Quest>
    {

        private DefaultLabel QuestButtonQuestName;
        // this.newquestbutton = new System.Windows.Forms.Button();

        public void moveAddon(QuestButton Other, int OtherPos) {
            UtilityFunctions.Swap2Variables(ref ThisQuestVariable.questID, ref Other.ThisQuestVariable.questID);
            UpdateValues();
            Other.UpdateValues();
            Interpreter.ThisForm.UpdateScreen();
        }

        public bool Trash()
        {
            if (ParentControlList.ThisList.Count > 1) {
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

        public static QuestButton Generate_2(Quest NewQuest, OrganizedControlList<QuestButton, Quest> Parent) {
            QuestButton ReturnValue = QuestButton.NewQuestButton(NewQuest, Parent);
            return ReturnValue;
        }

        public QuestButton() { GenerateFunction = Generate_2; }
        public QuestButton(OrganizedControlList<QuestButton, Quest> Parent) : base(Parent) { ContentsPanel.Click += new EventHandler(OnClick); }

        public static QuestButton NewQuestButton(Quest NewQuest, OrganizedControlList<QuestButton, Quest> Parent)
        {
            QuestButton ReturnValue = new QuestButton (Parent);

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
                QuestButton a_quest = (QuestButton)a;
                QuestButton b_quest = (QuestButton)b;
                if (a_quest.ThisQuestVariable.questID > b_quest.ThisQuestVariable.questID)
                {
                    return 1;
                }
                if (a_quest.ThisQuestVariable.questID < b_quest.ThisQuestVariable.questID)
                {
                    return -1;
                }
                return 0;
            }
        }
    }

    public class PrerequisitePanel : SortablePanel<PrerequisitePanel, KVPair>
    {
        ModifyQuestVariableTable ThisTable;

        public void moveAddon (PrerequisitePanel other, int OtherPos)
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

        public void moveAddon(QuestStagePanel other, int otherpos) {
            ThisQuestVariable.ThisEditorExternal.Parent.stages.Swap(ListPosition, otherpos);
            UtilityFunctions.Swap2Variables(ref ThisQuestVariable.ThisEditorExternal.StageNum, ref other.ThisQuestVariable.ThisEditorExternal.StageNum);
            OnValuesUpdated();
            other.OnValuesUpdated();
        }

        public void OnValuesUpdated ()
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

        public static QuestStagePanel Generate_2(QuestStage Item, OrganizedControlList<QuestStagePanel, QuestStage> Parent) {
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

        public QuestTaskPanel() { GenerateFunction = Generate_2;}

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

        public void moveAddon(QuestTaskPanel OtherObject, int OtherPosition) {
            ThisQuestVariable.ThisEditorExternal.ParentStage.tasks.Swap(ListPosition, OtherPosition);
        }

        public static QuestTaskPanel Generate_2(QuestTask Item, OrganizedControlList<QuestTaskPanel, QuestTask> Parent)
        {
            Type ReturnType;
            TaskToPanel.TryGetValue(Item.GetType(), out ReturnType);
#if DEBUG
            if (ReturnType.BaseType != typeof(QuestTaskPanel)) {
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
            ThisItem.listenString = MainForm.GetTextFromTextBox(sender);
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
            ReturnValue.ThisTable.AddItem("Listen String", new DefaultTextBox(Item.listenString), ReturnValue.ModifyTalkToListenString);
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

    public class QuestDialoguePanel : SortablePanel<QuestDialoguePanel, QuestDialogue> {
        public DefaultTable ThisTable;
        public DefaultTable NameTable;
        public DefaultTextBox NameBox;

        public ListPanel<QuestDialogueOptionsPanel, QuestDialogueOption> ThisOptionsList;

        public bool Trash() {
            Interpreter.SelectedQuest.injections.Remove(ThisQuestVariable);
            return true;
        }

        public void ModifyDialogueName(object sender, EventArgs e) {
            ThisQuestVariable.ThisEditorExternal.ChangeName (MainForm.GetTextFromTextBox(sender));
        }

        public QuestDialoguePanel() { GenerateFunction = Generate_2; }

        public QuestDialoguePanel(OrganizedControlList<QuestDialoguePanel, QuestDialogue> Parent) : base(Parent) {
            TrashFunction = Trash;
            FinishUpFunction = FinishUpFunction_2;
        }

        public QuestDialogueOption NewQuestDialogueOption() {
            return QuestDialogueOption.Generate(Interpreter.SelectedQuest, ThisQuestVariable.options);
        }

        public static QuestDialoguePanel Generate_2(QuestDialogue Item, OrganizedControlList<QuestDialoguePanel, QuestDialogue> Parent) {
            QuestDialoguePanel ReturnValue = new QuestDialoguePanel(Parent);

            ReturnValue.ThisTable = new DefaultTable(1, 2);
            ReturnValue.NameBox = new DefaultTextBox(Item.ThisEditorExternal.DialogueName);
            ReturnValue.ThisOptionsList = new ListPanel<QuestDialogueOptionsPanel, QuestDialogueOption>("New Dialogue Option", ReturnValue.NewQuestDialogueOption, QuestDialogueOptionsPanel.Sort()) ;

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

            ThisQuestVariable.ThisOptionalFields.sendListenString = MainForm.GetTextFromTextBox(sender);
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
        public static QuestDialogueOptionsPanel Generate_2 (QuestDialogueOption Item, OrganizedControlList<QuestDialogueOptionsPanel, QuestDialogueOption> Parent)
        {
            QuestDialogueOptionsPanel ReturnValue = new QuestDialogueOptionsPanel(Parent);

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

        public void FinishUp_2 ()
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

        public QuestStageDialoguePanel()
        {
            GenerateFunction = Generate_2;
        }

        public QuestStageDialoguePanel(OrganizedControlList<QuestStageDialoguePanel, KVPair> Parent) : base(Parent)
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

        public static QuestStageDialoguePanel Generate_2 (KVPair Item, OrganizedControlList<QuestStageDialoguePanel, KVPair> Parent) {
            QuestStageDialoguePanel ReturnValue = new QuestStageDialoguePanel(Parent);
            ReturnValue.ThisTable = new ModifyQuestVariableTable();
            ReturnValue.ThisTable.AddItem("NPC: ", new DefaultTextBox(Item.Key), ReturnValue.ModifyNPCName);
            ReturnValue.ThisTable.AddItem("Dialogue: ", new DefaultDropDown(Item.Value, Interpreter.SelectedQuest.ThisEditorExternal.PossibleDialogueInjections, false), ReturnValue.ModifyDialogue);
            //ReturnValue.ThisTable.AddItem("Text Box: ", new DefaultDropDown(Item.Key, , true), ReturnValue.ModifyNPCName);
            ReturnValue.AddControl(ReturnValue.ThisTable);
            return ReturnValue;
        }

        public override IComparer<QuestStageDialoguePanel> SortComparer()
        {
            return new SortablePanel_IComparer();
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
        public DefaultDropDown(string Text, UniqueList<string> Options, bool IsSearchBox = false)
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

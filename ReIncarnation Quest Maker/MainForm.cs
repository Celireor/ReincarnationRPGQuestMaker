using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ReIncarnation_Quest_Maker.Made_in_Abyss.QuestFormat;
using ReIncarnation_Quest_Maker.Made_in_Abyss.Parser;
using ReIncarnation_Quest_Maker.Made_in_Abyss.Utility;
using ReIncarnation_Quest_Maker.Obsidius;

namespace ReIncarnation_Quest_Maker
{
    public partial class MainForm : Form
    {
        public OrganizedControlList<QuestButton, Quest> QuestButtons;
        public OrganizedControlList<QuestStagePanel, QuestStage> QuestStagePanels;
        public OrganizedControlList<PrerequisitePanel, KVPair> PrerequisitePanels;
        public OrganizedControlList<QuestTaskPanel, QuestTask> QuestTaskPanels;
        //public OrganizedControlList<AffectedNPCsPanel, QuestInjectDialogue> AffectedNPCsPanels;
        //public 

        public MainForm()
        {
            InitializeComponent();
            QuestButtons = OrganizedControlList<QuestButton, Quest>.GenerateOrganizedControlList(QuestList, QuestButton.Sort());
            QuestStagePanels = OrganizedControlList<QuestStagePanel, QuestStage>.GenerateOrganizedControlList(QuestStageList, QuestStagePanel.Sort());
            PrerequisitePanels = OrganizedControlList<PrerequisitePanel, KVPair>.GenerateOrganizedControlList(PrerequisiteList, PrerequisitePanel.Sort());
            QuestTaskPanels = OrganizedControlList<QuestTaskPanel, QuestTask>.GenerateOrganizedControlList(QuestStageTaskList, QuestTaskPanel.Sort());

            //AffectedNPCsPanels = OrganizedControlList<AffectedNPCsPanel, QuestInjectDialogue>.GenerateOrganizedControlList(AffectedNPCsList, AffectedNPCsPanel.Sort());

            Interpreter.ThisForm = this;
            Interpreter.GenerateDefaultQuestList();
            InitialiseScreen();

        }

        public void InitialiseScreen() {
        }

        public void LoadValues()
        {
            QuestTypeIcon.UpdatePossibleValues(Interpreter.CurrentQuestList.ThisEditorExternal.PossibleTypeIcons);
            QuestStageTaskType.UpdatePossibleValues(QuestTask.PossibleTaskTypes);
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
            //

            UpdateStageData();
        }

        public void UpdateStageData()
        {
            QuestStageDescriptionBox.Text = Interpreter.SelectedQuestStage.description;
            QuestTaskPanels.Refresh(Interpreter.SelectedQuestStage.tasks);
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
    }

    public class OrganizedControlList <T, U> where T : SortablePanel <T, U>, new() {
        public U ThisQuestVariable;

        public List<T> ThisList = new List<T>();

        public List<U> RefreshList;

        public Panel BaseControl;

        public IComparer<T> OrderMetric;

        public int NextPos = 0;

        //public Func

        public static OrganizedControlList <T, U> GenerateOrganizedControlList (Panel BaseControl, IComparer<T> OrderMetric) {
            OrganizedControlList<T, U> ReturnValue = new OrganizedControlList<T, U> ();
            ReturnValue.OrderMetric = OrderMetric;
            ReturnValue.BaseControl = BaseControl;
            return ReturnValue;
        }

        public void Add(T Item) {
            ThisList.Add(Item);
            BaseControl.Controls.Add(Item);
            PositionNewItem(Item);
        }

        public void Remove(T Item) {
            ThisList.Remove(Item);
            BaseControl.Controls.Remove(Item);
            SortControls();
        }

        public void PositionNewItem (T Item)
        {
            Item.UpdateLocation(NextPos - BaseControl.VerticalScroll.Value);
            NextPos += Item.Height + 3;
        }

        public void SortControls() {
            ThisList.Sort(OrderMetric);
            NextPos = 0;
            ThisList.ForEach(obj =>
            {
                PositionNewItem(obj);
            });
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
        TableLayoutPanel ContentsTable;
        public Panel ContentsPanel;

        Button TrashButton;
        Button UpButton;
        Button DownButton;
        TableLayoutPanel UpDownButtonTable;
        public OrganizedControlList<T, U> ParentControlList;

        public int ListPosition;
        public Action<T, int> Move_Addon;
        public Func<bool> TrashFunction;

        public U ThisQuestVariable;

        public Func<U, OrganizedControlList<T, U>, T> GenerateFunction;

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

        public SortablePanel (OrganizedControlList<T, U> Parent) {

            ContentsTable = new DefaultTable(3, 1);
            ContentsPanel = new Panel();
            TrashButton = new DefaultButton("T");
            UpDownButtonTable = new DefaultTable(1, 2);
            UpButton = new DefaultButton("^");
            DownButton = new DefaultButton("V");

            TabIndex = 5;

            ParentControlList = Parent;

            int ExtraVerticalScrollSpace = 0;
            if (Parent.BaseControl.VerticalScroll.Visible)
            {
                ExtraVerticalScrollSpace = SystemInformation.VerticalScrollBarWidth;
            }
            MinimumSize = new System.Drawing.Size(Parent.BaseControl.Size.Width - 6 - ExtraVerticalScrollSpace, 50);
            MaximumSize = new System.Drawing.Size(Parent.BaseControl.Size.Width - 6 - ExtraVerticalScrollSpace, Screen.PrimaryScreen.Bounds.Height);

            BackColor = SystemColors.ControlDarkDark;

            ContentsPanel.AutoSize = true;
            ContentsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ContentsPanel.Dock = DockStyle.Fill;
            ContentsPanel.Margin = new Padding(0, 0, 0, 0);

            ContentsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            ContentsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ContentsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            ContentsTable.Margin = new Padding(0, 0, 0, 0);
            ContentsTable.TabIndex = 5;

            TrashButton.Click += Trash;

            UpDownButtonTable.Margin = new Padding(0, 0, 0, 0);

            UpButton.Click += MoveUp;
            DownButton.Click += MoveDown;

            ContentsTable.Controls.Add(ContentsPanel, 1, 0);

            ContentsTable.Controls.Add(TrashButton, 2, 0);

            UpDownButtonTable.Controls.Add(UpButton, 0, 0);
            UpDownButtonTable.Controls.Add(DownButton, 0, 1);

            ContentsTable.Controls.Add(UpDownButtonTable, 0, 0);

            Controls.Add(ContentsTable);
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

        public static T Generate(U Item, OrganizedControlList<T, U> Parent) {
            T ReturnValue = new T().GenerateFunction(Item, Parent);
            ReturnValue.ThisQuestVariable = Item;
            ReturnValue.FinishUp();
            return ReturnValue;
        }

        public void AddControl(Control Item) {
            ContentsPanel.Controls.Add(Item);
        }

        public void FinishUp()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            Dock = DockStyle.Fill;
            Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;


            ListPosition = ParentControlList.ThisList.Count;
            ParentControlList.Add((T)this);
        }

        public abstract IComparer<T> SortComparer();

        public class SortableButton_IComparer : IComparer<T>
        {
            public int Compare(T a, T b) //Compare TA to TB kappa ta loses
            {
                return a.ListPosition.CompareTo(b.ListPosition);
            }
        }
    }

    public abstract class ModifyQuestVariablePanel<T, U> : SortablePanel<T, U> where T : SortablePanel<T, U>, new()
    {
        DefaultTable ThisTable;

        public ModifyQuestVariablePanel() { }

        public ModifyQuestVariablePanel(OrganizedControlList<T, U> Parent) : base(Parent)
        {
            ThisTable = new DefaultTable(2, 1);
            AddControl(ThisTable);
        }

        public void AddItem(string VarString, Control VarControl, EventHandler OnValueChanged)
        {
            ThisTable.Controls.Add(new DefaultLabel(VarString), 0, ThisTable.RowCount - 1);
            ThisTable.Controls.Add(VarControl, 1, ThisTable.RowCount - 1);

            Type VarControlType = VarControl.GetType();
            if (VarControlType == typeof(DefaultTextBox))
            {
                TextBox temp = VarControl as TextBox;
                temp.TextChanged += OnValueChanged;
            }
            else if (VarControlType == typeof(ComboBox))
            {

                ComboBox temp = VarControl as ComboBox;
                temp.TextChanged += OnValueChanged;
            }
            else if (VarControlType == typeof(DefaultNumericUpDown))
            {

                DefaultNumericUpDown temp = VarControl as DefaultNumericUpDown;
                temp.ValueChanged += OnValueChanged;
            }

            ThisTable.RowCount++;
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

    public class PrerequisitePanel : ModifyQuestVariablePanel<PrerequisitePanel, KVPair>
    {

        public void moveAddon (PrerequisitePanel other, int OtherPos)
        {
            Interpreter.SelectedQuest.prerequisites.Swap(ListPosition, OtherPos);
        } 

        public void UpdateType(object sender, EventArgs e)
        {
            ThisQuestVariable.Key = MainForm.GetTextFromTextBox(sender);
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

            ReturnValue.Move_Addon = ReturnValue.moveAddon;
            ReturnValue.TrashFunction = ReturnValue.Trash;

            ReturnValue.AddItem("Type: ", new DefaultTextBox(Item.Key), ReturnValue.UpdateType);
            ReturnValue.AddItem("Value: ", new DefaultTextBox(Item.Value), ReturnValue.UpdateValue);

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

    public class QuestTaskPanel : ModifyQuestVariablePanel<QuestTaskPanel, QuestTask>
    {
        public static Dictionary<Type, Type> TaskToPanel = new Dictionary<Type, Type>();

        public QuestTaskPanel() { GenerateFunction = Generate_2;}

        public QuestTaskPanel(OrganizedControlList<QuestTaskPanel, QuestTask> Parent) : base(Parent)
        {
            Move_Addon = moveAddon;
        }

        static QuestTaskPanel()
        {
            TaskToPanel.Add(typeof(QuestTask_location), typeof(QuestTaskPanel_location));
            TaskToPanel.Add(typeof(QuestTask_talkto), typeof(QuestTaskPanel_talkto));
            TaskToPanel.Add(typeof(QuestTask_kill), typeof(QuestTaskPanel_kill));
            TaskToPanel.Add(typeof(QuestTask_gather), typeof(QuestTaskPanel_gather));
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
            return new SortableButton_IComparer();
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

            ReturnValue.AddItem("Name", new DefaultTextBox(Item.name), ReturnValue.ModifyLocationName);
            ReturnValue.AddItem("Location Name", new DefaultTextBox(Item.locationString), ReturnValue.ModifyLocationInternalName);
            ReturnValue.AddItem("Radius", new DefaultNumericUpDown(Item.radius), ReturnValue.ModifyLocationRadius);

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

            ReturnValue.AddItem("Description", new DefaultTextBox(Item.description), ReturnValue.ModifyTalkToDescription);
            ReturnValue.AddItem("Listen String", new DefaultTextBox(Item.listenString), ReturnValue.ModifyTalkToListenString);
            ReturnValue.AddItem("Completion String", new DefaultTextBox(Item.completionString), ReturnValue.ModifyTalkToCompletionString);

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

            ReturnValue.AddItem("Target Name", new DefaultTextBox(Item.ThisEditorExternal.TargetName), ReturnValue.ModifyKillTarget);
            ReturnValue.AddItem("Number To Kill", new DefaultNumericUpDown(Item.amount), ReturnValue.ModifyKillAmount);

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

            ReturnValue.AddItem("Item Name", new DefaultTextBox(Item.ThisEditorExternal.ItemName), ReturnValue.ModifyGatherObject);
            ReturnValue.AddItem("Number To Gather", new DefaultNumericUpDown(Item.required), ReturnValue.ModifyGatherAmount);

            return ReturnValue;
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
            Name = "PrerequisiteValueBox";
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

    public class DefaultButton : Button {
        public DefaultButton(string Text = "") {

            this.Size = new System.Drawing.Size(25, 25);
            this.Location = new System.Drawing.Point(0, 0);
            this.Text = Text;
            this.UseVisualStyleBackColor = true;
            this.Margin = new Padding(0, 0, 0, 0);
        }
    }

    public class DefaultDropDown : ComboBox
    {
        public DefaultDropDown()
        {
            Dock = System.Windows.Forms.DockStyle.Fill;
            DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            FormattingEnabled = true;
            Location = new System.Drawing.Point(130, 78);
            Size = new System.Drawing.Size(260, 21);
            TabIndex = 20;
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

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
    public class OrganizedControlList<T, U> where T : SortablePanel<T, U>, new()
    {
        public U ThisQuestVariable;

        public List<T> ThisList = new List<T>();

        public Panel BaseControl;

        public IComparer<T> OrderMetric;

        public int NextPos = 0;

        //public Func

        public static OrganizedControlList<T, U> GenerateOrganizedControlList(Panel BaseControl, IComparer<T> OrderMetric)
        {
            OrganizedControlList<T, U> ReturnValue = new OrganizedControlList<T, U>();
            ReturnValue.OrderMetric = OrderMetric;
            ReturnValue.BaseControl = BaseControl;
            ReturnValue.BaseControl.SizeChanged += ReturnValue.ResetAllItemSizes;
            return ReturnValue;
        }

        public void ResetAllItemSizes(object sender, EventArgs e)
        {
            ThisList.ForEach(obj => obj.ResetSize(this));
        }

        public void Add(T Item)
        {
            ThisList.Add(Item);
            PositionNewItem(Item);
            BaseControl.Controls.Add(Item);
            //RepositionItems();
        }

        public void Remove(T Item)
        {
            ThisList.Remove(Item);
            BaseControl.Controls.Remove(Item);
            SortControls();
        }

        public void RepositionItems()
        {

            NextPos = 0;
            ThisList.ForEach(obj =>
            {
                PositionNewItem(obj);
            });
        }

        public void PositionNewItem(T Item)
        {
            Item.UpdateLocation(NextPos - BaseControl.VerticalScroll.Value);
            NextPos += Item.Height + 3;
        }

        public void SortControls()
        {
            ThisList.Sort(OrderMetric);
            RepositionItems();
        }

        public void Clear()
        {
            ThisList.ForEach(obj =>
            {
                BaseControl.Controls.Remove(obj);
            });
            ThisList.Clear();
            NextPos = 0;
        }

        public void Refresh(List<U> RefreshList)
        {
            Clear();
            SortablePanel<T, U>.MassGenerate(RefreshList.ToArray(), this);
        }
    }


    public abstract class SortablePanel<T, U> : Panel where T : SortablePanel<T, U>, new()
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

        public static IComparer<T> Sort()
        {
            return new T().SortComparer();
        }

        public void Trash(object sender, EventArgs e)
        {
            bool CanTrash = true;
            if (TrashFunction != null)
            {
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

        public void MoveUp(object sender, EventArgs e)
        {
            MovePos(-1);
        }

        public void MoveDown(object sender, EventArgs e)
        {
            MovePos(1);
        }

        public void MovePos(int Displacement)
        {
            if (ListPosition + Displacement < 0 || ListPosition + Displacement >= ParentControlList.ThisList.Count)
            {
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

        public SortablePanel()
        {
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

        public void OnSizeChanged(object sender, EventArgs e)
        {
            ParentControlList.RepositionItems();
        }

        public SortablePanel(OrganizedControlList<T, U> Parent)
        {

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

        public void ResetSize(object sender, EventArgs e)
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
            if (typeof(TextBox).IsAssignableFrom(VarControlType))
            {
                TextBox temp = VarControl as TextBox;
                temp.TextChanged += OnValueChanged;
            }
            else if (typeof(ComboBox).IsAssignableFrom(VarControlType))
            {

                ComboBox temp = VarControl as ComboBox;
                temp.TextChanged += OnValueChanged;
            }
            else if (typeof(NumericUpDown).IsAssignableFrom(VarControlType))
            {

                NumericUpDown temp = VarControl as NumericUpDown;
                temp.ValueChanged += OnValueChanged;
                temp.KeyUp += new KeyEventHandler((obj, e) => OnValueChanged(obj, null));
            }
            else if (typeof(CheckBox).IsAssignableFrom(VarControlType))
            {

                CheckBox temp = VarControl as CheckBox;
                temp.CheckedChanged += OnValueChanged;
            }

            RowCount++;
        }
    }

    /*public class KVPanel<T> : SortablePanel<T, KVPair> where T : KVPanel<T>, new()
    {
        public ModifyQuestVariableTable ThisTable;

        string KeyString;
        string ValueString;

        public KVPanel()
        {

        }

        public KVPanel(OrganizedControlList<T, KVPair> Parent) : base(Parent)
        {
        }


        public static T Generate_2(KVPair Item, OrganizedControlList<T, KVPair> Parent)
        {
            T KVPanelValues_temp = new T();
            KVPanel<T> ReturnValue = new KVPanel<T>(Parent);

            ReturnValue.ThisTable = new ModifyQuestVariableTable();
            ReturnValue.ThisTable.AddItem(KVPanelValues_temp.KeyString, new DefaultTextBox(Item.Key), ReturnValue.ModifyNPCName);
            ReturnValue.ThisTable.AddItem("Dialogue: ", new DefaultDropDown(Item.Value, Interpreter.SelectedQuest.ThisEditorExternal.PossibleDialogueInjections, false), ReturnValue.ModifyDialogue);
            //ReturnValue.ThisTable.AddItem("Text Box: ", new DefaultDropDown(Item.Key, , true), ReturnValue.ModifyNPCName);
            ReturnValue.AddControl(ReturnValue.ThisTable);
            return (T) ReturnValue;
        }

        public override IComparer<T> SortComparer()
        {
            throw new NotImplementedException();
        }
    }*/
}

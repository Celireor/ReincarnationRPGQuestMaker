﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat;
using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Parser;
using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Utility;
using ReIncarnation_Quest_Maker.Obsidius;

namespace ReIncarnation_Quest_Maker
{
    class DrawingControl
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }
    }

    public class OrganizedControlList<T, U> where T : SortablePanel<T, U>
    {
        public U ThisQuestVariable;

        public ListeningList<T> ThisList = new ListeningList<T>();

        public Panel BaseControl;

        public IComparer<T> OrderMetric;

        public int NextPos = 0;

        public List<Action> OnTrash = new List<Action>();
        public List<Action> OnRefresh = new List<Action>();

        //public Func

        public static OrganizedControlList<T, U> GenerateOrganizedControlList(Panel BaseControl)
        {
            OrganizedControlList<T, U> ReturnValue = new OrganizedControlList<T, U>();
            ReturnValue.OrderMetric = SortablePanel<T, U>.Sort();
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
            DefaultTable.StartInit();
            ThisList.Add(Item);
            PositionNewItem(Item);
            BaseControl.Controls.Add(Item);
            //RepositionItems();
            DefaultTable.EndInit();
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
            int itemCount = 0;
            ThisList.ForEach(obj =>
            {
                PositionNewItem(obj);
                obj.ListPosition = itemCount;
                itemCount++;
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
            BaseControl.Controls.Clear();
            ThisList.ForEach(obj => obj.Dispose());
            ThisList.Clear();
            NextPos = 0;
        }

        public void Refresh(List<U> RefreshList)
        {
            //new RefreshThread(RefreshList, this).Refresh();
            //Thread RefreshThread = new Thread()
            DefaultTable.StartInit();
            Clear();
            BaseControl.Controls.AddRange(SortablePanel<T, U>.MassGenerate(RefreshList.ToArray(), this));
             DefaultTable.EndInit();
            OnRefresh.ForEach(obj => obj());
        }

        /*class RefreshThread {
            public List<U> RefreshList;
            public OrganizedControlList<T, U> Parent;
            public RefreshThread(List<U> RefreshList, OrganizedControlList<T, U> Parent) { this.RefreshList = RefreshList; this.Parent = Parent; }

            public void Refresh() {

                Thread RefreshThread = new Thread(new ThreadStart(ThisRefreshThread));
                RefreshThread.Start();
            }

            void ThisRefreshThread()
            {
                DefaultTable.StartInit();
                Parent.Clear();
                Parent.BaseControl.Controls.AddRange(SortablePanel<T, U>.MassGenerate(RefreshList.ToArray(), Parent));
                DefaultTable.EndInit();
                Parent.OnRefresh.ForEach(obj => obj());
            }
        }*/
    }


    public abstract class SortablePanel<T, U> : Panel where T : SortablePanel<T, U>
    {
        DefaultTable ThisTable;
        public Panel DownContentsPanel;

        DefaultTable ContentsTable;
        public Panel ContentsPanel;

        DefaultImagebutton TrashButton;

        DefaultImagebutton UpButton;
        DefaultImagebutton DownButton;
        DefaultPanel UpDownButtonPanel;

        public OrganizedControlList<T, U> ParentControlList;

        public int ListPosition;

        public U ThisQuestVariable;

        public Action FinishUpFunction;

        public static IComparer<T> Sort()
        {
            return new IComparer_Finder();
        }

        public void Trash(object sender, EventArgs e)
        {
            bool CanTrash = Trash_Addon();
            if (CanTrash)
            {
                ParentControlList.Remove((T)this);
                ParentControlList.ThisList.AssignIndexValuesToListItems((obj, index) => {
                    obj.ListPosition = index;
                });
                ParentControlList.OnTrash.ForEach(obj => obj());
            }
        }

        public virtual bool Trash_Addon() { return true; }

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
            int OtherPos = ListPosition + Displacement;
            if (OtherPos < 0 || OtherPos >= ParentControlList.ThisList.Count)
            {
                return;
            }
            T OtherItem = ParentControlList.ThisList.Swap(ListPosition, OtherPos);
            Move_Addon(OtherItem, OtherPos);
            ListPosition += Displacement;
            OtherItem.ListPosition -= Displacement;
            ParentControlList.SortControls();
        }

        public virtual void Move_Addon(T OtherItem, int OtherPos) { }

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
            SuspendLayout();

            ThisTable = new DefaultTable(1, 2);
            DownContentsPanel = new Panel();

            ContentsTable = new DefaultTable(3, 1);
            ContentsPanel = new Panel();
            TrashButton = new DefaultImagebutton(Trash, new Bitmap(Properties.Resources.Trash));
            UpDownButtonPanel = new DefaultPanel();
            UpButton = new DefaultImagebutton(MoveUp, new Bitmap(ReIncarnation_Quest_Maker.Properties.Resources.UpArrow));
            DownButton = new DefaultImagebutton(MoveDown, new Bitmap(ReIncarnation_Quest_Maker.Properties.Resources.DownArrow));

            ContentsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            ContentsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ContentsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));

            TabIndex = 5;

            ParentControlList = Parent;

            BackColor = SystemColors.ControlDarkDark;
            ContentsPanel.Margin = new Padding(0, 0, 0, 0);

            ContentsPanel.Dock = DockStyle.Fill;

            DownContentsPanel.Dock = DockStyle.Fill;
            DownContentsPanel.Margin = new Padding(0, 0, 0, 0);

            ContentsTable.Margin = new Padding(0, 0, 0, 0);
            ContentsTable.TabIndex = 5;

            UpDownButtonPanel.Margin = new Padding(0, 0, 0, 0);
            UpDownButtonPanel.BackColor = SystemColors.ControlDarkDark;

            UpDownButtonPanel.Controls.Add(UpButton);
            UpDownButtonPanel.Controls.Add(DownButton);

            DownButton.Location = new Point(0, UpButton.Height);

            UpDownButtonPanel.Size = new Size(UpButton.Width, UpButton.Height * 2);

            ContentsTable.Controls.Add(UpDownButtonPanel, 0, 0);
            ContentsTable.Controls.Add(ContentsPanel, 1, 0);
            ContentsTable.Controls.Add(TrashButton, 2, 0);
            //ContentsTable.Dock = DockStyle.Fill;

            ThisTable.Controls.Add(ContentsTable, 0, 0);
            ThisTable.Controls.Add(DownContentsPanel, 0, 1);

            Controls.Add(ThisTable);
        }

        public static T[] MassGenerate(U[] Item, OrganizedControlList<T, U> Parent)
        {
            List<T> ReturnValue = new List<T>();
            for (int x = 0; x < Item.Length; x++)
            {
                T NewItem = Generate(Item[x], Parent, false);
                ReturnValue.Add(NewItem);
                Parent.ThisList.Add(NewItem);
                Parent.PositionNewItem(NewItem);
            }
            return ReturnValue.ToArray();
        }

        public static T Generate(U Item, OrganizedControlList<T, U> Parent, bool ShouldAddToParentControlList = true)
        {
            //fix
            T ReturnValue = (T)Activator.CreateInstance(typeof(T), Parent);
            ReturnValue = ReturnValue.CreateInstanceAddon(Item, Parent);
            ReturnValue.Generate_Addon(Item, Parent);
            ReturnValue.ThisQuestVariable = Item;
            ReturnValue.FinishUp(ShouldAddToParentControlList);
            ReturnValue.ResumeLayout();
            return ReturnValue;
        }

        public virtual T CreateInstanceAddon(U Item, OrganizedControlList<T, U> Parent) {
            return (T)this;
        }

        public abstract void Generate_Addon(U Item, OrganizedControlList<T, U> Parent);

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

        public void FinishUp(bool ShouldAddToParentControlList)
        {
            Dock = DockStyle.Fill;
            Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            ListPosition = ParentControlList.ThisList.Count;

            FinishUpFunction?.Invoke();

            ContentsPanel.AutoSize = true;
            ContentsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            DownContentsPanel.AutoSize = true;
            DownContentsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            ResetSize(ParentControlList);

            SizeChanged += OnSizeChanged;

            if (ShouldAddToParentControlList) { ParentControlList.Add((T)this); }
        }

        public virtual IComparer<T> SortComparer() {
            return new SortablePanel_IComparer();
        }

        public class SortablePanel_IComparer : IComparer<T>
        {
            public int Compare(T a, T b) //Compare TA to TB kappa ta loses
            {
                return a.ListPosition.CompareTo(b.ListPosition);
            }
        }

        public class IComparer_Finder : IComparer<T>
        {
            public int Compare(T a, T b) //Compare TA to TB kappa ta loses
            {
                return a.SortComparer().Compare(a, b);
            }
        }
    }

    public class ListPanel<T, U> : Panel where T : SortablePanel<T, U>
    {
        public DefaultTable ThisTable;
        public DefaultPanel ThisPanel;
        public DefaultButton ThisButton;
        public OrganizedControlList<T, U> ThisList;

        public DefaultButton MinimiseButton;
        bool MinimisedState = true;
        bool HasMinimisedButton = false;

        public DefaultDropDown ThisDefaultQuestTypeSelector;

        public Func<string, U> NewDefaultItem;

        public void OnClick(object sender, EventArgs e)
        {
            string InputText = "";
            try
            {
                InputText = ThisDefaultQuestTypeSelector.Text;
            }
            catch (Exception)
            {

            }
            SortablePanel<T, U>.Generate(NewDefaultItem(InputText), ThisList);
            if (MinimisedState) {
                Minimise(null, null);
            }
            CheckIfShouldAddMinimiseButton();
        }

        public void CheckIfShouldAddMinimiseButton() {

            if (ThisList.ThisList.Count > 0)
            {
                if (!HasMinimisedButton)
                {
                    ThisTable.Controls.Add(MinimiseButton, 0, 0);
                    HasMinimisedButton = true;
                }
            }
            else
            {
                ThisTable.Controls.Remove(MinimiseButton);
                HasMinimisedButton = false;
            }
        }

        public void ResetSize(object sender, EventArgs e)
        {
            Size NewMaxSize = new System.Drawing.Size(Parent.Size.Width - 6, int.MaxValue);
            ThisTable.MaximumSize = NewMaxSize;
            NewMaxSize.Width -= 6;
            ThisPanel.MaximumSize = NewMaxSize;
            ThisButton.MaximumSize = NewMaxSize;
        }

        public void Minimise(object sender, EventArgs e)
        {
            if (MinimisedState) {
                MinimisedState = false;
                ThisTable.Controls.Add(ThisPanel, 1, 0);
                return;
            }
            MinimisedState = true;
            ThisTable.Controls.Remove(ThisPanel);
        }

        protected override void Dispose(bool disposing)
        {
            ThisList.OnTrash.Remove(CheckIfShouldAddMinimiseButton);
            ThisList.OnRefresh.Remove(CheckIfShouldAddMinimiseButton);
            base.Dispose(disposing);
        }

        public ListPanel(string ThisButtonText, Func<string, U> NewDefaultItem, DefaultDropDown ElementTypeSelector = null)
        {
            MinimiseButton = new DefaultButton(Minimise, "Minimise/Maxmise");
            MinimiseButton.Location = new Point(0, MinimiseButton.Height);

            ThisTable = new DefaultTable(1, 3);
            ThisTable.Margin = new Padding(0, 0, 0, 0);
            ThisTable.Padding = new Padding(0, 0, 0, 0);

            this.NewDefaultItem = NewDefaultItem;
            ThisPanel = new DefaultPanel();
            ThisButton = new DefaultButton(OnClick, ThisButtonText);
            ThisList = OrganizedControlList<T, U>.GenerateOrganizedControlList(ThisPanel);

            ThisPanel.Dock = DockStyle.Top;
            ThisButton.Dock = System.Windows.Forms.DockStyle.Top;
            ThisTable.Dock = DockStyle.Top;

            DefaultTable ButtonContainerTable = new DefaultTable(2, 1);

            ButtonContainerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            ButtonContainerTable.Controls.Add(ThisButton, 0, 0);
            if (ElementTypeSelector != null)
            {
                ButtonContainerTable.Controls.Add(ElementTypeSelector, 1, 0);
                ThisDefaultQuestTypeSelector = ElementTypeSelector;
                ButtonContainerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            }

            ButtonContainerTable.Dock = DockStyle.Fill;
            CheckIfShouldAddMinimiseButton();
            ThisTable.Controls.Add(ButtonContainerTable, 0, 2);
            Controls.Add(ThisTable);

            ThisPanel.AutoSize = true;

            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            SizeChanged += ResetSize;

            ThisList.OnTrash.Add(CheckIfShouldAddMinimiseButton);
            ThisList.OnRefresh.Add(CheckIfShouldAddMinimiseButton);
        }
    }

    /*public class ModifyQuestVariableTable : DefaultPanel
    {
        public DefaultPanel LabelPanel;
        public DefaultPanel ObjectPanel;

        public ModifyQuestVariableTable()// : base(2, 1)
        {
            Dock = DockStyle.Fill;
            LabelPanel = new DefaultPanel();
            ObjectPanel = new DefaultPanel();
            LabelPanel.Dock = DockStyle.Left;
            //LabelPanel.Anchor = AnchorStyles.Left;
            LabelPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            LabelPanel.AutoSize = true;
            LabelPanel.BackColor = SystemColors.ControlDarkDark;
            ObjectPanel.Dock = DockStyle.Left;
            //ObjectPanel.Anchor = AnchorStyles.Left;
            ObjectPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ObjectPanel.AutoSize = true;
            ObjectPanel.BackColor = SystemColors.ControlLightLight;
            Controls.Add(LabelPanel);
            Controls.Add(ObjectPanel);
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoSize = true;
        }

        public void AddItem(string VarString, Control VarControl, EventHandler OnValueChanged)
        {
            //VarControl.Location = new Point(0, RowCount * 33);
            VarControl.Dock = DockStyle.Top;

            DefaultLabel LabelControl = new DefaultLabel(VarString);
           // LabelControl.Location = new Point(0, RowCount * 33);
            LabelControl.Dock = DockStyle.Top;

            LabelPanel.Controls.Add(LabelControl);
            ObjectPanel.Controls.Add(VarControl);

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
        }
    }*/
    
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

    public abstract class MultiTypePanel<T, U> : SortablePanel<T, U> where T : SortablePanel<T, U>
    {
        public static Dictionary<Type, Type> ItemToPanel = new Dictionary<Type, Type>();

        public MultiTypePanel(OrganizedControlList<T, U> Parent) : base(Parent)
        {
        }

        public override T CreateInstanceAddon(U Item, OrganizedControlList<T, U> Parent)
        {
            Type ReturnType;
            ItemToPanel.TryGetValue(Item.GetType(), out ReturnType);
#if DEBUG
            if (ReturnType.BaseType != typeof(T))
            {
                throw new ArgumentNullException("put returntype in wtf");
            }
#endif
            T ReturnValue = (T)Activator.CreateInstance(ReturnType, Parent);

            return ReturnValue;
        }
    }

    public abstract class KVPanel<T> : SortablePanel<T, KVPair> where T : KVPanel<T>
    {
        public ModifyQuestVariableTable ThisTable = new ModifyQuestVariableTable();

        public KVPanel(OrganizedControlList<T, KVPair> Parent) : base(Parent) {
            AddControl(ThisTable);
        }

        public override bool Trash_Addon()
        {
            ThisQuestVariable.Trash();
            return true;
        }
    }
}

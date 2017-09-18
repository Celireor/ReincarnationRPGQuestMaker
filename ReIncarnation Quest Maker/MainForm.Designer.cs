namespace ReIncarnation_Quest_Maker
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromReincQuestFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CompileButton = new System.Windows.Forms.ToolStripButton();
            this.Quest_QuestList_Split = new System.Windows.Forms.SplitContainer();
            this.QuestTabs = new System.Windows.Forms.TabControl();
            this.QuestBasicInfoPage = new System.Windows.Forms.TabPage();
            this.QuestBasicInfo_QuestPrerequisites_Split = new System.Windows.Forms.SplitContainer();
            this.QuestBasicInfoTable = new System.Windows.Forms.TableLayoutPanel();
            this.QuestIDLabel = new System.Windows.Forms.Label();
            this.QuestNameLabel = new System.Windows.Forms.Label();
            this.InternalNameLabel = new System.Windows.Forms.Label();
            this.QuestPortraitLabel = new System.Windows.Forms.Label();
            this.QuestTypeIconLabel = new System.Windows.Forms.Label();
            this.QuestCantAbandonLabel = new System.Windows.Forms.Label();
            this.QuestRepeatableLabel = new System.Windows.Forms.Label();
            this.QuestDescriptionLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.QuestID = new System.Windows.Forms.NumericUpDown();
            this.UpdateIDButton = new System.Windows.Forms.Button();
            this.QuestName = new System.Windows.Forms.TextBox();
            this.QuestInternalName = new System.Windows.Forms.TextBox();
            this.QuestPortrait = new System.Windows.Forms.TextBox();
            this.QuestTypeIcon = new System.Windows.Forms.ComboBox();
            this.QuestCantAbandon = new System.Windows.Forms.CheckBox();
            this.QuestRepeatable = new System.Windows.Forms.CheckBox();
            this.QuestDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.PrerequisiteList = new System.Windows.Forms.Panel();
            this.NewPrerequisiteButton = new System.Windows.Forms.Button();
            this.Stages = new System.Windows.Forms.TabPage();
            this.QuestStageInfo_QuestStageList_Split = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.QuestStageDescriptionPanel_table = new System.Windows.Forms.TableLayoutPanel();
            this.QuestStageDescriptionLabel = new System.Windows.Forms.Label();
            this.QuestStageDescriptionBox = new System.Windows.Forms.TextBox();
            this.QuestStageTabs = new System.Windows.Forms.TabControl();
            this.QuestStageTasks = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.QuestStageTaskType = new System.Windows.Forms.ComboBox();
            this.newqueststagetaskbutton = new System.Windows.Forms.Button();
            this.QuestStageTaskList = new System.Windows.Forms.Panel();
            this.QuestStageDialogue = new System.Windows.Forms.TabPage();
            this.QuestStageList = new System.Windows.Forms.Panel();
            this.newqueststagebutton = new System.Windows.Forms.Button();
            this.QuestList = new System.Windows.Forms.Panel();
            this.newquestbutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Quest_QuestList_Split)).BeginInit();
            this.Quest_QuestList_Split.Panel1.SuspendLayout();
            this.Quest_QuestList_Split.Panel2.SuspendLayout();
            this.Quest_QuestList_Split.SuspendLayout();
            this.QuestTabs.SuspendLayout();
            this.QuestBasicInfoPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QuestBasicInfo_QuestPrerequisites_Split)).BeginInit();
            this.QuestBasicInfo_QuestPrerequisites_Split.Panel1.SuspendLayout();
            this.QuestBasicInfo_QuestPrerequisites_Split.Panel2.SuspendLayout();
            this.QuestBasicInfo_QuestPrerequisites_Split.SuspendLayout();
            this.QuestBasicInfoTable.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QuestID)).BeginInit();
            this.Stages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QuestStageInfo_QuestStageList_Split)).BeginInit();
            this.QuestStageInfo_QuestStageList_Split.Panel1.SuspendLayout();
            this.QuestStageInfo_QuestStageList_Split.Panel2.SuspendLayout();
            this.QuestStageInfo_QuestStageList_Split.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.QuestStageDescriptionPanel_table.SuspendLayout();
            this.QuestStageTabs.SuspendLayout();
            this.QuestStageTasks.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.CompileButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(864, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(38, 22);
            this.toolStripLabel1.Text = "File";
            this.toolStripLabel1.ToolTipText = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewQuestList);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromTextToolStripMenuItem,
            this.fromReincQuestFileToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // fromTextToolStripMenuItem
            // 
            this.fromTextToolStripMenuItem.Name = "fromTextToolStripMenuItem";
            this.fromTextToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.fromTextToolStripMenuItem.Text = "From Text";
            this.fromTextToolStripMenuItem.Click += new System.EventHandler(this.fromTextToolStripMenuItem_Click);
            // 
            // fromReincQuestFileToolStripMenuItem
            // 
            this.fromReincQuestFileToolStripMenuItem.Name = "fromReincQuestFileToolStripMenuItem";
            this.fromReincQuestFileToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.fromReincQuestFileToolStripMenuItem.Text = "From .ReincQuest File";
            this.fromReincQuestFileToolStripMenuItem.Click += new System.EventHandler(this.fromReincQuestFileToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // CompileButton
            // 
            this.CompileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.CompileButton.Image = ((System.Drawing.Image)(resources.GetObject("CompileButton.Image")));
            this.CompileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CompileButton.Name = "CompileButton";
            this.CompileButton.Size = new System.Drawing.Size(56, 22);
            this.CompileButton.Text = "Compile";
            this.CompileButton.Click += new System.EventHandler(this.toolStripDropDownCompileButton_Click);
            // 
            // Quest_QuestList_Split
            // 
            this.Quest_QuestList_Split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Quest_QuestList_Split.Location = new System.Drawing.Point(0, 25);
            this.Quest_QuestList_Split.Name = "Quest_QuestList_Split";
            // 
            // Quest_QuestList_Split.Panel1
            // 
            this.Quest_QuestList_Split.Panel1.Controls.Add(this.QuestTabs);
            // 
            // Quest_QuestList_Split.Panel2
            // 
            this.Quest_QuestList_Split.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Quest_QuestList_Split.Panel2.Controls.Add(this.QuestList);
            this.Quest_QuestList_Split.Panel2.Controls.Add(this.newquestbutton);
            this.Quest_QuestList_Split.Size = new System.Drawing.Size(864, 536);
            this.Quest_QuestList_Split.SplitterDistance = 637;
            this.Quest_QuestList_Split.TabIndex = 1;
            // 
            // QuestTabs
            // 
            this.QuestTabs.Controls.Add(this.QuestBasicInfoPage);
            this.QuestTabs.Controls.Add(this.Stages);
            this.QuestTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestTabs.Location = new System.Drawing.Point(0, 0);
            this.QuestTabs.Name = "QuestTabs";
            this.QuestTabs.SelectedIndex = 0;
            this.QuestTabs.Size = new System.Drawing.Size(637, 536);
            this.QuestTabs.TabIndex = 3;
            // 
            // QuestBasicInfoPage
            // 
            this.QuestBasicInfoPage.Controls.Add(this.QuestBasicInfo_QuestPrerequisites_Split);
            this.QuestBasicInfoPage.Location = new System.Drawing.Point(4, 22);
            this.QuestBasicInfoPage.Name = "QuestBasicInfoPage";
            this.QuestBasicInfoPage.Padding = new System.Windows.Forms.Padding(3);
            this.QuestBasicInfoPage.Size = new System.Drawing.Size(629, 510);
            this.QuestBasicInfoPage.TabIndex = 0;
            this.QuestBasicInfoPage.Text = "Quest Information";
            this.QuestBasicInfoPage.UseVisualStyleBackColor = true;
            // 
            // QuestBasicInfo_QuestPrerequisites_Split
            // 
            this.QuestBasicInfo_QuestPrerequisites_Split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestBasicInfo_QuestPrerequisites_Split.Location = new System.Drawing.Point(3, 3);
            this.QuestBasicInfo_QuestPrerequisites_Split.Name = "QuestBasicInfo_QuestPrerequisites_Split";
            // 
            // QuestBasicInfo_QuestPrerequisites_Split.Panel1
            // 
            this.QuestBasicInfo_QuestPrerequisites_Split.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.QuestBasicInfo_QuestPrerequisites_Split.Panel1.Controls.Add(this.QuestBasicInfoTable);
            // 
            // QuestBasicInfo_QuestPrerequisites_Split.Panel2
            // 
            this.QuestBasicInfo_QuestPrerequisites_Split.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.QuestBasicInfo_QuestPrerequisites_Split.Panel2.Controls.Add(this.PrerequisiteList);
            this.QuestBasicInfo_QuestPrerequisites_Split.Panel2.Controls.Add(this.NewPrerequisiteButton);
            this.QuestBasicInfo_QuestPrerequisites_Split.Size = new System.Drawing.Size(623, 504);
            this.QuestBasicInfo_QuestPrerequisites_Split.SplitterDistance = 393;
            this.QuestBasicInfo_QuestPrerequisites_Split.TabIndex = 4;
            // 
            // QuestBasicInfoTable
            // 
            this.QuestBasicInfoTable.BackColor = System.Drawing.SystemColors.ControlDark;
            this.QuestBasicInfoTable.ColumnCount = 2;
            this.QuestBasicInfoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.QuestBasicInfoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.QuestBasicInfoTable.Controls.Add(this.QuestIDLabel, 0, 0);
            this.QuestBasicInfoTable.Controls.Add(this.QuestNameLabel, 0, 1);
            this.QuestBasicInfoTable.Controls.Add(this.InternalNameLabel, 0, 2);
            this.QuestBasicInfoTable.Controls.Add(this.QuestPortraitLabel, 0, 3);
            this.QuestBasicInfoTable.Controls.Add(this.QuestTypeIconLabel, 0, 4);
            this.QuestBasicInfoTable.Controls.Add(this.QuestCantAbandonLabel, 0, 5);
            this.QuestBasicInfoTable.Controls.Add(this.QuestRepeatableLabel, 0, 6);
            this.QuestBasicInfoTable.Controls.Add(this.QuestDescriptionLabel, 0, 7);
            this.QuestBasicInfoTable.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.QuestBasicInfoTable.Controls.Add(this.QuestName, 1, 1);
            this.QuestBasicInfoTable.Controls.Add(this.QuestInternalName, 1, 2);
            this.QuestBasicInfoTable.Controls.Add(this.QuestPortrait, 1, 3);
            this.QuestBasicInfoTable.Controls.Add(this.QuestTypeIcon, 1, 4);
            this.QuestBasicInfoTable.Controls.Add(this.QuestCantAbandon, 1, 5);
            this.QuestBasicInfoTable.Controls.Add(this.QuestRepeatable, 1, 6);
            this.QuestBasicInfoTable.Controls.Add(this.QuestDescriptionTextBox, 1, 7);
            this.QuestBasicInfoTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestBasicInfoTable.Location = new System.Drawing.Point(0, 0);
            this.QuestBasicInfoTable.Name = "QuestBasicInfoTable";
            this.QuestBasicInfoTable.RowCount = 8;
            this.QuestBasicInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.QuestBasicInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.QuestBasicInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.QuestBasicInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.QuestBasicInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.QuestBasicInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.QuestBasicInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.QuestBasicInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.QuestBasicInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.QuestBasicInfoTable.Size = new System.Drawing.Size(393, 504);
            this.QuestBasicInfoTable.TabIndex = 4;
            // 
            // QuestIDLabel
            // 
            this.QuestIDLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.QuestIDLabel.AutoSize = true;
            this.QuestIDLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuestIDLabel.Location = new System.Drawing.Point(3, 4);
            this.QuestIDLabel.Name = "QuestIDLabel";
            this.QuestIDLabel.Size = new System.Drawing.Size(67, 16);
            this.QuestIDLabel.TabIndex = 0;
            this.QuestIDLabel.Text = "Quest ID";
            // 
            // QuestNameLabel
            // 
            this.QuestNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.QuestNameLabel.AutoSize = true;
            this.QuestNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuestNameLabel.Location = new System.Drawing.Point(3, 29);
            this.QuestNameLabel.Name = "QuestNameLabel";
            this.QuestNameLabel.Size = new System.Drawing.Size(49, 16);
            this.QuestNameLabel.TabIndex = 2;
            this.QuestNameLabel.Text = "Name";
            // 
            // InternalNameLabel
            // 
            this.InternalNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.InternalNameLabel.AutoSize = true;
            this.InternalNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InternalNameLabel.Location = new System.Drawing.Point(3, 54);
            this.InternalNameLabel.Name = "InternalNameLabel";
            this.InternalNameLabel.Size = new System.Drawing.Size(104, 16);
            this.InternalNameLabel.TabIndex = 21;
            this.InternalNameLabel.Text = "Internal Name";
            // 
            // QuestPortraitLabel
            // 
            this.QuestPortraitLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.QuestPortraitLabel.AutoSize = true;
            this.QuestPortraitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuestPortraitLabel.Location = new System.Drawing.Point(3, 79);
            this.QuestPortraitLabel.Name = "QuestPortraitLabel";
            this.QuestPortraitLabel.Size = new System.Drawing.Size(102, 16);
            this.QuestPortraitLabel.TabIndex = 4;
            this.QuestPortraitLabel.Text = "Quest Portrait";
            // 
            // QuestTypeIconLabel
            // 
            this.QuestTypeIconLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.QuestTypeIconLabel.AutoSize = true;
            this.QuestTypeIconLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuestTypeIconLabel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.QuestTypeIconLabel.Location = new System.Drawing.Point(3, 104);
            this.QuestTypeIconLabel.Name = "QuestTypeIconLabel";
            this.QuestTypeIconLabel.Size = new System.Drawing.Size(121, 16);
            this.QuestTypeIconLabel.TabIndex = 3;
            this.QuestTypeIconLabel.Text = "Quest Type Icon";
            // 
            // QuestCantAbandonLabel
            // 
            this.QuestCantAbandonLabel.AutoSize = true;
            this.QuestCantAbandonLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuestCantAbandonLabel.Location = new System.Drawing.Point(3, 131);
            this.QuestCantAbandonLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.QuestCantAbandonLabel.Name = "QuestCantAbandonLabel";
            this.QuestCantAbandonLabel.Size = new System.Drawing.Size(109, 16);
            this.QuestCantAbandonLabel.TabIndex = 17;
            this.QuestCantAbandonLabel.Text = "Can\'t Abandon";
            // 
            // QuestRepeatableLabel
            // 
            this.QuestRepeatableLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.QuestRepeatableLabel.AutoSize = true;
            this.QuestRepeatableLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuestRepeatableLabel.Location = new System.Drawing.Point(3, 154);
            this.QuestRepeatableLabel.Name = "QuestRepeatableLabel";
            this.QuestRepeatableLabel.Size = new System.Drawing.Size(90, 16);
            this.QuestRepeatableLabel.TabIndex = 19;
            this.QuestRepeatableLabel.Text = "Repeatable";
            // 
            // QuestDescriptionLabel
            // 
            this.QuestDescriptionLabel.AutoSize = true;
            this.QuestDescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuestDescriptionLabel.Location = new System.Drawing.Point(3, 181);
            this.QuestDescriptionLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.QuestDescriptionLabel.Name = "QuestDescriptionLabel";
            this.QuestDescriptionLabel.Size = new System.Drawing.Size(87, 16);
            this.QuestDescriptionLabel.TabIndex = 11;
            this.QuestDescriptionLabel.Text = "Description";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.Controls.Add(this.QuestID, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.UpdateIDButton, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(127, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(266, 25);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // QuestID
            // 
            this.QuestID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestID.Location = new System.Drawing.Point(3, 3);
            this.QuestID.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.QuestID.Name = "QuestID";
            this.QuestID.Size = new System.Drawing.Size(160, 20);
            this.QuestID.TabIndex = 9;
            // 
            // UpdateIDButton
            // 
            this.UpdateIDButton.Location = new System.Drawing.Point(169, 3);
            this.UpdateIDButton.Name = "UpdateIDButton";
            this.UpdateIDButton.Size = new System.Drawing.Size(94, 19);
            this.UpdateIDButton.TabIndex = 0;
            this.UpdateIDButton.Text = "Update ID";
            this.UpdateIDButton.UseVisualStyleBackColor = true;
            // 
            // QuestName
            // 
            this.QuestName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestName.Location = new System.Drawing.Point(130, 28);
            this.QuestName.Name = "QuestName";
            this.QuestName.Size = new System.Drawing.Size(260, 20);
            this.QuestName.TabIndex = 1;
            // 
            // QuestInternalName
            // 
            this.QuestInternalName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestInternalName.Location = new System.Drawing.Point(130, 53);
            this.QuestInternalName.Name = "QuestInternalName";
            this.QuestInternalName.Size = new System.Drawing.Size(260, 20);
            this.QuestInternalName.TabIndex = 22;
            this.QuestInternalName.TextChanged += new System.EventHandler(this.InternalName_TextChanged);
            // 
            // QuestPortrait
            // 
            this.QuestPortrait.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestPortrait.Location = new System.Drawing.Point(130, 78);
            this.QuestPortrait.Name = "QuestPortrait";
            this.QuestPortrait.Size = new System.Drawing.Size(260, 20);
            this.QuestPortrait.TabIndex = 6;
            // 
            // QuestTypeIcon
            // 
            this.QuestTypeIcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestTypeIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.QuestTypeIcon.FormattingEnabled = true;
            this.QuestTypeIcon.Location = new System.Drawing.Point(130, 103);
            this.QuestTypeIcon.Name = "QuestTypeIcon";
            this.QuestTypeIcon.Size = new System.Drawing.Size(260, 21);
            this.QuestTypeIcon.TabIndex = 20;
            this.QuestTypeIcon.SelectedIndexChanged += new System.EventHandler(this.QuestTypeIcon_SelectedIndexChanged);
            // 
            // QuestCantAbandon
            // 
            this.QuestCantAbandon.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.QuestCantAbandon.AutoSize = true;
            this.QuestCantAbandon.Location = new System.Drawing.Point(130, 130);
            this.QuestCantAbandon.Name = "QuestCantAbandon";
            this.QuestCantAbandon.Size = new System.Drawing.Size(15, 14);
            this.QuestCantAbandon.TabIndex = 15;
            this.QuestCantAbandon.UseVisualStyleBackColor = true;
            this.QuestCantAbandon.CheckedChanged += new System.EventHandler(this.QuestCantAbandon_CheckedChanged);
            // 
            // QuestRepeatable
            // 
            this.QuestRepeatable.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.QuestRepeatable.AutoSize = true;
            this.QuestRepeatable.Location = new System.Drawing.Point(130, 155);
            this.QuestRepeatable.Name = "QuestRepeatable";
            this.QuestRepeatable.Size = new System.Drawing.Size(15, 14);
            this.QuestRepeatable.TabIndex = 18;
            this.QuestRepeatable.UseVisualStyleBackColor = true;
            this.QuestRepeatable.CheckedChanged += new System.EventHandler(this.QuestRepeatable_CheckedChanged);
            // 
            // QuestDescriptionTextBox
            // 
            this.QuestDescriptionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestDescriptionTextBox.Location = new System.Drawing.Point(130, 178);
            this.QuestDescriptionTextBox.Multiline = true;
            this.QuestDescriptionTextBox.Name = "QuestDescriptionTextBox";
            this.QuestDescriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.QuestDescriptionTextBox.Size = new System.Drawing.Size(260, 323);
            this.QuestDescriptionTextBox.TabIndex = 16;
            // 
            // PrerequisiteList
            // 
            this.PrerequisiteList.AutoScroll = true;
            this.PrerequisiteList.BackColor = System.Drawing.Color.DarkGray;
            this.PrerequisiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PrerequisiteList.Location = new System.Drawing.Point(0, 0);
            this.PrerequisiteList.Margin = new System.Windows.Forms.Padding(0);
            this.PrerequisiteList.Name = "PrerequisiteList";
            this.PrerequisiteList.Size = new System.Drawing.Size(226, 454);
            this.PrerequisiteList.TabIndex = 7;
            // 
            // NewPrerequisiteButton
            // 
            this.NewPrerequisiteButton.AutoSize = true;
            this.NewPrerequisiteButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.NewPrerequisiteButton.Location = new System.Drawing.Point(0, 454);
            this.NewPrerequisiteButton.Name = "NewPrerequisiteButton";
            this.NewPrerequisiteButton.Size = new System.Drawing.Size(226, 50);
            this.NewPrerequisiteButton.TabIndex = 5;
            this.NewPrerequisiteButton.Text = "New Prerequisite";
            this.NewPrerequisiteButton.UseVisualStyleBackColor = true;
            this.NewPrerequisiteButton.Click += new System.EventHandler(this.NewPrerequisiteButton_Click);
            // 
            // Stages
            // 
            this.Stages.Controls.Add(this.QuestStageInfo_QuestStageList_Split);
            this.Stages.Location = new System.Drawing.Point(4, 22);
            this.Stages.Name = "Stages";
            this.Stages.Padding = new System.Windows.Forms.Padding(3);
            this.Stages.Size = new System.Drawing.Size(629, 510);
            this.Stages.TabIndex = 2;
            this.Stages.Text = "Quest Stages";
            this.Stages.UseVisualStyleBackColor = true;
            // 
            // QuestStageInfo_QuestStageList_Split
            // 
            this.QuestStageInfo_QuestStageList_Split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestStageInfo_QuestStageList_Split.Location = new System.Drawing.Point(3, 3);
            this.QuestStageInfo_QuestStageList_Split.Name = "QuestStageInfo_QuestStageList_Split";
            // 
            // QuestStageInfo_QuestStageList_Split.Panel1
            // 
            this.QuestStageInfo_QuestStageList_Split.Panel1.Controls.Add(this.splitContainer1);
            // 
            // QuestStageInfo_QuestStageList_Split.Panel2
            // 
            this.QuestStageInfo_QuestStageList_Split.Panel2.Controls.Add(this.QuestStageList);
            this.QuestStageInfo_QuestStageList_Split.Panel2.Controls.Add(this.newqueststagebutton);
            this.QuestStageInfo_QuestStageList_Split.Size = new System.Drawing.Size(623, 504);
            this.QuestStageInfo_QuestStageList_Split.SplitterDistance = 403;
            this.QuestStageInfo_QuestStageList_Split.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel1.Controls.Add(this.QuestStageDescriptionPanel_table);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.QuestStageTabs);
            this.splitContainer1.Size = new System.Drawing.Size(403, 504);
            this.splitContainer1.SplitterDistance = 134;
            this.splitContainer1.TabIndex = 0;
            // 
            // QuestStageDescriptionPanel_table
            // 
            this.QuestStageDescriptionPanel_table.ColumnCount = 1;
            this.QuestStageDescriptionPanel_table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.QuestStageDescriptionPanel_table.Controls.Add(this.QuestStageDescriptionLabel, 0, 0);
            this.QuestStageDescriptionPanel_table.Controls.Add(this.QuestStageDescriptionBox, 0, 1);
            this.QuestStageDescriptionPanel_table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestStageDescriptionPanel_table.Location = new System.Drawing.Point(0, 0);
            this.QuestStageDescriptionPanel_table.Name = "QuestStageDescriptionPanel_table";
            this.QuestStageDescriptionPanel_table.RowCount = 2;
            this.QuestStageDescriptionPanel_table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.QuestStageDescriptionPanel_table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.QuestStageDescriptionPanel_table.Size = new System.Drawing.Size(403, 134);
            this.QuestStageDescriptionPanel_table.TabIndex = 2;
            // 
            // QuestStageDescriptionLabel
            // 
            this.QuestStageDescriptionLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.QuestStageDescriptionLabel.AutoSize = true;
            this.QuestStageDescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuestStageDescriptionLabel.Location = new System.Drawing.Point(3, 4);
            this.QuestStageDescriptionLabel.Name = "QuestStageDescriptionLabel";
            this.QuestStageDescriptionLabel.Size = new System.Drawing.Size(131, 16);
            this.QuestStageDescriptionLabel.TabIndex = 1;
            this.QuestStageDescriptionLabel.Text = "Quest Description";
            // 
            // QuestStageDescriptionBox
            // 
            this.QuestStageDescriptionBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestStageDescriptionBox.Location = new System.Drawing.Point(3, 28);
            this.QuestStageDescriptionBox.Multiline = true;
            this.QuestStageDescriptionBox.Name = "QuestStageDescriptionBox";
            this.QuestStageDescriptionBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.QuestStageDescriptionBox.Size = new System.Drawing.Size(397, 103);
            this.QuestStageDescriptionBox.TabIndex = 0;
            this.QuestStageDescriptionBox.TextChanged += new System.EventHandler(this.QuestStageDescriptionBox_TextChanged);
            // 
            // QuestStageTabs
            // 
            this.QuestStageTabs.Controls.Add(this.QuestStageTasks);
            this.QuestStageTabs.Controls.Add(this.QuestStageDialogue);
            this.QuestStageTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestStageTabs.Location = new System.Drawing.Point(0, 0);
            this.QuestStageTabs.Multiline = true;
            this.QuestStageTabs.Name = "QuestStageTabs";
            this.QuestStageTabs.SelectedIndex = 0;
            this.QuestStageTabs.Size = new System.Drawing.Size(403, 366);
            this.QuestStageTabs.TabIndex = 0;
            // 
            // QuestStageTasks
            // 
            this.QuestStageTasks.Controls.Add(this.tableLayoutPanel1);
            this.QuestStageTasks.Location = new System.Drawing.Point(4, 22);
            this.QuestStageTasks.Name = "QuestStageTasks";
            this.QuestStageTasks.Padding = new System.Windows.Forms.Padding(3);
            this.QuestStageTasks.Size = new System.Drawing.Size(395, 340);
            this.QuestStageTasks.TabIndex = 0;
            this.QuestStageTasks.Text = "Tasks";
            this.QuestStageTasks.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.QuestStageTaskList, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(389, 334);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.QuestStageTaskType, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.newqueststagetaskbutton, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 302);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(383, 29);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // QuestStageTaskType
            // 
            this.QuestStageTaskType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestStageTaskType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.QuestStageTaskType.FormattingEnabled = true;
            this.QuestStageTaskType.Location = new System.Drawing.Point(194, 3);
            this.QuestStageTaskType.Name = "QuestStageTaskType";
            this.QuestStageTaskType.Size = new System.Drawing.Size(186, 21);
            this.QuestStageTaskType.TabIndex = 21;
            // 
            // newqueststagetaskbutton
            // 
            this.newqueststagetaskbutton.AutoSize = true;
            this.newqueststagetaskbutton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.newqueststagetaskbutton.Location = new System.Drawing.Point(3, 3);
            this.newqueststagetaskbutton.Name = "newqueststagetaskbutton";
            this.newqueststagetaskbutton.Size = new System.Drawing.Size(185, 32);
            this.newqueststagetaskbutton.TabIndex = 11;
            this.newqueststagetaskbutton.Text = "New Task";
            this.newqueststagetaskbutton.UseVisualStyleBackColor = true;
            this.newqueststagetaskbutton.Click += new System.EventHandler(this.newqueststagetaskbutton_Click);
            // 
            // QuestStageTaskList
            // 
            this.QuestStageTaskList.AutoScroll = true;
            this.QuestStageTaskList.BackColor = System.Drawing.SystemColors.ControlDark;
            this.QuestStageTaskList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestStageTaskList.Location = new System.Drawing.Point(3, 3);
            this.QuestStageTaskList.Name = "QuestStageTaskList";
            this.QuestStageTaskList.Size = new System.Drawing.Size(383, 293);
            this.QuestStageTaskList.TabIndex = 9;
            // 
            // QuestStageDialogue
            // 
            this.QuestStageDialogue.Location = new System.Drawing.Point(4, 22);
            this.QuestStageDialogue.Name = "QuestStageDialogue";
            this.QuestStageDialogue.Padding = new System.Windows.Forms.Padding(3);
            this.QuestStageDialogue.Size = new System.Drawing.Size(395, 340);
            this.QuestStageDialogue.TabIndex = 1;
            this.QuestStageDialogue.Text = "Dialogue";
            this.QuestStageDialogue.UseVisualStyleBackColor = true;
            // 
            // QuestStageList
            // 
            this.QuestStageList.AutoScroll = true;
            this.QuestStageList.BackColor = System.Drawing.SystemColors.ControlDark;
            this.QuestStageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestStageList.Location = new System.Drawing.Point(0, 0);
            this.QuestStageList.Name = "QuestStageList";
            this.QuestStageList.Size = new System.Drawing.Size(216, 454);
            this.QuestStageList.TabIndex = 6;
            // 
            // newqueststagebutton
            // 
            this.newqueststagebutton.AutoSize = true;
            this.newqueststagebutton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.newqueststagebutton.Location = new System.Drawing.Point(0, 454);
            this.newqueststagebutton.Name = "newqueststagebutton";
            this.newqueststagebutton.Size = new System.Drawing.Size(216, 50);
            this.newqueststagebutton.TabIndex = 5;
            this.newqueststagebutton.Text = "New Stage";
            this.newqueststagebutton.UseVisualStyleBackColor = true;
            this.newqueststagebutton.Click += new System.EventHandler(this.newqueststagebutton_Click);
            // 
            // QuestList
            // 
            this.QuestList.AutoScroll = true;
            this.QuestList.BackColor = System.Drawing.SystemColors.ControlDark;
            this.QuestList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestList.Location = new System.Drawing.Point(0, 0);
            this.QuestList.Name = "QuestList";
            this.QuestList.Size = new System.Drawing.Size(223, 486);
            this.QuestList.TabIndex = 1;
            // 
            // newquestbutton
            // 
            this.newquestbutton.AutoSize = true;
            this.newquestbutton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.newquestbutton.Location = new System.Drawing.Point(0, 486);
            this.newquestbutton.Name = "newquestbutton";
            this.newquestbutton.Size = new System.Drawing.Size(223, 50);
            this.newquestbutton.TabIndex = 0;
            this.newquestbutton.Text = "New Quest";
            this.newquestbutton.UseVisualStyleBackColor = true;
            this.newquestbutton.Click += new System.EventHandler(this.newQuestButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Quest ID";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 561);
            this.Controls.Add(this.Quest_QuestList_Split);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "ReIncarnation Quest Maker";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.Quest_QuestList_Split.Panel1.ResumeLayout(false);
            this.Quest_QuestList_Split.Panel2.ResumeLayout(false);
            this.Quest_QuestList_Split.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Quest_QuestList_Split)).EndInit();
            this.Quest_QuestList_Split.ResumeLayout(false);
            this.QuestTabs.ResumeLayout(false);
            this.QuestBasicInfoPage.ResumeLayout(false);
            this.QuestBasicInfo_QuestPrerequisites_Split.Panel1.ResumeLayout(false);
            this.QuestBasicInfo_QuestPrerequisites_Split.Panel2.ResumeLayout(false);
            this.QuestBasicInfo_QuestPrerequisites_Split.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QuestBasicInfo_QuestPrerequisites_Split)).EndInit();
            this.QuestBasicInfo_QuestPrerequisites_Split.ResumeLayout(false);
            this.QuestBasicInfoTable.ResumeLayout(false);
            this.QuestBasicInfoTable.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.QuestID)).EndInit();
            this.Stages.ResumeLayout(false);
            this.QuestStageInfo_QuestStageList_Split.Panel1.ResumeLayout(false);
            this.QuestStageInfo_QuestStageList_Split.Panel2.ResumeLayout(false);
            this.QuestStageInfo_QuestStageList_Split.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QuestStageInfo_QuestStageList_Split)).EndInit();
            this.QuestStageInfo_QuestStageList_Split.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.QuestStageDescriptionPanel_table.ResumeLayout(false);
            this.QuestStageDescriptionPanel_table.PerformLayout();
            this.QuestStageTabs.ResumeLayout(false);
            this.QuestStageTasks.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripLabel1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromReincQuestFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton CompileButton;
        private System.Windows.Forms.SplitContainer Quest_QuestList_Split;
        private System.Windows.Forms.Button newquestbutton;
        private System.Windows.Forms.Panel QuestList;
        private System.Windows.Forms.TabControl QuestTabs;
        private System.Windows.Forms.TabPage QuestBasicInfoPage;
        private System.Windows.Forms.SplitContainer QuestBasicInfo_QuestPrerequisites_Split;
        private System.Windows.Forms.TableLayoutPanel QuestBasicInfoTable;
        private System.Windows.Forms.CheckBox QuestRepeatable;
        private System.Windows.Forms.TextBox QuestDescriptionTextBox;
        private System.Windows.Forms.CheckBox QuestCantAbandon;
        private System.Windows.Forms.Label QuestPortraitLabel;
        private System.Windows.Forms.Label QuestNameLabel;
        private System.Windows.Forms.Label QuestIDLabel;
        private System.Windows.Forms.TextBox QuestName;
        private System.Windows.Forms.TextBox QuestPortrait;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.NumericUpDown QuestID;
        private System.Windows.Forms.Button UpdateIDButton;
        private System.Windows.Forms.Label QuestTypeIconLabel;
        private System.Windows.Forms.Label QuestDescriptionLabel;
        private System.Windows.Forms.Label QuestCantAbandonLabel;
        private System.Windows.Forms.Label QuestRepeatableLabel;
        private System.Windows.Forms.TabPage Stages;
        private System.Windows.Forms.SplitContainer QuestStageInfo_QuestStageList_Split;
        private System.Windows.Forms.Panel QuestStageList;
        private System.Windows.Forms.Button newqueststagebutton;
        private System.Windows.Forms.Panel PrerequisiteList;
        private System.Windows.Forms.Button NewPrerequisiteButton;
        private System.Windows.Forms.ComboBox QuestTypeIcon;
        private System.Windows.Forms.Label InternalNameLabel;
        private System.Windows.Forms.TextBox QuestInternalName;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel QuestStageDescriptionPanel_table;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox QuestStageDescriptionBox;
        private System.Windows.Forms.Label QuestStageDescriptionLabel;
        private System.Windows.Forms.TabControl QuestStageTabs;
        private System.Windows.Forms.TabPage QuestStageTasks;
        private System.Windows.Forms.TabPage QuestStageDialogue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button newqueststagetaskbutton;
        private System.Windows.Forms.Panel QuestStageTaskList;
        private System.Windows.Forms.ComboBox QuestStageTaskType;
    }
}


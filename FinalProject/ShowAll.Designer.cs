namespace FinalProject
{
    partial class ShowAll : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ShowAll()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.A = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.DateTimePicker = this.Factory.CreateRibbonButton();
            this.Allin = this.Factory.CreateRibbonButton();
            this.Show_All = this.Factory.CreateRibbonButton();
            this.GetProjectID = this.Factory.CreateRibbonEditBox();
            this.A.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // A
            // 
            this.A.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.A.Groups.Add(this.group1);
            this.A.Label = "TabAddIns";
            this.A.Name = "A";
            // 
            // group1
            // 
            this.group1.Items.Add(this.DateTimePicker);
            this.group1.Items.Add(this.Allin);
            this.group1.Items.Add(this.Show_All);
            this.group1.Items.Add(this.GetProjectID);
            this.group1.Label = "group1";
            this.group1.Name = "group1";
            // 
            // DateTimePicker
            // 
            this.DateTimePicker.Label = "Выберите Дату:";
            this.DateTimePicker.Name = "DateTimePicker";
            this.DateTimePicker.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.DateTimePicker_Click);
            // 
            // Allin
            // 
            this.Allin.Label = "Архивировать";
            this.Allin.Name = "Allin";
            this.Allin.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Allin_Click);
            // 
            // Show_All
            // 
            this.Show_All.Label = "Разархивировать";
            this.Show_All.Name = "Show_All";
            this.Show_All.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Show_All_Click);
            // 
            // GetProjectID
            // 
            this.GetProjectID.Label = "Введите ID проекта";
            this.GetProjectID.Name = "GetProjectID";
            this.GetProjectID.TextChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.editBox1_TextChanged);
            // 
            // ShowAll
            // 
            this.Name = "ShowAll";
            this.RibbonType = "Microsoft.Project.Project";
            this.Tabs.Add(this.A);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ShowAll_Load);
            this.A.ResumeLayout(false);
            this.A.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab A;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton DateTimePicker;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton Allin;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton Show_All;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox GetProjectID;
    }

    partial class ThisRibbonCollection
    {
        internal ShowAll ShowAll
        {
            get { return this.GetRibbon<ShowAll>(); }
        }
    }
}

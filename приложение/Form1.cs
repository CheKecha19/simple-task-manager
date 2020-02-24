using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace CustomizedTaskManager
{

    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class frmmain : System.Windows.Forms.Form
	{
        private const int V = 8192; // всего используемой памяти
        private System.Windows.Forms.NotifyIcon taskmgrnotify;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.ContextMenu lvcxtmnu;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Diagnostics.PerformanceCounter pcclrmemmngr;
		private System.Windows.Forms.MenuItem menuItem7;
		#region User-defined variables
		public static string newprocpathandparm,mcname;
		public static frmmain objtaskmgr;
		public static frmnewprcdetails objnewprocess;
		public System.Threading.Timer t =null;
		public System.Threading.Timer tclr =null;
		public bool erroccured = false;
		private System.Windows.Forms.MenuItem menuItem17;
		public Hashtable presentprocdetails;
        public Hashtable presprocv2;
        private ToolTip Подсказка;
        private System.Windows.Forms.Timer timer1;
        private PerformanceCounter pcMemoryAvailable;
        private ToolTip Обучалка;
        private PerformanceCounter procMemory;
        public Process[] processes = null;
        private StatusBarPanel threadcount;
        private StatusBarPanel processcount;
        private StatusBar statusBar1;
        private TabPage tabPage1;
        private TabControl tabControl1;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Label label3;
        private Label label2;
        private RadioButton radioButton4;
        private SplitContainer splitContainer1;
        private ListView lvprocesslist;
        private ColumnHeader procname;
        private ColumnHeader PID;
        private ColumnHeader phys;
        private ColumnHeader virt;
        private ColumnHeader priv;
        private ColumnHeader paged;
        private ColumnHeader nopaged;
        private ColumnHeader priority;
        private ProgressBar progressBar1;
        public Process[] processes2 = null;
        private Label label4;
        private Label label1;
        



        public object encoding { get; private set; }
        #endregion
        #region User-Defined Methods
        private void LoadAllProcessesOnStartup()
		{
			Process[] processes = null;
			try
			{
				processes = Process.GetProcesses(mcname);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
				Application.Exit();
				return;
			}
			int threadscount = 0;
			foreach(Process p in processes)
			{
				try
				{
#pragma warning disable CS0618 // Тип или член устарел
                    string[] prcdetails = new string[] { p.ProcessName, p.Id.ToString(), p.WorkingSet64.ToString(), p.VirtualMemorySize64.ToString(), p.PrivateMemorySize64.ToString(), p.PagedSystemMemorySize64.ToString(), p.NonpagedSystemMemorySize64.ToString(), p.BasePriority.ToString() };
#pragma warning restore CS0618 // Тип или член устарел
                    ListViewItem proc = new ListViewItem(prcdetails);
					lvprocesslist.Items.Add(proc);
					threadscount += p.Threads.Count;
				}
				catch{}
			}
			statusBar1.Panels[0].Text  = "Процессы : "+processes.Length.ToString();
			statusBar1.Panels[1].Text  = "Потоки : "+(threadscount+1).ToString();

            


        }
        
		private void LoadAllProcesses(object temp)
		{
			try
			{
				presentprocdetails.Clear();
				processes = Process.GetProcesses(mcname);
				bool runningproccountchanged= false;
				Hashtable lvprocesses = null;
				int threadscount = 0;
				foreach(Process p in processes)
				{
					try
					{
#pragma warning disable CS0618 // Тип или член устарел
                        string[] prcdetails = new string[] { p.ProcessName, p.Id.ToString(), (p.WorkingSet64.ToString()), p.VirtualMemorySize64.ToString(), p.PrivateMemorySize64.ToString(), p.PagedSystemMemorySize64.ToString(), p.NonpagedSystemMemorySize64.ToString(), p.BasePriority.ToString() };
#pragma warning restore CS0618 // Тип или член устарел
                        presentprocdetails.Add(prcdetails[1],prcdetails[0].ToString()+"#"+prcdetails[2].ToString()+"#"+prcdetails[3].ToString()+"#"+prcdetails[4].ToString()+"#"+prcdetails[5].ToString()+"#"+prcdetails[6].ToString()+"#"+prcdetails[7].ToString());
						threadscount += p.Threads.Count;
					}
					catch{}
				}
                         
                if ((processes.Length != lvprocesslist.Items.Count) || erroccured)
				{
					runningproccountchanged = true;
					lvprocesses = new Hashtable();
					foreach(ListViewItem item in lvprocesslist.Items)
					{
						lvprocesses.Add(item.SubItems[1].Text,"");
					}
				}
               
                if (runningproccountchanged || erroccured)
				{
					erroccured = false;
					foreach(Process p in Process.GetProcesses(mcname))
					{
						try
						{
							if(!lvprocesses.Contains(p.Id.ToString()))
							{
#pragma warning disable CS0618 // Тип или член устарел
                                string[] newprcdetails = new string[] { p.ProcessName, p.Id.ToString(), p.WorkingSet64.ToString(), p.VirtualMemorySize64.ToString(), p.PrivateMemorySize64.ToString(), p.PagedSystemMemorySize64.ToString(), p.NonpagedSystemMemorySize64.ToString(), p.BasePriority.ToString() };
#pragma warning restore CS0618 // Тип или член устарел
                                ListViewItem newprocess = new ListViewItem(newprcdetails);
								lvprocesslist.Items.Add(newprocess);
							}
							IDictionaryEnumerator enlvprocesses = lvprocesses.GetEnumerator();
							while(enlvprocesses.MoveNext())
							{
								if(!presentprocdetails.Contains(enlvprocesses.Key))
								{
									foreach(ListViewItem item in lvprocesslist.Items)
									{
										if(item.SubItems[1].Text.ToString().ToUpper() == enlvprocesses.Key.ToString().ToUpper())
										{
											lvprocesslist.Items.Remove(item);
										}
									}
								}
							}
						}
						catch{}
					}
				}

				IDictionaryEnumerator enpresentprodetails = presentprocdetails.GetEnumerator();
				bool valchanged = false;
				while (enpresentprodetails.MoveNext())
				{
					foreach(ListViewItem item in lvprocesslist.Items)
					{
						if(item.SubItems[1].Text.ToString().ToUpper() == enpresentprodetails.Key.ToString().ToUpper())
						{
							string[] presentprocessdetails = enpresentprodetails.Value.ToString().Split('#');
							if(item.SubItems[3].Text.ToString() != presentprocessdetails[2].ToString())
							{
								valchanged = true;
								item.SubItems[3].Text = presentprocessdetails[2].ToString();
							}
							if(item.SubItems[4].Text.ToString() != presentprocessdetails[3].ToString())
							{
								valchanged = true;
								item.SubItems[4].Text = presentprocessdetails[3].ToString();
							}
							if(item.SubItems[5].Text.ToString() != presentprocessdetails[4].ToString())
							{
								valchanged = true;
								item.SubItems[5].Text = presentprocessdetails[4].ToString();
							}
							if(item.SubItems[6].Text.ToString() != presentprocessdetails[5].ToString())
							{
								valchanged = true;
								item.SubItems[6].Text = presentprocessdetails[5].ToString();
							}
							if(item.SubItems[7].Text.ToString() != presentprocessdetails[6].ToString())
							{
								valchanged = true;
								item.SubItems[7].Text = presentprocessdetails[6].ToString();
							}
							if(menuItem17.Checked)
							{
								valchanged = false;
							}
							if(valchanged)
							{
								item.ForeColor = Color.Red;
								valchanged = false;
							}
							else
							{
								item.ForeColor = Color.Black;
							}
							break;
						}
					}
				}
				statusBar1.Panels[0].Text  = "Процессы : "+processes.Length.ToString();
				statusBar1.Panels[1].Text  = "Потоки : "+(threadscount+1).ToString();
			}
			catch{}
            
         }



        private void SetProcessPriority(MenuItem item)
		{
			try
			{
				int selectedpid = Convert.ToInt32(lvprocesslist.SelectedItems[0].SubItems[1].Text.ToString());
				Process selectedprocess = Process.GetProcessById(selectedpid,mcname);
				if(item.Text.ToUpper() == "HIGH")
					selectedprocess.PriorityClass = ProcessPriorityClass.High;
				else if(item.Text.ToUpper() == "LOW")
					selectedprocess.PriorityClass = ProcessPriorityClass.Idle;
				else if(item.Text.ToUpper() == "REAL-TIME")
					selectedprocess.PriorityClass = ProcessPriorityClass.RealTime;
				else if(item.Text.ToUpper() == "ABOVE NORMAL")
					selectedprocess.PriorityClass = ProcessPriorityClass.AboveNormal;
				else if(item.Text.ToUpper() == "BELOW NORMAL")
					selectedprocess.PriorityClass = ProcessPriorityClass.BelowNormal;
				else if(item.Text.ToUpper() == "NORMAL")
					selectedprocess.PriorityClass = ProcessPriorityClass.Normal;
				foreach(MenuItem mnuitem in menuItem10.MenuItems)
				{
					mnuitem.Checked = false;
				}
				item.Checked = true;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private  void LoadAllMemoryDetails(object temp)
		{
			try
			{
				
			}
			catch
			{
				
			}
		}
		#endregion
        
		public frmmain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmmain));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("programs", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("system", System.Windows.Forms.HorizontalAlignment.Left);
            this.taskmgrnotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.lvcxtmnu = new System.Windows.Forms.ContextMenu();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.pcclrmemmngr = new System.Diagnostics.PerformanceCounter();
            this.Подсказка = new System.Windows.Forms.ToolTip(this.components);
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.processcount = new System.Windows.Forms.StatusBarPanel();
            this.threadcount = new System.Windows.Forms.StatusBarPanel();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pcMemoryAvailable = new System.Diagnostics.PerformanceCounter();
            this.Обучалка = new System.Windows.Forms.ToolTip(this.components);
            this.procMemory = new System.Diagnostics.PerformanceCounter();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvprocesslist = new System.Windows.Forms.ListView();
            this.procname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.phys = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.virt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.priv = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.paged = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nopaged = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.priority = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pcclrmemmngr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.processcount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadcount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcMemoryAvailable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.procMemory)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // taskmgrnotify
            // 
            this.taskmgrnotify.Icon = ((System.Drawing.Icon)(resources.GetObject("taskmgrnotify.Icon")));
            this.taskmgrnotify.Text = " Perfomance Monitor Kecha Edition  is in visible Mode";
            this.taskmgrnotify.Visible = true;
            this.taskmgrnotify.Click += new System.EventHandler(this.taskmgrnotify_Click);
            // 
            // lvcxtmnu
            // 
            this.lvcxtmnu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem9,
            this.menuItem10});
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 0;
            this.menuItem9.Text = "End Process";
            this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 1;
            this.menuItem10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem11,
            this.menuItem15,
            this.menuItem16,
            this.menuItem12,
            this.menuItem13,
            this.menuItem14});
            this.menuItem10.Text = "Set Priority";
            this.menuItem10.Popup += new System.EventHandler(this.menuItem10_Popup);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 0;
            this.menuItem11.RadioCheck = true;
            this.menuItem11.Text = "High";
            this.menuItem11.Click += new System.EventHandler(this.menuItem11_Click);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 1;
            this.menuItem15.RadioCheck = true;
            this.menuItem15.Text = "Above Normal";
            this.menuItem15.Click += new System.EventHandler(this.menuItem15_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 2;
            this.menuItem16.RadioCheck = true;
            this.menuItem16.Text = "Below Normal";
            this.menuItem16.Click += new System.EventHandler(this.menuItem16_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 3;
            this.menuItem12.RadioCheck = true;
            this.menuItem12.Text = "Normal";
            this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 4;
            this.menuItem13.RadioCheck = true;
            this.menuItem13.Text = "Low";
            this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 5;
            this.menuItem14.RadioCheck = true;
            this.menuItem14.Text = "Real Time";
            this.menuItem14.Click += new System.EventHandler(this.menuItem14_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem5});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem7,
            this.menuItem17,
            this.menuItem2,
            this.menuItem3,
            this.menuItem4});
            this.menuItem1.Text = "Main";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 0;
            this.menuItem7.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            this.menuItem7.Text = "Connect To";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Checked = true;
            this.menuItem17.Index = 1;
            this.menuItem17.Text = "Stop Coloring current process";
            this.menuItem17.Click += new System.EventHandler(this.menuItem17_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.menuItem2.Text = "New Task ";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.menuItem3.Text = "End Process";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 4;
            this.menuItem4.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.menuItem4.Text = "Exit  Perfomance Monitor Kecha Edition ";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem6,
            this.menuItem8});
            this.menuItem5.Text = "Options";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 0;
            this.menuItem6.Text = "Always On Top";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 1;
            this.menuItem8.Text = "Hide When Minimized";
            this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
            // 
            // pcclrmemmngr
            // 
            this.pcclrmemmngr.CategoryName = ".NET CLR Memory";
            this.pcclrmemmngr.CounterName = "Large Object Heap size";
            this.pcclrmemmngr.InstanceName = "aspnet_wp";
            // 
            // Подсказка
            // 
            this.Подсказка.AutoPopDelay = 10000;
            this.Подсказка.InitialDelay = 500;
            this.Подсказка.ReshowDelay = 100;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 438);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.processcount,
            this.threadcount});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(1149, 23);
            this.statusBar1.TabIndex = 1;
            this.Подсказка.SetToolTip(this.statusBar1, "Количество запущенных процессов и  потоков ");
            // 
            // processcount
            // 
            this.processcount.Name = "processcount";
            this.processcount.Text = "statusBarPanel1";
            // 
            // threadcount
            // 
            this.threadcount.Name = "threadcount";
            this.threadcount.Text = "statusBarPanel1";
            // 
            // radioButton3
            // 
            this.radioButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButton3.AutoSize = true;
            this.radioButton3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radioButton3.Location = new System.Drawing.Point(467, 441);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(39, 17);
            this.radioButton3.TabIndex = 21;
            this.radioButton3.Text = "Gb";
            this.Подсказка.SetToolTip(this.radioButton3, "Единицы измерения");
            this.radioButton3.UseVisualStyleBackColor = false;
            // 
            // radioButton2
            // 
            this.radioButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButton2.AutoSize = true;
            this.radioButton2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(421, 441);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(40, 17);
            this.radioButton2.TabIndex = 20;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Mb";
            this.Подсказка.SetToolTip(this.radioButton2, "Единицы измерения");
            this.radioButton2.UseVisualStyleBackColor = false;
            // 
            // radioButton1
            // 
            this.radioButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButton1.AutoSize = true;
            this.radioButton1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radioButton1.Location = new System.Drawing.Point(377, 441);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(38, 17);
            this.radioButton1.TabIndex = 19;
            this.radioButton1.Text = "Kb";
            this.Подсказка.SetToolTip(this.radioButton1, "Единицы измерения");
            this.radioButton1.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(203, 443);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Доступно/Используется/Всего";
            this.Подсказка.SetToolTip(this.label2, "Единицы измерения");
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pcMemoryAvailable
            // 
            this.pcMemoryAvailable.CategoryName = "Память";
            this.pcMemoryAvailable.CounterName = "Доступно МБ";
            // 
            // Обучалка
            // 
            this.Обучалка.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.Обучалка.IsBalloon = true;
            this.Обучалка.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.Обучалка.ToolTipTitle = "Обучалка";
            // 
            // procMemory
            // 
            this.procMemory.CategoryName = "Память";
            this.procMemory.CounterName = "Доступно МБ";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1141, 412);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = " Perfomance Monitor Kecha Edition ";
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
            this.splitContainer1.Panel1.Controls.Add(this.lvprocesslist);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.progressBar1);
            this.splitContainer1.Size = new System.Drawing.Size(1141, 412);
            this.splitContainer1.SplitterDistance = 378;
            this.splitContainer1.TabIndex = 0;
            // 
            // lvprocesslist
            // 
            this.lvprocesslist.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.lvprocesslist.BackColor = System.Drawing.Color.White;
            this.lvprocesslist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.procname,
            this.PID,
            this.phys,
            this.virt,
            this.priv,
            this.paged,
            this.nopaged,
            this.priority});
            this.lvprocesslist.ContextMenu = this.lvcxtmnu;
            this.lvprocesslist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvprocesslist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvprocesslist.FullRowSelect = true;
            listViewGroup1.Header = "programs";
            listViewGroup1.Name = "listViewGroup1";
            listViewGroup2.Header = "system";
            listViewGroup2.Name = "listViewGroup2";
            this.lvprocesslist.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.lvprocesslist.Location = new System.Drawing.Point(0, 0);
            this.lvprocesslist.Name = "lvprocesslist";
            this.lvprocesslist.Size = new System.Drawing.Size(1141, 378);
            this.lvprocesslist.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvprocesslist.TabIndex = 3;
            this.lvprocesslist.UseCompatibleStateImageBehavior = false;
            this.lvprocesslist.View = System.Windows.Forms.View.Details;
            this.lvprocesslist.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvprocesslist_ColumnClick);
            // 
            // procname
            // 
            this.procname.Text = "Имя";
            this.procname.Width = 199;
            // 
            // PID
            // 
            this.PID.Text = "ID";
            this.PID.Width = 51;
            // 
            // phys
            // 
            this.phys.Text = "Физическая память (Байты)";
            this.phys.Width = 158;
            // 
            // virt
            // 
            this.virt.Text = "Виртуальная память (Байты)";
            this.virt.Width = 159;
            // 
            // priv
            // 
            this.priv.Text = "Закрытая память (Байты)";
            this.priv.Width = 143;
            // 
            // paged
            // 
            this.paged.Text = "Выгружаемая память (Байты)";
            this.paged.Width = 164;
            // 
            // nopaged
            // 
            this.nopaged.Text = "Невыгружаемая память (Байты)";
            this.nopaged.Width = 178;
            // 
            // priority
            // 
            this.priority.Text = "Приоритет";
            this.priority.Width = 67;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label4.Location = new System.Drawing.Point(1042, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Используется";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Доступно";
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.progressBar1.Cursor = System.Windows.Forms.Cursors.No;
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.ForeColor = System.Drawing.Color.Lime;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar1.Maximum = 8192;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1141, 30);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Location = new System.Drawing.Point(551, 443);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "label3";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1149, 438);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButton4.AutoSize = true;
            this.radioButton4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radioButton4.Location = new System.Drawing.Point(512, 441);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(33, 17);
            this.radioButton4.TabIndex = 22;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "%";
            this.radioButton4.UseVisualStyleBackColor = false;
            // 
            // frmmain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1149, 461);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1165, 500);
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(1165, 500);
            this.Name = "frmmain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Perfomance Monitor Kecha Edition";
            this.Load += new System.EventHandler(this.frmmain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pcclrmemmngr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.processcount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadcount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcMemoryAvailable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.procMemory)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			objtaskmgr = new frmmain();
			Application.Run(objtaskmgr);
		}

		private void taskmgrnotify_Click(object sender, System.EventArgs e)
		{
			if(objtaskmgr.Visible)
			{
				objtaskmgr.Visible = false;
				taskmgrnotify.Text = " Perfomance Monitor Kecha Edition  is in Invisible Mode";
			}
			else
			{
				objtaskmgr.Visible = true;
				taskmgrnotify.Text = " Perfomance Monitor Kecha Edition  is in visible Mode";
			}
		}
		private void frmmain_Load(object sender, System.EventArgs e)
		{
			mcname = ".";
			presentprocdetails = new Hashtable();
			LoadAllProcessesOnStartup();
			System.Threading.TimerCallback timerDelegate = 
				new System.Threading.TimerCallback(this.LoadAllProcesses);
			t = new System.Threading.Timer(timerDelegate,null,500,500);
    	}
	
		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			//Here,We are going to kill selected Process by getting ID...
			if(lvprocesslist.SelectedItems.Count>=1)
			{
				try
				{
					int selectedpid = Convert.ToInt32(lvprocesslist.SelectedItems[0].SubItems[1].Text.ToString());
					Process.GetProcessById(selectedpid,mcname).Kill();
				}
				catch
				{
					erroccured = true;
				}
			}
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			try
			{
			    objnewprocess = new frmnewprcdetails();
				objnewprocess.ShowDialog();
                if(newprocpathandparm.Length != 0)
				{
					if(newprocpathandparm.IndexOf("\\") == -1)
					{
						string[] newprocdetails = newprocpathandparm.Split(' ');
						if(newprocdetails.Length > 1)
						{
							Process newprocess = Process.Start(newprocdetails[0].ToString(),newprocdetails[1].ToString());
						}
						else
						{
							Process newprocess = Process.Start(newprocdetails[0].ToString());
						}
					}
					else
					{
						string procname = newprocpathandparm.Substring(newprocpathandparm.LastIndexOf("\\")+1);
						string[] newprocdetails = procname.Split(' ');
						if(newprocdetails.Length > 1)
						{
							Process newprocess = Process.Start(newprocpathandparm.Replace(newprocdetails[1].ToString(),""),newprocdetails[1].ToString());
						}
						else
						{
							Process newprocess = Process.Start(newprocpathandparm);
						}

					}
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			if(menuItem6.Checked)
			{
				menuItem6.Checked = false;
				objtaskmgr.TopMost = false;
			}
			else
			{
				menuItem6.Checked = true;
				objtaskmgr.TopMost = true;
			}
		}

		private void menuItem8_Click(object sender, System.EventArgs e)
		{
			if(menuItem8.Checked)
			{
				menuItem8.Checked = false;
				objtaskmgr.ShowInTaskbar = true;
			}
			else
			{
				menuItem8.Checked = true;
				objtaskmgr.ShowInTaskbar = false;
			}
		}

		private void menuItem9_Click(object sender, System.EventArgs e)
		{
			menuItem3_Click(sender,e);
		}

		private void menuItem11_Click(object sender, System.EventArgs e)
		{
			SetProcessPriority(menuItem11);
		}

		private void menuItem15_Click(object sender, System.EventArgs e)
		{
			SetProcessPriority(menuItem15);
		}

		private void menuItem16_Click(object sender, System.EventArgs e)
		{
			SetProcessPriority(menuItem16);
		}

		private void menuItem12_Click(object sender, System.EventArgs e)
		{
			SetProcessPriority(menuItem12);
		}
		private void menuItem13_Click(object sender, System.EventArgs e)
		{
			SetProcessPriority(menuItem13);
		}

		private void menuItem14_Click(object sender, System.EventArgs e)
		{
			SetProcessPriority(menuItem14);
		}

		private void menuItem10_Popup(object sender, System.EventArgs e)
		{
			try
			{
				int selectedpid = Convert.ToInt32(lvprocesslist.SelectedItems[0].SubItems[1].Text.ToString());
				Process selectedprocess = Process.GetProcessById(selectedpid,mcname);
				string priority = selectedprocess.PriorityClass.ToString();
				foreach(MenuItem mnuitem in menuItem10.MenuItems)
				{
					string mnutext = mnuitem.Text.ToUpper().Replace(" ","");
					if(mnutext == "LOW")
                       mnutext = "IDLE";
					if(mnutext != priority.ToUpper())
						mnuitem.Checked = false;
					else
					{
						mnuitem.Checked = true;
					}
				}
				
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			try
			{
				string caption = "Enter Machine Name";
				objnewprocess = new frmnewprcdetails(caption);
				if(objnewprocess.ShowDialog()!= DialogResult.Cancel)
				{
					t.Dispose();
					presentprocdetails.Clear();
					lvprocesslist.Items.Clear();
					LoadAllProcessesOnStartup();
					if(frmmain.mcname == ".")
					{
						frmmain.objtaskmgr.Text = " Perfomance Monitor Kecha Edition  Connected to Local";
						menuItem3.Visible = true;
						menuItem9.Visible = true;
						menuItem2.Visible = true;
						menuItem10.Visible = true;
					}
					else
					{
						frmmain.objtaskmgr.Text = " Perfomance Monitor Kecha Edition  Connected to "+frmmain.mcname;
						menuItem3.Visible = false;
						menuItem9.Visible = false;
						menuItem2.Visible = false;
						menuItem10.Visible = false;
					}
					System.Threading.TimerCallback timerDelegate = 
						new System.Threading.TimerCallback(this.LoadAllProcesses);
					t = new System.Threading.Timer(timerDelegate,null,1000,1000);
				}
				}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(tabControl1.SelectedIndex == 1)
			{
				System.Threading.TimerCallback timerDelegate = 
					new System.Threading.TimerCallback(this.LoadAllMemoryDetails);
				tclr = new System.Threading.Timer(timerDelegate,null,0,1000);
			}
			else
			{
				tclr.Dispose();
			}
		}

        private void lvprocesslist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            //кнопки под размерность и данные в боксе общие данные
            {
                //
                //MB
                //     
                if (radioButton2.Checked == true)

                {
                    //используемая память
                    float U = V - ((int)pcMemoryAvailable.NextValue());
                    // доступно,занято,всего памяти
                    label3.Text = ((long)pcMemoryAvailable.NextValue()).ToString() + " // " + U + " // " + V;

                }
                //
                //KB
                //
                if (radioButton1.Checked == true)
                {
                    //используемая память
                    long U = V * 1024 - ((long)pcMemoryAvailable.NextValue()) * 1024;
                    // доступно,занято,всего памяти
                    label3.Text = ((long)pcMemoryAvailable.NextValue()) * 1024 + " // " + U + " // " + V * 1024;

                }
                //
                //GB
                //
                if (radioButton3.Checked == true)
                {
                    //используемая память
                    float U = V / 1024 - ((float)pcMemoryAvailable.NextValue()) / 1024;
                    // доступно,занято,всего памяти
                    label3.Text = ((float)pcMemoryAvailable.NextValue()) / 1024 + " // " + U + " // " + V / 1024;

                }


                //
                //%
                //
                if (radioButton4.Checked == true)
                {
                    float Av = (pcMemoryAvailable.NextValue() / V * 100);
                    label3.Text = Av + " // " + (100 - Av);
                }

                // progressbar для нижнего гроупбокса, показывает лоступную память в %
                
        progressBar1.Value = (int)(pcMemoryAvailable.NextValue());

            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
                
        private void button3_Click(object sender, EventArgs e)
        {
            
        }
                       
        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
                
        private void lvprocesslist_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            
        }

        private void lvprocesslist_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvprocesslist.Sorting == SortOrder.Ascending)
            {
                lvprocesslist.Sorting = SortOrder.Descending;
            }
           else
            {
                lvprocesslist.Sorting = SortOrder.Ascending;
            }
        }

        private void menuItem17_Click(object sender, System.EventArgs e)
		{
			if(menuItem17.Checked)
				menuItem17.Checked = false;
			else
				menuItem17.Checked = true;
		}

	}
    

}

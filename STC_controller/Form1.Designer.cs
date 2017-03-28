namespace STC_controller
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tmr_add_user = new System.Windows.Forms.Timer(this.components);
            this.tmr_rquest = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbl_Machine_Name = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.rdo_real = new System.Windows.Forms.RadioButton();
            this.rdo_stg = new System.Windows.Forms.RadioButton();
            this.label20 = new System.Windows.Forms.Label();
            this.btn_recover = new System.Windows.Forms.Button();
            this.tgl_MT4_watch = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tgl_socket = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txt_request_interval = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_add_user_interval = new System.Windows.Forms.TextBox();
            this.tgl_reuest = new System.Windows.Forms.CheckBox();
            this.tgl_add_user = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_out_put = new System.Windows.Forms.TextBox();
            this.txt_get_url = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdo_url_add_EA = new System.Windows.Forms.RadioButton();
            this.rdo_url_request = new System.Windows.Forms.RadioButton();
            this.rdo_url_add_user = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_http_get = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.label29 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_http_post = new System.Windows.Forms.Button();
            this.txt_word = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdo_set_post_url = new System.Windows.Forms.RadioButton();
            this.rdo_kakuninn = new System.Windows.Forms.RadioButton();
            this.rdo_stc = new System.Windows.Forms.RadioButton();
            this.txt_post_url = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdo_get_url_request = new System.Windows.Forms.RadioButton();
            this.rdo_get_url_add_user = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btn_Conn_Watch = new System.Windows.Forms.Button();
            this.btn_csv_read = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txt_startprogram = new System.Windows.Forms.TextBox();
            this.txt_renameFile = new System.Windows.Forms.TextBox();
            this.btn_reame = new System.Windows.Forms.Button();
            this.btn_start = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.txt_stopprogram = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_readFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_readFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdo_add_user = new System.Windows.Forms.RadioButton();
            this.rdo_request = new System.Windows.Forms.RadioButton();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.txt_create_folders = new System.Windows.Forms.TextBox();
            this.btn_get_folders = new System.Windows.Forms.Button();
            this.btn_create_folders = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.btn_find_MT4 = new System.Windows.Forms.Button();
            this.cmb_EA_ID = new System.Windows.Forms.ComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.cmb_Time_Period = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.cmb_CCY = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.cmb_mt4ID = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.cmb_mt4SV = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.btn_getUser = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.cmb_stcUser = new System.Windows.Forms.ComboBox();
            this.tmr_conn_watch = new System.Windows.Forms.Timer(this.components);
            this.label30 = new System.Windows.Forms.Label();
            this.tgl_MT4_option = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmr_add_user
            // 
            this.tmr_add_user.Interval = 1000;
            this.tmr_add_user.Tick += new System.EventHandler(this.tmr_add_user_Tick);
            // 
            // tmr_rquest
            // 
            this.tmr_rquest.Interval = 5000;
            this.tmr_rquest.Tick += new System.EventHandler(this.tmr_rquest_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(503, 295);
            this.tabControl1.TabIndex = 53;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tgl_MT4_option);
            this.tabPage1.Controls.Add(this.label30);
            this.tabPage1.Controls.Add(this.lbl_Machine_Name);
            this.tabPage1.Controls.Add(this.label22);
            this.tabPage1.Controls.Add(this.label21);
            this.tabPage1.Controls.Add(this.rdo_real);
            this.tabPage1.Controls.Add(this.rdo_stg);
            this.tabPage1.Controls.Add(this.label20);
            this.tabPage1.Controls.Add(this.btn_recover);
            this.tabPage1.Controls.Add(this.tgl_MT4_watch);
            this.tabPage1.Controls.Add(this.label19);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.tgl_socket);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.txt_request_interval);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.txt_add_user_interval);
            this.tabPage1.Controls.Add(this.tgl_reuest);
            this.tabPage1.Controls.Add(this.tgl_add_user);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(495, 269);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "起動設定関連";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lbl_Machine_Name
            // 
            this.lbl_Machine_Name.AutoSize = true;
            this.lbl_Machine_Name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_Machine_Name.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_Machine_Name.Location = new System.Drawing.Point(6, 22);
            this.lbl_Machine_Name.Name = "lbl_Machine_Name";
            this.lbl_Machine_Name.Size = new System.Drawing.Size(101, 18);
            this.lbl_Machine_Name.TabIndex = 61;
            this.lbl_Machine_Name.Text = "マシン名表示";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 5);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(45, 12);
            this.label22.TabIndex = 60;
            this.label22.Text = "マシン名";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(292, 5);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(65, 12);
            this.label21.TabIndex = 59;
            this.label21.Text = "接続先設定";
            // 
            // rdo_real
            // 
            this.rdo_real.AutoSize = true;
            this.rdo_real.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdo_real.ForeColor = System.Drawing.Color.Green;
            this.rdo_real.Location = new System.Drawing.Point(411, 20);
            this.rdo_real.Name = "rdo_real";
            this.rdo_real.Size = new System.Drawing.Size(75, 16);
            this.rdo_real.TabIndex = 58;
            this.rdo_real.TabStop = true;
            this.rdo_real.Text = "本番接続";
            this.rdo_real.UseVisualStyleBackColor = true;
            // 
            // rdo_stg
            // 
            this.rdo_stg.AutoSize = true;
            this.rdo_stg.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdo_stg.ForeColor = System.Drawing.Color.DarkBlue;
            this.rdo_stg.Location = new System.Drawing.Point(294, 20);
            this.rdo_stg.Name = "rdo_stg";
            this.rdo_stg.Size = new System.Drawing.Size(111, 16);
            this.rdo_stg.TabIndex = 57;
            this.rdo_stg.TabStop = true;
            this.rdo_stg.Text = "ステージング接続";
            this.rdo_stg.UseVisualStyleBackColor = true;
            this.rdo_stg.CheckedChanged += new System.EventHandler(this.rdo_stg_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 191);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(101, 12);
            this.label20.TabIndex = 56;
            this.label20.Text = "最終指示状態再現";
            // 
            // btn_recover
            // 
            this.btn_recover.Location = new System.Drawing.Point(18, 230);
            this.btn_recover.Name = "btn_recover";
            this.btn_recover.Size = new System.Drawing.Size(75, 23);
            this.btn_recover.TabIndex = 55;
            this.btn_recover.Text = "再現実行";
            this.btn_recover.UseVisualStyleBackColor = true;
            this.btn_recover.Click += new System.EventHandler(this.btn_recover_Click);
            // 
            // tgl_MT4_watch
            // 
            this.tgl_MT4_watch.Appearance = System.Windows.Forms.Appearance.Button;
            this.tgl_MT4_watch.AutoSize = true;
            this.tgl_MT4_watch.Location = new System.Drawing.Point(245, 112);
            this.tgl_MT4_watch.Name = "tgl_MT4_watch";
            this.tgl_MT4_watch.Size = new System.Drawing.Size(74, 22);
            this.tgl_MT4_watch.TabIndex = 54;
            this.tgl_MT4_watch.Text = "watch false";
            this.tgl_MT4_watch.UseVisualStyleBackColor = true;
            this.tgl_MT4_watch.CheckedChanged += new System.EventHandler(this.tgl_MT4_watch_CheckedChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(243, 72);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(99, 12);
            this.label19.TabIndex = 53;
            this.label19.Text = "MT4接続状況監視";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(145, 72);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(86, 12);
            this.label15.TabIndex = 52;
            this.label15.Text = "監視ソケット稼働";
            // 
            // tgl_socket
            // 
            this.tgl_socket.Appearance = System.Windows.Forms.Appearance.Button;
            this.tgl_socket.AutoSize = true;
            this.tgl_socket.Location = new System.Drawing.Point(147, 112);
            this.tgl_socket.Name = "tgl_socket";
            this.tgl_socket.Size = new System.Drawing.Size(68, 22);
            this.tgl_socket.TabIndex = 51;
            this.tgl_socket.Text = "sock false";
            this.tgl_socket.UseVisualStyleBackColor = true;
            this.tgl_socket.CheckedChanged += new System.EventHandler(this.tgl_socket_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(85, 96);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(20, 12);
            this.label13.TabIndex = 50;
            this.label13.Text = "ms";
            // 
            // txt_request_interval
            // 
            this.txt_request_interval.Location = new System.Drawing.Point(25, 89);
            this.txt_request_interval.Name = "txt_request_interval";
            this.txt_request_interval.Size = new System.Drawing.Size(54, 19);
            this.txt_request_interval.TabIndex = 49;
            this.txt_request_interval.Text = "5000";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(133, 12);
            this.label12.TabIndex = 48;
            this.label12.Text = "requestの受信インターバル";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(427, 215);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 12);
            this.label11.TabIndex = 47;
            this.label11.Text = "ms";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(328, 191);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(143, 12);
            this.label10.TabIndex = 46;
            this.label10.Text = "Add_Userの受信インターバル";
            // 
            // txt_add_user_interval
            // 
            this.txt_add_user_interval.Enabled = false;
            this.txt_add_user_interval.Location = new System.Drawing.Point(367, 208);
            this.txt_add_user_interval.Name = "txt_add_user_interval";
            this.txt_add_user_interval.Size = new System.Drawing.Size(54, 19);
            this.txt_add_user_interval.TabIndex = 45;
            this.txt_add_user_interval.Text = "10000";
            // 
            // tgl_reuest
            // 
            this.tgl_reuest.Appearance = System.Windows.Forms.Appearance.Button;
            this.tgl_reuest.AutoSize = true;
            this.tgl_reuest.Location = new System.Drawing.Point(6, 112);
            this.tgl_reuest.Name = "tgl_reuest";
            this.tgl_reuest.Size = new System.Drawing.Size(118, 22);
            this.tgl_reuest.TabIndex = 44;
            this.tgl_reuest.Text = "requestタイマー false";
            this.tgl_reuest.UseVisualStyleBackColor = true;
            this.tgl_reuest.CheckedChanged += new System.EventHandler(this.tgl_reuest_CheckedChanged);
            // 
            // tgl_add_user
            // 
            this.tgl_add_user.Appearance = System.Windows.Forms.Appearance.Button;
            this.tgl_add_user.AutoSize = true;
            this.tgl_add_user.Enabled = false;
            this.tgl_add_user.Location = new System.Drawing.Point(330, 231);
            this.tgl_add_user.Name = "tgl_add_user";
            this.tgl_add_user.Size = new System.Drawing.Size(124, 22);
            this.tgl_add_user.TabIndex = 43;
            this.tgl_add_user.Text = "add_userタイマー false";
            this.tgl_add_user.UseVisualStyleBackColor = true;
            this.tgl_add_user.CheckedChanged += new System.EventHandler(this.tgl_add_user_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txt_out_put);
            this.tabPage2.Controls.Add(this.txt_get_url);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.btn_http_get);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(495, 269);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ユーザー追加関連";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 57);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(256, 12);
            this.label14.TabIndex = 48;
            this.label14.Text = "※add_userの場合はインストールフォルダを作成します";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 12);
            this.label7.TabIndex = 47;
            this.label7.Text = "ファイル出力先";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 12);
            this.label6.TabIndex = 46;
            this.label6.Text = "GET先URL指定";
            // 
            // txt_out_put
            // 
            this.txt_out_put.Location = new System.Drawing.Point(110, 107);
            this.txt_out_put.Name = "txt_out_put";
            this.txt_out_put.Size = new System.Drawing.Size(243, 19);
            this.txt_out_put.TabIndex = 45;
            // 
            // txt_get_url
            // 
            this.txt_get_url.Location = new System.Drawing.Point(110, 85);
            this.txt_get_url.Name = "txt_get_url";
            this.txt_get_url.Size = new System.Drawing.Size(243, 19);
            this.txt_get_url.TabIndex = 44;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdo_url_add_EA);
            this.groupBox2.Controls.Add(this.rdo_url_request);
            this.groupBox2.Controls.Add(this.rdo_url_add_user);
            this.groupBox2.Location = new System.Drawing.Point(4, 18);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(397, 36);
            this.groupBox2.TabIndex = 43;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "リクエストＵＲＬ選択";
            // 
            // rdo_url_add_EA
            // 
            this.rdo_url_add_EA.AutoSize = true;
            this.rdo_url_add_EA.Location = new System.Drawing.Point(102, 17);
            this.rdo_url_add_EA.Name = "rdo_url_add_EA";
            this.rdo_url_add_EA.Size = new System.Drawing.Size(83, 16);
            this.rdo_url_add_EA.TabIndex = 22;
            this.rdo_url_add_EA.TabStop = true;
            this.rdo_url_add_EA.Text = "add_EA.json";
            this.rdo_url_add_EA.UseVisualStyleBackColor = true;
            // 
            // rdo_url_request
            // 
            this.rdo_url_request.AutoSize = true;
            this.rdo_url_request.Location = new System.Drawing.Point(303, 14);
            this.rdo_url_request.Name = "rdo_url_request";
            this.rdo_url_request.Size = new System.Drawing.Size(88, 16);
            this.rdo_url_request.TabIndex = 21;
            this.rdo_url_request.Text = "Request.json";
            this.rdo_url_request.UseVisualStyleBackColor = true;
            // 
            // rdo_url_add_user
            // 
            this.rdo_url_add_user.AutoSize = true;
            this.rdo_url_add_user.Checked = true;
            this.rdo_url_add_user.Location = new System.Drawing.Point(6, 17);
            this.rdo_url_add_user.Name = "rdo_url_add_user";
            this.rdo_url_add_user.Size = new System.Drawing.Size(90, 16);
            this.rdo_url_add_user.TabIndex = 20;
            this.rdo_url_add_user.TabStop = true;
            this.rdo_url_add_user.Text = "add_user.json";
            this.rdo_url_add_user.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(195, 12);
            this.label4.TabIndex = 42;
            this.label4.Text = "結果ファイルはデスクトップに出力されます";
            // 
            // btn_http_get
            // 
            this.btn_http_get.Location = new System.Drawing.Point(357, 105);
            this.btn_http_get.Name = "btn_http_get";
            this.btn_http_get.Size = new System.Drawing.Size(75, 23);
            this.btn_http_get.TabIndex = 41;
            this.btn_http_get.Text = "実行";
            this.btn_http_get.UseVisualStyleBackColor = true;
            this.btn_http_get.Click += new System.EventHandler(this.btn_http_get_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Controls.Add(this.label29);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.btn_http_post);
            this.tabPage3.Controls.Add(this.txt_word);
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Controls.Add(this.txt_post_url);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.groupBox4);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(495, 269);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "デバッグ関連１";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 176);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 42;
            this.button1.Text = "btn_log_del";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(4, 161);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(88, 12);
            this.label29.TabIndex = 41;
            this.label29.Text = "logファイルの削除";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(235, 24);
            this.label5.TabIndex = 34;
            this.label5.Text = "パス指定されたjsonファイルをpostリクエストします\r\n空欄の場合は取得先の選択URLから取得します";
            // 
            // btn_http_post
            // 
            this.btn_http_post.Location = new System.Drawing.Point(359, 119);
            this.btn_http_post.Name = "btn_http_post";
            this.btn_http_post.Size = new System.Drawing.Size(75, 23);
            this.btn_http_post.TabIndex = 32;
            this.btn_http_post.Text = "http Post";
            this.btn_http_post.UseVisualStyleBackColor = true;
            this.btn_http_post.Click += new System.EventHandler(this.btn_http_post_Click);
            // 
            // txt_word
            // 
            this.txt_word.Location = new System.Drawing.Point(6, 35);
            this.txt_word.Name = "txt_word";
            this.txt_word.Size = new System.Drawing.Size(343, 19);
            this.txt_word.TabIndex = 33;
            this.txt_word.Text = "C:\\Users\\f464\\Desktop\\@test.txt";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdo_set_post_url);
            this.groupBox3.Controls.Add(this.rdo_kakuninn);
            this.groupBox3.Controls.Add(this.rdo_stc);
            this.groupBox3.Location = new System.Drawing.Point(215, 60);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(259, 36);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "post先の選択";
            // 
            // rdo_set_post_url
            // 
            this.rdo_set_post_url.AutoSize = true;
            this.rdo_set_post_url.Location = new System.Drawing.Point(178, 16);
            this.rdo_set_post_url.Name = "rdo_set_post_url";
            this.rdo_set_post_url.Size = new System.Drawing.Size(69, 16);
            this.rdo_set_post_url.TabIndex = 28;
            this.rdo_set_post_url.TabStop = true;
            this.rdo_set_post_url.Text = "URL指定";
            this.rdo_set_post_url.UseVisualStyleBackColor = true;
            // 
            // rdo_kakuninn
            // 
            this.rdo_kakuninn.AutoSize = true;
            this.rdo_kakuninn.Location = new System.Drawing.Point(109, 16);
            this.rdo_kakuninn.Name = "rdo_kakuninn";
            this.rdo_kakuninn.Size = new System.Drawing.Size(63, 16);
            this.rdo_kakuninn.TabIndex = 24;
            this.rdo_kakuninn.TabStop = true;
            this.rdo_kakuninn.Text = "確認くん";
            this.rdo_kakuninn.UseVisualStyleBackColor = true;
            // 
            // rdo_stc
            // 
            this.rdo_stc.AutoSize = true;
            this.rdo_stc.Checked = true;
            this.rdo_stc.Location = new System.Drawing.Point(6, 16);
            this.rdo_stc.Name = "rdo_stc";
            this.rdo_stc.Size = new System.Drawing.Size(93, 16);
            this.rdo_stc.TabIndex = 24;
            this.rdo_stc.TabStop = true;
            this.rdo_stc.Text = "シストレクラウド";
            this.rdo_stc.UseVisualStyleBackColor = true;
            // 
            // txt_post_url
            // 
            this.txt_post_url.Location = new System.Drawing.Point(112, 121);
            this.txt_post_url.Name = "txt_post_url";
            this.txt_post_url.Size = new System.Drawing.Size(243, 19);
            this.txt_post_url.TabIndex = 36;
            this.txt_post_url.Text = "http://192.168.33.10:8000/";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 124);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 12);
            this.label8.TabIndex = 37;
            this.label8.Text = "POST先URL指定";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdo_get_url_request);
            this.groupBox4.Controls.Add(this.rdo_get_url_add_user);
            this.groupBox4.Location = new System.Drawing.Point(6, 60);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(203, 36);
            this.groupBox4.TabIndex = 38;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "取得先のＵＲＬ選択";
            // 
            // rdo_get_url_request
            // 
            this.rdo_get_url_request.AutoSize = true;
            this.rdo_get_url_request.Location = new System.Drawing.Point(106, 17);
            this.rdo_get_url_request.Name = "rdo_get_url_request";
            this.rdo_get_url_request.Size = new System.Drawing.Size(88, 16);
            this.rdo_get_url_request.TabIndex = 21;
            this.rdo_get_url_request.Text = "Request.json";
            this.rdo_get_url_request.UseVisualStyleBackColor = true;
            // 
            // rdo_get_url_add_user
            // 
            this.rdo_get_url_add_user.AutoSize = true;
            this.rdo_get_url_add_user.Checked = true;
            this.rdo_get_url_add_user.Location = new System.Drawing.Point(6, 17);
            this.rdo_get_url_add_user.Name = "rdo_get_url_add_user";
            this.rdo_get_url_add_user.Size = new System.Drawing.Size(90, 16);
            this.rdo_get_url_add_user.TabIndex = 20;
            this.rdo_get_url_add_user.TabStop = true;
            this.rdo_get_url_add_user.Text = "add_user.json";
            this.rdo_get_url_add_user.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 102);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(375, 12);
            this.label9.TabIndex = 39;
            this.label9.Text = "psot先の選択でURL指定を選択した場合は下記にURLを必ず入力してください";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btn_Conn_Watch);
            this.tabPage4.Controls.Add(this.btn_csv_read);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Controls.Add(this.label16);
            this.tabPage4.Controls.Add(this.txt_startprogram);
            this.tabPage4.Controls.Add(this.txt_renameFile);
            this.tabPage4.Controls.Add(this.btn_reame);
            this.tabPage4.Controls.Add(this.btn_start);
            this.tabPage4.Controls.Add(this.btn_stop);
            this.tabPage4.Controls.Add(this.txt_stopprogram);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.txt_readFile);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Controls.Add(this.btn_readFile);
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(495, 269);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "デバッグ関連２";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btn_Conn_Watch
            // 
            this.btn_Conn_Watch.Location = new System.Drawing.Point(89, 240);
            this.btn_Conn_Watch.Name = "btn_Conn_Watch";
            this.btn_Conn_Watch.Size = new System.Drawing.Size(75, 23);
            this.btn_Conn_Watch.TabIndex = 60;
            this.btn_Conn_Watch.Text = "Conn Watch";
            this.btn_Conn_Watch.UseVisualStyleBackColor = true;
            this.btn_Conn_Watch.Click += new System.EventHandler(this.btn_Conn_Watch_Click);
            // 
            // btn_csv_read
            // 
            this.btn_csv_read.Location = new System.Drawing.Point(8, 240);
            this.btn_csv_read.Name = "btn_csv_read";
            this.btn_csv_read.Size = new System.Drawing.Size(75, 23);
            this.btn_csv_read.TabIndex = 59;
            this.btn_csv_read.Text = "CSV Read";
            this.btn_csv_read.UseVisualStyleBackColor = true;
            this.btn_csv_read.Click += new System.EventHandler(this.btn_csv_read_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 12);
            this.label1.TabIndex = 50;
            this.label1.Text = "スタートさせたいプログラムの絶対パスを入力してください";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(8, 195);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(102, 12);
            this.label16.TabIndex = 58;
            this.label16.Text = "ファイルリネームテスト";
            // 
            // txt_startprogram
            // 
            this.txt_startprogram.Location = new System.Drawing.Point(8, 26);
            this.txt_startprogram.Name = "txt_startprogram";
            this.txt_startprogram.Size = new System.Drawing.Size(343, 19);
            this.txt_startprogram.TabIndex = 46;
            this.txt_startprogram.Text = "C:\\Program Files (x86)\\MetaTrader 4\\terminal.exe";
            // 
            // txt_renameFile
            // 
            this.txt_renameFile.Location = new System.Drawing.Point(8, 210);
            this.txt_renameFile.Name = "txt_renameFile";
            this.txt_renameFile.Size = new System.Drawing.Size(343, 19);
            this.txt_renameFile.TabIndex = 57;
            this.txt_renameFile.Text = "C:\\Users\\f464\\Desktop\\request.json";
            // 
            // btn_reame
            // 
            this.btn_reame.Location = new System.Drawing.Point(361, 208);
            this.btn_reame.Name = "btn_reame";
            this.btn_reame.Size = new System.Drawing.Size(75, 23);
            this.btn_reame.TabIndex = 56;
            this.btn_reame.Text = "rename";
            this.btn_reame.UseVisualStyleBackColor = true;
            this.btn_reame.Click += new System.EventHandler(this.btn_reame_Click);
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(357, 26);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(75, 23);
            this.btn_start.TabIndex = 47;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(357, 67);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 23);
            this.btn_stop.TabIndex = 48;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // txt_stopprogram
            // 
            this.txt_stopprogram.Location = new System.Drawing.Point(8, 67);
            this.txt_stopprogram.Name = "txt_stopprogram";
            this.txt_stopprogram.Size = new System.Drawing.Size(343, 19);
            this.txt_stopprogram.TabIndex = 49;
            this.txt_stopprogram.Text = "Ava";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(369, 12);
            this.label2.TabIndex = 51;
            this.label2.Text = "停止させたいプログラムのウィンドウタイトルに含まれる文字列を入力してください";
            // 
            // txt_readFile
            // 
            this.txt_readFile.Location = new System.Drawing.Point(8, 126);
            this.txt_readFile.Name = "txt_readFile";
            this.txt_readFile.Size = new System.Drawing.Size(343, 19);
            this.txt_readFile.TabIndex = 52;
            this.txt_readFile.Text = "C:\\Users\\f464\\Desktop\\request.json";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(364, 24);
            this.label3.TabIndex = 53;
            this.label3.Text = "読込対象のjsonファイルパスを指定してください\r\n正常処理された場合、読み込みファイルと同一のフォルダに結果を出力します\r\n";
            // 
            // btn_readFile
            // 
            this.btn_readFile.Location = new System.Drawing.Point(357, 160);
            this.btn_readFile.Name = "btn_readFile";
            this.btn_readFile.Size = new System.Drawing.Size(75, 23);
            this.btn_readFile.TabIndex = 54;
            this.btn_readFile.Text = "read file";
            this.btn_readFile.UseVisualStyleBackColor = true;
            this.btn_readFile.Click += new System.EventHandler(this.btn_readFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdo_add_user);
            this.groupBox1.Controls.Add(this.rdo_request);
            this.groupBox1.Location = new System.Drawing.Point(8, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 37);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "読み込みファイル形式選択";
            // 
            // rdo_add_user
            // 
            this.rdo_add_user.AutoSize = true;
            this.rdo_add_user.Checked = true;
            this.rdo_add_user.Location = new System.Drawing.Point(6, 18);
            this.rdo_add_user.Name = "rdo_add_user";
            this.rdo_add_user.Size = new System.Drawing.Size(90, 16);
            this.rdo_add_user.TabIndex = 12;
            this.rdo_add_user.TabStop = true;
            this.rdo_add_user.Text = "add_user.json";
            this.rdo_add_user.UseVisualStyleBackColor = true;
            // 
            // rdo_request
            // 
            this.rdo_request.AutoSize = true;
            this.rdo_request.Location = new System.Drawing.Point(102, 18);
            this.rdo_request.Name = "rdo_request";
            this.rdo_request.Size = new System.Drawing.Size(88, 16);
            this.rdo_request.TabIndex = 13;
            this.rdo_request.Text = "Request.json";
            this.rdo_request.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.txt_create_folders);
            this.tabPage5.Controls.Add(this.btn_get_folders);
            this.tabPage5.Controls.Add(this.btn_create_folders);
            this.tabPage5.Controls.Add(this.label18);
            this.tabPage5.Controls.Add(this.label17);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(495, 269);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "復旧関連";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // txt_create_folders
            // 
            this.txt_create_folders.Location = new System.Drawing.Point(6, 50);
            this.txt_create_folders.Name = "txt_create_folders";
            this.txt_create_folders.Size = new System.Drawing.Size(343, 19);
            this.txt_create_folders.TabIndex = 61;
            this.txt_create_folders.Text = "C:\\STC_controller_log\\Folder_log\\";
            // 
            // btn_get_folders
            // 
            this.btn_get_folders.Location = new System.Drawing.Point(126, 7);
            this.btn_get_folders.Name = "btn_get_folders";
            this.btn_get_folders.Size = new System.Drawing.Size(75, 23);
            this.btn_get_folders.TabIndex = 59;
            this.btn_get_folders.Text = "get_folders";
            this.btn_get_folders.UseVisualStyleBackColor = true;
            this.btn_get_folders.Click += new System.EventHandler(this.btn_get_folders_Click);
            // 
            // btn_create_folders
            // 
            this.btn_create_folders.Location = new System.Drawing.Point(355, 46);
            this.btn_create_folders.Name = "btn_create_folders";
            this.btn_create_folders.Size = new System.Drawing.Size(88, 23);
            this.btn_create_folders.TabIndex = 60;
            this.btn_create_folders.Text = "create_folders";
            this.btn_create_folders.UseVisualStyleBackColor = true;
            this.btn_create_folders.Click += new System.EventHandler(this.btn_create_folders_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 12);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(114, 12);
            this.label18.TabIndex = 63;
            this.label18.Text = "フォルダ手動バックアップ";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 33);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(129, 12);
            this.label17.TabIndex = 62;
            this.label17.Text = "フォルダ復旧用ファイルパス";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.btn_find_MT4);
            this.tabPage6.Controls.Add(this.cmb_EA_ID);
            this.tabPage6.Controls.Add(this.label28);
            this.tabPage6.Controls.Add(this.cmb_Time_Period);
            this.tabPage6.Controls.Add(this.label27);
            this.tabPage6.Controls.Add(this.cmb_CCY);
            this.tabPage6.Controls.Add(this.label26);
            this.tabPage6.Controls.Add(this.cmb_mt4ID);
            this.tabPage6.Controls.Add(this.label25);
            this.tabPage6.Controls.Add(this.cmb_mt4SV);
            this.tabPage6.Controls.Add(this.label24);
            this.tabPage6.Controls.Add(this.btn_getUser);
            this.tabPage6.Controls.Add(this.label23);
            this.tabPage6.Controls.Add(this.cmb_stcUser);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(495, 269);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "対象MT4検索";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // btn_find_MT4
            // 
            this.btn_find_MT4.Location = new System.Drawing.Point(210, 167);
            this.btn_find_MT4.Name = "btn_find_MT4";
            this.btn_find_MT4.Size = new System.Drawing.Size(75, 23);
            this.btn_find_MT4.TabIndex = 13;
            this.btn_find_MT4.Text = "検索実行";
            this.btn_find_MT4.UseVisualStyleBackColor = true;
            this.btn_find_MT4.Click += new System.EventHandler(this.btn_find_MT4_Click);
            // 
            // cmb_EA_ID
            // 
            this.cmb_EA_ID.FormattingEnabled = true;
            this.cmb_EA_ID.Location = new System.Drawing.Point(109, 103);
            this.cmb_EA_ID.Name = "cmb_EA_ID";
            this.cmb_EA_ID.Size = new System.Drawing.Size(72, 20);
            this.cmb_EA_ID.TabIndex = 12;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(107, 88);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(35, 12);
            this.label28.TabIndex = 11;
            this.label28.Text = "EA_ID";
            // 
            // cmb_Time_Period
            // 
            this.cmb_Time_Period.FormattingEnabled = true;
            this.cmb_Time_Period.Location = new System.Drawing.Point(6, 103);
            this.cmb_Time_Period.Name = "cmb_Time_Period";
            this.cmb_Time_Period.Size = new System.Drawing.Size(97, 20);
            this.cmb_Time_Period.TabIndex = 10;
            this.cmb_Time_Period.SelectedIndexChanged += new System.EventHandler(this.cmb_Time_Period_SelectedIndexChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 88);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(66, 12);
            this.label27.TabIndex = 9;
            this.label27.Text = "Time_Period";
            // 
            // cmb_CCY
            // 
            this.cmb_CCY.FormattingEnabled = true;
            this.cmb_CCY.Location = new System.Drawing.Point(389, 48);
            this.cmb_CCY.Name = "cmb_CCY";
            this.cmb_CCY.Size = new System.Drawing.Size(100, 20);
            this.cmb_CCY.TabIndex = 8;
            this.cmb_CCY.SelectedIndexChanged += new System.EventHandler(this.cmb_CCY_SelectedIndexChanged);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(387, 33);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(28, 12);
            this.label26.TabIndex = 7;
            this.label26.Text = "CCY";
            // 
            // cmb_mt4ID
            // 
            this.cmb_mt4ID.FormattingEnabled = true;
            this.cmb_mt4ID.Location = new System.Drawing.Point(263, 48);
            this.cmb_mt4ID.Name = "cmb_mt4ID";
            this.cmb_mt4ID.Size = new System.Drawing.Size(120, 20);
            this.cmb_mt4ID.TabIndex = 6;
            this.cmb_mt4ID.SelectedIndexChanged += new System.EventHandler(this.cmb_mt4ID_SelectedIndexChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(261, 33);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(48, 12);
            this.label25.TabIndex = 5;
            this.label25.Text = "MT4のID";
            // 
            // cmb_mt4SV
            // 
            this.cmb_mt4SV.FormattingEnabled = true;
            this.cmb_mt4SV.Location = new System.Drawing.Point(109, 48);
            this.cmb_mt4SV.Name = "cmb_mt4SV";
            this.cmb_mt4SV.Size = new System.Drawing.Size(148, 20);
            this.cmb_mt4SV.TabIndex = 4;
            this.cmb_mt4SV.SelectedIndexChanged += new System.EventHandler(this.cmb_mt4SV_SelectedIndexChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(107, 33);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(103, 12);
            this.label24.TabIndex = 3;
            this.label24.Text = "接続先MT4サーバー";
            // 
            // btn_getUser
            // 
            this.btn_getUser.Location = new System.Drawing.Point(6, 6);
            this.btn_getUser.Name = "btn_getUser";
            this.btn_getUser.Size = new System.Drawing.Size(111, 23);
            this.btn_getUser.TabIndex = 2;
            this.btn_getUser.Text = "STCユーザーID抽出";
            this.btn_getUser.UseVisualStyleBackColor = true;
            this.btn_getUser.Click += new System.EventHandler(this.btn_getUser_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(4, 33);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(78, 12);
            this.label23.TabIndex = 1;
            this.label23.Text = "STCユーザーID";
            // 
            // cmb_stcUser
            // 
            this.cmb_stcUser.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_stcUser.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_stcUser.FormattingEnabled = true;
            this.cmb_stcUser.Location = new System.Drawing.Point(6, 48);
            this.cmb_stcUser.Name = "cmb_stcUser";
            this.cmb_stcUser.Size = new System.Drawing.Size(97, 20);
            this.cmb_stcUser.TabIndex = 0;
            this.cmb_stcUser.SelectedIndexChanged += new System.EventHandler(this.cmb_stcUser_SelectedIndexChanged);
            // 
            // tmr_conn_watch
            // 
            this.tmr_conn_watch.Interval = 6000;
            this.tmr_conn_watch.Tick += new System.EventHandler(this.tmr_conn_watch_Tick);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(354, 72);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(75, 12);
            this.label30.TabIndex = 62;
            this.label30.Text = "MT4起動引数";
            // 
            // tgl_MT4_option
            // 
            this.tgl_MT4_option.Appearance = System.Windows.Forms.Appearance.Button;
            this.tgl_MT4_option.AutoSize = true;
            this.tgl_MT4_option.Location = new System.Drawing.Point(356, 112);
            this.tgl_MT4_option.Name = "tgl_MT4_option";
            this.tgl_MT4_option.Size = new System.Drawing.Size(101, 22);
            this.tgl_MT4_option.TabIndex = 63;
            this.tgl_MT4_option.Text = "MT4 option false";
            this.tgl_MT4_option.UseVisualStyleBackColor = true;
            this.tgl_MT4_option.CheckedChanged += new System.EventHandler(this.tgl_MT4_option_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(528, 315);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "STC_controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmr_add_user;
        private System.Windows.Forms.Timer tmr_rquest;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.CheckBox tgl_socket;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txt_request_interval;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_add_user_interval;
        private System.Windows.Forms.CheckBox tgl_reuest;
        private System.Windows.Forms.CheckBox tgl_add_user;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_out_put;
        private System.Windows.Forms.TextBox txt_get_url;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdo_url_request;
        private System.Windows.Forms.RadioButton rdo_url_add_user;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_http_get;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_http_post;
        private System.Windows.Forms.TextBox txt_word;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdo_set_post_url;
        private System.Windows.Forms.RadioButton rdo_kakuninn;
        private System.Windows.Forms.RadioButton rdo_stc;
        private System.Windows.Forms.TextBox txt_post_url;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rdo_get_url_request;
        private System.Windows.Forms.RadioButton rdo_get_url_add_user;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_csv_read;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txt_startprogram;
        private System.Windows.Forms.TextBox txt_renameFile;
        private System.Windows.Forms.Button btn_reame;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.TextBox txt_stopprogram;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_readFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_readFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdo_add_user;
        private System.Windows.Forms.RadioButton rdo_request;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox txt_create_folders;
        private System.Windows.Forms.Button btn_get_folders;
        private System.Windows.Forms.Button btn_create_folders;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btn_Conn_Watch;
        private System.Windows.Forms.Timer tmr_conn_watch;
        private System.Windows.Forms.CheckBox tgl_MT4_watch;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.RadioButton rdo_url_add_EA;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btn_recover;
        private System.Windows.Forms.RadioButton rdo_real;
        private System.Windows.Forms.RadioButton rdo_stg;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label lbl_Machine_Name;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Button btn_getUser;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox cmb_stcUser;
        private System.Windows.Forms.ComboBox cmb_mt4SV;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox cmb_mt4ID;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.ComboBox cmb_CCY;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.ComboBox cmb_Time_Period;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.ComboBox cmb_EA_ID;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Button btn_find_MT4;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox tgl_MT4_option;
        private System.Windows.Forms.Label label30;
    }
}


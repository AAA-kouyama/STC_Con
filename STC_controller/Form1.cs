using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Codeplex.Data;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace STC_controller
{
    public partial class MainForm : Form
    {

        // private static string test_server_url = "https://stc.rool.ml/"; 
        private static string test_server_url = "https://218.222.227.232/";
        private static string real_server_url = "https://systrade-cloud.com/";
        //private static string real_server_url = "https://www.systrade-cloud.net/";

        // get先URL(rdo_stg_CheckedChanged()にて設定)
        private static string add_user_url = "";
        private static string request = "";
        private static string add_EA_url = "";
        private static string view_param_url = "";

        // post先URL(rdo_stg_CheckedChanged()にて設定)
        private static string upload = "";
        public static string resultbox = "";

        // 監視アラート用post先URL(rdo_stg_CheckedChanged()にて設定)
        //ea_alert.php
        public static string ea_alert = "";

        private retry_timer Retry_Timer;

        public static string machine_name = "";

        // MT4起動オプション引数格納用
        public static bool mt4_opt = true;

        // response時vol_1shotを強制的にゼロにするEA名称とbool値の文字列
        public static Dictionary<string, string> vol_1shot_zero = null;

        // 監視受入ポート
        public static int port_no = 0;

        public MainForm()
        {
            InitializeComponent();
            //起動後にキュータイマー稼働
            Retry_Timer = new retry_timer();
            Retry_Timer.NewTimer();
            Retry_Timer.StartTimer();

            // テストサーバーへの接続を設定
            rdo_stg.Checked = true;

            // マシン名のファイル読み込み
            // con_setting.txtに設定ファイルを統一
            // machine_name = File_CTRL.file_Read(@"./Machine_Name.txt");
            // lbl_Machine_Name.Text = machine_name;

            // 起動時の設定読み込み
            Dictionary<string, string> dict = File_CTRL.csv_reader(@"./con_setting.txt");

            // 各種起動時動作を行う
            foreach (var pair in dict)
            {
                switch (pair.Key)
                {
                    case "machine_name": // マシン名のファイル読み込み サーバー名文字列
                        System.Console.WriteLine(pair.Value);
                        machine_name = pair.Value;
                        lbl_Machine_Name.Text = pair.Value;
                        break;

                    case "real_server_url": // 本番接続先を設定フェイルから読み出します。
                        if (pair.Value != "")
                        {
                            real_server_url = pair.Value;
                        }
                        break;

                    case "stc_server": // STC_CONの接続先サーバー test…STG環境、real…本番環境
                        System.Console.WriteLine(pair.Value);
                        switch (pair.Value)
                        {
                            case "test": // STG環境へ接続
                                rdo_stg.Checked = true;
                                break;

                            case "real": // 本番環境へ接続
                                rdo_real.Checked = true;
                                break;

                            default: // 当てはまらない場合はSTG環境へ接続
                                rdo_stg.Checked = true;
                                break;
                        }

                        break;

                    case "recover_start": // 起動時実行状態再現 true…再現実行、false…再現はしない
                        System.Console.WriteLine(pair.Value);
                        if (pair.Value == "true")
                        {
                            MT4_CTLR.recover_MT4();
                        }
                        break;
                    case "request_timer": // リクエストポーリングタイマー true…起動時実行、false…起動時実行しない
                        System.Console.WriteLine(pair.Value);
                        if (pair.Value == "true")
                        {
                            tgl_reuest.Checked = true;
                        } else
                        {
                            tgl_reuest.Checked = false;
                        }

                        break;
                    case "sock_connect": // 監視ソケット true…起動時受入状態、false…起動時受け入れない
                        System.Console.WriteLine(pair.Value);

                        if (int.TryParse(pair.Value, out port_no)){
                            tgl_socket.Checked = true;
                        }
                        else
                        {
                            tgl_socket.Checked = false;
                        }

                        break;
                    case "MT4_watch": // MT4接続監視 true…監視する、false…監視しない
                        System.Console.WriteLine(pair.Value);
                        if (pair.Value == "true")
                        {
                            tgl_MT4_watch.Checked = true;
                            mt4_opt = true;
                        }
                        else
                        {
                            tgl_MT4_watch.Checked = false;
                            mt4_opt = false;
                        }
                        break;
                    case "MT4_option": // MT4起動時オプション /portable /skipupdate true…有効、false…無効
                        System.Console.WriteLine(pair.Value);
                        if (pair.Value == "true")
                        {
                            tgl_MT4_option.Checked = true;
                        }
                        else
                        {
                            tgl_MT4_option.Checked = false;
                        }
                        break;
                    default:
                        // 設定ファイル上の指示がおかしい場合
                        break;
                }

            }

            // EA毎のvol1応答の強制書き換え設定の取得
            get_vol1_setting(); 

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            program_CTRL.StartProgram(txt_startprogram.Text);
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            program_CTRL.StopProgram(txt_stopprogram.Text);
        }

        private void btn_readFile_Click(object sender, EventArgs e)
        {
            // ファイル読み込み
            string readed_file = File_CTRL.file_Read(txt_readFile.Text);
            // 各種初期化
            dynamic json_obj = "";
            bool status = false;
            string error = "";
            string out_put_file_name = "";

            if (readed_file.Length > 0)
            {
                if (rdo_add_user.Checked)
                {
                    json_obj = Json_add_user_action.add_user_motion(readed_file, out status, out error);
                    out_put_file_name = "end_init.json";
                }

                if (rdo_request.Checked)
                {
                    json_obj = Json_request_action.request_motion(readed_file, out status, out error);
                    out_put_file_name = "Response.json";

                }

                if (json_obj != null)
                {
                    string file_path = System.IO.Path.GetDirectoryName(txt_readFile.Text) + "\\" + out_put_file_name;

                    File_CTRL.file_OverWrite(json_obj.ToString(), file_path);
                    //MessageBox.Show("結果ファイル出力完了！");

                }
            }
        }

        private async void btn_http_post_Click(object sender, EventArgs e)
        {

            //if (txt_word.Text != "")
            //{
            //    if (!System.IO.File.Exists(txt_word.Text))
            //    {
            //        MessageBox.Show("ファイルが見つかりません！", "ファイル存在チェック", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //}
            

            string request_string = "";
            bool status = false;
            if (txt_word.Text != "")
            {
                // ファイル読み込み
                request_string = File_CTRL.file_Read(txt_word.Text);
            }
            else
            {
                string get_url = "";

                if (rdo_get_url_add_user.Checked)
                {
                    get_url = add_user_url;
                } 
                else
                {
                    get_url = request;
                }

                request_string = http_Request_CTRL.http_get(get_url, out status);
                if (!status)
                {
                    MessageBox.Show(request_string);
                    return;
                }
            }

            string post_url = "";
            if (rdo_stc.Checked)
            {
                if (rdo_get_url_add_user.Checked)
                {
                    post_url = upload; 
                }
                else
                {
                    post_url = resultbox;
                }
            }
            if (rdo_kakuninn.Checked)
            {
                post_url = "http://www.kojikoji.net/";
            }
            if (rdo_set_post_url.Checked)
            {
                post_url = txt_post_url.Text;
            }

            if (post_url == "")
            {
                MessageBox.Show("post url が未指定です！", "post対象チェック", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // 各種初期化
            dynamic json_obj = "";
            //bool status = false;
            string error = "";
            if (rdo_get_url_add_user.Checked)
            {
                json_obj = Json_add_user_action.add_user_motion(request_string, out status, out error);
            }
            else
            {
                json_obj = Json_request_action.request_motion(request_string, out status, out error);
            }
            
            if (status)
            {
                if (json_obj != null)
                {

                    //Console.WriteLine("呼出し:" + MT4_CTLR.operation(json_obj));

                    MessageBox.Show("Http Post開始！");
                    string result = await http_Request_CTRL.http_post(json_obj.ToString(), post_url);
                    string file_path = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\結果.html";
                    File_CTRL.file_OverWrite(result, file_path);
                    MessageBox.Show("Http Post終了！結果.htmlが生成されました");
                }
                else
                {
                    MessageBox.Show("postするべきデータが作れませんでした");
                }
            }

        }

        private void btn_http_get_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(txt_out_put.Text))
            {
                MessageBox.Show("指定されたフォルダが見当たりません！デスクトップの所定フォルダに強制設定します！");
                // パスをend_initとend_ea_add用のフォルダにします
                string output_path = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                if (rdo_url_add_user.Checked)
                {
                    // ユーザー追加
                    output_path += @"\end_init";
                }
                else if (rdo_url_add_EA.Checked)
                {
                    // EA追加
                    output_path += @"\end_ea_add";
                }

                txt_out_put.Text = output_path;
            }
            // URL用文字列
            string request_machine = "";
            string order_url = "";
            string add_user = "";
            string add_EA = "";

            if (txt_get_url.Text == "")
            {
                add_user = add_user_url;
                request_machine = request;
                add_EA = add_EA_url;
            }
            else
            {
                add_user = txt_get_url.Text;
                request_machine = txt_get_url.Text;
                add_EA = txt_get_url.Text;
            }

            // ラジオボタンはadd_userだけだが取り敢えずrequest_machineも分岐してみる
            if (rdo_url_add_user.Checked)
            {
                // ユーザー追加
                order_url = add_user;
            }
            else if(rdo_url_request.Checked)
            {
                // 操作リクエスト
                order_url = request_machine;
            }
            else if (rdo_url_add_EA.Checked)
            {
                // EA追加
                order_url = add_EA;
            }

            dynamic json_obj = "";

            bool status = false;
            string error = "";

            string result = http_Request_CTRL.http_get(order_url,out status);

            if (status)
            {
                

                string out_put_file_name = "";

                if (rdo_url_add_user.Checked || rdo_url_add_EA.Checked) {

                    add_mt4_folder(result);

                    /*
                    json_obj = Json_add_user_action.add_user_motion(result, out status, out error);

                    if (json_obj == null)
                    {
                        MessageBox.Show("対象データが取得できませんでした！con_setting.txtの記載順でreal_server_urlが2行目ですか？");
                        return;
                    }
                    /*
                    //jsonを1レコード単位に分解してファイル出力する
                    foreach (dynamic read_user in (object[])json_obj)
                    {
                         string created_user = read_user.Stc_ID + "_" 
                                            + read_user.MT4_Server + "_" 
                                            + read_user.MT4_ID + "_" 
                                            + read_user.Ccy + "_" 
                                            + read_user.Time_Period + "_" 
                                            + read_user.EA_ID;

                        System.Console.WriteLine(created_user);

                        if (rdo_url_add_user.Checked)
                        {
                            // ユーザー追加
                            out_put_file_name = "ei_" + created_user + ".json";
                        }
                        else if (rdo_url_add_EA.Checked)
                        {
                            // EA追加
                            out_put_file_name = "ea_" + created_user + ".json";
                        }

                        if (read_user != null)
                        {
                            File_CTRL.file_OverWrite("[" + read_user.ToString() + "]", txt_out_put.Text + "\\" + out_put_file_name);

                        }

                    }
                    */
                    /*
                    // 登録結果としてインストールフォルダのログを出力して保管
                    File_CTRL.get_folders();

                    MessageBox.Show("結果ファイル出力完了！");

                    */
                }
                else
                {
                    json_obj = Json_request_action.request_motion(result, out status, out error);
                    out_put_file_name = "Response.json";

                    if (json_obj != null)
                    {

                        File_CTRL.file_OverWrite(json_obj.ToString(), txt_out_put.Text + "\\" + out_put_file_name);
                        MessageBox.Show("結果ファイル出力完了！");
                    }
                }

            }
            else
            {
                MessageBox.Show(result);
            }
        }


        private async void tmr_add_user_Tick(object sender, EventArgs e)
        {
            // add_user取得先URL
            string get_url = add_user_url;
            // end_init登録先URL
            string post_url = upload;
            bool status = false;
            dynamic json_obj = "";
            string error = "";
            // add_user取得

            string request_string = http_Request_CTRL.http_get(get_url, out status);

            // 処理ステータス確認
            if (!status)
            {
                return;
            }
            // 取得情報チェックと処理結果生成
            json_obj = Json_add_user_action.add_user_motion(request_string, out status, out error);

            if (status)
            {
                if (json_obj != null)
                {
                    // 処理結果登録
                    await http_Request_CTRL.http_post(json_obj.ToString(), post_url);
                }

            }

        }

        private async void tmr_rquest_Tick(object sender, EventArgs e)
        {
            // request取得先URL
            string get_url = request;
            // response登録先URL
            string post_url = resultbox;
            bool status = false;
            dynamic json_obj = "";
            string error = "";
            // add_user取得
            string request_string = http_Request_CTRL.http_get(get_url, out status);
            // 処理ステータス確認
            if (!status)
            {
                return;
            }
            // 取得情報チェックと処理結果生成
            json_obj = Json_request_action.request_motion(request_string, out status, out error);

            if (status)
            {
                if (json_obj != null)
                {
                    // 処理結果登録
                    await http_Request_CTRL.http_post(json_obj.ToString(), post_url);
                }

            }
        }

        private void tgl_add_user_CheckedChanged(object sender, EventArgs e)
        {
            tmr_add_user.Enabled = tgl_add_user.Checked;
            tmr_add_user.Interval = Int32.Parse(txt_add_user_interval.Text);
            tgl_add_user.Text = "add_userタイマー " + tgl_add_user.Checked.ToString();
        }

        private void tgl_reuest_CheckedChanged(object sender, EventArgs e)
        {
            tmr_rquest.Enabled = tgl_reuest.Checked;
            tmr_rquest.Interval = Int32.Parse(txt_request_interval.Text);
            tgl_reuest.Text = "requestタイマー " + tgl_reuest.Checked.ToString();
        }

        // ソケット生成
        private System.Net.Sockets.TcpClient objSck = new System.Net.Sockets.TcpClient();
        private System.Net.Sockets.NetworkStream objStm;

        private void tgl_socket_CheckedChanged(object sender, EventArgs e)
        {
            tgl_socket.Text = "sock " + tgl_socket.Checked.ToString();

            if (tgl_socket.Checked)
            {
                if (!open_sock())
                {
                    tgl_socket.Text = "sock fail!";
                    MessageBox.Show("con_setting.txtのsock_connectにポート番号を設定してください");
                }
                    
            }
            else
            {
                close_sock();
            }


        }

        // ソケット・リスナー
        private TcpListener myListener;
        // クライアント送受信
        private ClientTcpIp[] myClient = new ClientTcpIp[4];

        Thread myServerThread;

        private bool open_sock()
        {
            // IPアドレス＆ポート番号設定
            int myPort = 56789;

            if (port_no > 0)
            {
                myPort = port_no;

                // どこからでも接続OK
                IPEndPoint myEndPoint = new IPEndPoint(IPAddress.Any, myPort);

                // リスナー開始
                myListener = new TcpListener(myEndPoint);
                myListener.Start();

                // クライアント接続待ち開始
                //myServerThread = new Thread(new ThreadStart(ServerThread));
                myServerThread = new Thread(new ThreadStart(ServerThread));
                myServerThread.Start();

                return true;
            }

            return false;

        }


        private void close_sock()
        {
            try
            {
                // リスナー終了
                myListener.Stop();
                // クライアント切断
                for (int i = 0; i <= myClient.GetLength(0) - 1; i++)
                {
                    if (myClient[i] != null)
                    {
                        if (myClient[i].objSck.Connected == true)
                        {
                            // ソケットクローズ
                            myClient[i].objStm.Close();
                            myClient[i].objSck.Close();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

        }

        // クライアント接続待ちスレッド
        private void ServerThread()
        {
            try
            {
                int intNo;
                while (true)
                {
                    // ソケット接続待ち
                    TcpClient myTcpClient = myListener.AcceptTcpClient();
                    // クライアントから接続有り
                    for (intNo = 0; intNo <= myClient.GetLength(0) - 1; intNo++)
                    {
                        if (myClient[intNo] == null)
                        {
                            break;
                        }
                        else if (myClient[intNo].objSck.Connected == false)
                        {
                            break;
                        }
                    }
                    if (intNo < myClient.GetLength(0))
                    {
                        // クライアント送受信オブジェクト生成
                        myClient[intNo] = new ClientTcpIp();
                        myClient[intNo].intNo = intNo + 1;
                        myClient[intNo].objSck = myTcpClient;
                        myClient[intNo].objStm = myTcpClient.GetStream();
                        // クライアントとの送受信開始
                        Thread myClientThread = new Thread(
                            new ThreadStart(myClient[intNo].ReadWrite));
                        myClientThread.Start();
                    }
                    else
                    {
                        // 接続拒否
                        myTcpClient.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            close_sock();
        }

        private void btn_reame_Click(object sender, EventArgs e)
        {
            File_CTRL.file_rename(txt_renameFile.Text, "request.json", "@terminal.exe");
        }

        private void btn_get_folders_Click(object sender, EventArgs e)
        {
            File_CTRL.get_folders();
            MessageBox.Show("フォルダバックアップ完了！ C:\\STC_controller_log\\Folder_logを確認してください。", "フォルダバックアップ完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_create_folders_Click(object sender, EventArgs e)
        {
            File_CTRL.recover_folder(txt_create_folders.Text);
            MessageBox.Show("フォルダ復旧完了！ C:\\Users\\GFITを確認してください。", "フォルダ復旧完了", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string direcotryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Console.WriteLine(direcotryPath);
        }

        private void btn_csv_read_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> dict = File_CTRL.csv_reader("all_setting.txt");
                
                dict[@"C:\Users\GFIT\u00001105\┗Ava-Demo\5997487\USDJPY\H1\ReloadTemplateEA.mq4"] = "AAA";

                System.Console.WriteLine(File_CTRL.csv_writer("all_setting.txt", dict));
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void btn_Conn_Watch_Click(object sender, EventArgs e)
        {
            MT4_CTLR.connect_watch();
        }

        private void tmr_conn_watch_Tick(object sender, EventArgs e)
        {
            MT4_CTLR.connect_watch();
        }

        private void tgl_MT4_watch_CheckedChanged(object sender, EventArgs e)
        {
            //
            tmr_conn_watch.Enabled = tgl_MT4_watch.Checked;
            tmr_conn_watch.Interval = Int32.Parse(txt_watch_interval.Text);
            tgl_MT4_watch.Text = "watch " + tgl_MT4_watch.Checked.ToString();
        }

        private void btn_recover_Click(object sender, EventArgs e)
        {
            if (!tgl_reuest.Checked)
            {
                MT4_CTLR.recover_MT4();
                MessageBox.Show("動作が完了しました。\n\r" + @"C:\STC_controller_log\Error_Logs" + "のエラーログに\n\rエラーが出ていないか確認してください。");
            }
            else
            {
                MessageBox.Show("リクエスト受信中は動作できません！");
            }
        }

        private void rdo_stg_CheckedChanged(object sender, EventArgs e)
        {

            if(rdo_stg.Checked == true)
            {
                this.BackColor = System.Drawing.Color.DarkBlue;
                // get先URL
                add_user_url = test_server_url + "SV/admin/add_user.php";
                request = test_server_url + "SV/admin/request.php";
                add_EA_url = test_server_url + "SV/admin/add_ea.php";
                view_param_url = test_server_url + "SV/admin/view_ea_param.php";


                // post先URL
                upload = test_server_url + "SV/admin/upload.php";
                resultbox = test_server_url + "SV/admin/resultbox.php";
                ea_alert = test_server_url + "SV/admin/ea_alert.php";

            }
            else
            {
                this.BackColor = System.Drawing.Color.Green;
                // get先URL
                add_user_url = real_server_url + "SV/admin/add_user.php";
                request = real_server_url + "SV/admin/request.php";
                add_EA_url = real_server_url + "SV/admin/add_ea.php";
                view_param_url = real_server_url + "SV/admin/view_ea_param.php";

                // post先URL
                upload = real_server_url + "SV/admin/upload.php";
                resultbox = real_server_url + "SV/admin/resultbox.php";
                ea_alert = real_server_url + "SV/admin/ea_alert.php";
            }
        }


        /// <summary>
        /// フォルダ情報から検索用の情報を取得して指定のコンボボックスへアイテム追加
        /// </summary>
        /// <param name="sender">アイテム登録対象のコンボボックス</param>
        /// <param name="folder_name">検索する対象のフォルダまでの絶対パス</param>
        private void set_cmb_data(object sender, string folder_name)
        {
            string search_str = ""; // ユーザーフォルダの直下は"****"だけ検索させてます。

            if (folder_name != "")
            {
                folder_name = @"\" + folder_name;
                search_str = "*";
            }
            else
            {
                search_str = "*";
            }

            if (sender != null)
            {
                System.Windows.Forms.ComboBox cmb_target = (System.Windows.Forms.ComboBox)sender;

                string path_string = @"C:\Users\GFIT" + folder_name;
                IEnumerable<string> subFolders = System.IO.Directory.EnumerateDirectories(path_string, search_str, System.IO.SearchOption.TopDirectoryOnly);

                // STCユーザーIDの設定前に一旦クリア
                cmb_target.Items.Clear();

                path_string = path_string + @"\";
                //サブフォルダを列挙する
                foreach (string subFolder in subFolders)
                {
                    // 列挙されたユーザーをcomboboxに設定
                    cmb_target.Items.Add(subFolder.Replace(path_string, ""));
                }
            }

        }

        private void clear_cmb(object sender)
        {
            System.Windows.Forms.ComboBox cmb_target = (System.Windows.Forms.ComboBox)sender;
            cmb_target.Items.Clear();
            cmb_target.Text = "";
        }

        private void btn_getUser_Click(object sender, EventArgs e)
        {

            set_cmb_data(cmb_stcUser, "");
            cmb_stcUser.Text = "";
            clear_cmb(cmb_mt4SV);
            clear_cmb(cmb_mt4ID);
            clear_cmb(cmb_CCY);
            clear_cmb(cmb_Time_Period);
            clear_cmb(cmb_EA_ID);
            MessageBox.Show("STCユーザー抽出完了\n\r引き続きコンボボックスで対象を選んでください", "起動中のMT4検索", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void cmb_stcUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_cmb_data(cmb_mt4SV, cmb_stcUser.SelectedItem.ToString());
            cmb_mt4SV.Text = "";
            clear_cmb(cmb_mt4ID);
            clear_cmb(cmb_CCY);
            clear_cmb(cmb_Time_Period);
            clear_cmb(cmb_EA_ID);
        }

        private void cmb_mt4SV_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_cmb_data(cmb_mt4ID, cmb_stcUser.SelectedItem.ToString() + @"\" + cmb_mt4SV.SelectedItem.ToString());
            cmb_mt4ID.Text = "";
            clear_cmb(cmb_CCY);
            clear_cmb(cmb_Time_Period);
            clear_cmb(cmb_EA_ID);
        }

        private void cmb_mt4ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_cmb_data(cmb_CCY, cmb_stcUser.SelectedItem.ToString() + @"\" 
                                + cmb_mt4SV.SelectedItem.ToString() + @"\"
                                + cmb_mt4ID.SelectedItem.ToString());
            cmb_CCY.Text = "";
            clear_cmb(cmb_Time_Period);
            clear_cmb(cmb_EA_ID);

        }

        private void cmb_CCY_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_cmb_data(cmb_Time_Period, cmb_stcUser.SelectedItem.ToString() + @"\"
                    + cmb_mt4SV.SelectedItem.ToString() + @"\"
                    + cmb_mt4ID.SelectedItem.ToString() + @"\"
                    + cmb_CCY.SelectedItem.ToString());
            cmb_Time_Period.Text = "";
            clear_cmb(cmb_EA_ID);
        }

        private void cmb_Time_Period_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_cmb_data(cmb_EA_ID, cmb_stcUser.SelectedItem.ToString() + @"\"
                    + cmb_mt4SV.SelectedItem.ToString() + @"\"
                    + cmb_mt4ID.SelectedItem.ToString() + @"\"
                    + cmb_CCY.SelectedItem.ToString() + @"\"
                    + cmb_Time_Period.SelectedItem.ToString());
        }

        private void btn_find_MT4_Click(object sender, EventArgs e)
        {
            
            if (cmb_stcUser.SelectedIndex >= 0 &&
                cmb_mt4SV.SelectedIndex >= 0 &&
                cmb_mt4ID.SelectedIndex >= 0 &&
                cmb_CCY.SelectedIndex >= 0 &&
                cmb_Time_Period.SelectedIndex >= 0 &&
                cmb_EA_ID.SelectedIndex >= 0)
            {
                string err = "";
                string path = @"C:\Users\GFIT\" + cmb_stcUser.SelectedItem.ToString() + @"\"
                        + cmb_mt4SV.SelectedItem.ToString() + @"\"
                        + cmb_mt4ID.SelectedItem.ToString() + @"\"
                        + cmb_CCY.SelectedItem.ToString() + @"\"
                        + cmb_Time_Period.SelectedItem.ToString() + @"\"
                        + cmb_EA_ID.SelectedItem.ToString() + @"\terminal.exe";

                if (!program_CTRL.SearchProgram(path, out err))
                {
                    MessageBox.Show(err, "起動中のMT4検索", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("みっけ！", "起動中のMT4検索", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (mt4_opt)
                    {
                        program_CTRL.StartProgram(path, "/portable /skipupdate");
                    }
                    else
                    {
                        program_CTRL.StartProgram(path);
                    }
                    
                    
                }
            }
            else
            {
                MessageBox.Show("検索対象の入力条件を選択してください。", "起動中のMT4検索", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            File_CTRL.log_file_del();
        }

        private void tgl_MT4_option_CheckedChanged(object sender, EventArgs e)
        {
            tgl_MT4_option.Text = "MT4 option " + tgl_MT4_option.Checked.ToString();
            mt4_opt = tgl_MT4_option.Checked;
        }

        private void btn_view_param_Click(object sender, EventArgs e)
        {
            get_vol1_setting();
            
        }

        private void get_vol1_setting()
        {
            bool status = false;
            string error = "";

            string result = http_Request_CTRL.http_get(view_param_url, out status);
            System.Console.WriteLine(result);

            vol_1shot_zero = Json_EA_Param_action.ea_param_motion(result, out status, out error);
        }

        private void rdo_checked_changed(object sender, EventArgs e)
        {
            txt_out_put.Text = "";
        }

        private void btn_read_all_setting_Click(object sender, EventArgs e)
        {
            String dict = File_CTRL.file_Read(File_CTRL.get_CodeBase_path() + "\\" + "all_setting.txt");
            System.Console.WriteLine(dict.ToString());
            txt_all_setting.Text = dict;
        }

        private void btn_getUser_from_text_Click(object sender, EventArgs e)
        {
            set_cmb_data2(cmb_stcUser2, "");
            cmb_stcUser2.Text = "";
            clear_cmb(cmb_mt4SV2);
            clear_cmb(cmb_mt4ID2);
            clear_cmb(cmb_CCY2);
            clear_cmb(cmb_Time_Period2);
            clear_cmb(cmb_EA_ID2);
            MessageBox.Show("STCユーザー抽出完了\n\r引き続きコンボボックスで対象を選んでください\n\r\n\r下側の検索ですよ！", "起動中のMT4検索", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// all_setting.txtから検索用の情報を取得して指定のコンボボックスへアイテム追加
        /// </summary>
        /// <param name="sender">アイテム登録対象のコンボボックス</param>
        /// <param name="folder_name">検索する対象のフォルダまでの絶対パス</param>
        private void set_cmb_data2(object sender, string folder_name)
        {

            folder_name = @"C:\Users\GFIT\" + folder_name;

            if (sender != null)
            {
                System.Windows.Forms.ComboBox cmb_target = (System.Windows.Forms.ComboBox)sender;

                //格納用配列を生成
                string[] ary1 = new string[] { };

                //配列をコレクションに変換する
                System.Collections.Generic.List<string> strList = new System.Collections.Generic.List<string>(ary1);

                // all_settingファイルを開く
                using (var sr = new System.IO.StreamReader(File_CTRL.get_CodeBase_path() + "\\all_setting.txt", Encoding.GetEncoding("SJIS")))
                {
                    
                    // ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // ファイルから一行読み込む
                        var line = sr.ReadLine();
                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        var values = line.Split('\\',',');

                        // Containsで上位フォルダ指定と一致しているか判定
                        if (line.Contains(folder_name))
                        {
                            int i = 0;
                            //コンボボックスの名前でcase分け
                            switch (cmb_target.Name)
                            {
                                case "cmb_stcUser2":
                                    i = 3;
                                    break;
                                case "cmb_mt4SV2":
                                    i = 4;
                                    break;
                                case "cmb_mt4ID2":
                                    i = 5;
                                    break;
                                case "cmb_CCY2":
                                    i = 6;
                                    break;
                                case "cmb_Time_Period2":
                                    i = 7;
                                    break;
                                case "cmb_EA_ID2":
                                    i = 8;
                                    break;

                            }
                            
                            strList.Add(values[i]);
                        }

                    }

                }

                if (strList.Count > 0)
                {
                    // 一意の要素を抜き出して、配列に変換する
                    string[] resultArray = strList.Distinct().ToArray();

                    // 設定前に一旦クリア
                    cmb_target.Items.Clear();

                    foreach (string subFolder in resultArray)
                    {
                        // 列挙された情報をcomboboxに設定
                        cmb_target.Items.Add(subFolder);
                    }
                }

            }

        }

        private void cmb_stcUser2_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_cmb_data2(cmb_mt4SV2, cmb_stcUser2.SelectedItem.ToString());
            cmb_mt4SV2.Text = "";
            clear_cmb(cmb_mt4ID2);
            clear_cmb(cmb_CCY2);
            clear_cmb(cmb_Time_Period2);
            clear_cmb(cmb_EA_ID2);
        }

        private void cmb_mt4SV2_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_cmb_data2(cmb_mt4ID2, cmb_stcUser2.SelectedItem.ToString() + @"\" + cmb_mt4SV2.SelectedItem.ToString());
            cmb_mt4ID2.Text = "";
            clear_cmb(cmb_CCY2);
            clear_cmb(cmb_Time_Period2);
            clear_cmb(cmb_EA_ID2);
        }

        private void cmb_mt4ID2_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_cmb_data2(cmb_CCY2, cmb_stcUser2.SelectedItem.ToString() + @"\"
                    + cmb_mt4SV2.SelectedItem.ToString() + @"\"
                    + cmb_mt4ID2.SelectedItem.ToString());
            cmb_CCY2.Text = "";
            clear_cmb(cmb_Time_Period2);
            clear_cmb(cmb_EA_ID2);
        }

        private void cmb_CCY2_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_cmb_data2(cmb_Time_Period2, cmb_stcUser2.SelectedItem.ToString() + @"\"
                + cmb_mt4SV2.SelectedItem.ToString() + @"\"
                + cmb_mt4ID2.SelectedItem.ToString() + @"\"
                + cmb_CCY2.SelectedItem.ToString());
            cmb_Time_Period2.Text = "";
            clear_cmb(cmb_EA_ID2);
        }

        private void cmb_Time_Period2_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_cmb_data2(cmb_EA_ID2, cmb_stcUser2.SelectedItem.ToString() + @"\"
                + cmb_mt4SV2.SelectedItem.ToString() + @"\"
                + cmb_mt4ID2.SelectedItem.ToString() + @"\"
                + cmb_CCY2.SelectedItem.ToString() + @"\"
                + cmb_Time_Period2.SelectedItem.ToString());
        }

        private void btn_find_MT42_Click(object sender, EventArgs e)
        {

            if (cmb_stcUser2.SelectedIndex >= 0 &&
                cmb_mt4SV2.SelectedIndex >= 0 &&
                cmb_mt4ID2.SelectedIndex >= 0 &&
                cmb_CCY2.SelectedIndex >= 0 &&
                cmb_Time_Period2.SelectedIndex >= 0 &&
                cmb_EA_ID2.SelectedIndex >= 0)
            {
                string err = "";
                string path = @"C:\Users\GFIT\" + cmb_stcUser2.SelectedItem.ToString() + @"\"
                        + cmb_mt4SV2.SelectedItem.ToString() + @"\"
                        + cmb_mt4ID2.SelectedItem.ToString() + @"\"
                        + cmb_CCY2.SelectedItem.ToString() + @"\"
                        + cmb_Time_Period2.SelectedItem.ToString() + @"\"
                        + cmb_EA_ID2.SelectedItem.ToString() + @"\terminal.exe";

                if (!program_CTRL.SearchProgram(path, out err))
                {
                    MessageBox.Show(err, "起動中のMT4検索", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("みっけ！", "起動中のMT4検索", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (mt4_opt)
                    {
                        program_CTRL.StartProgram(path, "/portable /skipupdate");
                    }
                    else
                    {
                        program_CTRL.StartProgram(path);
                    }


                }
            }
            else
            {
                MessageBox.Show("検索対象の入力条件を選択してください。", "起動中のMT4検索", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 追加対象のユーザー若しくはEAの情報を取得しcheckedListBoxにアイテムとして登録する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_get_addData_Click(object sender, EventArgs e)
        {

            dynamic json_obj = "";
            bool status = false;
            string error = "";

            // データ削除
            clb_target_data.Items.Clear();
            
            // 対象データ取得
            string result = http_Request_CTRL.http_get(get_add_mode_url(), out status);

            // データをjsonとして取り出し
            Json_Add_User JAU = new Json_Add_User();
            json_obj = JAU.Json_accept(result, out status, out error);

            if (json_obj != null)
            {
                // json化したデータを1レコード毎に分解して表示項目の並べ替えを実施した上でcheckedListBoxへ投入
                foreach (dynamic read_user in (object[])json_obj)
                {
                    System.Console.WriteLine(read_user.Stc_ID + read_user.MT4_Server);
                    string json = "";
                    json = "{";
                    json += "\"" + "Stc_ID" + "\"" + ":" + "\"" + read_user.Stc_ID + "\",";
                    json += "\"" + "MT4_Server" + "\"" + ":" + "\"" + read_user.MT4_Server + "\",";
                    json += "\"" + "MT4_ID" + "\"" + ":" + "\"" + read_user.MT4_ID + "\",";
                    json += "\"" + "Ccy" + "\"" + ":" + "\"" + read_user.Ccy + "\",";
                    json += "\"" + "Time_Period" + "\"" + ":" + "\"" + read_user.Time_Period + "\",";
                    json += "\"" + "EA_ID" + "\"" + ":" + "\"" + read_user.EA_ID + "\",";
                    json += "\"" + "Stc_Pwd" + "\"" + ":" + "\"" + read_user.Stc_Pwd + "\",";
                    json += "\"" + "Join_Time" + "\"" + ":" + "\"" + read_user.Join_Time + "\",";
                    json += "\"" + "Join_Time2" + "\"" + ":" + "\"" + read_user.Join_Time2 + "\",";
                    json += "\"" + "Mail_Address" + "\"" + ":" + "\"" + read_user.Mail_Address + "\",";
                    json += "\"" + "Last_Name" + "\"" + ":" + "\"" + read_user.Last_Name + "\",";
                    json += "\"" + "First_Name" + "\"" + ":" + "\"" + read_user.First_Name + "\",";
                    json += "\"" + "Stc_Name" + "\"" + ":" + "\"" + read_user.Stc_Name + "\",";
                    json += "\"" + "Broker_Name" + "\"" + ":" + "\"" + read_user.Broker_Name + "\",";
                    json += "\"" + "MT4_Pwd" + "\"" + ":" + "\"" + read_user.MT4_Pwd + "\",";
                    json += "\"" + "EA_Name" + "\"" + ":" + "\"" + read_user.EA_Name + "\",";
                    json += "\"" + "Course" + "\"" + ":" + "\"" + read_user.Course + "\",";
                    json += "\"" + "Vol_1shot" + "\"" + ":" + "\"" + read_user.Vol_1shot + "\",";
                    json += "\"" + "memo" + "\"" + ":" + "\"" + read_user.memo + "\",";
                    json += "\"" + "A_Start" + "\"" + ":" + "\"" + read_user.A_Start + "\",";
                    json += "\"" + "Ope_Tag" + "\"" + ":" + read_user.Ope_Tag + ",";
                    json += "\"" + "Check_Status" + "\"" + ":" + "\"" + read_user.Check_Status + "\"}";

                    //clb_target_data.Items.Add(read_user);
                    clb_target_data.Items.Add(json);

                }
            }
            else
            {
                MessageBox.Show("対象データがありません", "追加データなし！");
            }

        }

        /// <summary>
        /// ユーザー追加モードかEA追加モードかの判定機能
        /// </summary>
        /// <returns></returns>
        private string get_add_mode_url()
        {
            // ラジオボタンはadd_userだけだが取り敢えずrequest_machineも分岐してみる
            if (rdo_url_add_user.Checked)
            {
                // ユーザー追加
                return add_user_url;
            }
            else if (rdo_url_request.Checked)
            {
                // 操作リクエスト
               return request;
            }
            else if (rdo_url_add_EA.Checked)
            {
                // EA追加
                return add_EA_url;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// ユーザー追加若しくはEA追加の際に、checkedListBoxで選択されたデータをjson形式に再加工して登録を行います。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_create_checked_data_Click(object sender, EventArgs e)
        {
            // チェックされている項目を列挙
            string msg ="[";
            int i = 0;

            // チェック対象の文字列をjson形式の文字列へ再構成
            foreach (dynamic item in clb_target_data.CheckedItems)
            {
                i += 1;

                if (clb_target_data.CheckedItems.Count > 1 & i < clb_target_data.CheckedItems.Count)
                {
                    msg += item + ",";
                }
                else
                {
                    msg += item;
                }

            }
            msg += "]";

            //MessageBox.Show(msg, "選択された項目一覧");

            add_mt4_folder(msg);

        }

        /// <summary>
        /// 指定されたユーザー追加若しくはEA追加データを使ってインストールフォルダの準備を行う
        /// </summary>
        /// <param name="add_target">インストールフォルダ生成情報文字列add_user/add_EA</param>
        /// <returns></returns>
        private bool add_mt4_folder(string add_target)
        {

            MessageBox.Show("デスクトップの所定フォルダに強制設定します！");
            // パスをend_initとend_ea_add用のフォルダにします
            string output_path = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (rdo_url_add_user.Checked)
            {
                // ユーザー追加
                output_path += @"\end_init";
            }
            else if (rdo_url_add_EA.Checked)
            {
                // EA追加
                output_path += @"\end_ea_add";
            }

            txt_out_put.Text = output_path;
            string out_put_file_name = "";


            if (rdo_url_add_user.Checked || rdo_url_add_EA.Checked)
            {

                dynamic json_obj = "";
                bool status = false;
                string error = "";

                json_obj = Json_add_user_action.add_user_motion(add_target, out status, out error);

                if (json_obj == null)
                {
                    MessageBox.Show("対象データが取得できませんでした！\n\r対象を選択していますか？\n\rcon_setting.txtの記載順でreal_server_urlが2行目ですか？");
                    return false;
                }
                //jsonを1レコード単位に分解してファイル出力する
                foreach (dynamic read_user in (object[])json_obj)
                {
                    string created_user = read_user.Stc_ID + "_"
                                       + read_user.MT4_Server + "_"
                                       + read_user.MT4_ID + "_"
                                       + read_user.Ccy + "_"
                                       + read_user.Time_Period + "_"
                                       + read_user.EA_ID;

                    System.Console.WriteLine(created_user);

                    if (rdo_url_add_user.Checked)
                    {
                        // ユーザー追加
                        out_put_file_name = "ei_" + created_user + ".json";
                    }
                    else if (rdo_url_add_EA.Checked)
                    {
                        // EA追加
                        out_put_file_name = "ea_" + created_user + ".json";
                    }

                    if (read_user != null)
                    {
                        File_CTRL.file_OverWrite("[" + read_user.ToString() + "]", txt_out_put.Text + "\\" + out_put_file_name);

                    }

                }

                // 登録結果としてインストールフォルダのログを出力して保管
                File_CTRL.get_folders();

                MessageBox.Show("結果ファイル出力完了！");
                
            }

            return true;

        }
    }

}

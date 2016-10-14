﻿using System;
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

        private static string test_server_url = "https://218.222.227.232//";
        private static string real_server_url = "https://systrade-cloud.com/";

        // get先URL(rdo_stg_CheckedChanged()にて設定)
        private static string add_user_url = "";
        private static string request = "";
        private static string add_EA_url = "";

        // post先URL(rdo_stg_CheckedChanged()にて設定)
        private static string upload = "";
        private static string resultbox = ""; 

        private retry_timer Retry_Timer;

        public MainForm()
        {
            InitializeComponent();
            //起動後にキュータイマー稼働
            //tmr_retry.Enabled = true;
            Retry_Timer = new retry_timer();
            Retry_Timer.NewTimer();
            Retry_Timer.StartTimer();

            // テストサーバーへの接続を設定
            rdo_stg.Checked = true;
            

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
                MessageBox.Show("フォルダが見当たりません！デスクトップに強制設定します！");
                txt_out_put.Text = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
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

                    json_obj = Json_add_user_action.add_user_motion(result, out status, out error);

                    if (rdo_url_add_user.Checked)
                    {
                        // ユーザー追加
                        out_put_file_name = "end_init.json";
                    }
                    else if (rdo_url_add_EA.Checked)
                    {
                        // EA追加
                        out_put_file_name = "end_ea_add.json";
                    }

                    if (json_obj != null)
                    {
                        File_CTRL.file_OverWrite(json_obj.ToString(), txt_out_put.Text + "\\" + out_put_file_name);
                        MessageBox.Show("結果ファイル出力完了！");

                        // 登録結果としてインストールフォルダのログを出力して保管
                        File_CTRL.get_folders();
                    }
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
                open_sock();
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

        private void open_sock()
        {
            // IPアドレス＆ポート番号設定
            int myPort = 56789;
            // どこからでも接続OK
            IPEndPoint myEndPoint = new IPEndPoint(IPAddress.Any, myPort);

            // リスナー開始
            myListener = new TcpListener(myEndPoint);
            myListener.Start();

            // クライアント接続待ち開始
            //myServerThread = new Thread(new ThreadStart(ServerThread));
            myServerThread = new Thread(new ThreadStart(ServerThread));
            myServerThread.Start();
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
                Dictionary<string, string> dict = File_CTRL.csv_reader();
                
                dict[@"C:\Users\GFIT\u00001105\┗Ava-Demo\5997487\USDJPY\H1\ReloadTemplateEA.mq4"] = "AAA";

                System.Console.WriteLine(File_CTRL.csv_writer(dict));
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
            tmr_conn_watch.Enabled = tgl_MT4_watch.Checked;
            
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

                // post先URL
                upload = test_server_url + "SV/admin/upload.php";
                resultbox = test_server_url + "SV/admin/resultbox.php";
            }
            else
            {
                this.BackColor = System.Drawing.Color.Green;
                // get先URL
                add_user_url = real_server_url + "SV/admin/add_user.php";
                request = real_server_url + "SV/admin/request.php";
                add_EA_url = real_server_url + "SV/admin/add_ea.php";

                // post先URL
                upload = real_server_url + "SV/admin/upload.php";
                resultbox = real_server_url + "SV/admin/resultbox.php";
            }
        }
    }

}

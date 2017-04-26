using Codeplex.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace STC_controller
{

    class MT4_CTLR
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// requestのOpe_Codeによって処理の振り分けを行います。
        /// </summary>
        /// <param name="request">rquest.jsonです</param>
        /// <param name="status">処理結果を応答します。</param>
        /// <param name="error">エラー内容を応答します。</param>
        /// <returns></returns>
        public static dynamic operation(dynamic request, out bool status, out string error)
        {
            string ope_code = "";
            
            string req_errors = "";
            status = false;

            string res_request = "";

            foreach (dynamic read_req in (object[])request)
            {
                ope_code = Json_Util.get_Value(read_req, "Ope_Code");
                Console.WriteLine("指示コード：" + ope_code);

                if (ope_code == "")
                {
                    error = "パラメータエラー: ope_codeが存在していません。後続処理を中断します。";
                    status = false;
                    return null;
                }

                string req_error = "";

                // 自分のマシン名と一致するか確認
                if (Json_Util.get_Value(read_req, "Machine_Name") == MainForm.machine_name)
                {
                    switch (ope_code)
                    {
                        case "start": // MT4起動&自動売買開始
                            start_mode(read_req, out status, out req_error, true);
                            req_errors = req_errors + req_error;
                            break;
                        case "stop": // MT4停止&自動売買停止
                            stop_mode(read_req, out status, out req_error);
                            req_errors = req_errors + req_error;
                            break;
                        case "reload": // MT4再起動
                            status = program_watch(read_req, out req_error);
                            req_errors = req_errors + req_error;
                            if (status)
                            {
                                // (OFF→ON→OFF)で浦島様の指定
                                // 停止処理
                                stop_mode(read_req, out status, out req_error);
                                req_errors = req_errors + req_error;

                                // 開始処理
                                start_mode(read_req, out status, out req_error, false);
                                req_errors = req_errors + req_error;
                            }
                            else
                            {
                                // (ON→OFF→ON)で浦島様の指定
                                // 開始処理
                                start_mode(read_req, out status, out req_error, false);
                                req_errors = req_errors + req_error;

                                // 停止処理
                                stop_mode(read_req, out status, out req_error);
                                req_errors = req_errors + req_error;
                            }

                            break;

                        case "status": // STCログイン時のMT4起動状態応答用
                            get_status(read_req, out status, out req_error);
                            req_errors = req_errors + req_error;
                            break;

                        case "outage": // 退会処理(未テストなのでKaz様と打ち合わせてテストしましょう)
                            uninst_mode(read_req, out status, out req_error);
                            req_errors = req_errors + req_error;
                            break;

                        case "mod_ea": // EAの設定変更
                            //Ope_tag部分の出力機能を追加する予定です！
                            mod_ea_mode(read_req, out status, out req_error);
                            req_errors = req_errors + req_error;
                            break;

                        case "mod_brok": // ブローカーの設定変更

                            break;

                        case "watch_s": // 起動状態の監視
                            watch_mode(read_req, out status, out req_error);
                            req_errors = req_errors + req_error;
                            break;
                        case "watch_r": // 再起動状態の監視
                            watch_mode(read_req, out status, out req_error);
                            req_errors = req_errors + req_error;
                            break;

                        default:
                            req_errors = req_errors + "パラメータエラー:" + ope_code;
                            status = false;
                            break;
                    }

                    // 対象の指示のみを再結合させます
                    if (res_request != "")
                    {
                        res_request = res_request + ",";
                    }

                    res_request = res_request + read_req;

                }

            }

            dynamic dyn_obj;

            if (res_request != "")
            {
                dyn_obj = DynamicJson.Parse("[" + res_request + "]");
            }
            else
            {
                dyn_obj = null;
            }
            
            error = req_errors;
            status = true;
            return dyn_obj;
        }

        /// <summary>
        /// プログラム(MT4)の起動処理を行います。
        /// 起動後、ログインまでタイムラグが存在するので、直後に起動確認できない場合は処理キューのArrayListに登録します
        /// </summary>
        /// <param name="read_req">request.jsonの１レコード</param>
        /// <param name="status">処理結果(基本的にtreu)</param>
        /// <param name="error">起動時のエラー情報</param>
        /// <returns></returns>
        private static dynamic start_mode(dynamic read_req, out bool status, out string error, bool start_mode)
        {
            // パラメータチェック結果判定
            if (read_req.Check_Status == "NG")
            {
                error = "事前チェックエラー";
                status = false;
                return read_req;
            }
            status = program_start(read_req, out error);
            if (status)
            {
                // MT4起動のステータス設定
                read_req.EA_Status = "ON";
            }
            else
            {
                if (error == "対象が見つかりません")
                {
                    // 起動後、ログイン待ち状態でのキューイング
                    read_req.EA_Status = "NG";
                    if (start_mode)
                    {
                        // キューイングは起動監視のみに強制書き換え
                        read_req.Ope_Code = "watch_s";
                    }
                    else
                    {
                        // キューイングは再起動監視のみに強制書き換え
                        read_req.Ope_Code = "watch_r";
                    }

                    // 再検索用に対象をキューイングします。
                    // Check_Status、EA_Statusはキュー処理側のタイマーで削除対応を行います。
                    
                    Program.queue_list.Add(read_req.ToString());
                }
                else
                {
                    // 起動失敗
                    read_req.Check_Status = "NG";
                }
            }

            error = "";
            status = true;
            return read_req;

        }

        /// <summary>
        /// プログラム(MT4)の停止処理を行います。
        /// </summary>
        /// <param name="read_req">request.jsonの１レコード</param>
        /// <param name="status">処理結果(基本的にtreu)</param>
        /// <param name="error">停止時のエラー情報</param>
        /// <returns></returns>
        private static dynamic stop_mode(dynamic read_req, out bool status, out string error)
        {
            // パラメータチェック結果判定
            if (read_req.Check_Status == "NG")
            {
                error = "事前チェックエラー";
                status = false;
                return read_req;
            }

            status = program_stop(read_req, out error);
            if (status)
            {
                // MT4停止のステータス設定
                read_req.EA_Status = "OFF";
            }
            else
            {
                // 停止失敗
                read_req.Check_Status = "NG";
            }

            error = "";
            status = true;
            return read_req;
        }

        /// <summary>
        /// MT4が起動しているか確認して応答を行います。
        /// MT4が起動状態であればEA_StatusにONを設定し、起動していなければOFFを設定します。
        /// </summary>
        /// <param name="read_req">request.jsonの１レコード</param>
        /// <param name="status">状態取得でエラーが発生していなければtrue、発生した場合はFalse</param>
        /// <param name="error">確認時のエラー情報</param>
        /// <returns></returns>
        private static dynamic get_status(dynamic read_req, out bool status, out string error)
        {
            // パラメータチェック結果判定
            if (read_req.Check_Status == "NG")
            {
                error = "事前チェックエラー";
                status = false;
                return read_req;
            }

            status = program_watch(read_req, out error);
            if (status)
            {

                // mod_eaが存在し更新されたか判定
                if (file_updated_check(read_req))
                {
                    // MT4起動中のステータス設定
                    read_req.EA_Status = "ON";
                }
                else
                {
                    // MT4起動中のステータス設定
                    read_req.EA_Status = "mod_ea_fail";
                }

                Dictionary<string, string> dict = File_CTRL.csv_reader("all_setting.txt");
                string file_path = File_CTRL.get_folder_path(read_req);

                Console.WriteLine("リクエスト上のファイルパスは" + file_path);

                foreach (var pair in dict)
                {
                    Console.WriteLine("Dic設定上の" + pair.Key + "は" + pair.Value);
                    if (pair.Key == file_path)
                    {
                        if (pair.Value == "force_stop")
                        {
                            read_req.EA_Status = "OFF";
                        }
                        break;
                    }
                }

                    // MT4は起動しているが通信が切断している場合の判断を追加

                }
            else
            {
                // outageの場合の処理追加
                // 起動プログラムのパス
                //string folder_path = @"C:\Users\GFIT\" + read_req.Stc_ID + @"\" + read_req.MT4_Server + @"\" + read_req.MT4_ID + @"\" + read_req.Ccy + @"\" + read_req.Time_Period + @"\" + read_req.EA_Name;
                string folder_path = File_CTRL.get_folder_path(read_req);
                string file_path = folder_path + @"\terminal.exe";
                string outage_file_path = folder_path + @"\@terminal.exe";

                // プログラムの存在判断
                if (File.Exists(file_path))
                {
                    // MT4停止のステータス設定
                    read_req.EA_Status = "OFF";
                }
                else if (File.Exists(outage_file_path))
                {
                    // MT4停止のステータス設定 期限切れ
                    read_req.EA_Status = "outage";
                }
                else
                {
                    // MT4停止のステータス設定 存在していないのでUNKNOWN
                    read_req.EA_Status = "UNKNOWN";
                }

            }

            error = "";
            status = true;
            return read_req;
        }


        private static dynamic uninst_mode(dynamic read_req, out bool status, out string error)
        {
            // パラメータチェック結果判定
            if (read_req.Check_Status == "NG")
            {
                error = "事前チェックエラー";
                status = false;
                return read_req;
            }

            // 起動プログラムのパス
            //string folder_path = @"C:\Users\GFIT\" + read_req.Stc_ID + @"\" + read_req.MT4_Server + @"\" + read_req.MT4_ID + @"\" + read_req.Ccy + @"\" + read_req.Time_Period + @"\" + read_req.EA_Name;
            string folder_path = File_CTRL.get_folder_path(read_req);
            string file_path = folder_path + @"\terminal.exe";

            status = program_CTRL.SearchProgram(file_path, out error);
            if (status)
            {
                // 起動している場合は、事前停止動作をさせます
                program_CTRL.StopProgram(file_path);
            }
            
            status = File_CTRL.file_rename(file_path, "terminal.exe", "@terminal.exe");
            if (status)
            {
                error = "";
                read_req.EA_Status = "outage";

                // 指示をファイルに出力する機能の追加予定地
                string last_order = "outage";
                // 最終指示記録ファイルへの書き込み(bool値応答してくれるが一旦は実行のみ)
                File_CTRL.last_order_rewrite(folder_path, last_order);
            }
            else
            {
                if (File.Exists(folder_path + @"\@terminal.exe"))
                {
                    // outageが既に行われている場合の応答
                    error = "";
                    read_req.EA_Status = "outage";
                }
                else
                {
                    // outage対象が存在していない場合の応答
                    error = "リネーム失敗";
                    read_req.EA_Status = "UNKNOWN";
              }
            }
            
            return read_req;

        }

        /// <summary>
        /// キューイングされた起動確認処理についてログイン完了しているか監視します。
        /// </summary>
        /// <param name="read_req">request.jsonの１レコード</param>
        /// <param name="status">処理結果(基本的にtreu)</param>
        /// <param name="error">監視時のエラー情報</param>
        /// <returns></returns>
        private static dynamic watch_mode(dynamic read_req, out bool status, out string error)
        {
            // パラメータチェック結果判定
            if (read_req.Check_Status == "NG")
            {
                error = "事前チェックエラー";
                status = false;
                return read_req;
            }
            status = program_watch(read_req, out error);

            if (status)
            {
                // MT4起動のステータス設定
                read_req.EA_Status = "ON";
            }
            else
            {
                // 起動後のウィンドウテキスト判定によるログイン待ち状態でのキューイング
                read_req.EA_Status = "NG";
                // 再検索用に対象をキューイングします。
                // Check_Status、EA_Statusはキュー処理側のタイマーで削除対応を行います。
                Program.queue_list.Add(read_req.ToString());
            }

            error = "";
            status = true;
            return read_req;
        }

        /// <summary>
        /// EAパラメータの変更を受信してパラメータ内容をファイルに出力します。
        /// </summary>
        /// <param name="read_req">request.jsonの１レコード</param>
        /// <param name="status">処理結果(基本的にtreu)</param>
        /// <param name="error">監視時のエラー情報</param>
        /// <returns></returns>
        private static dynamic mod_ea_mode(dynamic read_req, out bool status, out string error)
        {

            status = false;
            error = "";

            // パラメータチェック結果判定
            if (read_req.Check_Status == "NG")
            {
                error = "事前チェックエラー";
                status = false;
                return read_req;
            }


            // portableオプションの確認ロジックを追加予定 mt4_optはtgl_MT4_option.Checkedの値を格納しています
            if (MainForm.mt4_opt)
            {
                // MT4が起動しているか確認してEA_Status　ON/OFF/NGで結果を準備
                get_status(read_req, out status, out error);

                // ファイルを指定のMT4のFILEフォルダーに書き出し
                status = output_ea_param(read_req, out error);

                if (status)
                {

                    string req_error = "";
                    // EAの起動状態の場合は停止して再起動するように変更
                    if (read_req.EA_Status == "ON" || read_req.EA_Status == "mod_ea_fail")
                    {

                        // 再起動する ステータスNGは見てません
                        // 停止処理
                        stop_mode(read_req, out status, out req_error);
                        error += req_error;
                    }

                    // 開始処理
                    start_mode(read_req, out status, out req_error, false);
                    error += req_error;

                    // 外部のファイル読み込んでリクエストに上書きする処理を追加予定
                    // 取り敢えず１０回ループまで許容してみる
                    int break_cnt = 0;
                    while(!read_ea_param(read_req, out req_error) & break_cnt < 10)
                    {
                        break_cnt += 1;
                    }
                    error += req_error;

                }
                else
                {
                    // ファイル出力失敗時はNGステータス(UNKOWNにて)でサーバーへ通知
                    read_req.Check_Status = "OK";

                    // MT4起動中のステータス設定
                    read_req.EA_Status = "mod_ea_fail";
                }
            }
            else
            {
                read_req.Check_Status = "NG";
            }


            return read_req;
        }
        



        /// <summary>
        /// EA用のパラメータファイルを出力します。
        /// STC_サーバーからOpe_Tagの内容をそのままファイル出力します。
        /// </summary>
        /// <param name="read_req"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool output_ea_param(dynamic read_req, out string error)
        {
            error = "";
            try
            {
                // 更新or作成前にファイルのタイムスタンプを取得予定地
                // mod_ea用のパラメータファイルを比較してチェック
                string folder_path = File_CTRL.get_folder_path(read_req) + @"\MQL4\Files";
                // EAパラメータファイル出力先のパス
                string read_full_path = folder_path + @"\read_parameters.csv";

                DateTime read_parameters = System.IO.File.GetLastWriteTime(read_full_path);


                // EAパラメータファイル出力先のMT4起動プログラムのパス
                string out_put_full_path = read_full_path;

                string param = "";
                param += Json_Util.get_Ope_Tag_Value(read_req, "header");
                param += "\r\n";
                param += Json_Util.get_Ope_Tag_Value(read_req, "param");

                File_CTRL.file_OverWrite(param, out_put_full_path, false);

                // 更新後、ファイルのタイムスタンプが更新されているかチェック予定地

                DateTime after_read_parameters = System.IO.File.GetLastWriteTime(read_full_path);
                TimeSpan timeSpan = after_read_parameters - read_parameters;   //DateTime の差が TimeSpan として返る

                // 更新されたか判定
                if ((int)timeSpan.TotalSeconds > 0)
                {
                    return true;
                }
                
                return false;

            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                error = ex.Message;
                return false;

            }

        }

        private static bool read_ea_param(dynamic read_req, out string error)
        {
            // 強制停止しないと処理が先に進むので。。。
            System.Threading.Thread.Sleep(5000);

            // ＥＡが出力するＣＳＶファイル読み込み
            error = "";

            try
            {
                // EAから出力されるファイルのパス
                string write_full_path = File_CTRL.get_folder_path(read_req) + @"\MQL4\Files\write_parameters.csv";

                // 更新されたか判定
                if (file_updated_check(read_req))
                {
                    //ファイル読み込み
                    string param = File_CTRL.file_Read(write_full_path);

                    System.Console.WriteLine(param);

                    //read_reqのOpe_Tagを読み込み内容で上書き
                    // 改行コード指定
                    string return_code = "\r\n";
                    // 改行コード位置特定
                    int return_point = param.IndexOf(return_code);
                    // 改行コード文字サイズ特定
                    int code_size = return_code.Length;
                    // 改行コードまでの1行目の読み込みとヘッダパラメータへの設定
                    read_req.Ope_Tag.header = param.Substring(0, return_point);
                    // 2行目位置を設定
                    return_point += code_size;
                    // 2行目改行のサイズ取得(末尾改行コード切り捨て)
                    int param_len = param.Length - return_point - code_size;
                    // 改行コード以降の2行目の読み込みとパラメターへの設定
                    read_req.Ope_Tag.param = param.Substring(return_point, param_len);

                    return true;
                }
                else
                {
                    //更新されていないのでfalseで応答
                    return false;
                }
                
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                error = ex.Message;
                return false;

            }


        }


        private static bool file_updated_check(dynamic read_req)
        {
            // mod_ea用のパラメータファイルを比較してチェック
            string folder_path = File_CTRL.get_folder_path(read_req) + @"\MQL4\Files";
            // EAから出力されるファイルのパス
            string write_full_path = folder_path + @"\write_parameters.csv";
            // EAパラメータファイル出力先のパス
            string read_full_path = folder_path + @"\read_parameters.csv";

            // mod_eaで入出力されるファイルが無い場合(mod_ea未対応ea)の場合は強制的にtrue応答
            if (!File_CTRL.check_file(folder_path, "write_parameters.csv"))
            {
                return true;
            }
            if (!File_CTRL.check_file(folder_path, "read_parameters.csv"))
            {
                return true;
            }


            //タイムスタンプの確認
            DateTime write_parameters = System.IO.File.GetLastWriteTime(write_full_path);
            DateTime read_parameters = System.IO.File.GetLastWriteTime(read_full_path);
            TimeSpan timeSpan = write_parameters - read_parameters;   //DateTime の差が TimeSpan として返る

            // 更新されたか判定
            if ((int)timeSpan.TotalSeconds > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 起動動作を行います。
        /// 起動後、直ぐにOSのプロセスIDで起動開始しているか確認を行います。
        /// </summary>
        /// <param name="read_req">request.jsonの１レコード</param>
        /// <param name="error">起動時のエラー情報</param>
        /// <returns></returns>
        private static bool program_start(dynamic read_req,out string error)
        {
            
            bool status = false;

            // 起動プログラムのパス
            //string folder_path = @"C:\Users\GFIT\" + read_req.Stc_ID + @"\" + read_req.MT4_Server + @"\" + read_req.MT4_ID + @"\" + read_req.Ccy + @"\" + read_req.Time_Period + @"\" + read_req.EA_Name;
            string folder_path = File_CTRL.get_folder_path(read_req);
            string program_path = folder_path + @"\terminal.exe";
            string option_param = "/portable /skipupdate";

            if (MainForm.mt4_opt)
            {
                status = program_CTRL.StartProgram(program_path, option_param);
            }
            else
            {
                status = program_CTRL.StartProgram(program_path);
            }
            
            if (!status)
            {
                error = "起動失敗しました。";
                return false;
            }

            //ファイル名:C:\Users\GFIT\u00000212\OANDA-Japan Practice - デモ口座\6835252\USDJPY\H1\terminal.exe
            string file_path = program_path;

            status = program_CTRL.SearchProgram(file_path, out error);
            if (!status)
            {
                return false;
            }

            // 指示をファイルに出力する機能の追加予定地
            string last_order = "start";
            // 最終指示記録ファイルへの書き込み(bool値応答してくれるが一旦は実行のみ)
            File_CTRL.last_order_rewrite(folder_path, last_order);

            return true;

        }

        /// <summary>
        /// 停止動作を行います。停止対象はOSのプロセスIDで判断します。
        /// </summary>
        /// <param name="read_req">request.jsonの１レコード</param>
        /// <param name="error">停止時のエラー情報</param>
        /// <returns></returns>
        private static bool program_stop(dynamic read_req, out string error)
        {
            bool status = false;

            //ファイル名:C:\Users\GFIT\u00000212\OANDA-Japan Practice - デモ口座\6835252\USDJPY\H1\terminal.exe
            // 起動プログラムのパス
            //string folder_path = @"C:\Users\GFIT\" + read_req.Stc_ID + @"\" + read_req.MT4_Server + @"\" + read_req.MT4_ID + @"\" + read_req.Ccy + @"\" + read_req.Time_Period + @"\" + read_req.EA_Name;
            string folder_path = File_CTRL.get_folder_path(read_req);
            string file_path = folder_path + @"\terminal.exe";

            status = program_CTRL.SearchProgram(file_path, out error);
            if (!status)
            {
                return true;
            }
            program_CTRL.StopProgram(file_path);

            // 指示をファイルに出力する機能の追加予定地
            string last_order = "stop";
            // 最終指示記録ファイルへの書き込み(bool値応答してくれるが一旦は実行のみ)
            File_CTRL.last_order_rewrite(folder_path, last_order);

            return true;

        }

        /// <summary>
        /// 起動後にOSプロセスとして存在しているか監視を行います。
        /// </summary>
        /// <param name="read_req">request.jsonの１レコード</param>
        /// <param name="error">監視時のエラー情報</param>
        /// <returns></returns>
        private static bool program_watch(dynamic read_req, out string error)
        {

            bool status = false;

            //ファイル名:C:\Users\GFIT\u00000212\OANDA-Japan Practice - デモ口座\6835252\USDJPY\H1\terminal.exe
            // 起動プログラムのパス
            //string file_path = @"C:\Users\GFIT\" + read_req.Stc_ID + @"\" + read_req.MT4_Server + @"\" + read_req.MT4_ID + @"\" + read_req.Ccy + @"\" + read_req.Time_Period + @"\" + read_req.EA_Name + @"\terminal.exe";
            string file_path = File_CTRL.get_folder_path(read_req) + @"\terminal.exe";
            status = program_CTRL.SearchProgram(file_path, out error);
            if (!status)
            {
                error = "起動していません";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 各MT4インストールフォルダで更新されているgfit.txtのタイムスタンプをチェックし
        /// MT4のダミーEAが動作しているか判定を行います。
        /// MT4が稼働指示されている状態でタイムスタンプが更新されていない場合は
        /// MT4サーバーへ接続できていないと判断し強制終了を行います。
        /// 将来的にはSTC_SVへ通知する機能を実装する構想です。
        /// </summary>
        public static void connect_watch()
        {
            try
            {

                // MT4の監視機能ではありますが、ログ掃除もここに配置しておく
                File_CTRL.log_file_del();

                string watch_log_file = @"\gfit.txt";

                // 最終指示状況をDictionary型で取得します。
                Dictionary<string, string> dict = File_CTRL.csv_reader("all_setting.txt");

                foreach (var pair in dict)
                {
                    //Console.WriteLine("Dic設定上の" + pair.Key + "は" + pair.Value);

                    if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(pair.Key + watch_log_file)))
                    {
                        switch (pair.Value)
                        {
                            case "start": // MT4起動&自動売買開始
                                          // 更新日時を取得する
                                DateTime dtUpdate = System.IO.File.GetLastWriteTime(pair.Key + watch_log_file);
                                TimeSpan ts = DateTime.Now - dtUpdate;   //DateTime の差が TimeSpan として返る
                                Console.WriteLine("最終更新日時からの秒差: " + ts.TotalSeconds + "秒の差があります。");

                                // 現在日時と更新日時が一定時間乖離した場合は以降の処理を実装(取り敢えず１０分＝600秒ぐらいかな)
                                if ((int)ts.TotalSeconds > 600)
                                {
                                    // 通知後に該当のMT4を強制停止
                                    Console.WriteLine("強制停止");
                                    string err = "";
                                    if (!program_CTRL.SearchProgram(pair.Key + @"\terminal.exe", out err)) // 否定ＩＦに変更して下記をコメントアウト
                                    /*{
                                        program_CTRL.StopProgram(pair.Key + @"\terminal.exe");
                                    }
                                    else*/
                                    {
                                        logger.Error(" 接続監視失敗 停止対象のプログラムがOS上で見つかりませんでした パス:" + pair.Key + " 最終指示:" + pair.Value);
                                    }
                                    
                                    // 強制停止後、最終指示一覧に強制停止である事を上書き
                                    File_CTRL.last_order_rewrite(pair.Key, "force_stop");
                                    // STCサーバーへ通知する機能の実装予定地
                                    System.Console.WriteLine(pair.Key);
                                    //throw_force_stop_status(pair.Key); //停止だけだったので開始と共通化しましたのでこちらは使いません
                                    throw_notice(pair.Key, "OFF");

                                }

                                break;
                            case "stop": // MT4停止&自動売買停止
                                break;
                            case "reload": // MT4再起動
                                break;
                            case "status": // STCログイン時のMT4起動状態応答用
                                break;
                            case "outage": // 退会処理
                                break;
                            case "mod_ea": // EAの設定変更
                                break;
                            case "mod_brok": // ブローカーの設定変更
                                break;
                            case "watch_s": // 起動状態の監視
                                break;
                            case "watch_r": // 再起動状態の監視
                                break;
                            case "force_stop": // 稼働検査でのタイムスタンプチェックで強制停止
                                DateTime lastUpdate = System.IO.File.GetLastWriteTime(pair.Key + watch_log_file);
                                TimeSpan timeSpan = DateTime.Now - lastUpdate;   //DateTime の差が TimeSpan として返る
                                Console.WriteLine("最終更新日時からの秒差: " + timeSpan.TotalSeconds + "秒の差があります。");

                                // 現在日時と更新日時が一定時間差以内になった場合は以降の処理を実装(取り敢えず30秒ぐらいかな)
                                if ((int)timeSpan.TotalSeconds < 30)
                                {
                                    // STCサーバーへの復帰の通知とall_setting.txtの最終指示更新

                                    // 通知後に該当のMT4を強制停止
                                    Console.WriteLine("復帰");
                                    string err = "";
                                    if (!program_CTRL.SearchProgram(pair.Key + @"\terminal.exe", out err))
                                    {
                                        logger.Error(" 接続監視失敗 復帰確認対象のプログラムがOS上で見つかりませんでした パス:" + pair.Key + " 最終指示:" + pair.Value);
                                    }

                                    // 復帰確認後後、最終指示一覧に起動中である事を上書き
                                    File_CTRL.last_order_rewrite(pair.Key, "start");
                                    // STCサーバーへ通知する機能の実装予定地
                                    System.Console.WriteLine(pair.Key);
                                    throw_notice(pair.Key, "ON");
                                }
                                
                                break;
                            default:
                                // 指示ファイル上の指示がおかしい場合
                                logger.Error(" 接続監視失敗 最終指示ファイル上のパラメータエラー: パス:" + pair.Key + " 最終指示:" + pair.Value);
                                break;
                        }

                    }
                    else
                    {
                        // 指定されたファイルが見つからない場合
                        logger.Error(" 接続監視失敗 最終指示ファイル上の指定フォルダのgfit.txtが見つかりません: パス:" + pair.Key + " 最終指示:" + pair.Value);
                    }

                }


            }
            catch (System.Exception ex)
            {
                // ファイルを開くのに失敗したとき
                logger.Error(" 接続監視失敗失敗: " + ex.Message);
            }

        }

        private static void throw_force_stop_status(string path_string)
        {
            try
            {

                // 取り敢えずここで強制停止時のJsonを生成してサーバーへ投げ込むまでの
                // 機能実装を行ってテストしみてる！

                // all_setting上のプログラムインストールパスを区切りで分割して配列に格納する
                string[] stArrayData = path_string.Split('\\');

                // データを確認する
                /*
                foreach (string stData in stArrayData)
                {
                    System.Console.WriteLine(stData);
                }
                System.Console.WriteLine(stArrayData[1]);
                */

                ArrayList OK_req_list = new ArrayList();


                //定義変更時はRequest_machine_checkの項目も併せて修正しましょう！
                dynamic req_obj = new DynamicJson(); // ルートのコンテナ
                
                req_obj.Ope_Number = -1;
                req_obj.Stc_ID = stArrayData[3];
                //req_obj.Request_Time = Json_Util.iso_8601_now();
                req_obj.Machine_Name = MainForm.machine_name;
                req_obj.Response_Time = Json_Util.iso_8601_now();
                req_obj.EA_ID = stArrayData[8];
                req_obj.Broker_Name = stArrayData[4];
                req_obj.MT4_Server = stArrayData[4];
                req_obj.MT4_ID = stArrayData[5];
                req_obj.Ccy = stArrayData[6];
                req_obj.Time_Period = stArrayData[7];
                req_obj.Ope_Code = "notice";
                //req_obj.Vol_1shot = "";
                //req_obj.A_Start = "";

                req_obj.EA_Status = "OFF";

                OK_req_list.Add(req_obj);
                

                dynamic out_put = Json_Util.reParse(OK_req_list);

                // 処理結果登録
                http_Request_CTRL.http_post(out_put.ToString(), MainForm.ea_alert); //MainForm//private void rdo_stg_CheckedChanged
                

            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }

        }

        private static void throw_notice(string path_string, string ea_status)
        {
            try
            {

                // 取り敢えずここで強制停止時のJsonを生成してサーバーへ投げ込むまでの
                // 機能実装を行ってテストしみてる！

                // all_setting上のプログラムインストールパスを区切りで分割して配列に格納する
                string[] stArrayData = path_string.Split('\\');

                // データを確認する
                /*
                foreach (string stData in stArrayData)
                {
                    System.Console.WriteLine(stData);
                }
                System.Console.WriteLine(stArrayData[1]);
                */

                ArrayList OK_req_list = new ArrayList();


                //定義変更時はRequest_machine_checkの項目も併せて修正しましょう！
                dynamic req_obj = new DynamicJson(); // ルートのコンテナ

                req_obj.Ope_Number = -1;
                req_obj.Stc_ID = stArrayData[3];
                req_obj.Machine_Name = MainForm.machine_name;
                req_obj.Response_Time = Json_Util.iso_8601_now();
                req_obj.EA_ID = stArrayData[8];
                req_obj.Broker_Name = stArrayData[4];
                req_obj.MT4_Server = stArrayData[4];
                req_obj.MT4_ID = stArrayData[5];
                req_obj.Ccy = stArrayData[6];
                req_obj.Time_Period = stArrayData[7];
                req_obj.Ope_Code = "notice";

                req_obj.EA_Status = ea_status;

                OK_req_list.Add(req_obj);


                dynamic out_put = Json_Util.reParse_solo(OK_req_list);//大かっこありだとエラーになっている

                // 処理結果登録
                http_Request_CTRL.http_post(out_put.ToString(), MainForm.ea_alert); //MainForm//private void rdo_stg_CheckedChanged


            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }


        /// <summary>
        /// MT4の起動状況の再現をall_setting.txtを使って行います。
        /// Windowsアップデートなどでサーバーがリブートした場合に
        /// MT4も停止するので、その状況からサービスを再開させる事が目的です。
        /// </summary>
        public static void recover_MT4()
        {
            try
            {

                // 最終指示状況をDictionary型で取得します。
                Dictionary<string, string> dict = File_CTRL.csv_reader("all_setting.txt");

                foreach (var pair in dict)
                {
                    //Console.WriteLine("Dic設定上の" + pair.Key + "は" + pair.Value);

                    switch (pair.Value)
                    {
                        case "start": // MT4起動&自動売買開始
                            bool status = false;
                            string program_path = pair.Key + @"\terminal.exe";
                            string error = "";
                            string option_param = "/portable /skipupdate";

                            // 起動指示前に起動しているかの確認
                            status = program_CTRL.SearchProgram(program_path, out error);

                            if (!status)
                            {
                                // 起動動作を行います
                                // program_CTRL.StartProgramはbool値返すのでfalseの時はログへ出力する
                                if (MainForm.mt4_opt)
                                {
                                    status = program_CTRL.StartProgram(program_path, option_param);
                                }
                                else
                                {
                                    status = program_CTRL.StartProgram(program_path);
                                }
                                
                                if (!status)
                                {
                                    logger.Error(" 起動状態再現失敗 起動する事が出来ませんでした: パス:" + pair.Key + " 最終指示:" + pair.Value);
                                }

                            }
                            else
                            {
                                logger.Error(" 起動状態再現 既に起動状態でした: パス:" + pair.Key + " 最終指示:" + pair.Value);
                            }

                            break;
                        case "stop": // MT4停止&自動売買停止
                            break;
                        case "reload": // MT4再起動
                            break;
                        case "status": // STCログイン時のMT4起動状態応答用
                            break;
                        case "outage": // 退会処理
                            break;
                        case "mod_ea": // EAの設定変更
                            break;
                        case "mod_brok": // ブローカーの設定変更
                            break;
                        case "watch_s": // 起動状態の監視
                            break;
                        case "watch_r": // 再起動状態の監視
                            break;
                        case "force_stop": // 稼働検査でのタイムスタンプチェックで強制停止
                            break;
                        default:
                            // 指示ファイル上の指示がおかしい場合
                            logger.Error(" 起動状態再現失敗 全体最終指示ファイル上のパラメータエラー: パス:" + pair.Key + " 最終指示:" + pair.Value);
                            break;
                    }

                }
            }
            catch (System.Exception ex)
            {
                // ファイルを開くのに失敗したとき
                logger.Error(" 起動状態再現失敗: " + ex.Message);
            }

        }





    }
}

using System;
using System.Collections.Generic;
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

            }
            error = req_errors;
            status = true;
            return request;
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
                // MT4起動中のステータス設定
                read_req.EA_Status = "ON";
            }
            else
            {
                // MT4停止のステータス設定
                read_req.EA_Status = "OFF";
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

            string file_path = @"C:\Users\GFIT\" + read_req.Stc_ID + @"\" + read_req.MT4_Server + @"\" + read_req.MT4_ID + @"\" + read_req.Ccy + @"\" + read_req.Time_Period + @"\" + read_req.EA_Name + @"\terminal.exe";
            
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
                read_req.EA_Status = "OFF";
            }
            else
            {
                error = "リネーム失敗";
                read_req.EA_Status = "UNKNOWN";
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
        /// 起動動作を行います。
        /// 起動後、直ぐにOSのプロセスIDで起動開始しているか確認を行います。
        /// </summary>
        /// <param name="read_req">request.jsonの１レコード</param>
        /// <param name="error">起動時のエラー情報</param>
        /// <returns></returns>
        private static bool program_start(dynamic read_req,out string error)
        {
            string program_path = @"C:\Users\GFIT\";
            bool status = false;

            // 起動プログラムのパス
            program_path = program_path + read_req.Stc_ID + @"\" + read_req.MT4_Server + @"\" + read_req.MT4_ID + @"\" + read_req.Ccy + @"\" + read_req.Time_Period + @"\" + read_req.EA_Name + @"\terminal.exe";
            status = program_CTRL.StartProgram(program_path);

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
            string file_path = @"C:\Users\GFIT\" + read_req.Stc_ID + @"\" + read_req.MT4_Server + @"\" + read_req.MT4_ID + @"\" + read_req.Ccy + @"\" + read_req.Time_Period  + @"\" + read_req.EA_Name + @"\terminal.exe";


            status = program_CTRL.SearchProgram(file_path, out error);
            if (!status)
            {
                return true;
            }
            program_CTRL.StopProgram(file_path);

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
            string file_path = @"C:\Users\GFIT\" + read_req.Stc_ID + @"\" + read_req.MT4_Server + @"\" + read_req.MT4_ID + @"\" + read_req.Ccy + @"\" + read_req.Time_Period + @"\" + read_req.EA_Name + @"\terminal.exe";
            status = program_CTRL.SearchProgram(file_path, out error);
            if (!status)
            {
                error = "起動していません";
                return false;
            }

            return true;
        }
    }
}

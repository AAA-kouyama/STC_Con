using Codeplex.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STC_controller
{
    class Json_add_user_action
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static dynamic add_user_motion(string add_user_strings, out bool status, out string error)
        {
            dynamic json_obj = "";
            status = false;
            error = "";

            Json_Add_User JAU = new Json_Add_User();
            json_obj = JAU.Json_accept(add_user_strings, out status, out error);

            //送受信正常結果格納用パス
            string filePaht = System.IO.Directory.GetCurrentDirectory() + @"\add_user_log";
            
            if (status)
            {
                //正常時、証跡の受信内容をファイル出力
                File_CTRL.Write_OK_Log(add_user_strings, filePaht + @"\Accept" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + @".txt");
            }
            else
            {
                //エラーログファイル出力
                logger.Error(" add_user.json 読み込みエラー: " + error + "\n\r" + add_user_strings);
            }

            if (json_obj != null)
            {
                // インストール用フォルダ生成
                json_obj = Create_Install_folder(json_obj, out error);

                if (error != "")
                {
                    //エラーログファイル出力
                    logger.Error(" インストールフォルダ準備エラー　対象Stc_ID:" + error + "\n\r" + add_user_strings);
                }

                // 応答用end_init.json生成
                json_obj = Json_End_Init.end_init(json_obj, out status, out error);

                if (status)
                {
                    //正常時、証跡の送信内容をファイル出力
                    File_CTRL.Write_OK_Log(json_obj.ToString(), filePaht + @"\Send" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + @".txt");
                }
                else
                {
                    //エラーログファイル出力
                    logger.Error(" end_init.json 応答準備エラー: " + error + "\n\r" + add_user_strings);
                }
            }

            return json_obj;

        }

        private static dynamic Create_Install_folder(dynamic json_obj, out string error)
        {
            // エラー初期化
            error = "";
            bool status = false;

            // フォルダ生成
            foreach (dynamic read_user in (object[])json_obj)
            {
                /* VPSでのMT4大量稼働試験で確認出来た、１つのOSユーザーで多数のMT4を稼働させる方式で検討を進めています。
                    現在の想定は
                    c:\Users\GFIT\"Stc_ID"\"Broker_Name"\"MT4_ID"\"Ccy"\"Time_Period"
                    のパスをMT4のインストールパスにしようと考えております。
                */
                string stc_id = "";
                status = File_CTRL.CreateUserFolder(read_user, out stc_id);
                if (!status)
                {
                    error = error + " Stc_ID:" + stc_id + "\n\r";
                    read_user.Check_Status = "NG";
                }
            }

            return json_obj;

        }

    }
}

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
    class Json_EA_Param_action
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Dictionary<string, string> ea_param_motion(string request_strings, out bool status, out string error)
        {
            dynamic json_obj = "";
            status = false;
            error = "";

            //送受信正常結果格納用パス
            string filePaht = System.IO.Directory.GetCurrentDirectory() + @"\rquest_log";

            Json_EA_Param JRs = new Json_EA_Param();
            json_obj = JRs.Json_accept(request_strings, out status, out error);

            if (status)
            {
                //正常時、証跡の受信内容をファイル出力
                File_CTRL.Write_OK_Log(request_strings, filePaht + @"\Accept" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + @".txt");
            }
            else
            {
                ///エラーログファイル出力
                logger.Error(" ea_param.json 読み込みエラー: " + error + "\n\r" + request_strings);
            }

            // 応答用Dictionary
            var dict = new Dictionary<string, string>();

            if (json_obj != null)
            {
                foreach (dynamic read_req in (object[])json_obj)
                {
                    // EAのパラメータファイルをメモリに格納するか　ファイルに出力する
                    System.Console.WriteLine(Json_Util.get_Value(read_req, "name"));
                    System.Console.WriteLine(Json_Util.get_Value(read_req, "type"));
                    System.Console.WriteLine(Json_Util.get_Value(read_req, "desc"));
                    System.Console.WriteLine(Json_Util.get_Value(read_req, "nonVolFlag"));
                    System.Console.WriteLine(Json_Util.get_Value(read_req, "PARAM"));

                    // Dictionaryに追加
                    dict.Add(Json_Util.get_Value(read_req, "name"), Json_Util.get_Value(read_req, "nonVolFlag"));
                }
            }

            return dict;

        }
    }
}

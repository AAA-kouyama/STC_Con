using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STC_controller
{
    class Json_Request : Json_acceptor_base

    {

        /// <summary>
        /// add_userで設定されている項目用の辞書を生成します
        /// end_initの変更時はこちらも確認してください
        /// </summary>
        /// <returns>チェック用ディクショナリー</returns>
        protected override Dictionary<object, int> Create_dictionary()
        {
            //request.jsonの項目名とフィールドサイズをディクショナリー化

            //定義変更時はResponse_machineの項目も併せて修正しましょう！
            //Ope_Tag部分は可変項目なのでSpecial_checkで行います
            var dic = new Dictionary<object, int>();
            dic.Add("Ope_Number", 20);
            dic.Add("Stc_ID", 10);
            dic.Add("Request_Time", 30);
            dic.Add("Ope_Code", 10);
            dic.Add("Broker_Name", 40);
            dic.Add("EA_Name", 60);
            dic.Add("MT4_Server", 40);
            dic.Add("MT4_ID", 20);
            dic.Add("Ccy", 10);
            dic.Add("Time_Period", 10);

            return dic;
        }

        /// <summary>
        /// Ope_Tagと特殊チェックの日時フォーマットを日時項目に対して行います
        /// Ope_Codeが増えた場合は分岐を増やす修正を行って下さい。
        /// 合せて分岐時に呼び出すファンクションを追加してください。
        /// </summary>
        /// <param name="dyn_check">チェック対象のjson</param>
        /// <param name="dic">チェック用ディクショナリー</param>
        /// <returns></returns>
        protected override string Special_check(dynamic dyn_check, Dictionary<object, int> dic)
        {
            //Ope_Tagの種類チェック
            string ope_code = dyn_check.Ope_Code;
            dynamic dyn_check_ope_tag = null;
            string Json_Base_check_error = "";
            switch (ope_code)
            {
                case "start":
                    // start用OpeTag取得
                    dyn_check_ope_tag = Create_dictionary_start();
                    break;
                case "stop":
                    dyn_check_ope_tag = Create_dictionary_stop();
                    break;
                case "reload":
                    dyn_check_ope_tag = Create_dictionary_reload();
                    break;
                case "status":
                    dyn_check_ope_tag = Create_dictionary_status();
                    break;
                case "watch_s":
                    dyn_check_ope_tag = Create_dictionary_watch_s();
                    break;
                case "watch_r":
                    dyn_check_ope_tag = Create_dictionary_watch_r();
                    break;
                default:
                    dyn_check.Check_Status = "NG";
                    return " Ope_Codeチェックエラー: " + ope_code + " はコード値としてプログラムに設定されていません";
            }

            // Ope_Tagチェック　但しチェック用のディクショナリーがある場合のみ
            if (dyn_check_ope_tag != null)
            {
                Json_Base_check_error = Base_check(dyn_check.Ope_Tag, dyn_check_ope_tag);
                if (Json_Base_check_error != "")
                {
                    dyn_check.Check_Status = "NG";
                    return " Ope_Tagチェックエラー: " + Json_Base_check_error;
                }
            }

            //時刻形式チェック
            if (!Json_Util.Time_Format_Check(dyn_check.Request_Time)) 
            {
                dyn_check.Check_Status = "NG";
                return " 日時フォーマットチェックエラー:Join_Time:" + dyn_check.Request_Time;
            }
            else
            {
                return "";
            }
        }

        private static Dictionary<object, int> Create_dictionary_start()
        {
            //request.jsonのOpe_Tag項目名とフィールドサイズをディクショナリー化
            //var dic_start = new Dictionary<object, int>();
            //dic_start.Add("Ccy", 10);
            //dic_start.Add("Time_Period", 10);
            //return dic_start;

            //startはOpe_Tag未使用なのでnullで応答します。
            return null;
        }

        private static Dictionary<object, int> Create_dictionary_stop()
        {
            //request.jsonのOpe_Tag項目名とフィールドサイズをディクショナリー化
            //var dic_start = new Dictionary<object, int>();
            //dic_start.Add("Ccy", 10);
            //dic_start.Add("Time_Period", 10);
            //return dic_start;

            //stopはOpe_Tag未使用なのでnullで応答します。
            return null;
        }

        private static Dictionary<object, int> Create_dictionary_reload()
        {
            //request.jsonのOpe_Tag項目名とフィールドサイズをディクショナリー化
            //var dic_start = new Dictionary<object, int>();
            //dic_start.Add("Ccy", 10);
            //dic_start.Add("Time_Period", 10);
            //return dic_start;

            //reloadはOpe_Tag未使用なのでnullで応答します。
            return null;
        }

        private static Dictionary<object, int> Create_dictionary_status()
        {
            //request.jsonのOpe_Tag項目名とフィールドサイズをディクショナリー化
            //var dic_start = new Dictionary<object, int>();
            //dic_start.Add("Ccy", 10);
            //dic_start.Add("Time_Period", 10);
            //return dic_start;

            //statusはOpe_Tag未使用なのでnullで応答します。
            return null;
        }

        private static Dictionary<object, int> Create_dictionary_watch_s()
        {
            //request.jsonのOpe_Tag項目名とフィールドサイズをディクショナリー化
            //var dic_start = new Dictionary<object, int>();
            //dic_start.Add("Ccy", 10);
            //dic_start.Add("Time_Period", 10);
            //return dic_start;

            //reloadはOpe_Tag未使用なのでnullで応答します。
            return null;
        }

        private static Dictionary<object, int> Create_dictionary_watch_r()
        {
            //request.jsonのOpe_Tag項目名とフィールドサイズをディクショナリー化
            //var dic_start = new Dictionary<object, int>();
            //dic_start.Add("Ccy", 10);
            //dic_start.Add("Time_Period", 10);
            //return dic_start;

            //reloadはOpe_Tag未使用なのでnullで応答します。
            return null;
        }

    }
}

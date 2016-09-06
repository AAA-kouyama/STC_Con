using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STC_controller
{
    class Json_Add_User : Json_acceptor_base
    {

        /// <summary>
        /// add_userで設定されている項目用の辞書を生成します
        /// end_initの変更時はこちらも確認してください
        /// </summary>
        /// <returns>チェック用ディクショナリー</returns>
        protected override Dictionary<object, int> Create_dictionary()
        {
            //add_user.jsonの項目名とフィールドサイズをディクショナリー化
            //定義変更時はend_initの項目も併せて修正しましょう！
            var dic = new Dictionary<object, int>();
            dic.Add("Stc_ID", 10);
            dic.Add("Stc_Pwd", 20);
            dic.Add("Mail_Address", 40);
            dic.Add("Join_Time", 30);
            dic.Add("Join_Time2", 30); 
            dic.Add("First_Name", 20);
            dic.Add("Last_Name", 20);
            dic.Add("Broker_Name", 40);
            dic.Add("MT4_Server", 40);
            dic.Add("MT4_ID", 20);
            dic.Add("MT4_Pwd", 20);
            //dic.Add("EA_ID", 4); //追加予定
            dic.Add("EA_Name", 60);
            dic.Add("Ccy", 10);
            dic.Add("Time_Period", 10);
            dic.Add("Course", 10);
            dic.Add("memo", 60);

            return dic;
        }

        /// <summary>
        /// 特殊チェックの日時フォーマットを日時項目に対して行います
        /// </summary>
        /// <param name="dyn_check">チェック対象のjson</param>
        /// <param name="dic">チェック用ディクショナリー</param>
        /// <returns></returns>
        protected override string Special_check(dynamic dyn_check, Dictionary<object, int> dic)
        {
            //時刻形式チェック
            if (!Json_Util.Time_Format_Check(dyn_check.Join_Time))
            {
                dyn_check.Check_Status = "NG";
                return " 日時フォーマットチェックエラー:Join_Time:" + dyn_check.Join_Time;
            }

            //時刻形式チェック
            if (!Json_Util.Time_Format_Check(dyn_check.Join_Time2))
            {
                dyn_check.Check_Status = "NG";
                return " 日時フォーマットチェックエラー:Join_Time2:" + dyn_check.Join_Time2;
            }

            return "";

        }

    }
}

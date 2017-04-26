using Codeplex.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STC_controller
{
    class Json_acceptor_base : Json_acceptor_abstract
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// json受信時のチェック処理用基本動作クラスです
        /// </summary>
        /// <param name="json_text">処理対象となるjsonのテキストを受け取ります</param>
        /// <param name="status">全体処理結果をOUTします　全て正常の場合はtrue 1件でもエラーがある場合はfalse</param>
        /// <param name="error">1件でもエラーがある場合はエラー内容をOUTします</param>
        /// <returns>statusがエラーでも正常処理できたレコードをjsonレコードとして応答します。</returns>
        public override dynamic Json_accept(string json_text, out bool status, out string error)
        {
            try
            {

                if (json_text == "ABORT")
                {
                    status = true;
                    error = "読込完了";
                    return null;
                }

                dynamic dyn_obj = DynamicJson.Parse(json_text);
                //Console.WriteLine(user);
                if (dyn_obj == null)
                {
                    status = true;
                    error = "読込完了";
                    return null;
                }

                // ステータスとエラーと正常読み込み対象の初期化
                status = false;
                error = null;

                // 処理済みjson用リスト
                ArrayList dyn_obj_list = new ArrayList();
                int dyn_count = 0; //json内のレコード数取得

                // 受信対象のチェック用ディクショナリー生成
                Dictionary<object, int> dic = Create_dictionary();

                foreach (dynamic item in (object[])dyn_obj)
                {
                    //Console.WriteLine(item);

                    // 共通チェック処理
                    string Json_Base_check_error = Base_check(item, dic);

                    if (Json_Base_check_error != "")
                    {
                        error = error + "\r\n処理レコード:" + dyn_count + ":" + Json_Base_check_error;
                        item.Check_Status = "NG";
                    }
                    else
                    {
                        // 特別チェック処理
                        string Json_Special_check_error = Special_check(item, dic);
                        if (Json_Special_check_error != "")
                        {
                            error = error + "\r\n処理レコード:" + dyn_count + ":" + Json_Special_check_error;
                            item.Check_Status = "NG";
                        } 
                        else
                        {
                            item.Check_Status = "OK";
                            
                        }
                    }

                    // チェックのステータスを保持したリストを格納
                    dyn_obj_list.Add(item);

                    dyn_count++;
                }

                //Console.WriteLine(user.ToString());

                dynamic return_json = null;

                // 正常読み込み対象があるか判定
                if (dyn_obj_list.Count > 0)
                {
                    return_json = Json_Util.reParse(dyn_obj_list);
                }

                if (error == "" || error == null)
                {
                    status = true;
                    error = "読込完了";
                }

                return return_json;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                status = false;
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 処理対象のディクショナリーを生成する箇所です必ずoverrideしてディクショナリーを生成してください
        /// </summary>
        /// <returns>チェックに使用するディクショナリーを応答します</returns>
        protected override Dictionary<object, int> Create_dictionary()
        {
            return null;
        }

        /// <summary>
        /// jsonの共通チェック処理です
        /// </summary>
        /// <param name="dyn_check">チェック対象のjsonレコードです</param>
        /// <param name="dic">チェックに使用するディクショナリーです</param>
        /// <returns>エラーの場合は内容を応答します　正常の場合は""空文字で応答します</returns>
        protected override string Base_check(dynamic dyn_check, Dictionary<object, int> dic)
        {
            //エラー箇所特定用変数
            string error_field;

            //ディクショナリーチェック
            if(dyn_check == null)
            {
                return " ディクショナリーエラー:　チェック用ディクショナリーが設定されていません　プログラムの誤りです";
            }

            //項目数一致チェック
            // 2016/06/23 かず様の拡張仕様発生の為、項目数についてはチェックを行わない様に修正
            //if (!Json_Util.Field_Count_Check(dyn_check, dic))
            //{
            //    return " 項目数チェックエラー:　jsonとプログラム内の項目数が不一致です";
            //}

            //存在チェック処理
            if (!Json_Util.Presence_Check(dyn_check, dic, out error_field))
            {
                return " 項目存在チェックエラー:" + error_field;
            }

            //サイズチェック
            if (!Json_Util.Size_Check(dyn_check, dic, out error_field))
            {
                return " 項目文字数チェックエラー:" + error_field;
            }

            return "";

        }

        /// <summary>
        /// 特別なチェックを実装します　日時のフォーマットチェックなど共通に無いチェックを実装する場合は必ずoverrideして実装してください
        /// </summary>
        /// <param name="dyn_check">チェック対象のjsonレコードです</param>
        /// <param name="dic">チェックに使用するディクショナリーです</param>
        /// <returns>エラーの場合は内容を応答します　正常の場合は""空文字で応答します</returns>
        protected override string Special_check(dynamic dyn_check, Dictionary<object, int> dic)
        {
            return "";
        }

    }
}

using Codeplex.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STC_controller
{
    class Json_End_Init
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 処理結果応答用のend_initをjsonとして生成します　生成時に必要項目を更新します
        /// </summary>
        /// <param name="read_json_obj">応答対象となる処理済みのjson(add_user.json)</param>
        /// <param name="status">全体処理結果をOUTします　全て正常の場合はtrue 1件でもエラーがある場合はfalse</param>
        /// <param name="error">1件でもエラーがある場合はエラー内容をOUTします</param>
        /// <returns>応答用のend_init形式のjson</returns>
        public static dynamic end_init(dynamic read_json_obj, out bool status, out string error)
        {
            try
            {
                ArrayList OK_user_list = new ArrayList();

                foreach (dynamic read_user in (object[])read_json_obj)
                {
                    //定義変更時はadd_user_checkの項目も併せて修正しましょう！
                    dynamic user_obj = new DynamicJson(); // ルートのコンテナ
                    user_obj.Stc_ID = Json_Util.get_Value(read_user, "Stc_ID");
                    user_obj.Stc_Pwd = Json_Util.get_Value(read_user, "Stc_Pwd");
                    user_obj.Mail_Address = Json_Util.get_Value(read_user, "Mail_Address");
                    user_obj.Join_Time = Json_Util.get_Value(read_user, "Join_Time");
                    user_obj.Join_Time2 = Json_Util.get_Value(read_user, "Join_Time2");
                    user_obj.Launch_Time = Json_Util.iso_8601_now();
                    //user_obj.Machine_Name = "Jupiter";
                    user_obj.Machine_Name = MainForm.machine_name;
                    user_obj.Broker_Name = Json_Util.get_Value(read_user, "Broker_Name");
                    user_obj.MT4_Server = Json_Util.get_Value(read_user, "MT4_Server");
                    user_obj.MT4_ID = Json_Util.get_Value(read_user, "MT4_ID");
                    user_obj.MT4_Pwd = Json_Util.get_Value(read_user, "MT4_Pwd");
                    user_obj.EA_ID = Json_Util.get_Value(read_user, "EA_ID");
                    user_obj.EA_Name = Json_Util.get_Value(read_user, "EA_Name");
                    user_obj.Course = Json_Util.get_Value(read_user, "Course");
                    user_obj.Ccy = Json_Util.get_Value(read_user, "Ccy");
                    user_obj.Time_Period = Json_Util.get_Value(read_user, "Time_Period");
                    user_obj.Vol_1shot = Json_Util.get_Value(read_user, "Vol_1shot");
                    user_obj.A_Start = Json_Util.get_Value(read_user, "A_Start");
                    // A_Startが"A_Start"の場合はOn、offの場合はOFFで応答
                    if (Json_Util.get_Value(read_user, "A_Start") == "A_Start")
                    {
                        user_obj.EA_Status = "ON";
                    }
                    else
                    {
                        user_obj.EA_Status = "OFF";
                    }

                    user_obj.Init_Status = Json_Util.get_Value(read_user, "Check_Status");

                    // 4/26かずさんが追加していたので対応
                    user_obj.Stc_Name = Json_Util.get_Value(read_user, "Stc_Name");

                    // Ope_Tagでの追加タグ生成
                    dynamic req_obj_ope_tag = Ope_Tag_Creator(user_obj, read_user);
                    if (req_obj_ope_tag != null)
                    {
                        user_obj = req_obj_ope_tag;
                    }

                    OK_user_list.Add(user_obj);
                }

                dynamic out_put = Json_Util.reParse(OK_user_list);

                status = true;
                error = "正常変換完了";
                return out_put;
            }
            catch (System.Exception ex)
            {
                status = true;
                error = ex.ToString();
                logger.Error(ex.Message);
                return null;

            }
        }

        /// <summary>
        /// Ope_Tag用の追加タグを生成するための分岐を行います。
        /// </summary>
        /// <param name="req_obj">応答用のjson</param>
        /// <param name="read_req">処理対象のjson</param>
        /// <returns></returns>
        private static dynamic Ope_Tag_Creator(dynamic req_obj, dynamic read_req)
        {

            dynamic return_json = null;

            if (Json_Util.get_Ope_Tag_Value(read_req, "header") == "")
            {
                return null;
            }

            // ope_tagが存在している場合は生成して応答
            return_json = Create_return_ope_tag(req_obj, read_req);

            return return_json;
        }


        private static dynamic Create_return_ope_tag(dynamic req_obj, dynamic read_req)
        {
            // 元のJsonのライブラリを改変してOpe_Tag用の1段階ネストを追加
            // System.Console.WriteLine(Json_Util.get_Ope_Tag_Value(read_req, "header"));
            // System.Console.WriteLine(Json_Util.get_Ope_Tag_Value(read_req, "param"));

            req_obj.Ope_Tag = new { }; // Ope_Tagのオブジェクトの追加
            req_obj.Ope_Tag.header = Json_Util.get_Ope_Tag_Value(read_req, "header");
            req_obj.Ope_Tag.param = Json_Util.get_Ope_Tag_Value(read_req, "param");

            return req_obj;
        }



    }
}

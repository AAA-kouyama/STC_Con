using Codeplex.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STC_controller
{
    class Json_Resopnse
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// requestで受信した内容の処理結果を応答するためのjsonを生成します。　生成時に必要項目を更新します
        /// Ope_Tagが増えた場合はOpe_Tag_Creatorに分岐を追加して、専用のファンクションを追加してください。
        /// </summary>
        /// <param name="read_json_obj"></param>
        /// <param name="status"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static dynamic Response_machine(dynamic read_json_obj, out bool status, out string error)
        {

            try
            {
                ArrayList OK_req_list = new ArrayList();

                foreach (dynamic read_req in (object[])read_json_obj)
                {
                    //定義変更時はRequest_machine_checkの項目も併せて修正しましょう！
                    dynamic req_obj = new DynamicJson(); // ルートのコンテナ

                    req_obj.Ope_Number = Json_Util.get_Value(read_req, "Ope_Number");
                    req_obj.Stc_ID = Json_Util.get_Value(read_req, "Stc_ID");
                    req_obj.Request_Time = Json_Util.get_Value(read_req, "Request_Time");
                    req_obj.Machine_Name = MainForm.machine_name;
                    req_obj.Response_Time = Json_Util.iso_8601_now();
                    req_obj.EA_ID = Json_Util.get_Value(read_req, "EA_ID");
                    req_obj.Broker_Name = Json_Util.get_Value(read_req, "Broker_Name");
                    req_obj.MT4_Server = Json_Util.get_Value(read_req, "MT4_Server");
                    req_obj.MT4_ID = Json_Util.get_Value(read_req, "MT4_ID");
                    req_obj.Ccy = Json_Util.get_Value(read_req, "Ccy");
                    req_obj.Time_Period = Json_Util.get_Value(read_req, "Time_Period");
                    //req_obj.EA_Status = Json_Util.get_Value(read_req, "EA_Status").ToUpper();
                    req_obj.Ope_Code = Json_Util.get_Value(read_req, "Ope_Code");
                    req_obj.Vol_1shot = Json_Util.get_Value(read_req, "Vol_1shot");
                    req_obj.A_Start = Json_Util.get_Value(read_req, "A_Start");
                    if (read_req.Check_Status == "NG")
                    {
                        req_obj.EA_Status = "UNKNOWN";
                    }
                    else
                    {
                        req_obj.EA_Status = Json_Util.get_Value(read_req, "EA_Status").ToUpper();
                    }
                    

                    // Ope_Tagでの追加タグ生成
                    dynamic req_obj_ope_tag = Ope_Tag_Creator(req_obj, read_req);
                    if (req_obj_ope_tag != null)
                    {
                        req_obj = req_obj_ope_tag;
                    }

                    OK_req_list.Add(req_obj);
                }

                dynamic out_put = Json_Util.reParse(OK_req_list);

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
            //Ope_Tagの種類チェック
            string ope_code = read_req.Ope_Code;

            switch (ope_code)
            {
                case "start":
                    // start用OpeTag取得
                    return_json = Create_return_json_start(req_obj, read_req);
                    break;
                case "stop":
                    return_json = Create_return_json_stop(req_obj, read_req);
                    break;
                case "reload":
                    return_json = Create_return_json_reload(req_obj, read_req);
                    break;
                case "mod_ea":
                    return_json = Create_return_json_mod_ea(req_obj, read_req);
                    break;
                case "mod_brok":
                    //return_json = Create_return_json_mod_brok(req_obj, read_req);
                    break;
                case "watch_s":
                    return_json = Create_return_json_watch_s(req_obj, read_req);
                    break;
                case "watch_r":
                    return_json = Create_return_json_watch_r(req_obj, read_req);
                    break;
                default:
                    return null;
            }

            return return_json;
        }

        private static dynamic Create_return_json_start(dynamic req_obj, dynamic read_req)
        {
            //request.jsonのOpe_Tag項目名とフィールドサイズをディクショナリー化
            //定義変更時はResponse_machineの項目も併せて修正しましょう！
            //req_obj.Ope_Tag = new { }; // Ope_Tagのオブジェクトの追加
            //req_obj.Ope_Tag.Ccy = Json_Util.get_Value(read_req, "Ope_Tag.Ccy");
            //req_obj.Ope_Tag.Time_Period = Json_Util.get_Value(read_req, "Ope_Tag.Time_Period");

            //Ope_Tag仕様が未確定なのでnullで応答します。
            //return req_obj;
            return null;
        }

        private static dynamic Create_return_json_stop(dynamic req_obj, dynamic read_req)
        {
            //request.jsonのOpe_Tag項目名とフィールドサイズをディクショナリー化
            //定義変更時はResponse_machineの項目も併せて修正しましょう！
            //req_obj.Ope_Tag = new { }; // Ope_Tagのオブジェクトの追加
            //req_obj.Ope_Tag.Ccy = Json_Util.get_Value(read_req, "Ope_Tag.Ccy");
            //req_obj.Ope_Tag.Time_Period = Json_Util.get_Value(read_req, "Ope_Tag.Time_Period");


            //Ope_Tag仕様が未確定なのでnullで応答します。
            //return req_obj;
            return null;
        }

        private static dynamic Create_return_json_reload(dynamic req_obj, dynamic read_req)
        {
            //request.jsonのOpe_Tag項目名とフィールドサイズをディクショナリー化
            //定義変更時はResponse_machineの項目も併せて修正しましょう！
            //req_obj.Ope_Tag = new { }; // Ope_Tagのオブジェクトの追加
            //req_obj.Ope_Tag.Ccy = Json_Util.get_Value(read_req, "Ope_Tag.Ccy");
            //req_obj.Ope_Tag.Time_Period = Json_Util.get_Value(read_req, "Ope_Tag.Time_Period");


            //Ope_Tag仕様が未確定なのでnullで応答します。
            //return req_obj;
            return null;
        }

        private static dynamic Create_return_json_mod_ea(dynamic req_obj, dynamic read_req)
        {
            // 元のJsonのライブラリを改変してOpe_Tag用の1段階ネストを追加
            // System.Console.WriteLine(Json_Util.get_Ope_Tag_Value(read_req, "header"));
            // System.Console.WriteLine(Json_Util.get_Ope_Tag_Value(read_req, "param"));

            req_obj.Ope_Tag = new { }; // Ope_Tagのオブジェクトの追加
            req_obj.Ope_Tag.header = Json_Util.get_Ope_Tag_Value(read_req, "header");
            req_obj.Ope_Tag.param = Json_Util.get_Ope_Tag_Value(read_req, "param");

            return req_obj;
        }


        private static dynamic Create_return_json_watch_s(dynamic req_obj, dynamic read_req)
        {
            //request.jsonのOpe_Tag項目名とフィールドサイズをディクショナリー化
            //定義変更時はResponse_machineの項目も併せて修正しましょう！
            //req_obj.Ope_Tag = new { }; // Ope_Tagのオブジェクトの追加
            //req_obj.Ope_Tag.Ccy = Json_Util.get_Value(read_req, "Ope_Tag.Ccy");
            //req_obj.Ope_Tag.Time_Period = Json_Util.get_Value(read_req, "Ope_Tag.Time_Period");


            //Ope_Tag仕様が未確定なのでnullで応答します。
            //return req_obj;
            return null;
        }

        private static dynamic Create_return_json_watch_r(dynamic req_obj, dynamic read_req)
        {
            //request.jsonのOpe_Tag項目名とフィールドサイズをディクショナリー化
            //定義変更時はResponse_machineの項目も併せて修正しましょう！
            //req_obj.Ope_Tag = new { }; // Ope_Tagのオブジェクトの追加
            //req_obj.Ope_Tag.Ccy = Json_Util.get_Value(read_req, "Ope_Tag.Ccy");
            //req_obj.Ope_Tag.Time_Period = Json_Util.get_Value(read_req, "Ope_Tag.Time_Period");


            //Ope_Tag仕様が未確定なのでnullで応答します。
            //return req_obj;
            return null;
        }
    }
}

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
    class Json_request_action
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static dynamic request_motion(string request_strings, out bool status, out string error)
        {
            dynamic json_obj = "";
            status = false;
            error = "";

            //送受信正常結果格納用パス
            string filePaht = System.IO.Directory.GetCurrentDirectory() + @"\rquest_log";

            Json_Request JRs = new Json_Request();
            json_obj = JRs.Json_accept(request_strings, out status, out error);

            if (status)
            {
                //正常時、証跡の受信内容をファイル出力
                File_CTRL.Write_OK_Log(request_strings, filePaht + @"\Accept" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + @".txt");
            }
            else
            {
                ///エラーログファイル出力
                logger.Error(" Request.json 読み込みエラー: " + error + "\n\r" + request_strings);
            }

            if (json_obj != null)
            {
                
                // Ope_Codeで指定された動作を実施
                json_obj = MT4_CTLR.operation(json_obj, out status, out error);

                // operationの処理結果のうち、Ope_Code=watchやEA_STATUS＝LOADINGは応答から除外する
                json_obj = remove_inner_resopense(json_obj);

                if (json_obj != null)
                {
                    // 応答用response.json生成
                    json_obj = Json_Resopnse.Response_machine(json_obj, out status, out error);

                    if (status)
                    {
                        //正常時、証跡の送信内容をファイル出力
                        File_CTRL.Write_OK_Log(json_obj.ToString(), filePaht + @"\Send" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + @".txt");
                    }
                    else
                    {
                        //エラーログファイル出力
                        logger.Error(" Resopnse.json 応答準備エラー: " + error + "\n\r" + request_strings);
                    }
                }
            }

            return json_obj;

        }

        private static dynamic remove_inner_resopense(dynamic json_obj)
        {
            dynamic response_json = null;
            ArrayList OK_req_list = new ArrayList();

            if (json_obj != null)
            {
                foreach (dynamic read_obj in (object[])json_obj)
                {
                    string ope_conde = read_obj.Ope_Code;
                    switch (ope_conde)
                    {
                        case "watch_s":
                            if (read_obj.EA_Status == "ON")
                            {
                                read_obj.Ope_Code = "start";
                                OK_req_list.Add(read_obj);
                            }
                            break;
                        case "watch_r":
                            if (read_obj.EA_Status == "ON")
                            {
                                read_obj.Ope_Code = "reload";
                                OK_req_list.Add(read_obj);
                            }
                            break;
                        default:
                            OK_req_list.Add(read_obj);
                            break;
                    }
                }
            } 

            if (OK_req_list.Count > 0)
            {
                response_json = Json_Util.reParse(OK_req_list);
            }
            
            return response_json;
        }
    }
}

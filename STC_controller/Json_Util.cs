using Codeplex.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STC_controller
{
    class Json_Util
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 複数レコード対応の為にArrayListを受け取って文字列に変換しなおしてからJsonに変換します
        /// </summary>
        /// <param name="target_list"></param>
        /// <returns></returns>
        public static dynamic reParse(ArrayList target_list)
        {
            try
            {
                string user_list = null;

                if (target_list.Count > 0)
                {
                    int cnt = 0;
                    user_list = "[";
                    foreach (dynamic OK_user in (ArrayList)target_list)
                    {
                        user_list += OK_user.ToString();
                        cnt++;
                        if (target_list.Count != cnt)
                        {
                            user_list += ",";
                        }
                    }
                    user_list += "]";
                }

                dynamic return_user = DynamicJson.Parse(user_list);

                return return_user;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                return null;
            }

        }

        /// <summary>
        /// jsonオブジェクトの指定の項目数とプログラム上の設定項目数が一致している場合はtrue、していない場合はfalse
        /// </summary>
        /// <param name="json_obj">判定対象のjsonオブジェク</param>
        /// <param name="dic">判定対象項目名称を保持するディクショナリーオブジェクト</param>
        /// <returns></returns>
        public static bool Field_Count_Check(dynamic json_obj, Dictionary<object, int> dic)
        {

            try
            {
                // Ope_Tagは仕様未確定なのでカウント対象から除外しました
                int json_count = 0;
                foreach (KeyValuePair<string, dynamic> item in json_obj)
                {
                    if (item.Key != "Ope_Tag")
                    {
                        json_count++;
                    }
                }

                int dic_count = 0;
                string check = null;
                foreach (KeyValuePair<object, int> item in dic)
                {
                    check = item.Key as string; //string変換できない場合はnullが設定されます
                    if (check != null)
                    {
                        dic_count++;
                    }

                }

                //Console.WriteLine("json項目数:" + json_count + "設定項目数:" + dic_count);

                if (json_count == dic_count)
                {
                    return true;
                }

            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }

            return false;

        }

        /// <summary>
        /// jsonオブジェクトの指定の項目が設計文字数に収まっている場合はtrue、収まっていない場合はfalse
        /// ネストするタグ対応としてOpe_Tagの場合はチェックを行いません
        /// </summary>
        /// <param name="json_obj">判定対象のjsonオブジェク</param>
        /// <param name="dic">判定対象項目名称を保持するディクショナリーオブジェクト</param>
        /// <param name="error_field">判定対象項目名称を応答するoutパラメータ</param>
        /// <returns></returns>
        public static bool Size_Check(dynamic json_obj, Dictionary<object, int> dic, out string error_field )
        {
            try
            {
                string check_value = null;
                foreach (KeyValuePair<string, dynamic> item in json_obj)
                {
                    //Ope_Tagはチェックから外しています
                    if (item.Key != "Ope_Tag")
                    {

                        check_value = item.Value;

                        //Console.WriteLine(item.Key + ":" + item.Value + ":" + dic.Get(item.Key) + ":" + check_value.Length);

                        // 2016/06/23 かず様による項目拡張対応の為、ディクショナリーに存在しない項目(dic.Get(item.Key) == 0)
                        // については、チェックスルーに変更
                        if (dic.Get(item.Key) > 0)
                        {
                            if (check_value.Length > dic.Get(item.Key))
                            {
                                error_field = item.Key;
                                return false;
                            }
                        }
                    }
                }

                error_field = "";
                return true;
            }
            catch(System.Exception ex)
            {
                logger.Error(ex.Message);
                error_field = "処理失敗！エラーログ確認ください！";
                return false;
            }
        }

        /// <summary>
        /// jsonオブジェクトに指定の項目が存在している場合はtrue、していない場合はfalse
        /// </summary>
        /// <param name="json_obj">判定対象のjsonオブジェクト</param>
        /// <param name="dic">判定対象項目名称を保持するディクショナリーオブジェクト</param>
        /// <param name="error_field">判定対象項目名称を応答するoutパラメータ</param>
        /// <returns></returns>
        public static bool Presence_Check(dynamic json_obj, Dictionary<object, int> dic, out string error_field )
        {
            try
            {
                string field_name = null;

                foreach (object field_name_obj in dic.Keys)
                {
                    // Ope_Tagの場合、Dicのキー項目に子タグを直接入れていますのでnullで応答する仕組みです
                    field_name = field_name_obj as string; //string変換できない場合はnullが設定されます

                    if (field_name != null)
                    {
                        //Console.WriteLine(field_name + " : " + json_obj.IsDefined(field_name));

                        if (!json_obj.IsDefined(field_name))
                        {
                            error_field = field_name;
                            return false;
                        }
                    }
                }

                error_field = "";
                return true;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                error_field = "処理失敗！エラーログ確認ください！";
                return false;
            }

        }

        /// <summary>
        /// json上の項目として指定の項目が存在しているか確認し
        /// 存在している場合は該当項目の値を応答し
        /// 存在していない場合は""空文字を応答する
        /// </summary>
        /// <param name="json_obj">確認対象のjsonレコード</param>
        /// <param name="Field_Name">確認対象の項目名称</param>
        /// <returns></returns>
        public static string get_Value(dynamic json_obj, string Field_Name)
        {
            try
            {
                return json_obj.IsDefined_getValue(Field_Name);

            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                return "";
            }

        }
        

        /// <summary>
        /// 日付項目について正しい内容であればtrue、間違っていればfalse
        /// </summary>
        /// <param name="time_item">チェック対象の日付項目</param>
        /// <returns></returns>
        public static bool Time_Format_Check(object time_item)
        {
            try
            {
                string checks = time_item as string;
                if (checks == null)
                {
                    return false;
                }


                //DateTimeに変換できるか確かめる
                DateTime dt = DateTime.Parse(checks, null, System.Globalization.DateTimeStyles.RoundtripKind);
                if (dt != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }

        }

        public static string iso_8601_now()
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "+09:00";
        }

    }

    public static class DictionaryExt
    {
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            TValue rtn;
            dic.TryGetValue(key, out rtn);
            return rtn;
        }
    }
}

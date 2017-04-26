using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STC_controller
{
    class Json_EA_Param : Json_acceptor_base

    {

        /// <summary>
        /// EAパラメータの設定受信用です。サーバーへの応答とか無いので。。。
        /// </summary>
        /// <returns>チェック用ディクショナリー</returns>
        protected override Dictionary<object, int> Create_dictionary()
        {
            //view_ea_param.jsonの項目名とフィールドサイズをディクショナリー化
            //Ope_Tag部分は可変項目なのでSpecial_checkで行います
            var dic = new Dictionary<object, int>();
            dic.Add("name", 99999);
            //dic.Add("type", 99999);
            //dic.Add("desc", 99999);
            dic.Add("nonVolFlag", 99999);
            //dic.Add("PARAM", 99999);


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

            return "";
        }

    }
}

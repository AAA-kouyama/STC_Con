using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STC_controller
{
    abstract class Json_acceptor_abstract
    {

        // jsonの受信チェック処理用抽象クラスです

        // jsonチェック動作本体です
        public abstract dynamic Json_accept(string json_text, out bool status, out string error);

        // jsonチェックの為のディクショナリー設定です
        protected abstract Dictionary<object, int> Create_dictionary();

        // jsonの基本チェック群です
        protected abstract string Base_check(dynamic dyn_check, Dictionary<object, int> dic);

        // jsonの特殊チェック群です
        protected abstract string Special_check(dynamic dyn_check, Dictionary<object, int> dic);
    }
}

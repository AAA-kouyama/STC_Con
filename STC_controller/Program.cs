using log4net;
using log4net.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STC_controller
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            /*ログテスト用
            logger.Debug("開発中のデバッグ／トレースに使用する");
            logger.Info("情報（操作履歴等）");
            logger.Warn("注意／警告（障害の一歩手前）");
            logger.Error("システムが停止するまではいかない障害が発生");
            logger.Fatal("システムが停止する致命的な障害が発生");
            */

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            

        }
        // App.configがら正常時の証跡ログ出力設定の読み込みを行う
        public static bool Success_log_switch = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Success_log_switch"]);
        // 再検索キューを生成する
        public static ArrayList queue_list = new ArrayList();
    }
}

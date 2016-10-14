using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STC_controller
{
    class File_CTRL
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string enc = "UTF-8";
        
        /// <summary>
        /// ユーザーのMT4用インストールフォルダを作成します
        /// </summary>
        /// <param name="user">受信しているadd_userリクエスト</param>
        /// <param name="stc_ID">STCユーザーID</param>
        /// <returns></returns>
        public static bool CreateUserFolder(dynamic user, out string stc_ID, out string ins_path)
        {
            try
            {
                string folder = @"C:\Users\GFIT\" + user.Stc_ID + @"\" + user.MT4_Server + @"\" + user.MT4_ID + @"\" + user.Ccy + @"\" + user.Time_Period + @"\" + user.EA_Name;
                stc_ID = user.Stc_ID;
                ins_path = folder;
                if (!System.IO.Directory.Exists(folder)){
                    System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(folder);
                    return true;
                }

                return false;

            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                stc_ID = "Sytem.Exception";
                ins_path = "";
                return false;
            }
            
        }

        /// <summary>
        /// 指定ファイルの読み込み
        /// </summary>
        /// <param name="filePath">指定ファイルのフルパス</param>
        /// <returns></returns>
        public static string file_Read(string filePath)
        {
            string text = "";

            try
            {
                StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding(enc));

                text = sr.ReadToEnd();
                sr.Close();
                //Console.Write(text);
                
            }
            catch (System.Exception ex)
            {
                //捕捉出来ていないイレギュラーの場合のエラー処理
                logger.Error(ex.Message);
            }

            return text;

        }


        /// <summary>
        /// ファイル出力（上書き）
        /// </summary>
        /// <param name="text">出力文字列</param>
        /// <param name="out_put_full_path">出力先ファイルのフルパス</param>
        /// <param name="WriteLine">オプションパラメータです
        /// 　　　　　　　　　　　　WriteLineで出力(最終改行付与)する場合は引数指定なしで、
        /// 　　　　　　　　　　　　Writeで文字列そのままで出力はFalse指定してください</param>
        public static void file_OverWrite(string text, string out_put_full_path, bool WriteLine = true)
        {
            try
            {
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(out_put_full_path)))
                {
                    System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(out_put_full_path));
                }

                var utf8_encoding = new System.Text.UTF8Encoding(false);
                var writer = new StreamWriter(out_put_full_path, false, utf8_encoding);
                if (WriteLine)
                {
                    writer.WriteLine(text);
                }
                else
                {
                    writer.Write(text);
                }
                
                writer.Close();

            }
            catch (System.Exception ex)
            {
                //捕捉出来ていないイレギュラーの場合のエラー処理
                logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// ファイル出力（追記）
        /// </summary>
        /// <param name="text">出力文字列</param>
        /// <param name="out_put_full_path">出力先ファイルのフルパス</param>
        public static void file_AddWrite(string text, string out_put_full_path)
        {

            try
            {
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(out_put_full_path)))
                {
                    System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(out_put_full_path));
                }

                var utf8_encoding = new System.Text.UTF8Encoding(false);
                var writer = new StreamWriter(out_put_full_path, true, utf8_encoding);
                writer.WriteLine(text);
                writer.Close();

            }
            catch (System.Exception ex)
            {
                //捕捉出来ていないイレギュラーの場合のエラー処理
                logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 正常操作時の証跡ログ出力ユーティリティ
        /// </summary>
        /// <param name="write_obj">出力文字列</param>
        /// <param name="file_full_path">出力先ファイルパス</param>
        public static void Write_OK_Log(string write_obj, string file_full_path)
        {
            //ファイル出力
            try
            {
                // App.configのSuccess_log_switchで出力制御を行います
                // 読み込みはProgramクラス(Program.cs)で行っています。
                if (Program.Success_log_switch)
                {
                    File_CTRL.file_OverWrite(write_obj, file_full_path);
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(" 正常処理結果証跡ファイル生成失敗: " + ex.Message);
            }
        }

        /// <summary>
        /// ファイルのリネームを行う
        /// 退会若しくは期限切れ時にアンインストールだと手動対応を行わなけれならないので
        /// 代わりにリネームして該当ファイルを使えなくする
        /// 対象ファイルを操作する前に必ず使用されていない状態にする必要があります
        /// </summary>
        /// <param name="file_full_path">リネーム対象のファイルフルパス</param>
        /// <param name="orign_name">リネームする対象のオリジナルファイル名</param>
        /// <param name="rename">リネーム後のファイル名</param>
        /// <returns></returns>
        public static bool file_rename(string file_full_path, string orign_name, string rename)
        {
            try
            {
                string rename_file = file_full_path.Replace(orign_name, rename);
                System.IO.FileInfo fi = new System.IO.FileInfo(file_full_path);
                
                fi.MoveTo(rename_file);
                return true;
            }
            catch (System.Exception ex)
            {
                logger.Error(" ファイルリネーム失敗: " + ex.Message);
                return false;
            }
        }


        private static string folder_log_header ="インストール済みフォルダの階層情報バックアップです";

        /// <summary>
        /// 顧客環境のインストールフォルダをログっとく為の機能です
        /// </summary>
        public static void get_folders()
        {
            try
            {
                //string[] subFolders = System.IO.Directory.GetDirectories(@"C:\Users\GFIT", "*", System.IO.SearchOption.AllDirectories);
                IEnumerable<string> subFolders = System.IO.Directory.EnumerateDirectories(@"C:\Users\GFIT", "*", System.IO.SearchOption.AllDirectories);

                string folder_log_path = @"C:\STC_controller_log\Folder_log\" + System.DateTime.Now.ToString("yyyyMMdd") + "folder_backup.txt";
                //
                file_AddWrite(folder_log_header, folder_log_path);

                //サブフォルダを列挙する
                foreach (string subFolder in subFolders)
                {
                    if (subFolder.StartsWith(@"C:\Users\GFIT\u"))
                    {
                        IEnumerable<string> Target_subFolders = System.IO.Directory.EnumerateDirectories(subFolder, "*", System.IO.SearchOption.AllDirectories);
                        //ターゲット以下のサブフォルダを列挙する
                        foreach (string Target_subFolder in Target_subFolders)
                        {
                            Console.WriteLine(Target_subFolder);
                            file_AddWrite(Target_subFolder, folder_log_path);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(" フォルダ取得失敗: " + ex.Message);
            }

        }

        /// <summary>
        /// folder引数で指定されたフォルダを作成します。
        /// </summary>
        /// <param name="folder">生成するフォルダ階層文字列</param>
        public static void create_folder(string folder)
        {

            try
            {
                if (!System.IO.Directory.Exists(folder))
                {
                    System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(folder);
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(" フォルダ作成失敗: " + ex.Message);
            }

        }

        /// <summary>
        /// バックアップファイルからフォルダ階層の情報を読み込みフォルダ作成を行います。
        /// </summary>
        /// <param name="back_up_log_file_path">フォルダ階層をバックアップしたファイル名</param>
        public static void recover_folder(string back_up_log_file_path)
        {
            string file_paths = file_Read(back_up_log_file_path);
            file_paths = file_paths.Replace(Environment.NewLine, "\r");
            file_paths = file_paths.Trim('\r');

            string[] file_path = file_paths.Split('\r');

            foreach (string item in file_path)
            {
                if (item != folder_log_header)
                {
                    create_folder(item);
                }

            }
        }

        /// <summary>
        /// アプリの起動EXE配置パスを応答します
        /// </summary>
        /// <returns>アプリの起動EXE配置パスを応答します</returns>
        public static string get_CodeBase_path()
        {
            // アプリの起動EXEが配置されているパスを応答
            string path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            //URIを通常のパス形式に変換する
            Uri u = new Uri(path);
            path = u.LocalPath + Uri.UnescapeDataString(u.Fragment);
            path = System.IO.Path.GetDirectoryName(path);

            return path;
        }


        /// <summary>
        /// 最終指示が格納されているall_setting.txtを読み出します。
        /// </summary>
        /// <returns>成功した場合はDictionary<string, string>で、失敗した場合はnullで応答します</returns>
        public static Dictionary<string, string> csv_reader()
        {
            try
            {
                // 応答用Dictionary
                var dict = new Dictionary<string, string>();

                // csvファイルを開く
                using (var sr = new System.IO.StreamReader(get_CodeBase_path() + "\\all_setting.txt", Encoding.GetEncoding(enc)))
                {
                    // ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // ファイルから一行読み込む
                        var line = sr.ReadLine();
                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        var values = line.Split(',');

                        // Dictionaryに追加
                        dict.Add(values[0], values[1]);

                        /*
                        Console.WriteLine(values[0] + "は" + values[1]);
                        foreach (var pair in dict)
                        {
                            Console.WriteLine("Dic設定上の" + pair.Key + "は" + pair.Value);
                        }
                        */
                    }
                }
                return dict;
            }
            catch (System.Exception ex)
            {
                // ファイルを開くのに失敗したとき
                logger.Error(" CSV読み込み失敗: " + ex.Message);
                return null;
            }
            
        }

        /// <summary>
        /// 受け取った最終指示が可能されているdcitをそのままall_setting.txtに書き出します。
        /// </summary>
        /// <param name="dict">全顧客の指示一覧</param>
        /// <returns>書き込み成功時true,失敗時false</returns>
        public static bool csv_writer(Dictionary<string, string> dict)
        {
            try
            {

                string write_text = "";
                foreach (var pair in dict)
                {
                    write_text += pair.Key + "," + pair.Value + "\r\n";
                }

                // 指示ファイル上書き指示
                File_CTRL.file_OverWrite(write_text, get_CodeBase_path() + "\\all_setting.txt", false);

                return true;
            }
            catch (System.Exception ex)
            {
                // ファイルを開くのに失敗したとき
                logger.Error(" CSV書き込み失敗: " + ex.Message);
                return false;
            }

        }

        /// <summary>
        /// フォルダパス(MT4インストール)のキー値を使用して最終指示を上書き更新します
        /// </summary>
        /// <param name="folder_path">MT4のインストールパス</param>
        /// <param name="last_order">最終指示内容</param>
        /// <returns></returns>
        public static bool last_order_rewrite(string folder_path, string last_order)
        {
            try
            {
                Dictionary<string, string> dict = File_CTRL.csv_reader();

                // キーの存在有無に関わらず、書き込みを行います。存在チェックをする場合はキー値で値を取り出すとエラートラップできます。
                // string test = dict[folder_path];
                // この様な形でやるとキーエラーが発生します。
                dict[folder_path] = last_order;

                return csv_writer(dict);
            }
            catch (System.Exception ex)
            {
                // ファイルを開くのに失敗したとき
                logger.Error(" 最終指示情報更新失敗: " + ex.Message);
                return false;
            }

        }


    }
}

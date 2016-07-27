using System;
using System.Collections.Generic;
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
        public static bool CreateUserFolder(dynamic user, out string stc_ID)
        {
            try
            {
                string folder = @"C:\Users\GFIT\" + user.Stc_ID + @"\" + user.Broker_Name + @"\" + user.MT4_ID + @"\" + user.Ccy + @"\" + user.Time_Period + @"\" + user.EA_Name;
                stc_ID = user.Stc_ID;
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
        public static void file_OverWrite(string text, string out_put_full_path)
        {
            try
            {
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(out_put_full_path)))
                {
                    System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(out_put_full_path));
                }

                var utf8_encoding = new System.Text.UTF8Encoding(false);
                var writer = new StreamWriter(out_put_full_path, false, utf8_encoding);
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


        public static void get_folders()
        {
            try
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"C:\Users\GFIT"); //C:\Users\GFIT
                IEnumerable<System.IO.DirectoryInfo> subFolders = di.EnumerateDirectories("*", System.IO.SearchOption.AllDirectories);

                string folder_log_path = @"C:\STC_controller_log\Folder_log\" + System.DateTime.Now.ToString("yyyyMMdd") + "folder_backup.txt";
                //
                file_AddWrite("インストール済みフォルダの階層情報バックアップです", folder_log_path);

                //サブフォルダを列挙する
                foreach (System.IO.DirectoryInfo subFolder in subFolders)
                {
                    Console.WriteLine(subFolder.FullName);
                    file_AddWrite(subFolder.FullName, folder_log_path);
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(" フォルダ取得失敗: " + ex.Message);
            }

        }

    }
}

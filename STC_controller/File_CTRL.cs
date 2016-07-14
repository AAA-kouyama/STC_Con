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
        private static string read_file = "test.txt";
        private static string write_file = "aaa.txt";
        
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

        public static void file_AddWrite(string text)
        {

            //おもちゃ実装なのでつかわないでね～
            try
            {
                var utf8_encoding = new System.Text.UTF8Encoding(false);
                var writer = new StreamWriter(write_file, true, utf8_encoding);
                writer.WriteLine(text);
                writer.Close();

            }
            catch (System.Exception ex)
            {
                //捕捉出来ていないイレギュラーの場合のエラー処理
                logger.Error(ex.Message);
            }
        }

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

    }
}

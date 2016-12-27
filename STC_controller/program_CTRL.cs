using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;


namespace STC_controller
{
    class program_CTRL 
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 指定プログラム開始関数
        /// </summary>
        /// <param name="ProgramPath">起動対象の絶対パス</param>
        public static bool StartProgram(string ProgramPath)
        {

            try
            {
                Process.Start(ProgramPath, "/skipupdate");
                return true;
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                //対象なしの場合のエラー処理
                logger.Error(ex.Message);
                return false;
            }
            catch (System.Exception ex)
            {
                //捕捉出来ていないイレギュラーの場合のエラー処理
                logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 指定プログラム停止関数
        /// </summary>
        /// <param name="WindowTexts">停止対象のウィンドウのタイトルテキスト</param>
        public static void StopProgram(string ExecutablePath)
        {
            try
            {
                string error = "";
                int procID = get_processID(ExecutablePath, out error);

                if (procID == 0)
                {
                    //対象不在エラー
                    error = "対象が見つかりません";
                }
                // プロセス ID が 0 のプロセスを取得する
                System.Diagnostics.Process target_hProcess = System.Diagnostics.Process.GetProcessById(procID);
                target_hProcess.Kill();
                //target_hProcess.CloseMainWindow();
                //target_hProcess.WaitForExit();
            }

            catch (System.IndexOutOfRangeException ex)
            {
                logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 対象のプログラムを検索します
        /// </summary>
        /// <param name="ExecutablePath">プログラム(EXE)のフルパス</param>
        /// <param name="error">エラー内容</param>
        /// <returns></returns>
        public static bool SearchProgram(string ExecutablePath, out string error)
        {
            try
            {
                error = "";

                int procID = get_processID(ExecutablePath, out error);

                if (procID == 0)
                {
                    //対象不在エラー
                    error = "対象が見つかりません";
                    //エラー処理追加予定
                    return false;
                }

                return true;
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
                error = ex.Message;
                return false;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// OS上の全プロセスからプロセスの実行パスを基に該当するプログラムのプロセスIDを取得します。
        /// 見つからない場合はゼロを応答します。
        /// </summary>
        /// <param name="ExecutablePath">探索対象のプログラムの起動パス</param>
        /// <param name="error"></param>
        /// <returns></returns>
        private static int get_processID(string ExecutablePath, out string error)
        {
            int procID = 0;
            error = "";

            try
            {

                System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_Process");
                System.Management.ManagementObjectCollection moc = mc.GetInstances();
                foreach (System.Management.ManagementObject mo in moc)
                {
                    /*
                    Console.WriteLine("プロセス名:{0}", mo["Name"]);
                    Console.WriteLine("プロセスID:{0}", mo["ProcessId"]);
                    Console.WriteLine("ファイル名:{0}", mo["ExecutablePath"]);
                    //ファイル名:C:\Users\GFIT\u00000212\OANDA-Japan Practice - デモ口座\6835252\USDJPY\H1\terminal.exe
                    */

                    if (ExecutablePath == (String)mo["ExecutablePath"])
                    {
                        procID = Convert.ToInt32(mo["ProcessId"]);
                    }
                    mo.Dispose();
                }

                moc.Dispose();
                mc.Dispose();
                return procID;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                error = ex.Message;
                return procID;
            }
        }
    }
}

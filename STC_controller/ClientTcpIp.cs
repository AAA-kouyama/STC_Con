using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace STC_controller
{
    class ClientTcpIp
    {
        public int intNo;
        public TcpClient objSck;
        public NetworkStream objStm;

        // クライアント送受信スレッド
        /// <summary>
        /// 監視用ソケットの送受信（主に接続維持）
        /// ちなみに、Telnetで指定ポートに接続して入力するとecho応答してきます
        /// </summary>
        public void ReadWrite()
        {
            try
            {
                while (true)
                {
                    // ソケット受信
                    Byte[] rdat = new Byte[1024];
                    int ldat = objStm.Read(rdat, 0, rdat.GetLength(0));
                    if (ldat > 0)
                    {
                        // クライアントからの受信データ有り
                        // 送信データ作成
                        Byte[] sdat = new Byte[ldat];
                        Array.Copy(rdat, sdat, ldat);
                        String msg = "(" + intNo + ")" + System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(sdat);
                        sdat = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetBytes(msg);
                        // ソケット送信
                        objStm.Write(sdat, 0, sdat.GetLength(0));
                    }
                    else
                    {
                        // ソケット切断有り
                        // ソケットクローズ
                        objStm.Close();
                        objSck.Close();
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}

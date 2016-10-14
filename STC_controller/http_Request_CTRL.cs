using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace STC_controller
{
    class http_Request_CTRL
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private static Task<HttpResponseMessage> httpClient;

        private static string username = "gfitadmin";
        private static string password = "Lvf24q6ngn4o";


        public static string http_get(string url, out bool status)
        {
            try
            {
                // オレオレ証明を強制的に正常証明とするクラスを呼び出します。
                System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

                //HttpWebRequestを作成
                System.Net.HttpWebRequest webreq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                //または、
                //System.Net.WebRequest webreq = System.Net.WebRequest.Create(url);
                
                //認証の設定
                webreq.Credentials = new System.Net.NetworkCredential(username, password);
                
                //サーバーからの応答を受信するためのHttpWebResponseを取得
                System.Net.HttpWebResponse webres = (System.Net.HttpWebResponse)webreq.GetResponse();
                //または、
                //System.Net.WebResponse webres = webreq.GetResponse();

                //応答データを受信するためのStreamを取得
                System.IO.Stream st = webres.GetResponseStream();
                //文字コードを指定して、StreamReaderを作成
                System.IO.StreamReader sr = new System.IO.StreamReader(st, System.Text.Encoding.UTF8);

                //データをすべて受信してUnicodeエスケープ文字を元に戻す
                string result = Regex.Unescape(sr.ReadToEnd());

                //Console.WriteLine(result);

                //閉じる
                sr.Close();
                st.Close();
                webres.Close();
                //「st.Close()」や「webres.Close()」だけでもよい

                //取得したソースを表示する
                //Console.WriteLine(htmlSource);
                status = true;
                return result;
            } catch (System.Exception ex)
            {
                status = false;
                logger.Error(ex.Message);
                return ex.ToString();
            }

        }

        public static async Task<string> http_post(string post_word, string target)
        {

            string result = "";
            try
            {

                STC_controller.File_CTRL.file_OverWrite(post_word, System.IO.Directory.GetCurrentDirectory() + @"\response.json");

                //送信するファイルのパス
                string filePath = System.IO.Directory.GetCurrentDirectory() + @"\response.json";
                string fileName = System.IO.Path.GetFileName(filePath);
                //送信先のURL
                string url = target;
                //文字コード
                System.Text.Encoding enc =
                    System.Text.Encoding.GetEncoding("UTF-8");
                //区切り文字列
                string boundary = System.Environment.TickCount.ToString();

                //WebRequestの作成
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)
                    System.Net.WebRequest.Create(url);

                //認証の設定
                req.Credentials = new System.Net.NetworkCredential(username, password);


                //メソッドにPOSTを指定
                req.Method = "POST";
                //ContentTypeを設定
                req.ContentType = "multipart/form-data; boundary=" + boundary;

                //POST送信するデータを作成
                string postData = "";
                postData = "--" + boundary + "\r\n" +
                    "Content-Disposition: form-data; name=\"comment\"\r\n\r\n" +
                    "これは、テストです。\r\n" +
                    "--" + boundary + "\r\n" +
                    "Content-Disposition: form-data; name=\"upfile\"; filename=\"" +
                        fileName + "\"\r\n" +
                    "Content-Type: application/octet-stream\r\n" +
                    "Content-Transfer-Encoding: binary\r\n\r\n";
                //バイト型配列に変換
                byte[] startData = enc.GetBytes(postData);
                postData = "\r\n--" + boundary + "--\r\n";
                byte[] endData = enc.GetBytes(postData);

                //送信するファイルを開く
                System.IO.FileStream fs = new System.IO.FileStream(
                    filePath, System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);

                //POST送信するデータの長さを指定
                req.ContentLength = startData.Length + endData.Length + fs.Length;

                //データをPOST送信するためのStreamを取得
                System.IO.Stream reqStream = req.GetRequestStream();
                //送信するデータを書き込む
                reqStream.Write(startData, 0, startData.Length);
                //ファイルの内容を送信
                byte[] readData = new byte[0x1000];
                int readSize = 0;
                while (true)
                {
                    readSize = fs.Read(readData, 0, readData.Length);
                    if (readSize == 0)
                        break;
                    reqStream.Write(readData, 0, readSize);
                }
                fs.Close();
                reqStream.Write(endData, 0, endData.Length);
                reqStream.Close();

                //サーバーからの応答を受信するためのWebResponseを取得
                System.Net.HttpWebResponse res =
                    (System.Net.HttpWebResponse)req.GetResponse();
                //応答データを受信するためのStreamを取得
                System.IO.Stream resStream = res.GetResponseStream();
                //受信して表示
                System.IO.StreamReader sr =
                    new System.IO.StreamReader(resStream, enc);
                Console.WriteLine(sr.ReadToEnd());
                //閉じる
                sr.Close();


            }
            catch (Exception ex)
            {
                result = "ERROR: " + ex.Message;
                logger.Error(ex.Message);
            }

            return result;
        }



    }


    // ステージング環境で証明書がオレオレ証明なのでチェックを回避するために追加実装
    // オレオレでも正しいと強制設定するクラスです。
    public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
    {

        public TrustAllCertificatePolicy() { }
        public bool CheckValidationResult(System.Net.ServicePoint sp,
            System.Security.Cryptography.X509Certificates.X509Certificate cert,
            System.Net.WebRequest req,
            int problem)
        {
            return true;
        }
    }

}

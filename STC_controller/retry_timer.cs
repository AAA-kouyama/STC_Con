using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace STC_controller
{

    public class retry_timer
    {
        private Timer myTimer;

        /// <summary>
        /// 監視用タイマーを生成します。
        /// </summary>
        public void NewTimer()
        {
            myTimer = new Timer();
            myTimer.Enabled = true;
            myTimer.AutoReset = true;
            myTimer.Interval = 5000;
            myTimer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
        }

        /// <summary>
        /// 監視用タイマーのタイマーイベントです。
        /// キューイングされているjsonから処理上に付加されたタグを除去してリクエスト時の状態に戻します。
        /// その上でリクエストの受信時の動作を呼び出して、再実行します。
        /// 戻りのPOSTリクエストをコメントにしてるのでasyncが警告となっています。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private async void OnTimerEvent(object source, ElapsedEventArgs e)
        {

            int get_cnt = Program.queue_list.Count;

            if (get_cnt > 0)
            {
                dynamic retry_obj = null;
                retry_obj = Json_Util.reParse(Program.queue_list);
                Program.queue_list.RemoveRange(0, get_cnt);

                foreach (dynamic retry in (object[])retry_obj)
                {

                    bool status = false;
                    string error = "";
                    // response登録先URL
                    string post_url = "https://systrade-cloud.com/server/resultbox.php";

                    //キューに入っている内容をリクエスト時の状態に再整形
                    if (Json_Util.get_Value(retry, "Check_Status") != "")
                    {
                        retry.Delete("Check_Status");
                    }
                    if (Json_Util.get_Value(retry, "EA_Status") != "")
                    {
                        retry.Delete("EA_Status");
                    }

                    dynamic json_obj = Json_request_action.request_motion("[" + retry.ToString() + "]", out status, out error);

                    if (status)
                    {
                        if (json_obj != null)
                        {
                            // 処理結果登録
                            await http_Request_CTRL.http_post(json_obj.ToString(), post_url);

                            // debug用
                            //string file_path = "C:\\Users\\f464\\Desktop\\Response.json";
                            //File_CTRL.file_OverWrite(json_obj.ToString(), file_path);
                        }

                    }

                }
            }
        }

        /// <summary>
        /// 監視用タイマーを開始します。
        /// </summary>
        public void StartTimer()
        {
            myTimer.Start();
        }

        /// <summary>
        /// 監視用タイマーを停止します。
        /// </summary>
        public void StopTimer()
        {
            myTimer.Stop();
        }
    }
}

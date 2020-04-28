using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class TimerScriber : MonoBehaviour
{
    public Text countdown4text;
    private int totaltime4;
    //public Text countdown3text;
    private string netChanged;
    public Text countdown1text;
    private int totaltime1;
    public Text countdown3text;
    private static readonly NetStatusPublisher nsp = new NetStatusPublisher();

    public string TimerCount(object sender, NetStatusPublisher.NetChangedEventArgs e)
    {

        NetStatusPublisher netStatusPublisher = (NetStatusPublisher)sender;
        // 访问sender中的公共字段
        netChanged = e.netStatusNow;

        // 触发事件，开始计时
        //StartCoroutine(TimerProcess());
        // TimerProcess();
        return null;
    }



    // 订阅事件（链）
    void ScribeMethod()
    {
        //NetStatusPublisher netStatusPublisher = new NetStatusPublisher();
        //TimerScriber timerScriber = new TimerScriber();

        // 注册事件
        //nsp.NetChanged += timerScriber.TimerCount;
        nsp.NetChanged += TimerCount;
    }

    // Start is called before the first frame update
    //private void CheckNet()
    //{
    //    nsp.CheckNet();
    //}
    void Start()
    {
        // 启动订阅事件方法
        ScribeMethod();

        countdown1text.text = string.Format("{0:00}:{1:00}", (int)totaltime1 / 60, (float)totaltime1 % 60);
        //countdown4text.text = string.Format("{0:00}:{1:00}:{2:00}", (int)totaltime4/60/60, ((int)totaltime4/60%60), ((int)totaltime4%60));

        // 触发事件，启动协程轮询网络状态
        //StartCoroutine( CountDown());
        //StartCoroutine( LocalTimeControlIE());

        // 启动事件发布协程
        StartCoroutine(nsp.CheckNet());
        //nsp.CheckNetAsync();
        //InvokeRepeating("TimerProcess",1,3);
        StartCoroutine(TimerProcess());
    }

    //private void RepeatTimerCount()
    //{
    //}

    //private async void TimerProcessAsync()
    //{
    //    await Task.Run(() =>
    //    {
    //        TimerProcess();
    //    }
    //    );
    //}

    IEnumerator TimerProcess()
    //private void TimerProcess()
    {
        while (true)
        {
            //nsp.NetChanged -= TimerCount;
            //NetTimer netTimer = new NetTimer();
            //LocalTimer localTimer = new LocalTimer();

            //while (true)
            //{
            Debug.Log("timerLoop, 网络状态：" + netChanged);
            // 访问sender中的公共字段
            if (netChanged == "DownToUp")
            //nettime
            {
                StopCoroutine("LocalTimeControlIE"); // 注意协程使用方法
                Debug.Log(" netstatus from Down to Up，关闭本地时间计时协程，开始获取网络时间");
                yield return StartCoroutine("NetworkTimeControlIE");

            }

            else if (netChanged == "UpToDown")
            //localtime
            {
                StopCoroutine("NetworkTimeControlIE");
                Debug.Log("netstatus from Up to Down，关闭网络时间计时协程，开始获取本地时间");
                // LocalTimer lt = new LocalTimer();
                // lt.CountUpTime(countdown4text);
                //StartCoroutine(localTimer.LocalTimeControlIE());
                yield return StartCoroutine("LocalTimeControlIE");
            }
            else
            {
                Debug.Log("nothing happened");
            }

            //yield return new WaitForSeconds(2.0f);
            //yield return null;
            //totaltime4--;
            //}
        }
    }

    // 实现倒计时方法一：用IEnumerator协程迭代器
    //IEnumerator CountDown()
    //{


    //    while (totaltime1 >= 0)
    //    {
    //        int M = (int)(totaltime1 / 60);
    //        float S = totaltime1 % 60;

    //        // 显示格式为 M分：S秒
    //        countdown1text.GetComponent<Text>().text = string.Format("{0:00}:{1:00}", M, S);
    //        // 每一帧update后等待1秒延迟再继续下一帧
    //        yield return new WaitForSeconds(1);
    //        // 时间减去一秒
    //        totaltime1--;

    //        // 计时器复位重新开始计时

    //        if (totaltime1 < 0)
    //        {
    //            totaltime1 = 20;
    //        }
    //    }
    //}


    // 获取本地操作系统的时间来倒计时
    /// <summary>
    /// 计时携程
    /// </summary>
    /// <returns></returns>
    //IEnumerator LocalTimeControlIE()
    //{
    //    while (true)
    //    {
    //        int H = (int)(23 - System.DateTime.Now.Hour);
    //        int M = (int)(60 - System.DateTime.Now.Minute);
    //        int S = (int)(60 - System.DateTime.Now.Second);
    //        TimerScriber.countdown4text.text = "本地时间倒计时\n" + string.Format("{0:00}:{1:00}:{2:00}", H, M, S);
    //        yield return new WaitForSeconds(1.0f);
    //    }
    //}







    // 本地正向计时器
    //public class LocalTimer : MonoBehaviour
    //{
    //public Text countdown3text;
    //private string netChanged;

    public void CountUpTime(Text countdown3text)
    {
        //
        int H = (int)(23 - System.DateTime.Now.Hour);
        int M = (int)(60 - System.DateTime.Now.Minute);
        int S = (int)(60 - System.DateTime.Now.Second);
        countdown3text.text = "本地时间倒计时\n" + string.Format("{0:00}:{1:00}:{2:00}", H, M, S);
    }

    // 获取本地操作系统的时间来倒计时
    /// <summary>
    /// 计时携程
    /// </summary>
    /// <returns></returns>
    public IEnumerator LocalTimeControlIE()
    {
        while (true)
        {
            if (netChanged == "DownToUp")
            {
                break;
            }
            int H = (int)(23 - System.DateTime.Now.Hour);
            int M = (int)(60 - System.DateTime.Now.Minute);
            int S = (int)(60 - System.DateTime.Now.Second);
            countdown4text.text = "本地时间倒计时\n" + string.Format("{0:00}:{1:00}:{2:00}", H, M, S);
            yield return new WaitForSeconds(1.0f);
        }
        
    }
    //}

    // 网络倒计时器
    //public class NetTimer
    //{
        private DateTime theNetTime;
        //private int totaltime4;
        //private Text countdown4text;

        /// <summary>
        /// 获取网络时间来倒计时
        /// </summary>
        /// <returns></returns>
        public IEnumerator NetworkTimeControlIE()
        {
            Debug.Log("开始网络倒计时协程");

            int nH = 00;
            int nM = 00;
            int nS = 00;


            Debug.Log("开始请求网络时间");
            //yield return StartCoroutine(RequestNetworkTime());
            GetNetStandardTime nst = new GetNetStandardTime();
            nst.RequestNetworkTime();

            NetStatusPublisher nsp = new NetStatusPublisher();
            // if (serverTime == null)
            //if (CheckServeStatus(urls) )
            Debug.Log("开始判断网络连接状态。");
            //if (nsp.CheckNetStatus() == true)
            if (true)
            {
                Debug.Log(" 获取时间");
                nst.DataStandardTime();
                nH = (int)nst.theNetTime.Hour;
                nM = (int)nst.theNetTime.Minute;
                nS = (int)nst.theNetTime.Second;
                totaltime4 = 86400 - ((nH * 60 * 60) + (nM * 60) + nS);
                //}
                //else
                //{
                //    Debug.Log(" 侦测到网络连接异常，时间设置为10秒");
                //    totaltime4 =10;
                //}


                while (totaltime4 >= 1)
                {
                    if (netChanged == "UpToDown")
                    {
                        break;
                    }
                    Debug.Log("开始倒计时");
                    int H = (int)totaltime4 / 60 / 60;
                    int M = (int)totaltime4 / 60 % 60;
                    int S = (int)totaltime4 % 60;
                    countdown4text.text = "网络时间倒计时\n" + string.Format("{0:00}:{1:00}:{2:00}", H, M, S);
                    yield return new WaitForSeconds(1.0f);
                    // 时间减去一秒
                    totaltime4--;

                    // 计时器复位重新开始计时
                    if (totaltime4 < 1)
                    {
                        totaltime4 = 86400 - ((nH * 60 * 60) + (nM * 60) + nS);
                    }

                }
            }
            else
            {
                yield break;
            }
        }
    //}
}



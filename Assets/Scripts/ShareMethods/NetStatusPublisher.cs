using DemoProject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Unity.Messenger.Models;
using UnityEngine;

/// <summary>
/// 网络状态发布器
/// </summary>
public class NetStatusPublisher 
{
    private bool lastNetStatus = false;
    private static bool isConnectedNow = false;
    private readonly DateTime startCountLocalTime =default;
    private static bool isPingSuccess;

    // 声明委托类
    public delegate string NetChangedEventHandler(System.Object sender, NetChangedEventArgs e);
    // 声明事件成员
    public event NetChangedEventHandler NetChanged;

    // 发布事件

    // 声明NetChangedEventArgs参数类，传递给Observer订阅者所感兴趣的信息
    public class NetChangedEventArgs : EventArgs
    {
        public readonly string netStatusNow;
        public NetChangedEventArgs(string netStatusNow)
        {
            this.netStatusNow=netStatusNow;
        }
    }

    // 调用事件
    // protected void OnBoilerEventLog(string message)
    // 虚方法可以供继承类重写，以便拒绝不需要的监听
    protected virtual void OnNetChanged(NetChangedEventArgs e)
    {
        if (NetChanged != null)  // 如果有对象注册
        {
            Debug.Log(" 发布事件消息，通知订阅者对接收到的消息做后续方法的操作");
            NetChanged(this, e);  
            //Delegate[] delegAry = NetChanged.GetInvocationList();
            //foreach (NetChangedEventHandler handler in delegAry)
            //{
            //    handler.BeginInvoke(this,e,CallBack, handler);
            //}
        }
    }

    public void CallBack(IAsyncResult obj)
    {
        NetChangedEventHandler handler = (NetChangedEventHandler)obj.AsyncState;
        string res = handler.EndInvoke(obj);
        //NetChanged(res);

    }

    //private void Start()
    //{
    //    InvokeRepeating("CheckNet" ,1,3);

    //}

    // 发布事件消息
    public IEnumerator CheckNet()
    //public void CheckNet()

    //public async void CheckNetAsync()
    {
        //CancellationTokenSource source = new CancellationTokenSource();
        //Task taskCheckNet = new Task(async () =>
        //{

        while (true)
        {
            //yield return CheckNetStatus();
            CheckNetStatusAsync();
            //bool connectedResult = CheckNetAsync(isConnectedNow).Result;
            string netChangedStatusNow = null;
            // 1.离线变为在线
            if (isConnectedNow == true && (lastNetStatus == false))
            {
                lastNetStatus = true;
                netChangedStatusNow = "DownToUp";
                //NetChangedEventArgs dtue = new NetChangedEventArgs(netChangedStatusNow);
                //Debug.Log("监听到事件发生：离线变为在线" + dtue);
                //OnNetChanged(dtue);
            }

            // 2.在线变为离线
            else if (isConnectedNow == false && (lastNetStatus == true))
            {
                lastNetStatus = false;
                netChangedStatusNow = "UpToDown";
                //    NetChangedEventArgs utde = new NetChangedEventArgs(netChangedStatusNow);
                //    Debug.Log("监听到事件发生：在线变为离线");
                //    startCountLocalTime = System.DateTime.Now;
                //    OnNetChanged(utde);
            }

            // 3.一开始就是离线启动的游戏
            else if (isConnectedNow == false && (lastNetStatus == false) && (startCountLocalTime == default))
            {
                Debug.Log("Always down, start local time count");
                netChangedStatusNow = "UpToDown";
                //NetChangedEventArgs utde = new NetChangedEventArgs(netChangedStatusNow);
                //Debug.Log("监听到事件发生：在线变为离线");
                //startCountLocalTime = System.DateTime.Now;
                //OnNetChanged(utde);
            }

            // 4.一直离线或一直在线则不做处理，不发消息
            else
            {
                Debug.Log("Always up or down, nothing to do");
                netChangedStatusNow = null;
            }

            NetChangedEventArgs dtue = new NetChangedEventArgs(netChangedStatusNow);
            Debug.Log("监听到事件发生：离线变为在线" + dtue);
            OnNetChanged(dtue);
            //等待5秒后进行下一次问询
            yield return new WaitForSeconds(5f);
            //yield return null;
            //Thread.Sleep(3000);
        }
        //}
        //);
        //taskCheckNet.Start();
        //source.CancelAfter(5000);

    }

    //public async Task<bool> CheckNetAsync(bool isconnectednow)
    //{

    //    bool connectedResult = await CheckNetStatusAsync(isconnectednow);
    //    return connectedResult;
    //}


    //void Start()
    //{
    //    // 触发事件，启动协程轮询网络状态
    //    StartCoroutine( CheckNet());
    //}

    //// 发布事件消息
    //public IEnumerator CheckNet()

    ////public async void CheckNetAsync()
    //{
    //    //CancellationTokenSource source = new CancellationTokenSource();
    //    //Task taskCheckNet = new Task(async () =>
    //    //{

    //        while (true)
    //        {
    //            yield return CheckNetStatus();
    //            //bool connectedResult = await CheckNetStatusAsync(isConnectedNow);
    //            string netChangedStatusNow = null;
    //            // 1.离线变为在线
    //            if (isConnectedNow == true && (lastNetStatus == false))
    //            {
    //                lastNetStatus = true;
    //                netChangedStatusNow = "DownToUp";
    //                //NetChangedEventArgs dtue = new NetChangedEventArgs(netChangedStatusNow);
    //                //Debug.Log("监听到事件发生：离线变为在线" + dtue);
    //                //OnNetChanged(dtue);
    //            }

    //            // 2.在线变为离线
    //            else if (isConnectedNow == false && (lastNetStatus == true))
    //            {
    //                lastNetStatus = false;
    //                netChangedStatusNow = "UpToDown";
    //            //    NetChangedEventArgs utde = new NetChangedEventArgs(netChangedStatusNow);
    //            //    Debug.Log("监听到事件发生：在线变为离线");
    //            //    startCountLocalTime = System.DateTime.Now;
    //            //    OnNetChanged(utde);
    //            }

    //            // 3.一开始就是离线启动的游戏
    //            else if (isConnectedNow == false && (lastNetStatus == false) && (startCountLocalTime == default))
    //            {
    //                Debug.Log("Always down, start local time count");
    //                netChangedStatusNow = "UpToDown";
    //                //NetChangedEventArgs utde = new NetChangedEventArgs(netChangedStatusNow);
    //                //Debug.Log("监听到事件发生：在线变为离线");
    //                //startCountLocalTime = System.DateTime.Now;
    //                //OnNetChanged(utde);
    //            }

    //            // 4.一直离线或一直在线则不做处理，不发消息
    //            else
    //            {
    //                Debug.Log("Always up or down, nothing to do");
    //            }

    //            NetChangedEventArgs dtue = new NetChangedEventArgs(netChangedStatusNow);
    //            Debug.Log("监听到事件发生：离线变为在线" + dtue);
    //            OnNetChanged(dtue);
    //            //等待5秒后进行下一次问询
    //            yield return new WaitForSeconds(1f);
    //            //Thread.Sleep(3000);
    //        }
    //    //}
    //    //);
    //    //taskCheckNet.Start();
    //    //source.CancelAfter(5000);

    //}



    /// <summary>
    /// 检测网络连接状态
    /// </summary>
    /// <param name="serverUrls"></param>
    //public static void CheckServeStatus(string[] urls)
    //public bool CheckNetStatus()
    //public static async Task<bool> CheckNetStatusAsync(bool isConnectedNow)
    public static async void CheckNetStatusAsync()
    {
        const string netUrls = "www.baidu.com;www.sina.com;www.cnblogs.com;www.google.com;www.163.com;www.csdn.com";
        string[] serverUrls = netUrls.Split(new char[] { ';' });
        int errCounts = 0;//ping时连接失败个数
        await Task.Run
            (
                () =>
                {
                    MyPing(serverUrls, out errCounts);
                }
            );
        //MyPing(serverUrls, out errCounts);

        if (!isPingSuccess)
        {
            if ((double)errCounts / serverUrls.Length >= 0.3)
            {
                Debug.Log("网络连接异常");
                isConnectedNow = false;
                //Console.WriteLine("网络异常~连接多次无响应");
            }
            else
            {
                Debug.Log("网络在颤抖");
                isConnectedNow = true;
            }
        }
        else
        {
            Debug.Log("网络连接正常");
            isConnectedNow = true;
            //Console.WriteLine("网络正常");
        }

        //return isConnectedNow;
    }

    /// <summary>
    /// Ping命令检测网络是否畅通
    /// </summary>
    /// <param name="urls">URL数据</param>
    /// <param name="errorCount">ping时连接失败个数</param>
    /// <returns></returns>
    public static bool MyPing(string[] urls, out int errorCount)
    {
        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
        errorCount = 0;
        try
        {
            PingReply pr;
            for (int i = 0; i < urls.Length; i++)
            {
                pr = ping.Send(urls[i]);
                if (pr.Status != IPStatus.Success)
                {
                    isPingSuccess = false;
                    errorCount++;
                    Debug.LogFormat("目标服务器{0}不可达,错误计数errorCount={1}", urls[i], errorCount);
                }
                // Console.WriteLine("Ping " + urls[i] + "    " + pr.Status.ToString());
                Debug.Log("Ping " + urls[i] + "    " + pr.Status.ToString());
            }
        }
        catch
        {
            Debug.Log("ping failed");
            isPingSuccess = false;
            errorCount = urls.Length;
        }
        //if (errorCount >= 0 && errorCount < 3)
        // isPingSuccess = true;
        Debug.Log("ping的状态：" + isPingSuccess);
        return isPingSuccess;
    }





}

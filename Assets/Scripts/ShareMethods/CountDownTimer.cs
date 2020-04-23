using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;
using UnityEngine.UI;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class CountDownTimer : MonoBehaviour
{
    // 剩余总时间
    private int totaltime1 = 5;
    private int totaltime2 = 6;
    private int totaltime3;
    private int totaltime4;

    private float intervaletime=1;

    // UI倒计时显示文本
    public Text countdown1text;
    public Text countdown2text;
    public Text countdown3text;
    public Text countdown4text;
    private DateTime dt;

    public string serverTime=null;
    private DateTime theNetTime;
    private const string url = "www.baidu.com;www.sina.com;www.cnblogs.com;www.google.com;www.163.com;www.csdn.com";
    private string[] urls = url.Split(new char[] { ';' });
    void Start()
    {
        // UI倒计时显示的格式
        countdown1text.text = string.Format("{0:00}:{1:00}", (int)totaltime1 / 60, (float)totaltime1 % 60);
        countdown2text.text = string.Format("{0:00}:{1:00}", (int)totaltime2 / 60, (float)totaltime2 % 60);
        countdown3text.text = string.Format("{0:00}:{1:00}:{2:00}", 24-(int)totaltime3, 60-((int)totaltime3 / 60), 60-((float)totaltime3 % 60));
        countdown4text.text = string.Format("{0:00}:{1:00}:{2:00}", (int)totaltime4/60/60, ((int)totaltime4/60%60), ((int)totaltime4%60));

        Debug.Log("开始倒计时迭代器协程");
        StartCoroutine(CountDown());
        StartCoroutine( LocalTimeControlIE());
        StartCoroutine(NetStatusHeartBeat());
        //StartCoroutine( NetworkTimeControlIE());
    }

    //多线程监听;
    //private Thread thread;
    //private void NetStatusHeartBeatLoop(object sender, EventArgs e)
    //{
    //    if (thread == null)
    //    {
    //        thread = new Thread(new ThreadStart(Loop));
    //        thread.Start();
    //        MessageBox.Show("成功启动轮检。");
    //    }
    //    else
    //    {
    //        MessageBox.Show("启动轮检失败，轮检已开始。");
    //    }
    //}
    //private void Loop()
    //{
    //    while (true)
    //    {
    //        if (CheckIfFtpFileExists() == true)
    //        {
    //            Tsystem();
    //        }
    //    }
    //    Thread.Sleep(5000);
    //}

    /// <summary>
    /// 监听网络状态的心跳ping包
    /// </summary>
    /// <returns></returns>
    IEnumerator NetStatusHeartBeat()
    {
        // yield return new WaitForSeconds(3f);
        yield return null;
        System.Timers.Timer t25yi = new System.Timers.Timer();//实例化Timer类，设置时间间隔为100毫秒
        t25yi.Elapsed += new System.Timers.ElapsedEventHandler(CheckNet);//当到达时间的时候执行MethodTimer2事件 
        Debug.Log("定时检查网络状态心跳开始");
        t25yi.Interval = 3000;
        t25yi.AutoReset = true;//false是执行一次，true是一直执行
        t25yi.Enabled = true;//设置是否执行System.Timers.Timer.Elapsed事件 
        //}
    }


    private void CheckNet(object sender, EventArgs e)
    {
        // 上一次心跳时的网络状态
        bool lastNetStatus = false;
        Debug.Log("上次的网络状态是：" + lastNetStatus);

        //while (true)
        //{

        bool isConnectedNow = CheckNetStatus(urls);
        if (isConnectedNow && lastNetStatus == false)
        {
            //开始在线倒计时
            StartCoroutine(NetworkTimeControlIE());
            lastNetStatus = true;
        }

        else if (!isConnectedNow && lastNetStatus == true)
        {

            // 开始离线正计时
            // StartCoroutine(LocalTimeControlIE())
            lastNetStatus = false;
        }

        else
        {
            // 一直在线，或一直离线

        }

    }

    // 实现倒计时方法一：用IEnumerator协程迭代器
    IEnumerator CountDown()
    {


        while (totaltime1 >=0)
        {
            int M = (int)(totaltime1 / 60);
            float S = totaltime1 % 60;

            // 显示格式为 M分：S秒
            countdown1text.GetComponent<Text>().text = string.Format("{0:00}:{1:00}", M,S);
            // 每一帧update后等待1秒延迟再继续下一帧
            yield return new WaitForSeconds(1);
            // 时间减去一秒
            totaltime1--;

            // 计时器复位重新开始计时

            if (totaltime1 < 0)
            {
                totaltime1 = 20;
            }
        }
    }


    //实现倒计时方法二：采用Update方法
    //Update is called once per frame
    void Update()
    {
        Debug.Log("开始Update倒计时");

        int M = (int)(totaltime2 / 60);
        float S = totaltime2 % 60;
        if (totaltime2 > 0)
        {
            intervaletime -= Time.deltaTime;
            if (intervaletime <= 0)
            {
                intervaletime += 1;
                totaltime2--;
                countdown2text.text = string.Format("{0:00}:{1:00}", M, S);

            }
        }
        if (totaltime2 <= 0)
        {
            totaltime2 = 20;
        }

    }


    // 获取本地操作系统的时间来倒计时
    /// <summary>
    /// 计时携程
    /// </summary>
    /// <returns></returns>
    IEnumerator LocalTimeControlIE()
    {
        while (true)
        {
            int H = (int)(23 - System.DateTime.Now.Hour);
            int M = (int)(60 - System.DateTime.Now.Minute);
            int S = (int)(60 - System.DateTime.Now.Second);
            countdown3text.text = "本地时间倒计时\n"  + string.Format("{0:00}:{1:00}:{2:00}", H, M, S);
            yield return new WaitForSeconds(1.0f);
        }
    }



// Constructor
//public void myMainPage()
//{
//        //判断当前网络连接状态

//        Network nw = new Network();

//        if (nw.IsAvailable)

//            lbmsg.Text = "网络已连接";

//        else

//            lbmsg.Text = "网络已断开";
//        //InitializeComponent();

//        // Subscribe to the NetworkAvailabilityChanged event
//        DeviceNetworkInformation.NetworkAvailabilityChanged += new EventHandler<NetworkNotificationEventArgs>(NetworkAvailabilityChanged);
//}

//void NetworkAvailabilityChanged(object sender, NetworkNotificationEventArgs e)
//{

//    string msg = "";
//    if (e.IsAvailable)
//    {
//        msg = "网络已连接";

//    }
//    else
//    {
//        msg = "网络已断开";
//    }
//    Dispatcher.BeginInvoke(() => {
//        lbmsg.Text = msg;
//    });
//}

// 请求网络时间来倒计时
/// <summary>
/// 计时携程
/// </summary>
/// <returns></returns>
IEnumerator NetworkTimeControlIE()
    {
        Debug.Log("开始网络倒计时协程");

        int nH = 00;
        int nM = 00;
        int nS = 00;


        yield return StartCoroutine(RequestNetworkTime());
        // if (serverTime == null)
        //if (CheckServeStatus(urls) )
        Debug.Log("开始判断网络连接状态。");
        if (CheckNetStatus(urls) == true)
        {
            Debug.Log(" 获取时间");
            DataStandardTime();
            nH = (int)theNetTime.Hour;
            nM = (int)theNetTime.Minute;
            nS = (int)theNetTime.Second;
            totaltime4 = 86400 - ((nH * 60 * 60) + (nM * 60) + nS);
            //}
            //else
            //{
            //    Debug.Log(" 侦测到网络连接异常，时间设置为10秒");
            //    totaltime4 =10;
            //}


            while (totaltime4 >= 1)
            {
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

    // 判断网络状态

    //public string GetHostNameByIp(string ip)
    //{
    //    ip = ip.Trim();
    //    if (ip == string.Empty)
    //        return string.Empty;
    //    //System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
    //    //{
    //        try
    //        {
    //            // 是否 Ping 的通 
    //            if (System.Net.NetworkInformation.Ping(ip))
    //            {
    //                System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(ip);
    //                return host.HostName;
    //            }
    //            else
    //                return string.Empty;
    //        }
    //        catch (Exception)
    //        {
    //            return string.Empty;
    //        }
    //    //}
    //}



    /// <summary>
    /// 检测网络连接状态
    /// </summary>
    /// <param name="serverUrls"></param>
    //public static void CheckServeStatus(string[] urls)
    public bool CheckNetStatus(string[] serverUrls)
    {

        int errCounts = 0;//ping时连接失败个数

        //if (!LocalConnectionStatus())
        //{
        //    Console.WriteLine("网络异常~无连接");
        //}
        if (!MyPing(serverUrls, out errCounts))
        {
            if ((double)errCounts / serverUrls.Length >= 0.3)
            {
                Debug.Log("网络连接异常");
                return false;
                //Console.WriteLine("网络异常~连接多次无响应");
            }
            else
            {
                Debug.Log("网络在颤抖");
                return true;
            }
        }
        else
        {
                Debug.Log("网络连接正常");
            return true;
            //Console.WriteLine("网络正常");
        }
    }

    /// <summary>
    /// Ping命令检测网络是否畅通
    /// </summary>
    /// <param name="urls">URL数据</param>
    /// <param name="errorCount">ping时连接失败个数</param>
    /// <returns></returns>
    public static bool MyPing(string[] urls, out int errorCount)
    {
        bool isPingSuccess = true;
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
            isPingSuccess = false;
            errorCount = urls.Length;
        }
        //if (errorCount >= 0 && errorCount < 3)
        // isPingSuccess = true;
        Debug.Log("ping的状态："+isPingSuccess);
        return isPingSuccess;
    }

  

    //IEnumerator GetNetworkTime()
    //{
    //    yield return null;
    //    RequestNetworkTime();
    //}
    // 请求网络时间
    IEnumerator RequestNetworkTime()
    {//返回国际标准时间
     //只使用的TimerServer的IP地址，未使用域名
        string[,] TimerServer = new string[14, 2];
        int[] ServerTab = new int[] { 3, 2, 4, 8, 9, 6, 11, 5, 10, 0, 1, 7, 12 };

        TimerServer[0, 0] = "time-a.nist.gov";
        TimerServer[0, 1] = "129.6.15.28";
        TimerServer[1, 0] = "time-b.nist.gov";
        TimerServer[1, 1] = "129.6.15.29";
        TimerServer[2, 0] = "time-a.timefreq.bldrdoc.gov";
        TimerServer[2, 1] = "132.163.4.101";
        TimerServer[3, 0] = "time-b.timefreq.bldrdoc.gov";
        TimerServer[3, 1] = "132.163.4.102";
        TimerServer[4, 0] = "time-c.timefreq.bldrdoc.gov";
        TimerServer[4, 1] = "132.163.4.103";
        TimerServer[5, 0] = "utcnist.colorado.edu";
        TimerServer[5, 1] = "128.138.140.44";
        TimerServer[6, 0] = "time.nist.gov";
        TimerServer[6, 1] = "192.43.244.18";
        TimerServer[7, 0] = "time-nw.nist.gov";
        TimerServer[7, 1] = "131.107.1.10";
        TimerServer[8, 0] = "nist1.symmetricom.com";
        TimerServer[8, 1] = "69.25.96.13";
        TimerServer[9, 0] = "nist1-dc.glassey.com";
        TimerServer[9, 1] = "216.200.93.8";
        TimerServer[10, 0] = "nist1-ny.glassey.com";
        TimerServer[10, 1] = "208.184.49.9";
        TimerServer[11, 0] = "nist1-sj.glassey.com";
        TimerServer[11, 1] = "207.126.98.204";
        TimerServer[12, 0] = "nist1.aol-ca.truetime.com";
        TimerServer[12, 1] = "207.200.81.113";
        TimerServer[13, 0] = "nist1.aol-va.truetime.com";
        TimerServer[13, 1] = "64.236.96.53";
        int portNum = 13;
        string hostName;
        byte[] bytes = new byte[1024];
        int bytesRead = 0;

        string strResponse = null;
        string str2 = null;


        System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
        for (int i = 0; i < 13; i++)
        {
            hostName = TimerServer[ServerTab[i], 0];

            Debug.Log("开始请求远端服务器 hostName:" + hostName);
            //try
            //{
            // 同步连接服务器
            // client.Connect(hostName, portNum);

            // 异步连接服务器
            // client.BeginConnect(hostName, Convert.ToInt32(portNum), new AsyncCallback(ConnectCallback),client);
            Debug.Log("开始异步连接服务器");
            var connectResult = client.BeginConnect(hostName, portNum, null, null);
            var connectOK = connectResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
            Debug.Log("连接服务器状态：" + connectOK);

            if (client.Connected)
            {

                yield return client.GetStream();
                System.Net.Sockets.NetworkStream ns = client.GetStream();
                Debug.Log("开始获取网络字节流：" + ns);

                //同步读取
                if (ns.CanRead)
                {
                    Debug.Log("开始同步接收远程服务器数据");
                    bytesRead = ns.Read(bytes, 0, bytes.Length);
                    client.Close();
                    ns.Close();
                    MyConnectCallback(connectResult);
                    break;
                }
                else
                {
                    client.Close();
                    ns.Close();
                    Debug.Log("网络错误！");
                    MyConnectCallback(connectResult);
                    break;
                }

                //// 异步读取
                //try
                //{

                //    byte[] result = new byte[client.Available];
                //    ns.BeginRead(result, 0, result.Length, new AsyncCallback(MyReadCallback), ns);
                //    Debug.Log("开始异步接收网络数据："+ns);
                //    strResponse = Encoding.ASCII.GetString(result).Trim();
                //    Debug.Log("得到异步接收的网络数据 strResponse ntp time:" +strResponse);
                //    str2 = strResponse;

                //    client.Close();
                //    break;
                //}
                //catch 
                //{ 
                //    Debug.Log("网络错误！");

                //}

            }
            else
            {

                Debug.Log("网络未连接");
                client.Close();
                MyConnectCallback(connectResult);
                yield break;
            }
            // client.EndConnect(connectResult);
            //}
            //catch (System.Exception)
            //{
            //Debug.Log("获取错误！");
            //}
        }
        str2 = System.Text.Encoding.ASCII.GetString(bytes, 0, bytesRead);
        Debug.Log("得到 ntp time:" + str2);
        serverTime = str2;
        //return serverTime;
    }

    // 转换得到网络标准时间
    //public static DateTime DataStandardTime()//使用时，将static 关键字删除，在其它位置方可使用
    public DateTime DataStandardTime()
    {

        if (serverTime != null)
        {
            char[] sp = new char[1];
            sp[0] = ' ';
            System.DateTime dt = new DateTime();
            // string str1;
            // str2 = str2.Replace("PDT", "");

            string[] s;
            //s = str1.Split(sp);
            s = serverTime.Split(sp);
            // "58961 20-04-22 07:22:32 50 0 0 900.6 UTC(NIST) *"
            dt = System.DateTime.Parse(s[1] + " " + s[2]);//得到标准时间
            //Debug.WriteLine("get:" + dt.ToShortTimeString());
            Debug.Log("得到标准时间:" + dt.ToShortTimeString());
            //dt=dt.AddHours (8);
            dt = dt.ToLocalTime();
            Debug.Log("得到本地时间:" + dt);
            theNetTime = dt;
            return theNetTime;
        }
        else
        {
            dt = System.DateTime.Parse("20200000000000");
            Debug.Log(" 获取网络时间失败，已经获取到本机系统时间.");
            theNetTime = dt;
            return theNetTime;
        }


    }
    private static void MyReadCallback(IAsyncResult iar)
    {
        //
        NetworkStream ns = (NetworkStream)iar.AsyncState;
        byte[] read = new byte[1024];
        String data = "";
        int recv;

        recv = ns.EndRead(iar);
        data = String.Concat(data, Encoding.ASCII.GetString(read, 0, recv));

        //接收到的消息长度可能大于缓冲区总大小，反复循环直到读完为止
        while (ns.DataAvailable)
        {
            ns.BeginRead(read, 0, read.Length, new AsyncCallback(MyReadCallback), ns);
        }
        //打印
        Console.WriteLine("您收到的信息是" + data);
        Debug.Log("您收到的信息是" + data);

    }

    private void MyConnectCallback(IAsyncResult ar)
    {
        TcpClient t = (TcpClient)ar.AsyncState;
        try
        {
            if (t.Connected)
            {
                t.EndConnect(ar);//函数运行到这里就说明连接成功
            }
            else
            {
            }
        }
        catch 
        { }
    }


}


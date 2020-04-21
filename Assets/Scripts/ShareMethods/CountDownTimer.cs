using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;
using DemoProject;
using UnityEngine.UI;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.IO;
using System.Text;

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
        StartCoroutine( NetworkTimeControlIE());
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


    // 实现倒计时方法二：采用Update方法
    // Update is called once per frame
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

            int nH = (int)DataStandardTime().Hour;
            int nM = (int)DataStandardTime().Minute;
            int nS = (int)DataStandardTime().Second;

        // 判断网络连接状态
        string url = "www.baidu.com;www.sina.com;www.cnblogs.com;www.google.com;www.163.com;www.csdn.com";
        string[] urls = url.Split(new char[] { ';' });
        if (CheckServeStatus(urls) )
        {
            totaltime4 = 86400 - ((nH * 60 * 60) + (nM * 60) + nS);
        }
        else
        {
            totaltime4 =10;
        }


        while (totaltime4>=1)
        {
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
                totaltime4 = 86400-((nH * 60 * 60) + (nM*60) + nS);
            }

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
    /// <param name="urls"></param>
    //public static void CheckServeStatus(string[] urls)
    public bool CheckServeStatus(string[] urls)
    {

        int errCount = 0;//ping时连接失败个数

        //if (!LocalConnectionStatus())
        //{
        //    Console.WriteLine("网络异常~无连接");
        //}
        if (!MyPing(urls, out errCount))
        {
            if ((double)errCount / urls.Length >= 0.3)
            {
                Debug.Log("网络连接异常");
                return false;
                //Console.WriteLine("网络异常~连接多次无响应");
            }
            //else
            //{
             //   Console.WriteLine("网络不稳定");
            //}
            else
            {
                Debug.Log("网络连接正常");
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
        bool isconnected = true;
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
                    isconnected = false;
                    errorCount++;
                }
                Console.WriteLine("Ping " + urls[i] + "    " + pr.Status.ToString());
            }
        }
        catch
        {
            isconnected = false;
            errorCount = urls.Length;
        }
        //if (errorCount > 0 && errorCount < 3)
        //  isconn = true;
        return isconnected;
    }

    //public static DateTime DataStandardTime()//使用时，将static 关键字删除，在其它位置方可使用
    public DateTime DataStandardTime()//使用时，将static 关键字删除，在其它位置方可使用
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

        


        System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
        for (int i = 0; i < 13; i++)
        {
            hostName = TimerServer[ServerTab[i], 0];

            Debug.Log("hostName:" + hostName);
            try
            {
                // 同步连接服务器
                // client.Connect(hostName, portNum);

                // 异步连接服务器
                // client.BeginConnect(hostName, Convert.ToInt32(portNum), new AsyncCallback(ConnectCallback),client);
                var connectResult = client.BeginConnect(hostName, portNum, null, null);
                var connectOK = connectResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3));
                if (!connectOK)
                {
                    client.Close();
                    break;
                }
                else
                {
                    System.Net.Sockets.NetworkStream ns = client.GetStream();



                    // 同步读取
                    //if (ns.CanRead)
                    //{
                    //    bytesRead = ns.Read(bytes, 0, bytes.Length);
                    //    client.Close();
                    //    ns.Close();
                    //    break;
                    //}
                    //else
                    //{
                    //    client.Close();
                    //    ns.Close();
                    //    Debug.Log("网络错误！");
                    //    break;
                    //}

                    // 异步读取
                    byte[] result = new byte[client.Available];
                    try
                    {
                        ns.BeginRead(result, 0, result.Length, new AsyncCallback(ReadCallback), ns);
                    }
                    catch { }
                    string strRespnse = Encoding.ASCII.GetString(result).Trim();
                }

                // client.EndConnect(connectResult);
                ConnectCallback(connectResult);
            }
            catch (System.Exception)
            {
                Debug.Log("获取错误！");
            }
        }



    }

    private void ReadCallback(IAsyncResult ar)
    {
        //

        char[] sp = new char[1];
        sp[0] = ' ';
        System.DateTime dt = new DateTime();
        //string str1;
        string str2;
        str2 = System.Text.Encoding.ASCII.GetString(bytes, 0, bytesRead);
        //str2 = str1.Replace("PDT", "");
        Debug.Log("得到 ntp time:" + str2);

        string[] s;
        //s = str1.Split(sp);
        s = str2.Split(sp);
        Debug.Log(s);
        dt = System.DateTime.Parse(s[1] + " " + s[2]);//得到标准时间
        //Debug.WriteLine("get:" + dt.ToShortTimeString());
        Debug.Log("得到标准时间:" + dt.ToShortTimeString());
        //dt=dt.AddHours (8);
        dt = dt.ToLocalTime();
        Debug.Log("得到本地时间:" + dt);
        return dt;
    }

    private void ConnectCallback(IAsyncResult ar)
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


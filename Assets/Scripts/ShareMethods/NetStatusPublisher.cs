using DemoProject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetStatusPublisher : MonoBehaviour
{
    // 网络状态发布器
    public bool lastNetStatus = false;
    public bool nowNetStatus = false;
    public string netDownToUp;
    public string netUpToDown;
    private string[] serverUrls;

    // 声明委托类
    public delegate void NetChangedEventHandler(System.Object sender, NetChangedEventArgs e);
    // 声明事件成员
    public event NetChangedEventHandler NetChanged;

    // 发布事件

    // 声明NetChangedEventArgs参数类，传递给Observer订阅者所感兴趣的信息
    public class NetChangedEventArgs : EventArgs
    {
        public readonly string netDownToUp;
        public readonly string netUpToDown;
        public NetChangedEventArgs(string netDownToUp)
        {
            this.netDownToUp = netDownToUp;
        }
    }

    protected virtual void OnNetChanged(NetChangedEventArgs e)
    {
        if (NetChanged != null)  // 如果有对象注册
        {
            NetChanged(this, e);  // 发布事件消息，通知订阅者对接收到的消息做后续方法的操作
        }
    }


    // 启动协程轮询网络状态
    StartCoroutine(CheckNet());

    IEnumerator CheckNet()
    {
        while(true)
        {
            yield return CountDownTimer.CheckNetStatus(serverUrls);
            bool isConnectedNow = CountDownTimer.CheckNetStatus(serverUrls);

            // 1.离线变为在线
            if (isConnectedNow == true && (lastNetStatus == false))
            {
                OnNetChanged(status1);
            }

            // 2.在线变为离线
            else if (isConnectedNow == false && (lastNetStatus == true))
            {

                OnNetChanged(status2);
            }
            // 3.一直离线或一直在线则不做处理，不发消息

            // 等待2秒后进行下一次问询
           yield return new WaitForSeconds(2.0f);

        }
    }


}

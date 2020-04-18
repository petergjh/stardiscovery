using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;
using DemoProject;
using UnityEngine.UI;


public class CountDownTimer : MonoBehaviour
{
    private int totaltime1 = 5;
    private int totaltime2 = 6;
    private float intervaletime=1;
    public Text countdown1text;
    public Text countdown2text;

    void Start()
    {
        countdown1text.text = string.Format("{0:00}:{1:00}", (int)totaltime1 / 60, (float)totaltime1 % 60);
        countdown2text.text = string.Format("{0:00}:{1:00}", (int)totaltime2 / 60, (float)totaltime2 % 60);
        StartCoroutine(CountDown());
    }

    // 方法一：用IEnumerator协程迭代器实现倒计时器
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
                totaltime1 = 5;
            }
        }
    }


    // 方法二：采用Update方法实现倒计时
    // Update is called once per frame
    void Update()
    {
        
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
                totaltime2 = 6;
            }
        
    }

}


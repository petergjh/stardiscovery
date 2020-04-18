using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;
using DemoProject;
using UnityEngine.UI;


public class CountDownTimer : MonoBehaviour
{
    public GameObject text;
    public int TotalTime = 5;

    void Start()
    {

        StartCoroutine(CountDown());

    }

    IEnumerator CountDown()
    {


        while (TotalTime >=0)
        {
            int M = (int)(TotalTime / 60);
            float S = TotalTime % 60;

            //text.GetComponent<Text>().text = TotalTime.ToString();
            text.GetComponent<Text>().text = M + ":" + string.Format("{0:00}", S);
            yield return new WaitForSeconds(1);
            TotalTime--;

            if (TotalTime < 0)
            {
                TotalTime = 6;
            }
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;
using DemoProject;
using UnityEngine.UI;


    public class CountDownTimer : MonoBehaviour
    {
        public GameObject text;
        public int TotalTime = 60;

        void Start()
        {

            StartCoroutine(CountDown());

        }

        IEnumerator CountDown()
        {
            while (TotalTime >= 0)
            {
                text.GetComponent<Text>().text = TotalTime.ToString();
                yield return new WaitForSeconds(1);
                TotalTime--;

            }
        }

    }


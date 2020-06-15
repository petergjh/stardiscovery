using System.Collections;
using System.Collections.Generic;
using UIFrame;
using UnityEngine;
using UnityEngine.UI;

namespace DemoProject
{

    public class LoadingSliderUIForm : BaseUIForm
    {
        private Slider slider;//Slider 对象
        private Text text;//Text 对象
        private void Awake()
        {
            Debug.Log("窗体LoadingSliderUIForm开始初始化");
            // 窗体性质
            CurrentUIType.UIForms_Type = UIFormType.PopUP;  // 窗体位置类型：弹出窗体
            CurrentUIType.UIForms_LucencyType = UIFormLucencyType.Lucency;  // 透明度类型：半透明度 
            CurrentUIType.UIForms_ShowMode = UIFormShowMode.ReverseChange; // 窗体显示类型：反向切换
            Debug.Log("已设置窗体性质。");

            // 注册按钮事件
        }



        /// <summary>
        /// 初始化组件
        /// </summary>
        void Start()
        {
            // 获取本对象的slider组件
            slider = GameObject.Find("Slider").GetComponent<Slider>();
            // 获取slider组件的text组件
            text = slider.transform.Find("Text").GetComponent<Text>();
            // 设置slider组件的默认value值为0
            slider.value = 0;
            // 设置text组件的默认text文本值为空字符串
            text.text = " ";
        }

        void Update()
        {
            if (slider.value < 3)
            {
                // 在slider组件上设置MaxValue为3，value值按秒自增
                slider.value += Time.deltaTime;
                // 同时文本根据value值的自增而同时显示百分比
                text.text = (slider.value * 33).ToString("F") + "%";
            }
            else if(slider.value==3)
            {
                // 当slider的value值达到预设定的最大值3，则还原回默认值（方便窗体返回时重新调用）
                slider.value = 0;
                // 关闭本窗体
                CloseUIForm();
            }
        }
    }



}



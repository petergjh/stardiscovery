using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;
using DG.Tweening;


namespace DemoProject
{
    //纸张正反面
    public enum PageState
    {
        Front,
        Back
    }

    public class DrawBoardUIForm : BaseUIForm
    {

        public GameObject mFront ;//纸张正面
        public GameObject mBack;//纸张背面
        public PageState mPageState = PageState.Front;//纸张当前的状态，是正面还是背面？
        private float mTime =0.5f;
        private bool isActive = false;//true代表正在执行翻转，不许被打断

        #region 事件注册
        private void Awake()
        {
            Debug.Log("公告栏窗体DrawBoardUIForm开始初始化");
            // 窗体性质
            CurrentUIType.UIForms_Type = UIFormType.PopUP;  // 窗体位置类型：弹出窗体
            CurrentUIType.UIForms_LucencyType = UIFormLucencyType.Lucency;  // 透明度类型：半透明度 
            CurrentUIType.UIForms_ShowMode = UIFormShowMode.ReverseChange; // 窗体显示类型：反向切换
            Debug.Log("已设置窗体性质。");

            // 注册按钮事件
            RigisterButtonObjectEvent("Btn_Close",
                p=> CloseUIForm()
                );

            // 注册按钮事件
            RigisterButtonObjectEvent("Btn_zhi1",
                p =>StartBack()
                );

            // 注册按钮事件
            RigisterButtonObjectEvent("Btn_zhi2",
                p =>StartFront()
                );

            //// 注册道具事件：盔甲
            //RigisterButtonObjectEvent("BtnCloth",
            //     p =>
            //    {
            //        // 打开子窗体
            //        OpenUIForm("PropDetailUIForm");
            //        // 传值
            //        // KeyValueUpdate kvs = new KeyValueUpdate("cloth", "盔甲道具详情:");
            //        // MessageCenter.SendMessage("Props", kvs);
            //        // 可以发送多种类型数据
            //        string[] strArray = new string[] { "盔甲详情","盔甲详细介绍。。。。" };
            //        SendMessage("Props", "cloth", strArray);
            //    }
            //    );





        }
        #endregion 事件注册结束

        /// <summary>
        /// 初始化纸张角度，根据mPageState
        /// </summary>
        public void Init()
        {
            //查找子节点
            mFront = UnityHelper.FindTheChildNode(this.gameObject, "Img_zhi1").gameObject;
            Debug.Log("查找到子节点mFront" + mFront.name);
            mBack = UnityHelper.FindTheChildNode(this.gameObject, "Img_zhi2").gameObject;
            Debug.Log("查找到子节点mBack" +mBack.name);
            mFront.transform.SetAsLastSibling();
            Debug.Log("初始化窗体时把第一页放到最前面");
            //if (mPageState == PageState.Front)
            //{
            //    //如果是从正面开始，则将背面旋转90度，这样就看不见背面了
            //    mBack.transform.eulerAngles = new Vector3(0, 90, 0);
            //    mFront.transform.eulerAngles = Vector3.zero;
            //}
            //else
            //{
            //    //从背面开始，同理
            //    mFront.transform.eulerAngles = new Vector3(0, 90, 0);
            //    mBack.transform.eulerAngles = Vector3.zero;
            //}
        }
        private void Start()
        {
            Init();
        }

        /// <summary>
        /// 留给外界调用的接口
        /// </summary>
        public void StartBack()
        {
            if (isActive)
                return;
            StartCoroutine(ToBack());
        }
        /// <summary>
        /// 留给外界调用的接口
        /// </summary>
        public void StartFront()
        {
            if (isActive)
                return;
            StartCoroutine(ToFront());
        }

        /// <summary>
        /// 切换到后一页
        /// </summary>
        IEnumerator ToBack()
        {
            Debug.Log("翻到后页协程");
            isActive = true;
            //第一页在mTime时间内右移到坐标
            mFront.transform.DOBlendableLocalMoveBy(new Vector3(300, -20, 0), mTime);
            //第二页在2倍mTime时间内弹向坐标并返回
            mBack.transform.DOPunchPosition(new Vector3(-80, 10, 0), mTime * 2, 0, 0);
            //协程挂起等待0.3秒后继续
            yield return new WaitForSeconds(0.3f);
            //把第二页移到所有transform组件列表的最后边（UI节点的的最下边即UI层次的最前面）
            mBack.transform.SetAsLastSibling();
            //角度变化
            //mBack.transform.DOBlendablePunchRotation(new Vector3(10, 10, 0),1f, 1, 0);
            //把最开始第一页再移回原点，注意是local参照坐标
            mFront.transform.DOLocalMove(new Vector3(0, 0, 0), mTime);
            isActive = false;

        }
        /// <summary>
        /// 切换到首页
        /// </summary>
        IEnumerator ToFront()
        {
            isActive = true;
            mBack.transform.DOLocalMove(new Vector3(300, -20, 0), mTime);
            yield return new WaitForSeconds(0.5f);
            mFront.transform.DOPunchPosition(new Vector3(-80, 10, 0), mTime, 1,0);
            mFront.transform.SetAsLastSibling();
            mBack.transform.DOLocalMove(new Vector3(0, 0, 0), mTime);
            isActive = false;

        }

        ///// <summary>
        ///// 翻转到背面
        ///// </summary>
        //IEnumerator ToBack()
        //{
        //    Debug.Log("翻到后页协程");
        //    isActive = true;
        //    mFront.transform.DORotate(new Vector3(0, 90, 0), mTime);
        //    for (float i = mTime; i >= 0; i -= Time.deltaTime)
        //        yield return 0;
        //    mBack.transform.DORotate(new Vector3(0, 0, 0), mTime);
        //    isActive = false;

        //}
        /// <summary>
        /// 翻转到正面
        /// </summary>
        //IEnumerator ToFront()
        //{
        //    isActive = true;
        //    mBack.transform.DORotate(new Vector3(0, 90, 0), mTime);
        //    for (float i = mTime; i >= 0; i -= Time.deltaTime)
        //        yield return 0;
        //    mFront.transform.DORotate(new Vector3(0, 0, 0), mTime);
        //    isActive = false;
        //}

    }

}

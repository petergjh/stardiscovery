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


            // 注册道具事件：神杖
            RigisterButtonObjectEvent("BtnTicket",
                p =>
                {
                    // 打开子窗体
                    // OpenUIForm("PropDetailUIForm");
                    OpenUIForm(ProjectConst.PROP_DETAIL_UIFORM );
                    // 传值
                    // 可以发送多种类型数据
                    // KeyValueUpdate kvs = new KeyValueUpdate("ticket", "神杖道具详情:");
                    string[] strArray = new string[] {"神杖详情", "神杖道具详情介绍。。。"};
                    KeyValueUpdate kvs = new KeyValueUpdate("ticket",strArray);
                    MessageCenter.SendMessage("Props", kvs);
                }
                );

            // 注册道具事件：战靴
            RigisterButtonObjectEvent("BtnShoe",
                 p =>
                {
                    // 打开子窗体
                    OpenUIForm("PropDetailUIForm");
                    // 传值
                    // KeyValueUpdate kvs = new KeyValueUpdate("shoes", "战靴道具详情:");
                    // MessageCenter.SendMessage("Props", kvs);
                    // 发送方法进行了封装重构
                    // 可以发送多种类型数据
                    string[] strArray = new string[] {"战靴详情", "战靴详情介绍。。。"};
                    SendMessage("Props","shoes",strArray);
                }

                );

            // 注册道具事件：盔甲
            RigisterButtonObjectEvent("BtnCloth",
                 p =>
                {
                    // 打开子窗体
                    OpenUIForm("PropDetailUIForm");
                    // 传值
                    // KeyValueUpdate kvs = new KeyValueUpdate("cloth", "盔甲道具详情:");
                    // MessageCenter.SendMessage("Props", kvs);
                    // 可以发送多种类型数据
                    string[] strArray = new string[] { "盔甲详情","盔甲详细介绍。。。。" };
                    SendMessage("Props", "cloth", strArray);
                }
                );


        }
        #endregion 事件注册结束

        /// <summary>
        /// 初始化纸张角度，根据mPageState
        /// </summary>
        public void Init()
        {
            mFront = UnityHelper.FindTheChildNode(this.gameObject, "Btn_zhi1").gameObject;
            mBack = UnityHelper.FindTheChildNode(this.gameObject, "Btn_zhi2").gameObject;
            if (mPageState == PageState.Front)
            {
                //如果是从正面开始，则将背面旋转90度，这样就看不见背面了
                mBack.transform.eulerAngles = new Vector3(0, 90, 0);
                mFront.transform.eulerAngles = Vector3.zero;
            }
            else
            {
                //从背面开始，同理
                mFront.transform.eulerAngles = new Vector3(0, 90, 0);
                mBack.transform.eulerAngles = Vector3.zero;
            }
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
            mFront.transform.DOBlendableLocalMoveBy(new Vector3(400, -20, 0), mTime);
            mBack.transform.DOPunchPosition(new Vector3(-80, 10, 0), mTime * 2, 0, 0);
            yield return new WaitForSeconds(0.3f);
            mBack.transform.SetAsLastSibling();
            //mBack.transform.DOBlendablePunchRotation(new Vector3(10, 10, 0),1f, 1, 0);
            mFront.transform.DOLocalMove(new Vector3(0, 0, 0), mTime);
            isActive = false;

        }
        /// <summary>
        /// 切换到首页
        /// </summary>
        IEnumerator ToFront()
        {
            isActive = true;
            mBack.transform.DOMove(new Vector3(30, -8, 0), mTime);
            yield return new WaitForSeconds(0.5f);
            mFront.transform.DOPunchPosition(new Vector3(-20, 10, 0), mTime, 1,0);
            mFront.transform.SetAsLastSibling();
            mBack.transform.DOMove(new Vector3(0, 0, 0), mTime);
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

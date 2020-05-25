using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using Steamworks;

class AchievementManager : MonoBehaviour
{
    //成就
    private enum Achievement : int
    {
        ACH_WIN_ONE_GAME,
        //有待添加
    };



    private Achievment_t[] m_Achievements = new Achievment_t[]
    {
  new Achievment_t(Achievement.ACH_WIN_ONE_GAME, "第一个成就","启动游戏立即获得"),
    };

    private class Achievment_t
    {
        public Achievement m_eAchievementID;
        public string m_strName;
        public string m_strDescription;
        public bool m_bAchieved;

        public Achievment_t(Achievement achivementID, string name, string desc)
        {
            m_eAchievementID = achivementID;
            m_strName = name;
            m_strDescription = desc;
            m_bAchieved = false;
        }
    }

    //游戏状态
    private enum GameState
    {
        GAME_ACTIVE,
        //有待添加
    };

    //GameID
    private CGameID m_GameID;

    //从Steam获取统计数据?
    private bool m_bRequestedStats;
    private bool m_bStatsValid;

    //这一帧存储统计数据?
    private bool m_bStoresStats;

    //游戏数据
    private int m_nTotalNumWins;

    protected Callback<UserStatsReceived_t> m_UserStatsReceived;
    protected Callback<UserStatsStored_t> m_UserStatsStored;
    protected Callback<UserAchievementStored_t> m_UserAchievementStored;

    //把回调创建放在OnEnable方法内以确保Unity加载完成后重复创建回调
    private void OnEnable()
    {
        if (!SteamManager.Initialized) return;

        m_GameID = new CGameID(SteamUtils.GetAppID());

        m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
        m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

        m_bRequestedStats = false;
        m_bStatsValid = false;
    }

    private void Start()
    {
        m_nTotalNumWins = 0;
        OnGameStateChange(GameState.GAME_ACTIVE);
    }

    private void Update()
    {
        if (!SteamManager.Initialized) return;

        if (!m_bRequestedStats)
        {
            if (!SteamManager.Initialized)
            {
                m_bRequestedStats = true;
                return;
            }
            //从 Steam 后端调出用户的统计与成就数据
            bool bSuccess = SteamUserStats.RequestCurrentStats();
            m_bRequestedStats = bSuccess;
        }
        //if (!m_bStatsValid) return;

        foreach (Achievment_t achievement in m_Achievements)
        {
            if (achievement.m_bAchieved) continue;
            switch (achievement.m_eAchievementID)
            {
                case Achievement.ACH_WIN_ONE_GAME:
                    if (m_nTotalNumWins != 0)
                    { UnlockAchievement(achievement); }
                    break;
            }
        }
        //在游戏中的某个节点上传变更
        if (m_bStoresStats)
        {
            SteamUserStats.SetStat("NumWins", m_nTotalNumWins);
            bool bSuccess = SteamUserStats.StoreStats();
            m_bStoresStats = !bSuccess;
        }

    }

    // 数据准备完成后（RequestCurrentStats），回调
    private void OnUserStatsReceived(UserStatsReceived_t pCallback)
    {
        if (!SteamManager.Initialized) return;
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            //加载成就
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("Received stats and achievements from Steam");
                m_bStatsValid = true;

                foreach (Achievment_t ach in m_Achievements)
                {
                    //迭代每项数据并初始化游戏状态
                    bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);

                    if (ret)
                    {
                        //在游戏里显示成就的可读属性（name&des）
                        ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
                        ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
                    }
                    else
                    {
                        Debug.LogWarning("failed for getting achievement" + ach.m_eAchievementID);
                    }
                }

                SteamUserStats.GetStat("NumWins", out m_nTotalNumWins);
            }
            else
                Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
        }
    }


    //回调上传的变更（StoreStats）
    private void OnUserStatsStored(UserStatsStored_t pCallback)
    {
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("StroeStats-success");
            }
            else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
            {
                Debug.Log("StoreStats-failed to validate");
                UserStatsReceived_t callback = new UserStatsReceived_t();
                callback.m_eResult = EResult.k_EResultOK;
                callback.m_nGameID = (ulong)m_GameID;
                OnUserStatsReceived(callback);
            }
            else
            {
                Debug.Log("failed-StroeStats");
            }
        }
    }


    //解锁成就
    private void UnlockAchievement(Achievment_t achievement)
    {
        achievement.m_bAchieved = true;
        SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());
        m_bStoresStats = true;
    }


    //回调解锁的成就
    private void OnAchievementStored(UserAchievementStored_t pCallback)
    {
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (0 == pCallback.m_nMaxProgress)
            {
                Debug.Log("Achievement" + pCallback.m_rgchAchievementName + "unlocked");
            }
            else
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
            }
        }
    }


    //切换游戏状态
    private void OnGameStateChange(GameState eNewState)
    {
        //if (!m_bStatsValid) return;
        if (eNewState == GameState.GAME_ACTIVE)
        {
            m_nTotalNumWins++;
        }
        m_bStoresStats = true;
    }


}

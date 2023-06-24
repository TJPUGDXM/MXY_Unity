using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr 
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    //记录选择的角色数据 用于游戏场景
    public RoleInfo nowSelRole;

    public PlayerData playerData;
    public MusicData musicData;

    //全部的角色信息
    public List<RoleInfo> roleInfoList;

    //全部场景数据
    public List<SceneInfo> sceneInfList;

    //全部怪物数据
    public List<MonsterInfo> monsterInfoList;

    //全部塔数据
    public List<TowerInfo> towerInfoList;
    GameDataMgr()
    {
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        sceneInfList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
    }

    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }

    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }


    /// <summary>
    /// 播放音效方法
    /// </summary>
    /// <param name="resName"></param>
    public void PlaySound(string resName)
    {
        GameObject musicObj = new GameObject();
        AudioSource a = musicObj.AddComponent<AudioSource>();
        a.clip = Resources.Load<AudioClip>(resName);
        a.volume = musicData.soundValue;
        a.mute =! musicData.openSound;
        a.Play();

        GameObject.Destroy(musicObj, 1);
    }
}

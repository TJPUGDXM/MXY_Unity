using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    //怪物有多少波
    public int maxWave;

    //每波怪物有多少只
    public int monsterNumOneWave;
    //当前波次还有多少怪物没创建
    private int nowNum;

    //怪物ID
    public List<int> monsterIDs;
    private int nowID;
    //单只怪物的间隔时间
    public float createOffsetTime;

    //波次的间隔时间
    public float delayTime;

    //第一波怪物的间隔时间
    public float firstDelayTime;



    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateWave", firstDelayTime);
        GameLevelMgr.Instance.addMonsterPoint(this);
        GameLevelMgr.Instance.UpdateMaxNum(maxWave);
    }

    

    /// <summary>
    /// 创建第一波怪物
    /// </summary>
    private void CreateWave()
    {
        //得到当前波次怪物的id
        nowID = monsterIDs[Random.Range(0, monsterIDs.Count)];
        //当前怪物还有多少只
        nowNum = monsterNumOneWave;
        //创建怪物
        CreateMonster();
        //减少波数
        maxWave--;
        GameLevelMgr.Instance.ChangeNowWaveNum(1);
    }


    private void CreateMonster()
    {
        //创建怪物;
        //取出怪物数据
        MonsterInfo info = GameDataMgr.Instance.monsterInfoList[nowID - 1];

        //创建怪物预设体
        GameObject obj = Instantiate(Resources.Load<GameObject>(info.res), transform.position, Quaternion.identity);
        //添加怪物脚本
        MonsterObject monsterObject = obj.AddComponent<MonsterObject>();
        monsterObject.InitInfo(info);

        GameLevelMgr.Instance.AddMonster(monsterObject);

        //怪物数量减少
        nowNum--;
        if (nowNum==0)
        {
            if (maxWave>0)
            {
                Invoke("CreateWave", delayTime);
            }
        }
        else
        {
            Invoke("CreateMonster", createOffsetTime);
        }
    }

    /// <summary>
    /// 出怪点是否出怪结束
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        return nowNum == 0 && maxWave == 0;
    }
}

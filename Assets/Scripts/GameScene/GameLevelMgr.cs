using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelMgr 
{
    private static GameLevelMgr instance = new GameLevelMgr();

    public static GameLevelMgr Instance => instance;

    public PlayerObject player;
    //记录所有出怪点
    public List<MonsterPoint> points = new List<MonsterPoint>();
    //记录一共有多少波怪物
    private int maxWaveNum = 0;
    //当前还有多少波怪物
    private int nowWaveNum = 0;

    //记录当前场景上的怪物数量
    //private int nowMonsterNum = 0;

    //记录当前场景上 怪物的列表
    private List<MonsterObject> monsterList = new List<MonsterObject>();
    private GameLevelMgr()
    {

    }


    /// <summary>
    /// 切换游戏场景时 动态创建玩家
    /// </summary>
    public void InitInfo(SceneInfo sceneInfo)
    {
        //显示游戏界面
        UIManager.Instance.ShowPanel<GamePanel>();
        //创建玩家
        //获取之前选择的玩家数据
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRole;
        //获取场景中的玩家出生位置
        Transform heroPos = GameObject.Find("HeroPoint").transform;
        //实例化玩家预设体
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res),heroPos.position,heroPos.rotation);
        //对玩家对象进行初始化
        player = heroObj.GetComponent<PlayerObject>();
        player.InitPlayerInfo(roleInfo.atk, sceneInfo.money);
        //让摄像机看向动态创建出的玩家
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);

        //初始化 中央 保护区域的血量
        MainTowerObject.Instance.UpdateHp(sceneInfo.towerHp,sceneInfo.towerHp);

    }

    /// <summary>
    /// 将出怪点全部加的集合中
    /// </summary>
    /// <param name="point"></param>
    public void addMonsterPoint(MonsterPoint point)
    {
        points.Add(point);
    }


    public void UpdateMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
        //更新界面
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }

    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum -= num;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }

    /// <summary>
    /// 检测是否胜利
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (!points[i].CheckOver())
            {
                return false;
            }
        }
        if (monsterList.Count>0)
        {
            return false;
        }
        Debug.Log("胜利");
        return true;
    }

    /// <summary>
    /// 改变当前场景上怪物的数量
    /// </summary>
    /// <param name="num"></param>
    //public void ChangeMonsterNum(int num)
    //{
    //    nowMonsterNum += num;
    //}

    ///记录怪物到列表
    public void AddMonster(MonsterObject obj)
    {
        monsterList.Add(obj);
    }
    /// <summary>
    /// 移除怪物 死亡时调用
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveMonster(MonsterObject obj)
    {
        monsterList.Remove(obj);
    }

    /// <summary>
    /// 寻找满足攻击距离的单个怪物
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public MonsterObject FindMonster(Vector3 pos, int range)
    {
        //在怪物列表中找到在攻击范围内的怪物
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (!monsterList[i].isDead&& Vector3.Distance(pos,monsterList[i].transform.position)<=range)
            {
                return monsterList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 寻找所有满足攻击距离所有怪物
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public List<MonsterObject> FindeMonsters(Vector3 pos, int range)
    {
        //寻找满足攻击距离的所有怪物
        List<MonsterObject> list = new List<MonsterObject>();
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                list.Add(monsterList[i]);
            }
        }

        return list;
    }

    /// <summary>
    /// 游戏结束后复位方法
    /// </summary>
    public void ClearInfo()
    {
        points.Clear();
        monsterList.Clear();
          nowWaveNum = maxWaveNum = 0;
    }
}

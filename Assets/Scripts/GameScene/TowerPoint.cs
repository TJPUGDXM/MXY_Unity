using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    //造塔点关联的塔对象
    private GameObject towerObj = null;
    //造塔点关联的 塔的数据
    public TowerInfo nowTowerInfo = null;
    //存储造塔点可造塔的id
    public List<int> chooseIDs;

    public void CreateTower(int id)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        //判断钱是否够
        if (info.money>GameLevelMgr.Instance.player.money)
        {
            return;
        }

        //扣钱
        GameLevelMgr.Instance.player.AddMoney(-info.money);
        //创建塔
        if (towerObj!=null)
        {
            Destroy(towerObj);
            towerObj = null;
        }
        //实例化塔对象
        towerObj = Instantiate(Resources.Load<GameObject>(info.res), transform.position, Quaternion.identity);
        //初始化塔
        towerObj.GetComponent<TowerObject>().InitInfo(info);

        //记录当前塔的数据
        nowTowerInfo = info;

        //塔建造完毕 跟新游戏界面
        if (nowTowerInfo.nextLev!=0)
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        else
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //如果有塔 不用在现实造塔界面 ，如果塔是最高级也不要升级
        if (nowTowerInfo!=null&&nowTowerInfo.nextLev==0)
        {
            return;
        }
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
    }

    private void OnTriggerExit(Collider other)
    {
        //隐藏造塔界面 传null即可
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }
}

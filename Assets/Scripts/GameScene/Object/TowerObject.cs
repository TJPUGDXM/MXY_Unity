using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    //炮台头部
    public Transform head;
    //开火点
    public Transform gunPoint;
    //炮台头部旋转速度
    private float roundSpeed = 25;
    //炮台关联的数据
    private TowerInfo info;
    //攻击目标
    private MonsterObject targetObj;
    //当前要攻击的目标们
    private List<MonsterObject> targetObjs;

    //用于计时
    private float nowTime;

    //记录怪物位置
    private Vector3 monsterPos;

    public void InitInfo(TowerInfo info)
    {
        this.info = info;
        Debug.Log("塔攻击力：" + this.info.atk);
        Debug.Log("塔攻击范围" + this.info.atkRange);
    }


    // Update is called once per frame
    void Update()
    {
        //单体攻击逻辑
        if (info.atkType == 1)
        {
            //没有目标或者目标死亡或者目标超出功能攻击距离
            if (targetObj == null||targetObj.isDead||Vector3.Distance(transform.position,targetObj.transform.position)>info.atkRange)
            {
                //Debug.Log("他攻击");
                targetObj = GameLevelMgr.Instance.FindMonster(transform.position, info.atkRange);
                Debug.Log(targetObj);
            }
            if (targetObj==null)
            {
                return;
            }
            monsterPos = targetObj.transform.position;
            monsterPos.y = head.position.y;
            //旋转炮台头部
            head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(monsterPos - head.position),roundSpeed*Time.deltaTime);

            //判断炮台头部和monster之间的夹角 是否小于一定范围
            if (Vector3.Angle(head.forward,monsterPos-head.position)<5&&Time.time-nowTime>=info.offsetTime)
            {
                //目标受伤
                
                targetObj.Wound(info.atk);
                //播放音效
                GameDataMgr.Instance.PlaySound("Music/Tower");
                //创建特效
                GameObject effobj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);
                //延迟移除特效
                Destroy(effobj, 1);
                nowTime = Time.time;
            }
        }
        //群体攻击逻辑
        else
        {
            targetObjs = GameLevelMgr.Instance.FindeMonsters(transform.position, info.atkRange);
            if (targetObjs.Count>0&&Time.time-nowTime>=info.offsetTime)
            {
                //创建开发特特效
                GameObject effobj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);
                //延迟删除
                Destroy(effobj, 0.2f);

                //目标受伤
                for (int i = 0; i < targetObjs.Count; i++)
                {
                    targetObjs[i].Wound(info.atk);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObject : MonoBehaviour
{
    //动画相关
    private Animator animator;
    //寻路组件
    private NavMeshAgent navMeshAgent;
    //怪物基础数据
    private MonsterInfo monsterInfo;

    //当前血量
    private int hp;
    public bool isDead = false;

    //上一次攻击的时间
    private float fronttime;
    
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void InitInfo(MonsterInfo info)
    {
        monsterInfo = info;
        //加载状态机
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        //当前的血量
        hp = monsterInfo.hp;
        //初始化速度
        navMeshAgent.speed = monsterInfo.moveSpeed;
        //旋转速度
        navMeshAgent.angularSpeed = monsterInfo.roundSpeed;
    }

    public void Wound(int dmg)
    {
        if (isDead)
        {
            return;
        }
        //减少血量
        hp -= dmg;
        //播放受伤动画
        animator.SetTrigger("Wound");
        if (hp <= 0)
        {
            Dead();
        }
        else
        {
            GameDataMgr.Instance.PlaySound("Music/Wound");
        }
    }

    public void Dead()
    {
        isDead = true;
        //停止移动
        navMeshAgent.isStopped = true;
        //死亡动画
        animator.SetBool("Dead",true);
        //播放音效
        GameDataMgr.Instance.PlaySound("Music/dead");
        //给玩家加钱
        GameLevelMgr.Instance.player.AddMoney(20);
    }

    /// <summary>
    /// 死亡动画完毕后 调用的事件
    /// </summary>
    public void DeadEvent()
    {
        //死亡动画播放完毕后移除对象
        GameLevelMgr.Instance.RemoveMonster(this);
        Destroy(gameObject);
        //检测是否胜利
        if (GameLevelMgr.Instance.CheckOver())
        {
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo(GameLevelMgr.Instance.player.money, true);
        }
    }

    /// <summary>
    /// 出生结束后让怪物移动
    /// </summary>
    public void BornOver()
    {
        //设置目标点
        navMeshAgent.SetDestination(MainTowerObject.Instance.transform.position);
        animator.SetBool("Run", true);
    }


    // Update is called once per frame
    void Update()
    {
        //什么时候停下 ，并攻击
        if (isDead)
        {
            return;
        }
        //停止奔跑
        animator.SetBool("Run",navMeshAgent.velocity!=Vector3.zero );

        //是否攻击
        if (Vector3.Distance(transform.position,MainTowerObject.Instance.transform.position)<5&&Time.time-fronttime>monsterInfo.atkOffset)
        {
            Debug.Log("攻击");
            animator.SetTrigger("Atk");
            fronttime = Time.time;
        }
    }

    /// <summary>
    /// 伤害检测
    /// </summary>
    public void AtkEvent()
    {
        //范围检测
       Collider[] colliders= Physics.OverlapSphere(transform.position+transform.forward + transform.up, 1, 1 << LayerMask.NameToLayer("MainTower"));
        GameDataMgr.Instance.PlaySound("Music/Eat");
        for (int i = 0; i < colliders.Length; i++)
        {
            if (MainTowerObject.Instance.gameObject==colliders[i].gameObject)
            {
                MainTowerObject.Instance.Wound(monsterInfo.atk);
            }
        }
    }
}

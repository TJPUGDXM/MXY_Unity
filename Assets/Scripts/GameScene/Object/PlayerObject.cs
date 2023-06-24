using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    //1.玩家属性的初始化
    private Animator animator;
    //攻击力
    private int atk;
    //玩家的钱
    public int money;
    //旋转的速度
    private float roundSpeed = 50;
    //打击特效
    public string effStr;
    //开火点
    public Transform firePoint;
    //2.移动变化 动作变化
    //3.攻击动作的不同处理
    //4.血量更新和金钱更新

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    public void InitPlayerInfo(int atk,int money)
    {
        this.atk = atk;
        this.money = money;
        UpdateMoney();
    }

    // Update is called once per frame
    void Update()
    {
        //2.角色动画切换
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));

        //旋转
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * roundSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1, 1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Roll");
        }
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Fire");
        }
    }

    /// <summary>
    /// 专门处理刀攻击的伤害检测事件
    /// </summary>
    public void KnifeEvent()
    {
        //伤害检测
       Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward + transform.up, 1, 1 << LayerMask.NameToLayer("Monster"));
        //播放音效
        GameDataMgr.Instance.PlaySound("Music/Knife");

        for (int i = 0; i < colliders.Length; i++)
        {
            //得到所有怪物脚本，让怪物受伤
            MonsterObject monster= colliders[i].gameObject.GetComponent<MonsterObject>();
            if (monster!=null&&!monster.isDead)
            {
                monster.Wound(atk);
            }
        }
    }

    /// <summary>
    /// 枪的的动画事件
    /// </summary>
    public void ShootEvent()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(firePoint.position, transform.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));

        //播放音效
        GameDataMgr.Instance.PlaySound("Music/Gun");
        //获取怪物脚本
        for (int i = 0; i < hits.Length; i++)
        {
            MonsterObject monster = hits[i].collider.gameObject.GetComponent<MonsterObject>();
            if (monster != null&&!monster.isDead)
            {
                //打击特性
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));
                effObj.transform.position = hits[i].point;
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                Destroy(effObj, 1.2f);

                monster.Wound(atk);
                break;
            }
        }
    }

    //更新界面上的钱
    public void UpdateMoney()
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }

    //增加钱
    public void AddMoney(int money)
    {
        this.money = money;
        UpdateMoney();
    }

}

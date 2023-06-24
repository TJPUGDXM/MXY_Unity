using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerObject : MonoBehaviour
{
    //血量相关
    private int hp;
    private int maxHp;
    //是否死亡
    private bool isDead;

    private static MainTowerObject instance;

    public static MainTowerObject Instance => instance;
    private void Awake()
    {
        instance = this;
    }



    /// <summary>
    /// 更新血量
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="maxHP"></param>
    public void UpdateHp(int hp, int maxHP)
    {
        this.hp = hp;
        this.maxHp = maxHP;

        //更新界面血量显示
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerHp(hp, maxHP);
    }


    //受到伤害
    public void Wound(int dmg)
    {
        if (isDead)
        {
            return;
        }

        //受伤
        hp -= dmg;
        if (hp<=0)
        {
            isDead = true;
            hp = 0;
            //游戏结束
            GameOverPanel panel= UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f),false);
        }
        //更新血量
        UpdateHp(hp, maxHp);
    } 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        instance = null;   
    }
}

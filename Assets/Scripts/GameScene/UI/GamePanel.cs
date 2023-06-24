using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Image imgHP;
    public Text txtHP;

    public Text txtWave;
    public Text txtMoney;

    //HP的初始宽 可以 在外面去控制它 到底有多宽
    public float hpW = 500;

    public Button btnQuit;

    //下方造塔组件控件的父对象
    public Transform botTrans;

    public List<TowerBtn> towerBtns = new List<TowerBtn>();

    //当前选中的造塔点
    public TowerPoint nowSelTowerPointl;


    private bool checkInput;
    protected override void Init()
    {
        btnQuit.onClick.AddListener(() =>
        {
            //隐藏游戏面板
            UIManager.Instance.HidePanel<GamePanel>();
            //返回到开始界面
            SceneManager.LoadScene("BeginScene");
            //其他
        });

        //一开始隐藏下方和造塔相关的UI
        botTrans.gameObject.SetActive(false);
        //锁定鼠标
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// 更新安全区域血量
    /// </summary>
    /// <param name="hp">当前血量</param>
    /// <param name="maxHP">最大血量</param>
    public void UpdateTowerHp(int hp, int maxHP)
    {
        txtHP.text = hp + "/" + maxHP;
        //更新血条长度
        (imgHP.transform as RectTransform).sizeDelta = new Vector2((float)hp / maxHP * hpW, 26.30521f);
    }

    /// <summary>
    /// 更新波数函数
    /// </summary>
    /// <param name="nowNum">当前剩余波数</param>
    /// <param name="maxNum">最大波数 </param>
    public void UpdateWaveNum(int nowNum, int maxNum)
    {
        txtWave.text = nowNum + "/" + maxNum;
    }


    /// <summary>
    /// 更新金币
    /// </summary>
    /// <param name="money">当前关卡获得的金币</param>
    public void UpdateMoney(int money)
    {
        txtMoney.text = money.ToString();
    }

    /// <summary>
    /// 更新当前选中造塔点的变化
    /// </summary>
    public void UpdateSelTower(TowerPoint point)
    {
        
        //根据造塔点信息决定界面的显示内容
        nowSelTowerPointl = point;
        //如果传入的是null
        if (nowSelTowerPointl == null)
        {
            botTrans.gameObject.SetActive(false);
            checkInput = false;
        }
        else
        {
            botTrans.gameObject.SetActive(true);
            checkInput = true;
            if (nowSelTowerPointl.nowTowerInfo == null)
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(true);
                    towerBtns[i].InitInfo(nowSelTowerPointl.chooseIDs[i], "数字键" + (i + 1).ToString());
                }
            }
            else
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(false);
                }
                towerBtns[1].gameObject.SetActive(true);
                towerBtns[1].InitInfo(nowSelTowerPointl.nowTowerInfo.nextLev, "按空格升级");
            }
        }
        
    }

    /// <summary>
    /// 注意用于造塔点的键盘输入
    /// </summary>
    protected override void Update()
    {
        base.Update();
        if (!checkInput)
        {
            return;
        }
        if (nowSelTowerPointl.nowTowerInfo==null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPointl.CreateTower(nowSelTowerPointl.chooseIDs[0]);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPointl.CreateTower(nowSelTowerPointl.chooseIDs[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPointl.CreateTower(nowSelTowerPointl.chooseIDs[2]);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nowSelTowerPointl.CreateTower(nowSelTowerPointl.nowTowerInfo.nextLev);
            }
        }
    }
}

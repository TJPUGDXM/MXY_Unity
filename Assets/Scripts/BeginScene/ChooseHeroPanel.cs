using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    public Button btnLeft;
    public Button btnRight;

    //购买按钮
    public Button btnUnLock;
    public Text txtUnlock;

    //开始和返回
    public Button btnStart;
    public Button btnBack;

    //左上角金钱
    public Text txtMoney;

    //角色名
    public Text txtName;

    //角色预设体的位置
    private Transform heroPos;

    //当前场景中显示的对象
    private GameObject heroObj;
    //当前使用的数据
    private RoleInfo nowRoleData;
    //当前使用数据的索引
    private int nowIndex;
    protected override void Init()
    {
        heroPos = GameObject.Find("HeroPos").transform;

        //更新左上角的钱
        txtMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();

        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if (nowIndex<0)
            {
                nowIndex = GameDataMgr.Instance.roleInfoList.Count-1;
            }
            //模型更新
            ChangeHero();
        });

        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if (nowIndex>=GameDataMgr.Instance.roleInfoList.Count)
            {
                nowIndex = 0;
            }
            ChangeHero();
        });

        btnUnLock.onClick.AddListener(() =>
        {
            PlayerData data = GameDataMgr.Instance.playerData;
            //钱够
            if (data.haveMoney>=nowRoleData.lockMoney)
            {
                //减少钱
                data.haveMoney -= nowRoleData.lockMoney;
                //更新界面
                txtMoney.text = data.haveMoney.ToString();
                //添加id
                data.buyHero.Add(nowRoleData.id);
                //保存
                GameDataMgr.Instance.SavePlayerData();
                //更新解锁按钮
                UpdateLockBtn();
                //提示购买成功
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("购买成功");
            }
            //钱不够
            else
            {
                //提示钱不够
                Debug.Log("钱不够");
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("金钱不足");
            }
        });

        btnStart.onClick.AddListener(() =>
        {
            // 记录当前选择的角色
            GameDataMgr.Instance.nowSelRole = nowRoleData;
            //隐藏面板
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });

        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });

        ChangeHero();
    }

    /// <summary>
    /// 更新场景上的模型
    /// </summary>
    private void ChangeHero()
    {
        if (heroObj!=null)
        {
            Destroy(heroObj);
            heroObj = null;
        }
        nowRoleData = GameDataMgr.Instance.roleInfoList[nowIndex];
        //实例化对象 记录下来 便于删除
        heroObj =  Instantiate(Resources.Load<GameObject>(nowRoleData.res), heroPos.position, heroPos.rotation);
        txtName.text = nowRoleData.tips;
        Destroy(heroObj.GetComponent<PlayerObject>());
        Destroy(heroObj.GetComponent<CharacterController>());
        UpdateLockBtn();
    }


    private void UpdateLockBtn()
    {
        //如果该角色 需要解锁 并且没有解锁 的话  应该显示解锁按钮 ，并隐藏开始按钮
        if (nowRoleData.lockMoney>0&&!GameDataMgr.Instance.playerData.buyHero.Contains(nowRoleData.id))
        {
            //更新解锁按钮显示 并更新上面的钱
            btnUnLock.gameObject.SetActive(true);
            txtUnlock.text = "￥"+nowRoleData.lockMoney.ToString();
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            btnUnLock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }

    public override void HideMe(UnityAction action)
    {
        base.HideMe(action);
        if (heroObj!=null)
        {
            Destroy(heroObj);
            heroObj = null;
        }
    }
}

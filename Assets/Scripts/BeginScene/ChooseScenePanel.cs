using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    public Button btnLeft;
    public Button btnRight;
    public Button btnStart;
    public Button btnBack;

    public Text txtInfo;
    public Image imgScene;

    //当前索引值
    private int nowIndex;
    //当前场景信息
    private SceneInfo nowSceneInfo;
    protected override void Init()
    {
        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if (nowIndex<0)
            {
                nowIndex = GameDataMgr.Instance.sceneInfList.Count - 1;
            }
            ChangeScene();
        });

        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if (nowIndex>=GameDataMgr.Instance.sceneInfList.Count)
            {
                nowIndex = 0;
            }
            ChangeScene();
        });

        btnStart.onClick.AddListener(() =>
        {
            //隐藏当前面板
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            //切换场景
           AsyncOperation ao= SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);
            //关卡初始化
            ao.completed += (obj) =>
             {
                 GameLevelMgr.Instance.InitInfo(nowSceneInfo);
             };
            
        });

        btnBack.onClick.AddListener(() =>
        {
            //隐藏自己
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            //显示选角面板
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });
        ChangeScene();
    }

    /// <summary>
    /// 切换场景信息
    /// </summary>
    public void ChangeScene()
    {
        nowSceneInfo = GameDataMgr.Instance.sceneInfList[nowIndex];
        //更新图片
        imgScene.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);
        //更新文字
        txtInfo.text = "名称：\n" + nowSceneInfo.name + "\n" + "描述：\n" + nowSceneInfo.tips;

    }
}

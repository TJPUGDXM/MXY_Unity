using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button BtnQuit;

    protected override void Init()
    {
        btnStart.onClick.AddListener(() => 
        {
            UIManager.Instance.HidePanel<BeginPanel>();
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            {
                Debug.Log("选角");
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            });
        });

        btnSetting.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });

        BtnQuit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

}

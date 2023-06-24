using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    protected override void Init()
    {
        //初始化面板为保存的数据
        MusicData data = GameDataMgr.Instance.musicData;
        togMusic.isOn = data.openMusic;
        togSound.isOn = data.openSound;

        sliderMusic.value = data.musicValue;
        sliderSound.value = data.soundValue;
        

        btnClose.onClick.AddListener(() =>
        {
            //设置完成后关闭面板保存数据
            GameDataMgr.Instance.SaveMusicData();
            UIManager.Instance.HidePanel<SettingPanel>();
        });

        togMusic.onValueChanged.AddListener((b) =>
        {
            //修改音乐的开火
            BKMusic.Instance.SetOpenMusic(b);
            //修改音乐数据
            GameDataMgr.Instance.musicData.openMusic = b;
            GameDataMgr.Instance.SaveMusicData();
        });

        togSound.onValueChanged.AddListener((b)=>
        {
            GameDataMgr.Instance.musicData.openSound = b;
        });

        sliderMusic.onValueChanged.AddListener((v) =>
        {
            BKMusic.Instance.SetValueMusic(v);
            GameDataMgr.Instance.musicData.musicValue = v;
        });

        sliderSound.onValueChanged.AddListener((v) =>
        {
            GameDataMgr.Instance.musicData.soundValue = v;
        });
    }
}

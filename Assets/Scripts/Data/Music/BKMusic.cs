using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;

    public static BKMusic Instance => instance;

    private AudioSource audioSource;
    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        MusicData data = GameDataMgr.Instance.musicData;
        SetOpenMusic(data.openMusic);
        SetValueMusic(data.musicValue);
    }
    
    public void SetOpenMusic(bool isOpen)
    {
        audioSource.mute = !isOpen;
    }

    public void SetValueMusic(float v)
    {
        audioSource.volume = v;
    }
}

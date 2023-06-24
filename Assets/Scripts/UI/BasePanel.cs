using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{

    private CanvasGroup canvasGroup;
    private bool isShow;
    private float alphaSpeed = 10;
    private UnityAction hideAction=null;
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    // Start is called before the first frame update
    protected virtual void  Start()
    {
        Init();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isShow&&canvasGroup.alpha!=1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha>=1)
            {
                canvasGroup.alpha = 1;
            }
        }

        if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideAction?.Invoke();
            }
        }
    }

    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }

    public virtual void HideMe(UnityAction action)
    {
        canvasGroup.alpha = 1;
        isShow = false;
        hideAction = action;
    }

    protected virtual void Init()
    {

    }
}

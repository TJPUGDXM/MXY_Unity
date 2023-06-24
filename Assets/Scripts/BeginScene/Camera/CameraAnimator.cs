using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    private UnityAction unityAction;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TurnLeft(UnityAction action)
    {
        animator.SetTrigger("Left");
        unityAction = action;
    }

    public void TurnRight(UnityAction action)
    {
        animator.SetTrigger("Right");
        unityAction = action;
    }

    public void TurnOver()
    {
        unityAction?.Invoke();
    }
}

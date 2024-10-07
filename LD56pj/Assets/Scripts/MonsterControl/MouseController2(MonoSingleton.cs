using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using QFramework;
using UnityEngine;
using DG.Tweening;
using UnityEngine.PlayerLoop;

public class MouseController2 : MonoSingleton<MouseController2>
{
    
    // Start is called before the first frame update
    public enum MouseState2
    {
        Idle,Eat,Kill
    }
    [Header("此脚本需要一个Trigger子物体，用于检测mouse的攻击范围")]
    public MouseState2 curState;

    private Animator animator;
    private bool isEat;
    private Vector3 originPosition;
    private void Awake()
    {
        originPosition = transform.position;
        isEat = false;
        animator = gameObject.GetComponent<Animator>();
        curState = MouseState2.Idle;
        TypeEventSystem.Global.Register<OnLevelResetEvent>(e =>
        {
            Init();
        });
    }
    
    public void Init()
    {
        isEat = false;
        transform.position = originPosition;
        animator.CrossFade("Idle",0);
    }
    public void SwitchToKill()
    {
        if (!isEat)
        {
            StartCoroutine(KillAction());
        }
    }
    IEnumerator KillAction()
    {
        yield break;
    }

    public void SwitchToEat(Vector3 foodPositionWS)
    {
        isEat = true;
        transform.DOMove(foodPositionWS, 0.1f);
        animator.CrossFade("Eat",0);
    }
}

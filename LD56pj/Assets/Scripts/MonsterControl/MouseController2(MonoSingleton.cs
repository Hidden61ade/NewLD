using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using QFramework;
using UnityEngine;
using DG.Tweening;
using UnityEngine.PlayerLoop;
using Debug = UnityEngine.Debug;

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
    private Transform playerTransform;
    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        originPosition = transform.position;
        isEat = false;
        animator = gameObject.GetComponent<Animator>();
        curState = MouseState2.Idle;
        animator.CrossFade("Walk",0);
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
        animator.CrossFade("Kill",0);
        transform.DOMove(playerTransform.position, 0.4f);
        // 获取 Animator 的 RuntimeAnimatorController
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        // 遍历 AnimationClips
        float animationTime = 0.4f;
        foreach (AnimationClip clip in controller.animationClips)
        {
            if (clip.name == "Kill")
            {
                animationTime = clip.length;
            }
        }
        yield return new WaitForSeconds(animationTime);
        TypeEventSystem.Global.Send<OnPlayerDiedEvents>();
        yield break;
    }

    public void SwitchToEat(Vector3 foodPositionWS)
    {
        isEat = true;
        transform.DOMove(foodPositionWS, 0.1f);
        animator.CrossFade("PickCheese",0);
    }
}

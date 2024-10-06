using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public SpiderState curState;
    public float maxExposureTime;
    public float timer;
    private GameManager gameManager;

    // Capture parameters
    public float killTime = 0.3f;
    
    // Internal variables
    private Rigidbody2D rb;
    private Vector3 respawnPoint;
    private bool isCapturing = false;
    
    [Header("以下内容可自动获取引用")]
    public Transform playerTransform; // Reference to the player

    public Animator animator;
    // Start is called before the first frame update
    public enum SpiderState
    {
        Idle,
        Kill 
    }
    void Awake()
    {
        curState = SpiderState.Idle;
        timer = maxExposureTime;
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (playerTransform == null)
            {
                Debug.LogError("Player not found! 请确保玩家对象有 'Player' 标签。");
            }
        }

        if (animator == null)
        {
            animator = gameObject.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.Log("请确保脚本挂载在怪物身上，且怪物身上有对应的animator");
            }
        }
        if (gameManager == null)
        {
            Debug.LogError("GameManager 未找到！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer<=0)
        {
            curState = SpiderState.Kill;
        }

        switch (curState)
        {
            case SpiderState.Idle:
            {
                HandleIdleState();
                break;
            }
            case SpiderState.Kill:
            {
                HandleKillState();
                break;
            }
        }
    }

    public void TimerReset()
    {
        timer = maxExposureTime;
        
    }

    private void HandleIdleState()
    {
        //TODO：animator change;
    }

    private void HandleKillState()
    {
        StartCoroutine(KillAction());
        //TODO：animator change;
    }

    public IEnumerator KillAction()
    {
        transform.DOMove(playerTransform.position, killTime);//用DOTween包实现移动。
        yield break;
    }
}

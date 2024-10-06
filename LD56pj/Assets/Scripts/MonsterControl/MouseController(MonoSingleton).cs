using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;

public class MouseController : MonoSingleton<MouseController>
{
    public MouseState curState;
    public float maxExposureTime;
    public float timer;
    private GameManager gameManager;

    // Chasing parameters
    [Header("当人物远离怪物时,速度提升率")] 
    public float ChasingIncrese = 0f;

    public float maxSpeed = 2;

    public float minSpeed = 1;
    private float disdance;

    private Vector2 dir;
    // Internal variables
    private Rigidbody2D rb;
    private Vector3 respawnPoint;
    private bool isCapturing = false;
    
    [Header("以下内容可自动获取引用")]
    public Transform playerTransform; // Reference to the player

    public Animator animator;
    // Start is called before the first frame update
    public enum MouseState
    {
        Idle,
        Chasing,
        Kill 
    }
    void Awake()
    {
        curState = MouseState.Idle;
        disdance = 10000;
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
        disdance = (transform.position - playerTransform.position).magnitude;
        switch (curState)
        {
            case MouseState.Idle:
            {
                HandleIdleState();
                break;
            }
            case MouseState.Chasing:
            {
                HandleChasingState();
                break;
            }
            case MouseState.Kill:
            {
                HandleKillState();
                break;
            }
        }
    }

    public void HandleChasingState()
    {
        //rb.velocity = dir*Mathf.Clamp(minSpeed+)
    }
    private void HandleIdleState()
    {
        //TODO：animator change;
    }

    private void HandleKillState()
    {
        StartCoroutine(KillAction());
    }

    public IEnumerator KillAction()
    {
        TypeEventSystem.Global.Send<OnPlayerDiedEvents>();
        yield break;
    }
}

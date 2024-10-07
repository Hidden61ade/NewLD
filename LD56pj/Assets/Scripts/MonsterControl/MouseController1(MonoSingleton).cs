using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using UnityEngine;

public class MouseController1 : MonoSingleton<MouseController1>
{
    public MouseState curState;
    [Header("出现、消失动画的时间")]
    public float appearTime;
    private GameManager gameManager;

    // Chasing parameters
    [Header("当人物远离怪物时,速度提升率")] 
    public float SpeedIncrese = 0f;

    public float maxSpeed = 2;

    public float minSpeed = 1;
    private float disdance;

    private Vector2 dir;
    // Internal variables
    private Rigidbody2D rb;
    private Vector3 respawnPoint;
    private bool isCapturing = false;

    private Vector3 origenPosition;
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
        gameObject.GetComponent<SpriteRenderer>().DOFade(1, 0.01f);
        origenPosition = transform.position;
        TypeEventSystem.Global.Register<OnLevelResetEvent>(e =>
        {
            Init();
        });
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

    public void Init()
    {
        transform.position = origenPosition;
        animator.CrossFade("Idle",0);
        curState = MouseState.Idle;
        disdance = 10000;
        isCapturing = false;
    }
    // Update is called once per frame
    void Update()
    {
        disdance = ((Vector2)transform.position - (Vector2)playerTransform.position).magnitude;
        dir = (-(Vector2)transform.position + (Vector2)playerTransform.position).normalized;
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
        rb.velocity = dir * Mathf.Clamp(minSpeed + SpeedIncrese * disdance, minSpeed, maxSpeed);
        animator.CrossFade("Chase",0);
    }
    private void HandleIdleState()
    {
        animator.CrossFade("Idle",0);
        rb.velocity = Vector2.zero;
    }

    private void HandleKillState()
    {
        //animator.SetBool("Kill",true);
        StartCoroutine(KillAction());
    }

    public IEnumerator KillAction()
    {
        TypeEventSystem.Global.Send<OnPlayerDiedEvents>();
        yield break;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            curState = MouseState.Kill;
            HandleKillState();
        }
    }

    public void AppearAction()
    {
        gameObject.GetComponent<SpriteRenderer>().DOFade(1, appearTime);
    }
    
    public void DisAppearAction()
    {
        gameObject.GetComponent<SpriteRenderer>().DOFade(0, appearTime);
    }

    public void DigIntoGround()
    {
        animator.CrossFade("DestroyGround",0);
    }
}

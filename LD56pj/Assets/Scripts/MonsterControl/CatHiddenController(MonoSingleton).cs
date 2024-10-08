using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using UnityEngine;
using UnityEngine.Diagnostics;

public class CatHiddenController : MonoBehaviour
{
    public CatState curState;
    public float maxExposureTime;
    public float timer;
    public bool canAttack;
    public float animationSpeed = 1;
    private GameManager gameManager;

    private Vector3 pawOriginPos;
    // Internal variables
    private Rigidbody2D rb;
    private Vector3 respawnPoint;
    private bool isCapturing = false;
    
    [Header("以下内容可自动获取引用")]
    public Transform paw;
    public Transform playerTransform; // Reference to the player

    public Animator animator;
    // Start is called before the first frame update
    public enum CatState
    {
        Idle,
        Kill 
    }
    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        canAttack = false;
        animator.speed = animationSpeed;
        curState = CatState.Idle;
        timer = maxExposureTime;
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        paw = transform.Find("paw");
        pawOriginPos = paw.position;
        TypeEventSystem.Global.Register<OnLevelResetEvent>(e =>
        {
            Init();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
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
        paw.position = pawOriginPos;
        canAttack = false;
        animator.speed = animationSpeed;
        TimerReset();
    }
    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            timer -= Time.deltaTime;
            if (timer<=0)
            {
                curState = CatState.Kill;
                animator.SetTrigger("Kill");
            }
            else 
            {
                //摇头越快，代表remained exposure time的减少
                animator.speed = animationSpeed * (Mathf.Clamp(maxExposureTime / timer, 1, 10));
            }
        }
        switch (curState)
        {
            case CatState.Idle:
            {
                HandleIdleState();
                break;
            }
            case CatState.Kill:
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
        animator.CrossFade("Idle",0);
    }

    private void HandleKillState()
    {
        StartCoroutine(KillAction());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canAttack = true;
            Init();
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canAttack = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canAttack = false;
            Init();
        }
    }
    public IEnumerator KillAction()
    {
        paw.DOMove(playerTransform.position, 0.3f);
        yield return new WaitForSeconds(0.3f);
        PlayControl.Instance.ActionDie();
        TypeEventSystem.Global.Send<OnPlayerDiedEvents>();
        yield break;
    }
}

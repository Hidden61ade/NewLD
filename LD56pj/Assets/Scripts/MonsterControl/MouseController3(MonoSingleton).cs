using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using DG.Tweening;
public class MouseController3 : MonoSingleton<MouseController3>
{
public MouseState curState;
    [Header("出现、消失动画的时间")]
    public float appearTime;
    private GameManager gameManager;

    [Header("徘徊的两个位置的x坐标")] 
    public float leftPoint;

    public float rightPoint;

    public float wanderIntervalTime;

    private int facing;

    private float velocity;
    // Chasing parameters
    [Header("当人物远离怪物时,速度提升率")] 
    public float SpeedIncrese = 0f;

    public float maxSpeed = 2;

    public float minSpeed = 1;
    private float disdance;

    private Vector2 dir;
    // Internal variables
    private Rigidbody2D rb;
    private bool isGoingToKillYou = false;
    private Vector3 originPosition;
    [Header("以下内容可自动获取引用")]
    public Transform playerTransform; // Reference to the player

    public Animator animator;
    // Start is called before the first frame update
    public enum MouseState
    {
        Idle,
        Walk,
        Chasing,
        Kill 
    }
    void Awake()
    {
        facing = 1;
        curState = MouseState.Walk;
        disdance = 10000;
        velocity = (rightPoint-leftPoint)/wanderIntervalTime;
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        gameObject.GetComponent<SpriteRenderer>().DOFade(1, 0.01f);
        originPosition = transform.position;
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
        animator.CrossFade("Walk",0);
        bool isGoingToKillYou = false;
        transform.position = originPosition;
        curState = MouseState.Walk;
        disdance = 10000;
        facing = 1;
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
            case MouseState.Walk:
            {
                HandleWalkState();
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

    private void HandleWalkState()
    {
        animator.CrossFade("Chase",0);
        //获取朝向
        if (facing==-1&&transform.position.x<leftPoint)
        {
            facing = 1;
        }

        if (facing==1&&transform.position.x>rightPoint)
        {
            facing = -1;
        }
        //处理SR朝向
        Vector3 v = gameObject.transform.localScale;
        v.x = facing * Mathf.Abs(v.x);
        gameObject.transform.localScale = v;
        
        
        Vector3 p = transform.position;
        p.x += velocity * facing*Time.deltaTime;
        transform.position = p;
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
        if ((!isGoingToKillYou)&&other.CompareTag("Player"))
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

    public void startChasing()
    {
        curState = MouseState.Chasing;
        transform.DOMove(originPosition, 0.1f);
    }
    public void SwitchToKill()
    {
        if (curState==MouseState.Walk)
        {
            StartCoroutine(KillActionWhenWander());
        }
    }
    IEnumerator KillActionWhenWander()
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
}

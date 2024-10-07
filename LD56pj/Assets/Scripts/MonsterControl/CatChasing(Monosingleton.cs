using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using DG.Tweening;
public class CatChasing : MonoSingleton<CatChasing>
{
    [Header("徘徊的两个位置的x坐标")] 
    public float leftPoint;

    public float rightPoint;

    public float wanderIntervalTime;

    private int facing;
    private GameManager gameManager;
    private float velocity;
    private Vector3 originPosition;
    [Header("以下内容可自动获取引用")]
    public Transform playerTransform; // Reference to the player

    public Animator animator;
    void Awake()
    {
        facing = 1;
        velocity = (rightPoint-leftPoint)/wanderIntervalTime;
        gameManager = GameManager.Instance;
        gameObject.GetComponent<SpriteRenderer>().DOFade(1, 0.01f);
        originPosition = transform.position;
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
        facing = 1;
        transform.position = originPosition;
        animator.CrossFade("Walk",0);
    }

    public void Update()
    {
        animator.CrossFade("Walk",0);
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(HandlePlayerDead());
        }
    }

    IEnumerator HandlePlayerDead()
    {
        transform.DOMove(playerTransform.position, 0.3f);
        yield return new WaitForSeconds(0.3f);
        PlayControl.Instance.ActionDie();
        yield return new WaitForSeconds(0.1f);
        TypeEventSystem.Global.Send<OnPlayerDiedEvents>();
        yield break;
    }
}

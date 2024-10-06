using System;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using Unity.Burst.CompilerServices;

public class PlayControl : MonoSingleton<PlayControl>
{
    public SpriteRenderer playerSR;
    [Header("各种参数")]
    public float jumpPower = 10;// 跳跃动力   
    public float rollPower = 1;// 翻滚动力
    public float walkSpeed = 1;// 行走速度
    public float runSpeed = 1;// 跑步速度
    public float getDownSpeed = 1;// 蹲下速度
    public float moveSpeed;// 行动速度,由是奔跑还是行走觉得
    public float pushSpeed = 1;// 推物速度
    private string item;// 道具tag

    //public float g = 9.8f;
    //public float yDown = 0;

    public int facing = 1; // 面朝向系数,控制翻滚时力的朝向,1向右,-1向左

    public bool canCatch = false;// 能否持物
    public bool canPush = false;// 能否推动
    public bool canMoveR = true;//防止蹭墙
    public bool canMoveL = true;//防止蹭墙
    public bool isGround = true;// 是否在地面，关系能否跳跃等
    public bool isRun = false;// 是否在奔跑
    public bool isGetDown = false;// 是否在趴下 TODO: 动画
    //public bool facingRight = true;// 是否面朝右
    public bool isRoll = false;// 是否在翻滚
    private bool isCatch = false;// 是否持物
    public bool isPush = false;// 是否推动
    private bool canStand = true;

    public float RollDuration;// 翻滚持续时间 //周：此处的时间需要与动画长度相同


    private float axisH;
    private bool ditectGround = true;
    private bool canInteract = false;
    private bool isCollL = false;
    private bool isCollR = false;
    private bool isColl = false;
    public bool isJump = false;

    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;

    private LayerMask groundLayer;

    private string pushWall = "PushWall";// 推动墙layer
    private string ground = "Ground";// 地面layer
    private string interact = "Interact";

    private Vector3 jumpCollisionPos; // 繁琐的三面碰撞检测坐标
    private Vector3 moveCollisionPosL;
    private Vector3 moveCollisionPosR;
    private Vector3 standColiisionPosR;
    private Vector3 standColiisionPosL;

    private Vector2 collisionBoxSize;
    private Vector2 collisionBoxCenter;


    private GameObject pushBox = null;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();// 获得刚体组件
        boxCollider2d = GetComponent<BoxCollider2D>();
        TypeEventSystem.Global.Register<OnLevelResetEvent>((e) =>
        {
            Initialization();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        collisionBoxSize = boxCollider2d.size;
        collisionBoxCenter = boxCollider2d.offset;
        groundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        axisH = Input.GetAxis("Horizontal");
        CollisionDetection();// 更新碰撞检测位置
        Actions();// 更新动作
        // TODO: 速度
    }

    private void Initialization()
    {
        PlayerAnimatorManager.Instance.ChangeCrouchState(false);
        PlayerAnimatorManager.Instance.ChangeJumpState(false);
        PlayerAnimatorManager.Instance.ChangeLiftState(false);
        PlayerAnimatorManager.Instance.ChangePushState(false);
        PlayerAnimatorManager.Instance.SwitchToWalk();
        isRun = false;// 是否在奔跑
        isGetDown = false;// 是否在趴下
        isRoll = false;// 是否在翻滚
        isCatch = false;// 是否持物
        isPush = false;// 是否推动
        facing = 1; // 面朝向系数,控制翻滚时力的朝向,1向右,-1向左
        canCatch = false;// 能否持物
        canPush = false;// 能否推动
        canMoveR = true;//防止蹭墙
        canMoveL = true;//防止蹭墙
        isGround = true;// 是否在地面，关系能否跳跃等

    }

    private void OnTriggerStay2D(Collider2D other) // TODU: 与道具互动
    {
        //if (other.gameObject.CompareTag(item))
        //    canCatch = true;
        //if (other.gameObject.CompareTag(interact))
        //{
        //    canInteract = true;
        //}
        if (other.gameObject.CompareTag(interact))
        {
            //Debug.Log("碰撞了");
            if (Input.GetKeyDown(KeyCode.E))
            {
                ButtonOfDoor buttonofdoor = other.GetComponent<ButtonOfDoor>();
                if (buttonofdoor != null)
                {
                    Debug.Log("按下了");
                    buttonofdoor.Open();
                }

            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) // 推东西
    {
        int a = 0; // 用来计数水平碰撞
        int b = 0; // 用来计数垂直碰撞
        bool tempCanPush = false; // 用临时变量记录能否推

        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;
            //Debug.Log(normal);

            // 水平方向碰撞
            if (normal.y < 0.001f || normal.y > -0.001f)
            {
                a += 1;
            }

            if (contact.collider.CompareTag(pushWall) && isGround && axisH != 0)
            {
                if (normal.y == 0)
                {
                    tempCanPush = true; // 用临时变量记录
                    pushBox = contact.collider.gameObject;
                }
            }
        }

        // 在循环外统一更新推状态
        canPush = tempCanPush;
        PlayerAnimatorManager.Instance.ChangePushState(isPush);

        // 检查是否碰撞到了水平面
        isColl = a > 0;

        //// 检查是否接触到了地面
        //isGround = b > 0;
    }



    void Actions()
    {
        GetState();// 获得状态(是否在奔跑等)

        ActionRoll();// 翻滚动作

        if (!isRoll)
        {
            ActionJump();
            ActionPush();
            ActionMove();
            //ActionCatch();
            ActionSetMoveValueInAnimator();
        }// 如果不在翻滚,移动跳跃
         //朝向左边时，翻转x轴

        GetFacing();// 获得面朝向
        Vector3 v = gameObject.transform.localScale;
        v.x = facing * Mathf.Abs(v.x);
        gameObject.transform.localScale = v;
    }

    void ActionSetMoveValueInAnimator()
    {
        if (isRun)
        {
            PlayerAnimatorManager.Instance.SwitchToRun();
        }
        else if (moveSpeed > 0.0001)
        {
            PlayerAnimatorManager.Instance.SwitchToWalk();
        }
        else
        {
            PlayerAnimatorManager.Instance.SwitchToIdle();
        }
    }
    void CollisionDetection()
    {
        // TODO: 需要地面layer为Ground
        if (boxCollider2d != null)
        {
            Vector3 localCenter = boxCollider2d.bounds.center;
            // 获取 BoxCollider 的局部中心
            Vector3 localSize = boxCollider2d.size;
            // 获取世界空间中的大小
            Vector3 worldSize = Vector3.Scale(localSize, transform.lossyScale);

            if (localCenter != null)
            {
                jumpCollisionPos = new Vector3(localCenter.x, localCenter.y - GetComponent<Collider2D>().bounds.extents.y - 0.01f, 0f);
                moveCollisionPosL = new Vector3(localCenter.x - GetComponent<Collider2D>().bounds.extents.x - 0.01f, localCenter.y, 0f);
                moveCollisionPosR = new Vector3(localCenter.x + GetComponent<Collider2D>().bounds.extents.x + 0.01f, localCenter.y, 0f);
                standColiisionPosL = new Vector3(localCenter.x - GetComponent<Collider2D>().bounds.extents.x, localCenter.y);
                standColiisionPosR = new Vector3(localCenter.x + GetComponent<Collider2D>().bounds.extents.x, localCenter.y); ;
            }
        }
        // 检查角色是否在地面上（可以使用射线检测等方法）

        isCollR = (Physics2D.Raycast(moveCollisionPosR - Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.right, 0.01f) || Physics2D.Raycast(moveCollisionPosR + Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.right, 0.01f) || Physics2D.Raycast(moveCollisionPosR, Vector2.right, 0.01f));
        isCollL = ((Physics2D.Raycast(moveCollisionPosL - Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.left, 0.01f) || Physics2D.Raycast(moveCollisionPosL + Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.left, 0.01f)) || Physics2D.Raycast(moveCollisionPosL, Vector2.left, 0.01f));
        //// 是否撞墙,防止粘墙上

        canStand = !(Physics2D.Raycast(standColiisionPosL, Vector2.up, 1f, LayerMask.GetMask("Ground")) || Physics2D.Raycast(standColiisionPosR, Vector2.up, 1f, LayerMask.GetMask("Ground")));

        ditectGround = Physics2D.Raycast(jumpCollisionPos + Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down, 0.01f) || Physics2D.Raycast(jumpCollisionPos - Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down, 0.01f);
        // 是否在地面,觉得跳跃翻滚

        //canPush = Physics2D.Raycast(moveCollisionPosR, Vector2.right, 0.01f) || Physics2D.Raycast(moveCollisionPosL, Vector2.left, 0.01f);
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(standColiisionPosL, Vector2.up);
    //    Gizmos.DrawRay(standColiisionPosR, Vector2.up);
    //    Gizmos.DrawRay(moveCollisionPosR, Vector2.right);
    //}

    void ActionJump()
    {

        if (Input.GetKeyDown(KeyCode.Space ) && isGround )
        {
            if (!isPush && !isGetDown)
            {
                rigidbody2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                isJump = true;
                PlayerAnimatorManager.Instance.ChangeJumpState(isJump);
            }
           
        }

        // TODO: 动画:跳跃,关卡:确定跳跃高度,腾空时间等
    }

    void ActionMove()
    {
        float move = axisH;
        if (move != 0)
        {
            if (!isPush)
            {
                if (isRun) { moveSpeed = runSpeed; }
                else if (isGetDown) { moveSpeed = getDownSpeed; }
                else //if (isGround)
                { moveSpeed = walkSpeed; } // 根据状态确定速度
                canMoveR = !(isCollR && isColl);
                canMoveL = !(isCollL && isColl);
                if (move < 0 && !canMoveL) { move = 0; }
                if (move > 0 && !canMoveR) { move = 0; }// 碰墙后不能移动,防止粘墙上
            }
            else
            {
                moveSpeed = pushSpeed;
            }
        }
        else
        {
            moveSpeed = 0;
        }
        rigidbody2d.velocity = new Vector2(move * moveSpeed, rigidbody2d.velocity.y);
    }// TODO: 关卡:确定速度,程序:与墙的交互

    void ActionPush()
    {

        if (pushBox != null && isGround)
        {
            if (canPush && axisH * facing > 0)
            { 
                pushBox.transform.SetParent(transform);
                isPush = true;
            }
            else  if (axisH == 0)
            { 
                pushBox.transform.SetParent(null); 
                isPush = false;
            }
        }


    }

    void ActionRoll()
    {
        StartCoroutine(IEActionRoll());
    }// 不是我写的看不懂

    public IEnumerator IEActionRoll()
    {
        if (Input.GetMouseButtonDown(0) && isGround && !isPush)
        {
            rigidbody2d.AddForce(Vector2.right * rollPower * facing, ForceMode2D.Impulse);
            isRoll = true;
            PlayerAnimatorManager.Instance.SwitchToDash();
        }
        else
        {
            yield break;
        }
        yield return new WaitForSeconds(RollDuration); //周：此处的时间需要与动画长度相同
        isRoll = false;// 不是我写的看不懂   //周：Dash（Roll）动画在播放完后自动进入walk或run，所以不需要再改改动动画机
    }// TODO: 距离



    void GetState()
    {
        isGround = ditectGround;
        if (!isPush)
        {
            if (Input.GetKey(KeyCode.S))
            {
                isGetDown = true;
                boxCollider2d.size = new Vector2(collisionBoxSize.x, collisionBoxSize.y / 2);
                boxCollider2d.offset = new Vector2(collisionBoxCenter.x, (collisionBoxCenter.y - (collisionBoxSize.y - boxCollider2d.size.y) / 2));
                PlayerAnimatorManager.Instance.ChangeCrouchState(isGetDown);
            }
            else
            {
                //float extraHeight = collisionBoxSize.y / 2; // 原始高度的一半
                //Vector2 headPosition = new Vector2(transform.position.x, transform.position.y + extraHeight - 0.1f);
                //Collider2D hit = Physics2D.OverlapCircle(headPosition, boxCollider2d.size.y / 2, groundLayer); 

                //Debug.Log(hit);
                if (canStand)
                {
                    isGetDown = false;
                    boxCollider2d.size = collisionBoxSize;
                    boxCollider2d.offset = collisionBoxCenter;
                    PlayerAnimatorManager.Instance.ChangeCrouchState(isGetDown);
                }


                //else { Debug.Log(hit); }
                
            }// 是否下蹲

            if (Input.GetKey(KeyCode.LeftShift) && !isGetDown && axisH != 0 && isGround)
            {
                isRun = true;
                PlayerAnimatorManager.Instance.SwitchToRun();
            }
            else if (isGround) 
            {
                isRun = false;
                PlayerAnimatorManager.Instance.SwitchToWalk();
            }// 是否奔跑(蹲下不能跑)
            if (isJump && isGround)
            {
                isJump = false;
                PlayerAnimatorManager.Instance.ChangeJumpState(isJump);
            }
        }

    }

    void GetFacing()
    {
        float move = axisH;
       if(!isPush)
        {
            if (move > 0)
            {
                facing = 1;
            }
            else if (move < 0)
            {
                facing = -1;
            }

        }
      
    }// 面朝向


    //void FakeG()
    //{
    //    if(isGround)
    //    {
    //        yDown = 0;
    //    }
    //    else
    //    {
    //        yDown += g * Time.deltaTime / 10;
    //    }
    //}
    //void OnDrawGizmos()
    //{
    //    // 可视化射线检测
    //    Gizmos.color = Color.red;
    //    //Gizmos.DrawRay(jumpCollisionPos, Vector2.down);
    //    Gizmos.DrawRay(jumpCollisionPos + Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down);
    //    Gizmos.DrawRay(jumpCollisionPos - Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down);
    //    //Gizmos.DrawRay(moveCollisionPosR + Vector3.up * transform.position.y, Vector2.right);
    //}


    //void ActionCatch()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        //Debug.Log("按");
    //        if (!isCatch && canCatch)
    //        {
    //            isCatch = true;
    //            PlayerAnimatorManager.Instance.ChangePickState(isCatch);
    //            //Debug.Log("拿");
    //        }
    //        else if (isCatch)
    //        {
    //            isCatch = false;
    //            PlayerAnimatorManager.Instance.ChangePickState(isCatch);
    //            //Debug.Log("放");
    //        }
    //    }
    //}// 道具交互

}


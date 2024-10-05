using System;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public string item;// 道具tag
    public string pushWall = "PushWall";// 推动墙layer
    public string ground = "Ground";// 地面layer
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
    public bool isCatch = false;// 是否持物
    public bool isPush = false;// 是否推动

    public float RollDuration;// 翻滚持续时间 //周：此处的时间需要与动画长度相同

    private Rigidbody2D rigidbody2d;

    private Vector3 jumpCollisionPos; // 繁琐的三面碰撞检测坐标
    private Vector3 moveCollisionPosL;
    private Vector3 moveCollisionPosR;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();// 获得刚体组件
        TypeEventSystem.Global.Register<OnLevelResetEvent>((e) =>
        {
            Initialization();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
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
        if (other.gameObject.CompareTag(item))
            canCatch = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(item)) // 检查是否是特定标签的物体
        {
            canCatch = false; // 离开触发器
        }
    }
    private void OnCollisionStay2D(Collision2D collision)// 推东西
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.collider.CompareTag(pushWall) && (Input.GetAxis("Horizontal") * (contact.collider.gameObject.transform.position.x - transform.position.x)) > 0 && canPush)
            {
                isPush = true;
                PlayerAnimatorManager.Instance.ChangePushState(isPush);
            }
            else
            {
                isPush = false;
                PlayerAnimatorManager.Instance.ChangePushState(isPush);
            }

        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    foreach (ContactPoint2D contact in collision.contacts)
    //    {
    //        if (contact.collider.CompareTag(pushWall))
    //        {
    //            isPush = false; // 停止碰撞时将 isPush 设置为 false
    //        }
    //    }
    //}

    void Actions()
    {
        GetState();// 获得状态(是否在奔跑等)
        GetFacing();// 获得面朝向
        ActionRoll();// 翻滚动作
        if (!isRoll) 
        {
            ActionJump();
            ActionMove();
            ActionCatch();
        }// 如果不在翻滚,移动跳跃
        //朝向左边时，翻转x轴
        if (facing==1)
        {
            playerSR.flipX = false;
        }
        else if (facing==-1)
        {
            playerSR.flipX = true;
        }
    }
     void CollisionDetection()
    {
        // TODO: 需要地面layer为Ground

        // 检查角色是否在地面上（可以使用射线检测等方法）
        jumpCollisionPos = new Vector3(transform.position.x, transform.position.y - GetComponent<Collider2D>().bounds.extents.y - 0.01f, 0f);
        moveCollisionPosL = new Vector3(transform.position.x - GetComponent<Collider2D>().bounds.extents.x - 0.01f, transform.position.y, 0f);
        moveCollisionPosR = new Vector3(transform.position.x + GetComponent<Collider2D>().bounds.extents.x + 0.01f, transform.position.y, 0f);
        // 更新坐标,以玩家位置为中心,检测四个角、三个面

        //isGround = Physics2D.Raycast(jumpCollisionPos + Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down, 0.01f, LayerMask.GetMask(ground)) || Physics2D.Raycast(jumpCollisionPos - Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down, 0.01f,LayerMask.GetMask(ground));
        //// 是否在地面,觉得跳跃翻滚

        //canMoveR = !((Physics2D.Raycast(moveCollisionPosR - Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.right, 0.01f, LayerMask.GetMask(ground)) || Physics2D.Raycast(moveCollisionPosR + Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.right, 0.01f, LayerMask.GetMask(ground))) || Physics2D.Raycast(moveCollisionPosR , Vector2.right, 0.01f, LayerMask.GetMask(ground)));
        //canMoveL = !((Physics2D.Raycast(moveCollisionPosL - Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.left, 0.01f, LayerMask.GetMask(ground)) || Physics2D.Raycast(moveCollisionPosL + Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.left, 0.01f, LayerMask.GetMask(ground))) || Physics2D.Raycast(moveCollisionPosL , Vector2.left, 0.01f, LayerMask.GetMask(ground)));
        //// 是否撞墙,防止粘墙上
        canMoveR = !((Physics2D.Raycast(moveCollisionPosR - Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.right, 0.01f) || Physics2D.Raycast(moveCollisionPosR + Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.right, 0.01f)) || Physics2D.Raycast(moveCollisionPosR, Vector2.right, 0.01f));
        canMoveL = !((Physics2D.Raycast(moveCollisionPosL - Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.left, 0.01f) || Physics2D.Raycast(moveCollisionPosL + Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.left, 0.01f)) || Physics2D.Raycast(moveCollisionPosL, Vector2.left, 0.01f));
        // 是否撞墙,防止粘墙上

        isGround = Physics2D.Raycast(jumpCollisionPos + Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down, 0.01f) || Physics2D.Raycast(jumpCollisionPos - Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down, 0.01f);
        // 是否在地面,觉得跳跃翻滚

        canPush = Physics2D.Raycast(moveCollisionPosR, Vector2.right, 0.01f) || Physics2D.Raycast(moveCollisionPosL, Vector2.left, 0.01f);
    }
 
    void ActionCatch()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("按");
            if (!isCatch && canCatch)
            {
                isCatch = true;
                PlayerAnimatorManager.Instance.ChangePickState(isCatch);
                Debug.Log("拿");
            }
            else if(isCatch)
            {
                isCatch = false;
                PlayerAnimatorManager.Instance.ChangePickState(isCatch);
                Debug.Log("放");
            }
        }
    }// 道具交互

    void ActionJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGround)
        {
            rigidbody2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        
        // TODO: 动画:跳跃,关卡:确定跳跃高度,腾空时间等
    }

    void ActionMove()
    {   
        if (!isPush)
        {
            if (isRun) { moveSpeed = runSpeed; }
            else if (isGetDown) { moveSpeed = getDownSpeed; }
            else if(isGround)
            { moveSpeed = walkSpeed; } // 根据状态确定速度

            float move = Input.GetAxis("Horizontal");

            if (move < 0 && !canMoveL) { move = 0; }
            if (move > 0 && !canMoveR) { move = 0; }// 碰墙后不能移动,防止粘墙上
            rigidbody2d.velocity = new Vector2(move * moveSpeed, rigidbody2d.velocity.y);
        }
        else
        {
            moveSpeed = pushSpeed;
            float move = Input.GetAxis("Horizontal");
            rigidbody2d.velocity = new Vector2(move * moveSpeed, rigidbody2d.velocity.y);
        }
    }// TODO: 关卡:确定速度,程序:与墙的交互

    void ActionRoll()
    {
        StartCoroutine(IEActionRoll());
    }// 不是我写的看不懂

    public IEnumerator IEActionRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && !isPush)
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
        if (Input.GetKey(KeyCode.S))
        {
            isGetDown = true;
            PlayerAnimatorManager.Instance.ChangeCrouchState(isGetDown);
        }
        else
        {
            isGetDown= false;
            PlayerAnimatorManager.Instance.ChangeCrouchState(isGetDown);
        }// 是否下蹲

        if (Input.GetKey(KeyCode.LeftShift) && !isGetDown)
        {
            isRun = true;
            PlayerAnimatorManager.Instance.SwitchToRun();
        }
        else 
        { 
            isRun = false;
            PlayerAnimatorManager.Instance.SwitchToWalk();
        }// 是否奔跑(蹲下不能跑)
    }

    void GetFacing()
    {
        float move = Input.GetAxis("Horizontal");
        if (move > 0)
        {
            facing = 1;
        }
        else
        {
            facing = -1;
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
}


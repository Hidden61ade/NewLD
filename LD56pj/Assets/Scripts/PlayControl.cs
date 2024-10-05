using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControl : MonoBehaviour
{
    public float jumpPower = 10;// 跳跃动力   
    public float rollPower = 1;// 翻滚动力
    public float walkSpeed = 1;// 行走速度
    public float runSpeed = 1;// 跑步速度
    public float getDownSpeed = 1;// 蹲下速度
    public float moveSpeed;// 行动速度,由是奔跑还是行走觉得
    public float g = 9.8f;
    public float yDown = 0;

    public int facing = 1; // 面朝向系数,控制翻滚时力的朝向,1向右,-1向左

    private Vector3 jumpCollisionPos;
    private Vector3 moveCollisionPosL;
    private Vector3 moveCollisionPosR;

    public bool canMoveR = true;//防止蹭墙
    public bool canMoveL = true;//防止蹭墙
    public bool isGround = true;// 是否在地面，关系能否跳跃等
    public bool isRun = false;// 是否在奔跑
    public bool isGetDown = false;// 是否在趴下
    //public bool facingRight = true;// 是否面朝右
    public bool isRoll = false;// 是否在翻滚

    public float RollDuration;// 翻滚持续时间

    private Rigidbody2D rigidbody2d;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();// 获得刚体组件
    }

    // Update is called once per frame
    void Update()
    {

        CollisionDetection();
        Actions();// 更新动作
        // TODO: 速度
    }

    void Actions()
    {
        GetState();// 获得状态(是否在奔跑等)
        GetFacing();// 获得面朝向
        ActionRoll();// 翻滚动作
        if (!isRoll) 
        {
            ActionJump();
            ActionMove();
        }// 如果不在翻滚,移动跳跃

    }
     void CollisionDetection()
    {
        // 检查角色是否在地面上（可以使用射线检测等方法）
        jumpCollisionPos = new Vector3(transform.position.x, transform.position.y - GetComponent<Collider2D>().bounds.extents.y - 0.01f, 0f);
        moveCollisionPosL = new Vector3(transform.position.x - GetComponent<Collider2D>().bounds.extents.x - 0.01f, transform.position.y, 0f);
        moveCollisionPosR = new Vector3(transform.position.x + GetComponent<Collider2D>().bounds.extents.x + 0.01f, transform.position.y, 0f);

        isGround = Physics2D.Raycast(jumpCollisionPos + Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down, 0.01f);
        canMoveR = !(Physics2D.Raycast(moveCollisionPosR - Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.right, 0.01f) || Physics2D.Raycast(moveCollisionPosR + Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.right, 0.01f));
        canMoveL = !(Physics2D.Raycast(moveCollisionPosL - Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.left, 0.01f) || Physics2D.Raycast(moveCollisionPosL + Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.left, 0.01f));
    }
    void OnDrawGizmos()
    {
        // 可视化射线检测
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(jumpCollisionPos, Vector2.down);
        Gizmos.DrawRay(jumpCollisionPos + Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down);
        //Gizmos.DrawRay(moveCollisionPosR + Vector3.up * transform.position.y, Vector2.right);
    }

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
            if (isRun)
            {
                moveSpeed = runSpeed;
            }
            else if (isGetDown)
            {
                moveSpeed = getDownSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }// 根据状态确定速度

            float move = Input.GetAxis("Horizontal");
            rigidbody2d.velocity = new Vector2(move * moveSpeed, rigidbody2d.velocity.y);

        
        
    }// TODO: 动画:走路\跑步,动画,关卡:确定速度

    void ActionRoll()
    {
        StartCoroutine(IEActionRoll());
        
    }

    public IEnumerator IEActionRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rigidbody2d.AddForce(Vector2.right * rollPower * facing, ForceMode2D.Impulse);
            isRoll = true;
        }
        else
        {
            yield break;
        }
        yield return new WaitForSeconds(RollDuration);
        isRoll =false;
    }// TODO: 翻滚动画,距离



    void GetState()
    {
        if (Input.GetKey(KeyCode.S))
        {
            isGetDown = true;
        }
        else
        {
            isGetDown= false;
        }// 是否下蹲

        if (Input.GetKey(KeyCode.LeftShift) && !isGetDown)
        {
            isRun = true;
        }
        else 
        { 
            isRun = false;
        }// 是否奔跑(蹲下不能跑)



    }

    void GetFacing()
    {
        if (Input.GetKey(KeyCode.D))
        {
            facing = 1;
        }
        else if (Input.GetKey(KeyCode.A))
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
}


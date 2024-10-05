using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControl : MonoBehaviour
{
    public float jumpPower = 10;// ��Ծ����   
    public float rollPower = 1;// ��������
    public float walkSpeed = 1;// �����ٶ�
    public float runSpeed = 1;// �ܲ��ٶ�
    public float getDownSpeed = 1;// �����ٶ�
    public float moveSpeed;// �ж��ٶ�,���Ǳ��ܻ������߾���
    public float g = 9.8f;
    public float yDown = 0;

    public int facing = 1; // �泯��ϵ��,���Ʒ���ʱ���ĳ���,1����,-1����

    private Vector3 jumpCollisionPos;
    private Vector3 moveCollisionPosL;
    private Vector3 moveCollisionPosR;

    public bool canMoveR = true;//��ֹ��ǽ
    public bool canMoveL = true;//��ֹ��ǽ
    public bool isGround = true;// �Ƿ��ڵ��棬��ϵ�ܷ���Ծ��
    public bool isRun = false;// �Ƿ��ڱ���
    public bool isGetDown = false;// �Ƿ���ſ��
    //public bool facingRight = true;// �Ƿ��泯��
    public bool isRoll = false;// �Ƿ��ڷ���

    public float RollDuration;// ��������ʱ��

    private Rigidbody2D rigidbody2d;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();// ��ø������
    }

    // Update is called once per frame
    void Update()
    {

        CollisionDetection();
        Actions();// ���¶���
        // TODO: �ٶ�
    }

    void Actions()
    {
        GetState();// ���״̬(�Ƿ��ڱ��ܵ�)
        GetFacing();// ����泯��
        ActionRoll();// ��������
        if (!isRoll) 
        {
            ActionJump();
            ActionMove();
        }// ������ڷ���,�ƶ���Ծ

    }
     void CollisionDetection()
    {
        // ����ɫ�Ƿ��ڵ����ϣ�����ʹ�����߼��ȷ�����
        jumpCollisionPos = new Vector3(transform.position.x, transform.position.y - GetComponent<Collider2D>().bounds.extents.y - 0.01f, 0f);
        moveCollisionPosL = new Vector3(transform.position.x - GetComponent<Collider2D>().bounds.extents.x - 0.01f, transform.position.y, 0f);
        moveCollisionPosR = new Vector3(transform.position.x + GetComponent<Collider2D>().bounds.extents.x + 0.01f, transform.position.y, 0f);

        isGround = Physics2D.Raycast(jumpCollisionPos + Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down, 0.01f);
        canMoveR = !(Physics2D.Raycast(moveCollisionPosR - Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.right, 0.01f) || Physics2D.Raycast(moveCollisionPosR + Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.right, 0.01f));
        canMoveL = !(Physics2D.Raycast(moveCollisionPosL - Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.left, 0.01f) || Physics2D.Raycast(moveCollisionPosL + Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.left, 0.01f));
    }
    void OnDrawGizmos()
    {
        // ���ӻ����߼��
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
        // TODO: ����:��Ծ,�ؿ�:ȷ����Ծ�߶�,�ڿ�ʱ���
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
            }// ����״̬ȷ���ٶ�

            float move = Input.GetAxis("Horizontal");
            rigidbody2d.velocity = new Vector2(move * moveSpeed, rigidbody2d.velocity.y);

        
        
    }// TODO: ����:��·\�ܲ�,����,�ؿ�:ȷ���ٶ�

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
    }// TODO: ��������,����



    void GetState()
    {
        if (Input.GetKey(KeyCode.S))
        {
            isGetDown = true;
        }
        else
        {
            isGetDown= false;
        }// �Ƿ��¶�

        if (Input.GetKey(KeyCode.LeftShift) && !isGetDown)
        {
            isRun = true;
        }
        else 
        { 
            isRun = false;
        }// �Ƿ���(���²�����)



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
    }// �泯��

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


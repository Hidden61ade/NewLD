using System;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControl : MonoSingleton<PlayControl>
{
    public SpriteRenderer playerSR;
    [Header("���ֲ���")]
    public float jumpPower = 10;// ��Ծ����   
    public float rollPower = 1;// ��������
    public float walkSpeed = 1;// �����ٶ�
    public float runSpeed = 1;// �ܲ��ٶ�
    public float getDownSpeed = 1;// �����ٶ�
    public float moveSpeed;// �ж��ٶ�,���Ǳ��ܻ������߾���
    public float pushSpeed = 1;// �����ٶ�
    public string item;// ����tag
    public string pushWall = "PushWall";// �ƶ�ǽlayer
    public string ground = "Ground";// ����layer
    //public float g = 9.8f;
    //public float yDown = 0;

    public int facing = 1; // �泯��ϵ��,���Ʒ���ʱ���ĳ���,1����,-1����

    public bool canCatch = false;// �ܷ����
    public bool canPush = false;// �ܷ��ƶ�
    public bool canMoveR = true;//��ֹ��ǽ
    public bool canMoveL = true;//��ֹ��ǽ
    public bool isGround = true;// �Ƿ��ڵ��棬��ϵ�ܷ���Ծ��
    public bool isRun = false;// �Ƿ��ڱ���
    public bool isGetDown = false;// �Ƿ���ſ�� TODO: ����
    //public bool facingRight = true;// �Ƿ��泯��
    public bool isRoll = false;// �Ƿ��ڷ���
    public bool isCatch = false;// �Ƿ����
    public bool isPush = false;// �Ƿ��ƶ�

    public float RollDuration;// ��������ʱ�� //�ܣ��˴���ʱ����Ҫ�붯��������ͬ

    private Rigidbody2D rigidbody2d;

    private Vector3 jumpCollisionPos; // ������������ײ�������
    private Vector3 moveCollisionPosL;
    private Vector3 moveCollisionPosR;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();// ��ø������
        TypeEventSystem.Global.Register<OnLevelResetEvent>((e) =>
        {
            Initialization();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        CollisionDetection();// ������ײ���λ��
        Actions();// ���¶���
        // TODO: �ٶ�
    }
    
    private void Initialization()
    {
        PlayerAnimatorManager.Instance.ChangeCrouchState(false);
        PlayerAnimatorManager.Instance.ChangeJumpState(false);
        PlayerAnimatorManager.Instance.ChangeLiftState(false);
        PlayerAnimatorManager.Instance.ChangePushState(false);
        PlayerAnimatorManager.Instance.SwitchToWalk();
        isRun = false;// �Ƿ��ڱ���
        isGetDown = false;// �Ƿ���ſ��
        isRoll = false;// �Ƿ��ڷ���
        isCatch = false;// �Ƿ����
        isPush = false;// �Ƿ��ƶ�
        facing = 1; // �泯��ϵ��,���Ʒ���ʱ���ĳ���,1����,-1����
        canCatch = false;// �ܷ����
        canPush = false;// �ܷ��ƶ�
        canMoveR = true;//��ֹ��ǽ
        canMoveL = true;//��ֹ��ǽ
        isGround = true;// �Ƿ��ڵ��棬��ϵ�ܷ���Ծ��
    }

    private void OnTriggerStay2D(Collider2D other) // TODU: ����߻���
    {
        if (other.gameObject.CompareTag(item))
            canCatch = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(item)) // ����Ƿ����ض���ǩ������
        {
            canCatch = false; // �뿪������
        }
    }
    private void OnCollisionStay2D(Collision2D collision)// �ƶ���
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.collider.CompareTag(pushWall) && (Input.GetAxis("Horizontal") * (contact.collider.gameObject.transform.position.x - transform.position.x)) > 0)
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
    //            isPush = false; // ֹͣ��ײʱ�� isPush ����Ϊ false
    //        }
    //    }
    //}

    void Actions()
    {
        GetState();// ���״̬(�Ƿ��ڱ��ܵ�)
        GetFacing();// ����泯��
        ActionRoll();// ��������
        if (!isRoll)
        {
            ActionJump();
            ActionMove();
            ActionCatch();
        }// ������ڷ���,�ƶ���Ծ
        //�������ʱ����תx��
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
        // TODO: ��Ҫ����layerΪGround

        // ����ɫ�Ƿ��ڵ����ϣ�����ʹ�����߼��ȷ�����
        jumpCollisionPos = new Vector3(transform.position.x, transform.position.y - GetComponent<Collider2D>().bounds.extents.y - 0.01f, 0f);
        moveCollisionPosL = new Vector3(transform.position.x - GetComponent<Collider2D>().bounds.extents.x - 0.01f, transform.position.y, 0f);
        moveCollisionPosR = new Vector3(transform.position.x + GetComponent<Collider2D>().bounds.extents.x + 0.01f, transform.position.y, 0f);
        // ��������,�����λ��Ϊ����,����ĸ��ǡ�������

        isGround = Physics2D.Raycast(jumpCollisionPos + Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down, 0.01f, LayerMask.GetMask(ground)) || Physics2D.Raycast(jumpCollisionPos - Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down, 0.01f, LayerMask.GetMask(ground));
        // �Ƿ��ڵ���,������Ծ����

        canMoveR = !((Physics2D.Raycast(moveCollisionPosR - Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.right, 0.01f, LayerMask.GetMask(ground)) || Physics2D.Raycast(moveCollisionPosR + Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.right, 0.01f, LayerMask.GetMask(ground))) || Physics2D.Raycast(moveCollisionPosR, Vector2.right, 0.01f, LayerMask.GetMask(ground)));
        canMoveL = !((Physics2D.Raycast(moveCollisionPosL - Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.left, 0.01f, LayerMask.GetMask(ground)) || Physics2D.Raycast(moveCollisionPosL + Vector3.up * GetComponent<Collider2D>().bounds.extents.y, Vector2.left, 0.01f, LayerMask.GetMask(ground))) || Physics2D.Raycast(moveCollisionPosL, Vector2.left, 0.01f, LayerMask.GetMask(ground)));
        // �Ƿ�ײǽ,��ֹճǽ��
        //canPush = Physics2D.Raycast(moveCollisionPosR, Vector2.right, 0.01f, LayerMask.GetMask("PushWall")) || Physics2D.Raycast(moveCollisionPosL, Vector2.left, 0.01f, LayerMask.GetMask("PushWall"));
    }

    void ActionCatch()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("��");
            if (!isCatch && canCatch)
            {
                isCatch = true;
                PlayerAnimatorManager.Instance.ChangePickState(isCatch);
                Debug.Log("��");
            }
            else if (isCatch)
            {
                isCatch = false;
                PlayerAnimatorManager.Instance.ChangePickState(isCatch);
                Debug.Log("��");
            }
        }
    }// ���߽���

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
        if (!isPush)
        {
            if (isRun) { moveSpeed = runSpeed; }
            else if (isGetDown) { moveSpeed = getDownSpeed; }
            else { moveSpeed = walkSpeed; } // ����״̬ȷ���ٶ�

            float move = Input.GetAxis("Horizontal");

            if (move < 0 && !canMoveL) { move = 0; }
            if (move > 0 && !canMoveR) { move = 0; }// ��ǽ�����ƶ�,��ֹճǽ��
            rigidbody2d.velocity = new Vector2(move * moveSpeed, rigidbody2d.velocity.y);
        }
        else
        {
            moveSpeed = pushSpeed;
            float move = Input.GetAxis("Horizontal");
            rigidbody2d.velocity = new Vector2(move * moveSpeed, rigidbody2d.velocity.y);
        }
    }// TODO: �ؿ�:ȷ���ٶ�,����:��ǽ�Ľ���

    void ActionRoll()
    {
        StartCoroutine(IEActionRoll());
    }// ������д�Ŀ�����

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
        yield return new WaitForSeconds(RollDuration); //�ܣ��˴���ʱ����Ҫ�붯��������ͬ
        isRoll = false;// ������д�Ŀ�����   //�ܣ�Dash��Roll�������ڲ�������Զ�����walk��run�����Բ���Ҫ�ٸĸĶ�������
    }// TODO: ����



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
        }// �Ƿ��¶�

        if (Input.GetKey(KeyCode.LeftShift) && !isGetDown)
        {
            isRun = true;
            PlayerAnimatorManager.Instance.SwitchToRun();
        }
        else
        {
            isRun = false;
            PlayerAnimatorManager.Instance.SwitchToWalk();
        }// �Ƿ���(���²�����)
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
    //void OnDrawGizmos()
    //{
    //    // ���ӻ����߼��
    //    Gizmos.color = Color.red;
    //    //Gizmos.DrawRay(jumpCollisionPos, Vector2.down);
    //    Gizmos.DrawRay(jumpCollisionPos + Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down);
    //    Gizmos.DrawRay(jumpCollisionPos - Vector3.right * GetComponent<Collider2D>().bounds.extents.x, Vector2.down);
    //    //Gizmos.DrawRay(moveCollisionPosR + Vector3.up * transform.position.y, Vector2.right);
    //}
}


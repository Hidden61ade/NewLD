using System.Collections;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;

public enum AntState
{
    Idle,
    Chase,
    Kill
}

public class AntController : MonoBehaviour
{
    public AntState currentState = AntState.Idle;

    // References
     // Reference to the player
    private GameManager gameManager;

    // Movement parameters
    public float moveSpeed = 2f;
    public float verticalMoveSpeed = 2f;

    // Capture parameters
    public float killRange = 0.5f;
    public float preKillDelay = 0.5f; // 前摇时间
    public float triggerChaseRange = 5f;
    public float keepChaseRange = 5f;
    private float distance;
    private float chaseRange;
    private Vector3 originPosition;
    public LayerMask wallLayerMask;

    // Internal variables
    private Rigidbody2D rb;
    private Vector3 respawnPoint;
    private bool isCapturing = false;
    
    [Header("可自动获取")]
    public Transform player;
    private Animator animator;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        animator = gameObject.GetComponent<Animator>();
        originPosition = transform.position;
        TypeEventSystem.Global.Register<OnLevelResetEvent>(e =>
        {
            Init();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        if (animator==null)
        {
            Debug.Log("Animator not found! 确保ant装载了animator!");
        }
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            if (player == null)
            {
                Debug.LogError("Player not found! 请确保玩家对象有 'Player' 标签。");
            }
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager 未找到！");
        }
    }
    
    
    public void Init()
    {
        transform.position = originPosition;
        currentState = AntState.Idle;
        animator.CrossFade("Idle",0);
        isCapturing = false;
        chaseRange = triggerChaseRange;
    }
    void Start()
    {
        // Initial state is Idle; no additional setup needed
        currentState = AntState.Idle;
        chaseRange = triggerChaseRange;
    }

    void Update()
    {
        distance =  Vector2.Distance(transform.position, player.position);
        JudgeChaseCondition();
        HandleOrientation();
        switch (currentState)
        {
            case AntState.Idle:
                HandleChaseState();
                HandleIdleState();
                break;
            case AntState.Chase:
                HandleChaseState();
                break;
            case AntState.Kill:
                HandleChaseState();
                break;
        }
    }

    public void HandleOrientation()
    {
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // 转换为度
        // 获取当前旋转角度
        float currentAngle = transform.eulerAngles.z;
        float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, 100 * Time.deltaTime);

        // 应用旋转
        rb.MoveRotation(Quaternion.Euler(0,0, smoothAngle));
    }
    void HandleIdleState()
    {
        chaseRange = triggerChaseRange;
        animator.SetBool("Walk",false);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    void HandleChaseState()
    {
        chaseRange = keepChaseRange;
        animator.SetBool("Walk",true);
        rb.constraints = RigidbodyConstraints2D.None;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        // 检查路径上的墙壁，调整移动方向
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, wallLayerMask);
        if (hit.collider != null)
        {
            // 如果遇到垂直墙面，调整为垂直移动
            Vector2 newDirection = Vector2.up; // 根据具体需求调整方向
            rb.velocity = newDirection * verticalMoveSpeed;
            // 可选：旋转蚂蚁的精灵以适应新方向
            //transform.rotation = Quaternion.Euler(0, 0, 90f);
        }
        else
        {
            // 如果没有遇到墙面，保持水平移动
            rb.velocity = direction * moveSpeed;
            //transform.rotation = Quaternion.identity; // 恢复至水平
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (currentState == AntState.Chase && other.gameObject.CompareTag("Player"))
        {
            // 转换到捕杀状态
            
            currentState = AntState.Kill;
            //rb.velocity = Vector2.zero; // 停止移动
            StartCoroutine(KillRoutine());
        }
    }

    IEnumerator KillRoutine()
    {
        animator.SetBool("Kill",true);
        if (isCapturing)
            yield break;

        isCapturing = true;

        // 等待前摇时间
        yield return new WaitForSeconds(preKillDelay);

        // 检查玩家是否仍在捕杀范围内
        if (distance <= killRange)
        {
            gameManager.HandlePlayerDeath();
        }

        // 重置状态
        currentState = AntState.Chase;
        isCapturing = false;
        animator.SetBool("Kill",false);
    }

    // Method to trigger state transition from Idle to Chase
    public void JudgeChaseCondition()
    {
        if (distance<chaseRange)
        {
            StartChasing();
        }
    }
    public void StartChasing()
    {
        if (currentState == AntState.Idle)
        {
            currentState = AntState.Chase;
            //transform.rotation = Quaternion.identity; // 确保为水平状态
        }
    }
}
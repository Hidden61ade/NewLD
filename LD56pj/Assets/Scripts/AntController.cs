using System.Collections;
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
    public Transform player; // Reference to the player
    private GameManager gameManager;

    // Movement parameters
    public float moveSpeed = 2f;
    public float verticalMoveSpeed = 2f;

    // Capture parameters
    public float killRange = 0.5f;
    public float preKillDelay = 0.5f; // 前摇时间

    public LayerMask wallLayerMask;

    // Internal variables
    private Rigidbody2D rb;
    private Vector3 respawnPoint;
    private bool isCapturing = false;

    private Animator animator;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        animator = gameObject.GetComponent<Animator>();
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

    void Start()
    {
        // Initial state is Idle; no additional setup needed
    }

    void Update()
    {
        switch (currentState)
        {
            case AntState.Idle:
                HandleIdleState();
                break;
            case AntState.Chase:
                HandleChaseState();
                break;
            case AntState.Kill:
                // Kill state handled via collision and coroutine
                break;
        }
    }

    void HandleIdleState()
    {
        animator.SetBool("Walk",false);
        // 待机时可以播放待机动画，或者其他逻辑
    }

    void HandleChaseState()
        {
            animator.SetBool("Walk",false);
            if (player == null)
                return;

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
                transform.rotation = Quaternion.Euler(0, 0, 90f);
            }
            else
            {
                // 如果没有遇到墙面，保持水平移动
                rb.velocity = direction * moveSpeed;
                transform.rotation = Quaternion.identity; // 恢复至水平
            }
        }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (currentState == AntState.Chase && other.CompareTag("Player"))
        {
            // 转换到捕杀状态
            currentState = AntState.Kill;
            rb.velocity = Vector2.zero; // 停止移动
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
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= killRange)
        {
            gameManager.HandlePlayerDeath();
        }

        // 重置状态
        currentState = AntState.Chase;
        isCapturing = false;
    }

    // Method to trigger state transition from Idle to Chase
    public void StartChasing()
    {
        if (currentState == AntState.Idle)
        {
            currentState = AntState.Chase;
            transform.rotation = Quaternion.identity; // 确保为水平状态
        }
    }
}
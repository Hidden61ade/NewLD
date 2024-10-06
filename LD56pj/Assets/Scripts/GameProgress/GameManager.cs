using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Kuchinashi.SceneControl;
using UnityEngine.SceneManagement;
using QFramework;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject Player { get; private set; }
    private bool isGamePaused = false;

    public Scene currentscene;

    // 当前复活点的位置
    private Vector3 respawnPoint;

    // 玩家对象的引用
    private Transform playerTransform;

    void Awake()
    {
        TypeEventSystem.Global.Register<OnPlayerDiedEvents>(e => HandlePlayerDeath()).UnRegisterWhenGameObjectDestroyed(gameObject);
        TypeEventSystem.Global.Register<OnLevelCompleteEvent>(e => HandleLevelComplete()).UnRegisterWhenGameObjectDestroyed(gameObject);

        // 初始化复活点为玩家的起始位置
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            playerTransform = Player.transform;
            respawnPoint = playerTransform.position;
        }
        else
        {
            Debug.LogError("Player not found in the scene. Please ensure the player has the 'Player' tag.");
        }
    }
    private void Start()
    {
        TypeEventSystem.Global.Register<OnSceneLoadedEvent>(e =>
        {
            Player = GameObject.FindGameObjectWithTag("Player");

            if (Player != null)
            {
                playerTransform = Player.transform;
                respawnPoint = playerTransform.position;
            }
            else
            {
                Debug.LogError("Player not found in the scene. Please ensure the player has the 'Player' tag.");
            }
            GameObject.Find("Virtual Camera").GetComponent<CameraControl>().SetCameraFollow(playerTransform);
        });
    }

    // 设置复活点的方法，由 Checkpoint 调用
    public void SetRespawnPoint(Vector3 newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
        Debug.Log("Respawn point set to: " + respawnPoint);
    }

    // 处理角色死亡状态
    public void HandlePlayerDeath()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = 0; // 暂停游戏
            UIManager.Instance.ShowDeathMessage();

            // 等待玩家左键点击以复活
            StartCoroutine(WaitForPlayerRespawn());
        }
    }

    IEnumerator WaitForPlayerRespawn()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1; // 恢复游戏
                isGamePaused = false;
                TypeEventSystem.Global.Send<OnLevelResetEvent>();

                // 复活玩家
                if (playerTransform != null)
                {
                    playerTransform.position = respawnPoint;
                    Debug.Log("Player respawned at: " + respawnPoint);
                }
                else
                {
                    Debug.LogError("Player Transform is null. Cannot respawn.");
                }

                break;
            }
            yield return null;
        }
    }

    void HandleLevelComplete()
    {
        SceneControl.LoadScene("!!!!!!!!!!!NextLevelScene");
    }
}
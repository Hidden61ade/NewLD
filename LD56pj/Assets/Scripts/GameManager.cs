using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Kuchinashi.SceneControl;
using UnityEngine.SceneManagement;
using QFramework;

public class GameManager : MonoSingleton<GameManager>
{
    private bool isGamePaused = false;

    public Scene currentscene;

    void Awake()
    {
        TypeEventSystem.Global.Register<OnPlayerDiedEvents>(e=>HandlePlayerDeath()).UnRegisterWhenGameObjectDestroyed(gameObject);
        TypeEventSystem.Global.Register<OnLevelCompleteEvent>(e=>HandleLevelComplete()).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    // 处理角色死亡状态
    void HandlePlayerDeath()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = 0; // 暂停游戏
            UIManager.Instance.ShowDeathMessage();
            // 等待玩家左键点击以重置游戏
            StartCoroutine(WaitForPlayerClick());
        }
    }

    void HandleLevelComplete()
    {
        SceneControl.LoadScene("!!!!!!!!!!!NextLevelScene");
    }

    IEnumerator WaitForPlayerClick()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1; // 恢复游戏
                isGamePaused = false;
                TypeEventSystem.Global.Send<OnLevelResetEvent>();
                SceneControl.ReloadSceneWithoutConfirm(SceneControl.CurrentScene);//当前场景重新加载
                break;
            }
            yield return null;
        }
    }
}

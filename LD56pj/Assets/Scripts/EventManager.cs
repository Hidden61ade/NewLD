using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Kuchinashi.SceneControl;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public Text deathMessage;
    private bool isGamePaused = false;

    public Scene currentscene;

    void Start()
    {
        if(deathMessage != null){
            deathMessage.gameObject.SetActive(false);
    }
    }

    void Update()
    {
        if (/*角色死亡条件*/)
        {
            HandlePlayerDeath();
        }

        if (/*关卡通关条件*/)
        {
            HandleLevelComplete();
        }
    }

    // 处理角色死亡状态
    void HandlePlayerDeath()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = 0; // 暂停游戏
            if (deathMessage != null)
            {
                deathMessage.gameObject.SetActive(true); // 显示死亡消息
                deathMessage.text = "You died";
                deathMessage.text += "\nClick to restart level";
            }
            // 等待玩家左键点击以重置游戏
            StartCoroutine(WaitForPlayerClick());
        }
    }

    void HandleLevelComplete()
    {
        SceneControl.LoadScene("NextLevelScene");
    }

    IEnumerator WaitForPlayerClick()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1; // 恢复游戏
                SceneControl.ReloadSceneWithoutConfirm("CurrentScene");//当前场景重新加载
                break;
            }
            yield return null;
        }
    }
}

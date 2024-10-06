using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Kuchinashi.SceneControl;
using UnityEngine.SceneManagement;
using QFramework;
using TMPro;

public class UIManager : MonoSingleton<UIManager>
{
    private Transform UICanvas;
    private GameObject mDeathMessage;
    private GameObject mDialoguePanel;
    private GameObject mPausePanel;

    private void Start()
    {
        UICanvas = GameObject.Find("UICanvas").transform;
        mDeathMessage = UICanvas.Find("DeathMessage").gameObject;
        mDialoguePanel = UICanvas.Find("DialoguePanel").gameObject;
        mPausePanel = UICanvas.Find("PausePanel").gameObject;

        mDeathMessage.SetActive(false);
        mDialoguePanel.SetActive(false);
        mPausePanel.SetActive(false);

        // 注册 OnLevelResetEvent 事件，用于在关卡重置时隐藏死亡消息
        TypeEventSystem.Global.Register<OnLevelResetEvent>(e =>
        {
            HideDeathMessage();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    // 显示死亡消息的方法
    public void ShowDeathMessage()
    {
        if (mDeathMessage != null)
        {
            Debug.Log("显示死亡消息: " + mDeathMessage.name);
            var temp = mDeathMessage.GetComponentInChildren<TextMeshProUGUI>();
            if (temp != null)
            {
                temp.text = "You died";
                temp.text += "\nClick to restart level";
            }
            else
            {
                Debug.LogWarning("DeathMessage 对象中未找到 TextMeshProUGUI 组件。");
            }
            mDeathMessage.SetActive(true);
        }
        else
        {
            Debug.LogWarning("mDeathMessage 未被赋值。无法显示死亡消息。");
        }
    }

    // 隐藏死亡消息的方法
    public void HideDeathMessage()
    {
        if (mDeathMessage != null)
        {
            mDeathMessage.SetActive(false);
            Debug.Log("隐藏死亡消息: " + mDeathMessage.name);
        }
        else
        {
            Debug.LogWarning("mDeathMessage 未被赋值。无法隐藏死亡消息。");
        }
    }
}
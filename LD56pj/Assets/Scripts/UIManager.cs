using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Kuchinashi.SceneControl;
using UnityEngine.SceneManagement;
using QFramework;
using TMPro;

public class UIManager : MonoSingleton<UIManager>
{
    private GameObject mDeathMessage;
    private void Awake()
    {
        TypeEventSystem.Global.Register<OnLevelResetEvent>(e =>
        {
            mDeathMessage = GameObject.Find("UICanvas/DeathMessage");
            mDeathMessage.SetActive(false);
        });
        mDeathMessage = GameObject.Find("DeathMessage");
        mDeathMessage.SetActive(false);
    }
    public void ShowDeathMessage()
    {
        Debug.Log(mDeathMessage);
        var temp = mDeathMessage.GetComponentInChildren<TextMeshProUGUI>();
        temp.text = "You died";
        temp.text += "\nClick to restart level";
        mDeathMessage.SetActive(true);
    }
}
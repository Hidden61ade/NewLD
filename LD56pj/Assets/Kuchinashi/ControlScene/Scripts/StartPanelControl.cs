using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kuchinashi.SceneControl;

public class StartPanelControl : MonoBehaviour
{
    private Button mStartButton;
    private Button mExitButton;
    private Button mCreditButton;
    private void Start()
    {
        mStartButton = transform.Find("StartButton").GetComponent<Button>();
        mExitButton = transform.Find("ExitButton").GetComponent<Button>();
        mCreditButton = transform.Find("CreditButton").GetComponent<Button>();
        

        mStartButton.onClick.AddListener(() =>
        {
            // SceneControl.SwitchSceneWithoutConfirm("TestScene");
            SceneControl.SwitchSceneWithoutConfirm("chapter 1");
        });
        mExitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
        mCreditButton.onClick.AddListener(()=>{
            UIManager.Instance.ShowCredits();
        });
    }
}

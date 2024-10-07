using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class PauseControl : MonoBehaviour
{
    Button mResume;
    Button mReset;
    private void Start() {
        mResume = transform.Find("ResumeButton").GetComponent<Button>();
        mReset = transform.Find("RestartButton").GetComponent<Button>();
        mReset.onClick.AddListener(() =>
        {
            try{
            TypeEventSystem.Global.Send<OnPlayerDiedEvents>(new OnPlayerDiedEvents(true));
            Time.timeScale = 1;
            gameObject.SetActive(false);
            }catch(Exception){
                
            }
        });
        mResume.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        });
    }
    private void OnEnable()
    {
        Time.timeScale = 0;
    }
    private void OnDisable() {
        Time.timeScale = 1;
    }
}

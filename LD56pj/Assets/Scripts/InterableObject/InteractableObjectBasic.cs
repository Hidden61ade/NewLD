using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectBasic : MonoBehaviour
{
    // Start is called before the first frame update
    private string uniqueId;
    private string positionXId;
    private string positionYId;
    private string positionZId;
    private bool completed = false;
    private void Awake()
    {
        // 在实例创建时生成唯一标识符
        uniqueId = System.Guid.NewGuid().ToString();
        positionXId = System.Guid.NewGuid().ToString();
        positionYId = System.Guid.NewGuid().ToString();
        positionZId = System.Guid.NewGuid().ToString();
    }

    private void OnEnable()
    {
        LoadState();
    }

    // 方法用于保存状态
    public void SaveState()
    {
        if (IsCompleted())
        {
            //储存位置
            PlayerPrefs.SetFloat(positionXId,transform.position.x);
            PlayerPrefs.SetFloat(positionYId,transform.position.y);
            PlayerPrefs.SetFloat(positionZId,transform.position.z);
        }
        PlayerPrefs.SetInt(uniqueId, IsCompleted() ? 1 : 0);
    }

    // 方法用于加载状态与位置
    public void LoadState()
    {
        if (PlayerPrefs.HasKey(uniqueId))
        {
            if (PlayerPrefs.GetInt(uniqueId) == 1)
            {
                transform.position = new Vector3(PlayerPrefs.GetFloat(positionXId), PlayerPrefs.GetFloat(positionYId),
                    PlayerPrefs.GetFloat(positionZId));
            }
        }
        LoadAdditionalState();
    }

    //子类中可以实现额外的加载方法
    public virtual void LoadAdditionalState()
    {
        
    }
    // 检查物体是否完成，可以在子类中重写
    public virtual bool IsCompleted()
    {
        return false;
    }
}

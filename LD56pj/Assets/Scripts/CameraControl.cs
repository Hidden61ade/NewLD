using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;
using MoonSharp.VsCodeDebugger.SDK;
using System;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance; // 单例实例

    public Transform player; // 角色的 Transform
    private CinemachineVirtualCamera virtualCamera;
    public NoiseSettings noiseSettings;
    private CinemachineFramingTransposer framingTransposer;
    public bool isShaking = false;

    void Awake()
    {
        // 实现单例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保持在场景切换时不被销毁
            // 获取 CinemachineVirtualCamera 组件
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            if (virtualCamera != null)
            {
                // 获取 FramingTransposer 组件
                framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
            else
            {
                Debug.LogError("CinemachineVirtualCamera 组件未找到。请在包含 CameraControl 脚本的 GameObject 上添加 Cinemachine Virtual Camera 组件。");
            }

            // 订阅场景加载完成事件
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // 如果已存在实例，销毁重复的对象
            return;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // 场景加载完成后调用的方法
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 在此处进行必要的初始化，例如查找玩家对象并设置摄像机跟随
        GameObject playerObj = GameObject.FindWithTag("Player"); // 假设玩家有 "Player" 标签
        if (playerObj != null)
        {
            SetCameraFollow(playerObj.transform);
            // 如果您希望手电筒自动获取玩家引用，可以进一步进行设置
        }
        else
        {
            Debug.LogWarning("在新加载的场景中未找到带有 'Player' 标签的对象。");
        }
    }

    public void SetCameraFollow(Transform playerTransform)
    {
        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera 组件未找到。无法设置跟随目标。");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogError("传入的玩家 Transform 为 null。");
            return;
        }

        // 设置摄像机跟随目标为角色
        virtualCamera.Follow = playerTransform;

        // 设置角色在屏幕中的位置，偏左
        if (framingTransposer != null)
        {
            framingTransposer.m_ScreenX = 0.3f; // 0.3 表示角色在屏幕左侧 30% 的位置
        }
        else
        {
            Debug.LogWarning("CinemachineFramingTransposer 组件未找到。请在 Cinemachine Virtual Camera 上添加 Framing Transposer。");
        }
    }

    
    [Obsolete]
    void CameraShakeUntil()
    {
        //持续修改isShaking的值
        StartCoroutine(IENoise(1, false));
    }

    IEnumerator IENoise(float duration, bool isTimeControlled)
    {
        CinemachineBasicMultiChannelPerlin noise = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_NoiseProfile = noiseSettings;
        if (isTimeControlled)
        {
            yield return new WaitForSeconds(duration);
        }
        else
        {
            yield return new WaitUntil(() => { return !isShaking; });
        }
        virtualCamera.DestroyCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
}
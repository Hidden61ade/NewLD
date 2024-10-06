using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;
using MoonSharp.VsCodeDebugger.SDK;
using System;

public class CameraControl : MonoBehaviour
{
    public static Transform player; // 角色的Transform
    private CinemachineVirtualCamera virtualCamera;
    public NoiseSettings noiseSettings;
    private CinemachineFramingTransposer framingTransposer;
    public bool isShaking = false;

    public void SetCameraFollow(Transform player)
    {
        // 获取Cinemachine虚拟摄像机组件
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            // 获取FramingTransposer组件
            framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        // 设置摄像机跟随目标为角色
        virtualCamera.Follow = player;

        // 设置角色在屏幕中的位置，偏左
        framingTransposer.m_ScreenX = 0.3f; // 0.3表示角色在屏幕左侧30%的位置
    }

    void CameraShake(float duration = 1f)
    {
        StartCoroutine(IENoise(duration, true));
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
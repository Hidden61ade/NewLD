using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public Transform player; // 角色的Transform
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer framingTransposer;

    void Start()
    {
        // 获取Cinemachine虚拟摄像机组件
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            // 获取FramingTransposer组件
            framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    void Update()
    {
        if (player != null && framingTransposer != null)
        {
            // 设置摄像机跟随目标为角色
            virtualCamera.Follow = player;

            // 设置角色在屏幕中的位置，偏左
            framingTransposer.m_ScreenX = 0.3f; // 0.3表示角色在屏幕左侧30%的位置
        }
    }
}
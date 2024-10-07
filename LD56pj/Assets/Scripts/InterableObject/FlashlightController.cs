using UnityEngine;
using Cinemachine;

public class FlashlightController : MonoBehaviour
{
    [Header("角色 Transform")]
    public Transform player; // 玩家角色的 Transform
/*
    [Header("Cinemachine Virtual Camera")]
    public CinemachineVirtualCamera virtualCamera; // 场景中的 Cinemachine Virtual Camera
*/
    [Header("手电筒旋转速度")]
    [Range(1f, 20f)]
    public float rotationSpeed = 10f; // 旋转的平滑速度

    public Camera mainCamera;

    void Awake()
    {
        // //if (virtualCamera != null)
        // {
        //     // 获取 Virtual Camera 所控制的主 Camera
        //     // mainCamera = virtualCamera.VirtualCameraGameObject.GetComponent<Camera>();
        //     if (mainCamera == null)
        //     {
        //         // 如果 Virtual Camera 的 GameObject 上没有 Camera 组件，尝试使用 Camera.main
        //         mainCamera = Camera.main;
        //         if (mainCamera == null)
        //         {
        //             Debug.LogError("无法找到主摄像机。请确保场景中有一个 Camera 被 Cinemachine Virtual Camera 控制。");
        //         }
        //     }
        // }
        //else
        // {
        //     // 如果未在 Inspector 中指定 Virtual Camera，使用默认的 Camera.main
        //     mainCamera = Camera.main;
        //     if (mainCamera == null)
        //     {
        //         Debug.LogError("无法找到主摄像机。请确保场景中有一个 Camera 被 Cinemachine Virtual Camera 控制。");
        //     }
        // }

        if (player == null)
        {
            Debug.LogError("玩家的 Transform 未被赋值。请在 Inspector 中将玩家对象拖拽到 'Player' 字段。");
        }
    }

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player 的 Transform 未被赋值。FlashlightController 无法正常工作。");
            return;
        }

        if (CameraControl.Instance != null)
        {
            CameraControl.Instance.SetCameraFollow(player);
        }
        else
        {
            Debug.LogError("CameraControl 实例未找到。请确保 CameraControl 脚本已正确初始化并在场景中。");
        }
    }

    void Update()
    {
        if (mainCamera == null || player == null)
            return;

        // 设置屏幕点的 z 值为摄像机到玩家平面的距离
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = Mathf.Abs(mainCamera.transform.position.z); // 假设玩家在 z = 0

        // 将鼠标位置从屏幕空间转换到世界空间
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(screenPoint);
        mousePos.z = 0f; // 确保 z 轴为 0

        // 调试日志
        // Debug.Log($"Camera Position: {mainCamera.transform.position}, Player Position: {player.position}, Mouse World Position: {mousePos}");

        // 计算手电筒到鼠标的方向
        Vector3 direction = (mousePos - player.position).normalized;
        // Debug.Log($"Direction: {direction}");

        // 计算目标旋转
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

        // 平滑过渡到目标旋转
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 绘制射线（调试）
        Debug.DrawRay(player.position, direction * 5f, Color.red);
        Debug.DrawLine(player.position, mousePos, Color.blue);
        Debug.DrawRay(transform.position, transform.up * 5f, Color.green);
    }
}
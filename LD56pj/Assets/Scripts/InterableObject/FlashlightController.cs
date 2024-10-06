using UnityEngine;
using Cinemachine;

public class FlashlightController : MonoBehaviour
{
    [Header("角色 Transform")]
    public Transform player; // 玩家角色的 Transform

    [Header("Cinemachine Virtual Camera")]
    public CinemachineVirtualCamera virtualCamera; // 场景中的 Cinemachine Virtual Camera

    [Header("手电筒旋转速度")]
    [Range(1f, 20f)]
    public float rotationSpeed = 10f; // 旋转的平滑速度

    private Camera mainCamera;

    void Awake()
    {
        if (virtualCamera != null)
        {
            // 获取 Virtual Camera 所控制的主 Camera
            mainCamera = virtualCamera.VirtualCameraGameObject.GetComponent<Camera>();
            if (mainCamera == null)
            {
                // 如果 Virtual Camera 的 GameObject 上没有 Camera 组件，尝试使用 Camera.main
                mainCamera = Camera.main;
                if (mainCamera == null)
                {
                    Debug.LogError("无法找到主摄像机。请确保场景中有一个 Camera 被 Cinemachine Virtual Camera 控制。");
                }
            }
        }
        else
        {
            // 如果未在 Inspector 中指定 Virtual Camera，使用默认的 Camera.main
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("无法找到主摄像机。请确保场景中有一个 Camera 被 Cinemachine Virtual Camera 控制。");
            }
        }

        if (player == null)
        {
            Debug.LogError("玩家的 Transform 未被赋值。请在 Inspector 中将玩家对象拖拽到 'Player' 字段。");
        }
    }

    void Update()
    {
        if (mainCamera == null || player == null)
            return;

        // 获取鼠标在世界空间的位置
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // 确保 z 轴为 0

        // 计算手电筒到鼠标的方向
        Vector3 direction = (mousePos - player.position).normalized;
        
        // 计算目标旋转角度
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // 获取当前旋转角度
        float currentAngle = transform.eulerAngles.z;

        // 计算平滑过渡后的角度
        float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        // 应用旋转
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, smoothAngle));
    }
}
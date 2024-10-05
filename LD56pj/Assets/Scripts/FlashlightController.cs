using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [Header("角色 Transform")]
    // 玩家角色的 Transform
    public Transform player;

    [Header("Cinemachine 2D Camera")]
    public Cinemachine.CinemachineVirtualCamera cinemachineCamera; // 使用Cinemachine的2D摄像机

    [Header("手电筒旋转速度")]
    public float rotationSpeed = 10f; // 旋转的平滑速度

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = cinemachineCamera.VirtualCameraGameObject.GetComponent<Camera>().ScreenToWorldPoint(mousePos);
        mousePos.z = 0f; 

        // 计算手电筒到鼠标的方向
        Vector3 direction = (mousePos - player.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = transform.eulerAngles.z;
        float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, smoothAngle));
    }
}
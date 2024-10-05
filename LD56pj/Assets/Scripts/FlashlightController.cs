using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [Header("角色 Transform")]
    public Transform player; // 玩家角色的 Transform

    [Header("主摄像机")]
    public Camera mainCamera; // 场景的主摄像机

    [Header("手电筒旋转速度")]
    public float rotationSpeed = 10f; // 旋转的平滑速度

    private void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; 

        // 计算手电筒到鼠标的方向
        Vector3 direction = (mousePos - player.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = transform.eulerAngles.z;
        float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, smoothAngle));
    }
}
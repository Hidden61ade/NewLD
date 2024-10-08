using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatTrigger : MonoBehaviour
{
    public Transform player; // 玩家对象
    public float hearDistance = 10f; // 听到的距离阈值
    public float increaseDistance = 5f; // 加强的距离阈值
    public float maxDistance = 2f; // 最大的距离阈值

    public float hearVolume = 0.2f; // 听到的音量
    public float increaseVolume = 0.5f; // 加强的音量
    public float maxVolume = 1f; // 最大的音量

    private HeartbeatController heartbeatController;
    private int currentVolume;
    private float[] vols = new float[4];
    void Start()
    {
        heartbeatController = HeartbeatController.Instance;
        currentVolume = 0;
        vols[0] = 0;
        vols[1] = hearVolume;
        vols[2] = increaseVolume;
        vols[3] = maxVolume;
    }

    void Update()
    {
        // 计算玩家与怪物的距离
        float distance = Vector3.Distance(player.position, transform.position);
        int targetVolume = 0;

        // 根据距离设置目标音量
        if (distance <= maxDistance)
        {
            targetVolume = 3;
        }
        else if (distance <= increaseDistance)
        {
            targetVolume = 2;
        }
        else if (distance <= hearDistance)
        {
            targetVolume = 1;
        }

        // 只有在音量状态发生变化时才设置音量
        if (targetVolume != currentVolume)
        {
            heartbeatController.SetVolume(vols[targetVolume]);
            currentVolume = targetVolume;
        }
    }

    void OnDrawGizmosSelected()
    {
        // 在Scene视图中绘制距离阈值范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, increaseDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hearDistance);
    }
}
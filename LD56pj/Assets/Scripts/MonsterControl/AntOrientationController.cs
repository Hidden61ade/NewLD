using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntOrientationController : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleOrientation();
    }
    
    public void HandleOrientation()
    {
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // 转换为度
        Debug.DrawRay(transform.position,dir*100);
        // 获取当前旋转角度
        float currentAngle = transform.eulerAngles.z;
        float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, 100 * Time.deltaTime);

        // 应用旋转
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, smoothAngle));
        Debug.Log(smoothAngle);
    }
}

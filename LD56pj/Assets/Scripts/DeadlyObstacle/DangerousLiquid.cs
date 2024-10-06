using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DangerousLiquid : MonoBehaviour
{
    [Header("液体滴下的间隔")]
    public float deltaTime = 3.0f;

    public float timer;

    [Header("可自动获取")] public GameObject DeadlyDrp;
    private void OnEnable()
    {
        timer = deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        DeadlyDrp = GameObject.Find("DeadlyDrop");
        if (DeadlyDrp==null)
        {
            Debug.Log("Drop not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = deltaTime;
            float x = Random.Range(-0.5f, 0.5f);
            Instantiate(DeadlyDrp, transform.position + new Vector3(x, 0, 0),Quaternion.identity);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using Timer = Unity.VisualScripting.Timer;

public class DeadlyDrop : MonoBehaviour
{
    private float timer;
    public float livingTime = 15f;
    private void Start()
    {
        timer = 0;
    }
    
    public float speed = 3;
    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TypeEventSystem.Global.Send<OnPlayerDiedEvents>();
        }
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x,transform.position.y-Time.deltaTime * speed,transform.position.z) ;
        timer += Time.deltaTime;
        if (timer>livingTime)
        {
            Destroy(gameObject);
        }
    }
}

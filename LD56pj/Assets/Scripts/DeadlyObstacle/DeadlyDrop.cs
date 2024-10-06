using System.Collections;
using System.Collections.Generic;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;

public class DeadlyDrop : DeadlyColliderBehavior
{
    public float speed = 3;
    // Start is called before the first frame update
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TypeEventSystem.Global.Send<OnPlayerDiedEvents>();
        }
        else
        {
            Destroy(this);
        }
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x,transform.position.y-Time.deltaTime * speed,transform.position.z) ;
    }
}

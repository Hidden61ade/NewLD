using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class P8_1_Box : MonoSingleton<P8_1_Box>
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        MouseController1.Instance.DigIntoGround();
    }
}

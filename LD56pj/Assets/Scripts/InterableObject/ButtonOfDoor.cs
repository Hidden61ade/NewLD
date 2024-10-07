using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ButtonOfDoor : MonoBehaviour
{
    public bool isOpen = false;
    public GameObject light;

    Light2D light2d;

    // Start is called before the first frame update
    void Start()
    {
        light2d = light.GetComponent<Light2D>();
        light2d.color = new Color(255f / 255f, 33f / 255f, 20f / 255f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            
            light2d.color = new Color(20f / 255f, 255f / 255f, 35f / 255f);
        }
        else
        {
            light2d.color = new Color(255f / 255f, 33f / 255f, 20f / 255f);
        }
    }

    public void Open()
    {
        isOpen = true;
        GetComponent<SpriteRenderer>().flipX = true;
    }
}

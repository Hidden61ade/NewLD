using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ButtonOfPressureSensitiveDoor : MonoBehaviour
{
    public bool isOpen = false;
    public Sprite Off;
    public Sprite On;

    public GameObject light;

    Light2D light2d;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        light2d = light.GetComponent<Light2D>();
        light2d.color = new Color(255f / 255f, 33f / 255f, 20f / 255f);
    }

    private void Update()
    {
        if(isOpen)
        {
            spriteRenderer.sprite = On;
            light2d.color = new Color(20f / 255f, 255f / 255f, 35f / 255f);
        }
        else
        {
            spriteRenderer.sprite = Off;
            light2d.color = new Color(255f / 255f, 33f / 255f, 20f / 255f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            Debug.Log("触发");
            isOpen = true;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            isOpen = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            Debug.Log("出去");
            isOpen = false;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            isOpen = false;
        }
    }

}

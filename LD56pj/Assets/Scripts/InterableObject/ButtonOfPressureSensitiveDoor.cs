using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOfPressureSensitiveDoor : MonoBehaviour
{
    public bool isOpen = false;
    public Sprite Off;
    public Sprite On;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(isOpen)
        {
            spriteRenderer.sprite = On;
        }
        else
        {
            spriteRenderer.sprite = Off;
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

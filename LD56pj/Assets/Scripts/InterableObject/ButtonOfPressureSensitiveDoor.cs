using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ButtonOfPressureSensitiveDoor : MonoBehaviour
{
    bool hasOpend = false;
    public bool isOpen = false;
    public Sprite Off;
    public Sprite On;

    public GameObject mlight;



    Light2D light2d;

    private SpriteRenderer spriteRenderer;
    private PressureSensitiveDoors ressureSensitiveDoors;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        light2d = mlight.GetComponent<Light2D>();
        light2d.color = new Color(255f / 255f, 33f / 255f, 20f / 255f);
        ressureSensitiveDoors = transform.parent.GetComponent<PressureSensitiveDoors>();
    }

    private void Update()
    {
        if (ressureSensitiveDoors.telSon)
        {
            isOpen = true;
        }
        if(isOpen)
        {
            //if(hasOpend){
            //    return;
            //}
            spriteRenderer.sprite = On;
            light2d.color = new Color(20f / 255f, 255f / 255f, 35f / 255f);
            AudioKit.PlaySound("Switch");
            hasOpend = true;
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

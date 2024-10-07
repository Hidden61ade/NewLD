using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class InteractableDoor : MonoBehaviour
{
    private GameObject button;
    private string buttonName = "Button";
    private BoxCollider2D boxCollider;

    public Sprite close;
    public Sprite open;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        button = transform.Find(buttonName).gameObject;
        spriteRenderer.sprite = close;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(button.GetComponent<ButtonOfDoor>().isOpen);
        if (button.GetComponent<ButtonOfDoor>().isOpen)
        {
            boxCollider.enabled = false;
            spriteRenderer.sprite = open;
        }
    }
}

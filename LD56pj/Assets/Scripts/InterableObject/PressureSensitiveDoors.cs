using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class PressureSensitiveDoors : MonoBehaviour
{
    public GameObject[] buttons;

    [SerializeField] private bool isOpen;
    private bool hasOpend = false;
    private BoxCollider2D boxCollider;
    public Sprite close;
    public Sprite open;

    public bool alwaysOpen = false;
    public bool telSon = false;
    private SpriteRenderer spriteRenderer;

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(CheckDoorOpen());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(button.GetComponent<ButtonOfDoor>().isOpen);
        if( hasOpend && alwaysOpen)
        {
            isOpen = true;
            telSon = true;
        }
        if (isOpen )
        {
            if (!hasOpend)
            {
                AudioKit.PlaySound("Switch");
            }
            boxCollider.enabled = false;
            spriteRenderer.sprite = open;
            hasOpend = true;
            
        }
        else
        {
            boxCollider.enabled = true;
            spriteRenderer.sprite = close;
        }
    }
    IEnumerator CheckDoorOpen()
    {
        while (true)
        {
            bool temp = true;
            foreach (var item in buttons)
            {
                temp &= item.GetComponent<ButtonOfPressureSensitiveDoor>().isOpen;
                yield return null;
            }
            this.isOpen = temp;
            
        }
    }
}

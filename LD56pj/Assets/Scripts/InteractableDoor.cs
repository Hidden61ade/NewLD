using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableDoor : MonoBehaviour
{
    private GameObject button;
    private string buttonName = "Botton";
    private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        button = transform.Find(buttonName).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (button.GetComponent<ButtonOfDoor>().isOpen)
        {
            boxCollider.enabled = false;
        }
    }
}

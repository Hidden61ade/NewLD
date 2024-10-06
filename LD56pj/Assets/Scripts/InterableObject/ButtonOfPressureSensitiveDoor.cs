using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOfPressureSensitiveDoor : MonoBehaviour
{
    public bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            Debug.Log("´¥·¢");
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
            Debug.Log("³öÈ¥");
            isOpen = false;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            isOpen = false;
        }
    }

}

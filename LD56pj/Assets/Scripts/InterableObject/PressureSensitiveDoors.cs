using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class PressureSensitiveDoors : MonoBehaviour
{
    public GameObject[] buttons;
    public bool isButton = false;
    [SerializeField] private bool isOpen;
    private BoxCollider2D boxCollider;
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(CheckDoorOpen());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(button.GetComponent<ButtonOfDoor>().isOpen);
        if (isOpen)
        {
            boxCollider.enabled = false;
        }
        else
        {
            boxCollider.enabled = true;
        }
        isButton = buttons[0].GetComponent<ButtonOfPressureSensitiveDoor>().isOpen;
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

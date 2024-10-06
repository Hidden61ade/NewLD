using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PovitTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider != null)
        {
            // 设置新的中心点，相对于物体本身的局部坐标系
            boxCollider.offset = new Vector3(0, 1, 0);
        }
    }
}

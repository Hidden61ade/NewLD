using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayer : MonoBehaviour
{
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Vector3 pos = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pos);
            worldPosition.z = 0;
            Player.transform.position = worldPosition;
        }

        
    }

}

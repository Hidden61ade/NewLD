using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public AntController ant; // Reference to the AntController

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && ant != null)
        {
            ant.StartChasing();
            Debug.Log("Player entered Door Detection Area. Ant is now chasing.");
        }
    }
}
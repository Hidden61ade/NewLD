using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestruction : MonoBehaviour
{
    public Tilemap tilemap; // 引用 Tilemap
    public float destructionDelay = 2f; // 延迟时间

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 hitPosition = other.transform.position;
            Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);

            TileBase hitTile = tilemap.GetTile(tilePosition);

            if (hitTile is BreakableTile)
            {
                StartCoroutine(DestroyTileAfterDelay(tilePosition, destructionDelay));
            }
        }
    }

    IEnumerator DestroyTileAfterDelay(Vector3Int position, float delay)
    {
        yield return new WaitForSeconds(delay);
        tilemap.SetTile(position, null);
    }
}
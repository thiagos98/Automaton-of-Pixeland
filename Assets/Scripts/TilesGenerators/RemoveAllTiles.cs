using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilesGenerators
{
    public class RemoveAllTiles : MonoBehaviour
    {
        private Tilemap tilemap;

        private void Start()
        {
            TryGetComponent(out tilemap);
            RemoveTiles();
        }

        private void RemoveTiles()
        {
            tilemap.ClearAllTiles();
        }
    }
}

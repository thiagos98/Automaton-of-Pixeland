using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

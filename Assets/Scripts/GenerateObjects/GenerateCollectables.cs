using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateCollectables : MonoBehaviour
{
    public GameObject collectable;
    
    public Vector3 position;
    public Tilemap tilemap;
    private float tile_anchor;

    private void Start()
    {
        tile_anchor = tilemap.tileAnchor.x;
        Generate();
    }

    private void Generate()
    {
        Instantiate(collectable, position, Quaternion.identity);
        
    }
    
}    


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell : MonoBehaviour
{
    public Tilemap tilemap;
    public RuleTile tile;
    
    public int minWidth = -16;
    public int maxWidth = 14;
    
    public int minHeight = -8;
    public int maxHeight = -1;

    private void Update()
    {
        for (var i = minWidth; i < maxWidth; i++)
        {
            for (var j = minHeight; j < maxHeight; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }
        
    }
}

public class CellularAutomata : MonoBehaviour
{

}
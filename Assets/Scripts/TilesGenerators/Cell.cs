using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace TilesGenerators
{
    public class Cell : MonoBehaviour
    {
        public Tilemap tilemap;
        public RuleTile tile;

        public const int width = 31;
        public const int height = 7;
    
        public int minWidth = -16;
        public int maxWidth = 14;
    
        public int minHeight = -8;
        public int maxHeight = -1;
        
        public bool[,] cellmap = new bool[width, height];
        public float chanceToStartAlive = 0.75f;

        private void Start()
        {
            InitialiseMap(cellmap);
            
        }

        private void Update()
        {
            ShowMap();
        }

        public void ShowMap()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (cellmap[x, y])
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
            }
        }

        public bool[,] InitialiseMap(bool[,] map)
        {
            tilemap.transform.position = gameObject.transform.localPosition;
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height-1; j++)
                {
                    if (Random.Range(0f, 1f) < chanceToStartAlive)
                    {
                        map[i, j] = true;
                    }
                }
            }

            return map;
        }
    }

    public class CellularAutomata : MonoBehaviour
    {
        // tilemap.SetTile(new Vector3Int(i, j, 0), tile);
    }
}
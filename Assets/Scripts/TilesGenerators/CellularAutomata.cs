using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace TilesGenerators
{
    public class CellularAutomata : MonoBehaviour
    {
        public Tilemap tilemap;
        public RuleTile tile;

        public const int width = 31;
        public const int height = 6;

        // public int minWidth = -16;
        // public int maxWidth = 14;
        //
        // public int minHeight = -8;
        // public int maxHeight = -1;

        public float chanceToStartAlive = 0.75f;
        public int deathLimit = 4;
        public int birthLimit = 4;
        public int numberOfSteps = 5;

        private void Start()
        {
            ExecuteScript();
        }

        public void ExecuteScript()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            bool[,] cellmap = new bool[width, height];
            cellmap = InitialiseMap(cellmap);
            for (int i = 0; i < numberOfSteps; i++)
            {
                cellmap = doSimulationStep(cellmap);
            }
            ShowMap(cellmap);
        }
        
        public void ShowMap(bool[,] cellmap)
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
                for (var j = 0; j < height - 1; j++)
                {
                    if (Random.Range(0f, 1f) < chanceToStartAlive)
                    {
                        map[i, j] = true;
                    }
                }
            }

            return map;
        }

        public bool[,] doSimulationStep(bool[,] oldMap)
        {
            bool[,] newMap = new bool[width, height];
            for (int x  = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int neighbours = countAliveNeighbours(oldMap, x, y);
                    if (oldMap[x, y])
                    {
                        if (neighbours < deathLimit)
                        {
                            newMap[x, y] = false;
                        }
                        else
                        {
                            newMap[x, y] = true;
                        }
                    }
                    else
                    {
                        if (neighbours > birthLimit)
                        {
                            newMap[x, y] = true;
                        }
                        else
                        {
                            newMap[x, y] = false;
                        }
                    }
                }
            }

            return newMap;
        }

        public int countAliveNeighbours(bool[,] map, int x, int y)
        {
            int countAlive = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int neighbourX = x + i;
                    int neighbourY = y + j;
                    if (i == 0 && j == 0)
                    {
                        
                    }
                    else if(neighbourX < 0 || neighbourY < 0 || neighbourX >= width || neighbourY >= height)
                    {
                        countAlive++;
                    }
                    else if (map[neighbourX, neighbourY])
                    {
                        countAlive++;
                    }
                }
            }
            return countAlive;
        }
    }
}
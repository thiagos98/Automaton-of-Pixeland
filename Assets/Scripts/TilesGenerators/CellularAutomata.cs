using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace TilesGenerators
{
    public class CellularAutomata : MonoBehaviour
    {
        public Tilemap tilemap;
        public RuleTile tile;
        public const int Width = 65;
        public const int Height = 34;
        
        // how dense the initial grid is with living cells
        public const float ChanceToStartAlive = 0.4f;
        // number of neighbours that cause a alive cell to become dead
        public int deathLimit = 4;
        // number of neighbours that cause a dead cell to become alive
        public int birthLimit = 4;
        // number of times we perform the simulation step
        public int numberOfSteps = 3;
        
        private void Start()
        {
            tilemap.transform.position = gameObject.transform.localPosition;
            ExecuteScript();
        }

        private void ExecuteScript()
        {
            GenerateMap();
        }

        private void GenerateMap()
        {
            bool[,] cellmap = new bool[Width, Height];
            cellmap = InitialiseMap(cellmap);
            for (int i = 0; i < numberOfSteps; i++)
            {
                cellmap = DoSimulationStep(cellmap);
            }
            DrawMap(cellmap);
        }

        private void DrawMap(bool[,] cellmap)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (cellmap[x, y])
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
            }
        }

        private bool[,] InitialiseMap(bool[,] map, bool resetMap=false)
        {
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height - 1; j++)
                {
                    if (resetMap)
                    {
                        map[i, j] = false;
                    }
                    else if (Random.Range(0f, 1f) < ChanceToStartAlive)
                    {
                        map[i, j] = true;
                    }
                }
            }

            return map;
        }

        private bool[,] DoSimulationStep(bool[,] oldMap)
        {
            bool[,] newMap = new bool[Width, Height];
            for (int x  = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int neighbours = CountAliveNeighbours(oldMap, x, y);
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

        private int CountAliveNeighbours(bool[,] map, int x, int y)
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
                    else if(neighbourX < 0 || neighbourY < 0 || neighbourX >= Width || neighbourY >= Height)
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
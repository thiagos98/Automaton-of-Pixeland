using System;
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
        public const int Height = 35;
        
        public const float ChanceToStartAlive = 0.4f;
        [FormerlySerializedAs("DeathLimit")] public int deathLimit = 4;
        [FormerlySerializedAs("BirthLimit")] public int birthLimit = 3;
        [FormerlySerializedAs("NumberOfSteps")] public int numberOfSteps = 3;

        private void Start()
        {
            ExecuteScript();
        }

        public void ExecuteScript()
        {
            GenerateMap();
        }
        
        private bool VerifyCollision(Vector2 pos)
        {
            return tilemap.GetComponent<TilemapCollider2D>().OverlapPoint(pos);
        }

        public void GenerateMap()
        {
            bool[,] cellmap = new bool[Width, Height];
            cellmap = InitialiseMap(cellmap);
            for (int i = 0; i < numberOfSteps; i++)
            {
                cellmap = doSimulationStep(cellmap);
                ShowMap(cellmap);
            }
        }
        
        public void ShowMap(bool[,] cellmap)
        {
            var playerPosition = FindObjectOfType<Player>().GetComponent<Transform>().position;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (cellmap[x, y] && !VerifyCollision(new Vector3(-15.53f, 1.68f, 0f)))
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
            }
        }

        public bool[,] InitialiseMap(bool[,] map)
        {
            tilemap.transform.position = gameObject.transform.localPosition;
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height - 1; j++)
                {
                    if (Random.Range(0f, 1f) < ChanceToStartAlive)
                    {
                        map[i, j] = true;
                    }
                }
            }

            return map;
        }

        public bool[,] doSimulationStep(bool[,] oldMap)
        {
            bool[,] newMap = new bool[Width, Height];
            for (int x  = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
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
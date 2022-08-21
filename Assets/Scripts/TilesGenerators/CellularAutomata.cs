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
        public const int Height = 35;
        
        public const float ChanceToStartAlive = 0.3f;
        [FormerlySerializedAs("DeathLimit")] public int deathLimit = 3;
        [FormerlySerializedAs("BirthLimit")] public int birthLimit = 3;
        [FormerlySerializedAs("NumberOfSteps")] public int numberOfSteps = 5;

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
            cellmap = InitialiseMap(cellmap, true); // Resetar o mapa com todas as celulas mortas
            cellmap = InitialiseMap(cellmap);
            for (int i = 0; i < numberOfSteps; i++)
            {
                cellmap = DoSimulationStep(cellmap);
            }
            ShowMap(cellmap);
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

        public bool[,] InitialiseMap(bool[,] map, bool resetMap=false)
        {
            tilemap.transform.position = gameObject.transform.localPosition;
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
        
        public bool[,] DoSimulationStep(bool[,] oldMap)
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

        public int CountAliveNeighbours(bool[,] map, int x, int y)
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

        public void Dijkstra(bool[,] map, int x, int y)
        {
            var queue = new Queue<Vector2>();
            var visited = new bool[Width, Height];
            var distance = new int[Width, Height];
            var previous = new Vector2[Width, Height];
            queue.Enqueue(new Vector2(x, y));
            visited[x, y] = true;
            distance[x, y] = 0;
            previous[x, y] = new Vector2(x, y);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        int neighbourX = (int)current.x + i;
                        int neighbourY = (int)current.y + j;
                        if (i == 0 && j == 0)
                        {
                            
                        }
                        else if(neighbourX < 0 || neighbourY < 0 || neighbourX >= Width || neighbourY >= Height)
                        {
                            continue;
                        }
                        else if (map[neighbourX, neighbourY] && !visited[neighbourX, neighbourY])
                        {
                            visited[neighbourX, neighbourY] = true;
                            previous[neighbourX, neighbourY] = current;
                            distance[neighbourX, neighbourY] = distance[(int)current.x, (int)current.y] + 1;
                            queue.Enqueue(new Vector2(neighbourX, neighbourY));
                        }
                    }
                }
            }
        }
    }
}
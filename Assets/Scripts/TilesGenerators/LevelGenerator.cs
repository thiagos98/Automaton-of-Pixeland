using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace TilesGenerators
{
    public class LevelGenerator : MonoBehaviour
    {
        #region CellularAutomataVariables

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

        #endregion

        #region SpawnObjectsVariables

        public List<GameObject> spawnPoolCollectables;
        public List<GameObject> spawnPoolEnemies;
        public GameObject background;

        private SpriteRenderer sr;
        public int[] spawnsCollectableToLevels;
        public int[] spawnsEnemyToLevels;
        public int[] deathLimitToLevelsToLevels;
        public int[] birthLimitToLevels;
        public int[] numberOfStepsToLevels;

        public string[] rawInput;
        private int lenghtGame;

        #endregion

        private void Start()
        {
            tilemap.transform.position = gameObject.transform.localPosition;
            ExecuteScript();
        }

        public void ExecuteScript()
        {
            ConvertJsonToInputData();
            InitializeObjectsPerLevel();
            deathLimit = UpdateVariables(deathLimitToLevelsToLevels, deathLimit);
            birthLimit = UpdateVariables(birthLimitToLevels, birthLimit);
            numberOfSteps = UpdateVariables(numberOfStepsToLevels, numberOfSteps);
            GenerateMap();
            sr = background.GetComponent<SpriteRenderer>();
            Spawn(spawnPoolCollectables, spawnsCollectableToLevels, "Collectable");
            Spawn(spawnPoolEnemies, spawnsEnemyToLevels, "enemy");
        }

        #region CellularAutomataAlgorithm
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

        private bool[,] InitialiseMap(bool[,] map, bool resetMap = false)
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
            for (int x = 0; x < Width; x++)
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
                    else if (neighbourX < 0 || neighbourY < 0 || neighbourX >= Width || neighbourY >= Height)
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
        #endregion
        

        #region General

        private int UpdateVariables(IReadOnlyList<int> valueToLevel, int variable)
        {
            variable = valueToLevel[GameController.instance.GetCurrentLevel()];
            return variable;
        }
        
        private void ConvertJsonToInputData()
        {
            string json = PlayerPrefs.GetString("InputLevel");
            InputData data = JsonUtility.FromJson<InputData>(json);
            rawInput = data.input;
            lenghtGame = rawInput.Length;
            PlayerPrefs.SetInt("LenghtGame", lenghtGame);
        }

        private void InitializeObjectsPerLevel()
        {
            // 0: collectable
            // 1: enemy
            // 2: deathLimit
            // 3: birthLimit
            // 4: numberOfSteps
            spawnsCollectableToLevels = new int[lenghtGame];
            spawnsEnemyToLevels = new int[lenghtGame];
            deathLimitToLevelsToLevels = new int[lenghtGame];
            birthLimitToLevels = new int[lenghtGame];
            numberOfStepsToLevels = new int[lenghtGame];

            for (int i = 0; i < lenghtGame; i++)
            {
                spawnsCollectableToLevels[i] = int.Parse(rawInput[i][0].ToString());
                spawnsEnemyToLevels[i] = int.Parse(rawInput[i][1].ToString());
                deathLimitToLevelsToLevels[i] = int.Parse(rawInput[i][2].ToString());
                birthLimitToLevels[i] = int.Parse(rawInput[i][3].ToString());
                numberOfStepsToLevels[i] = int.Parse(rawInput[i][4].ToString());
            }
        }

        private void DestroyObjects(string tagValue)
        {
            foreach (var obj in GameObject.FindGameObjectsWithTag(tagValue))
            {
                Destroy(obj);
            }
        }

        private bool VerifyCollision(Vector2 pos)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 0.5f);
            foreach (var collider in colliders)
            {
                if (collider.gameObject.CompareTag("Collectable") || collider.gameObject.CompareTag("enemy") ||
                    collider.gameObject.CompareTag("Player"))
                {
                    return true;
                }
            }
            
            return false;
        }

        #endregion

        # region Spawning

        private void Spawn(IReadOnlyList<GameObject> pool, IReadOnlyList<int> spawnToLevels, string tagValue)
        {
            DestroyObjects(tagValue);

            int numberToSpawn = spawnToLevels[GameController.instance.GetCurrentLevel()];
            for (int i = 0; i < numberToSpawn; i++)
            {
                
                var randomItem = Random.Range(0, pool.Count);
                var toSpawn = pool[randomItem];

                var screenPos = GenerateNewPosition();
                var cantInstantiate = VerifyCollision(screenPos);
                if (!cantInstantiate)
                {
                    var newObj = Instantiate(toSpawn, screenPos, toSpawn.transform.rotation);
                    newObj.transform.SetParent(gameObject.transform);
                }
                else
                {
                    screenPos = GenerateNewPosition();
                    cantInstantiate = VerifyCollision(screenPos);
                    while (cantInstantiate)
                    {
                        screenPos = GenerateNewPosition();
                        cantInstantiate = VerifyCollision(screenPos);
                    }

                    var newObj = Instantiate(toSpawn, screenPos, toSpawn.transform.rotation);
                    newObj.transform.SetParent(gameObject.transform);
                }
            }
        }

        private Vector2 GenerateNewPosition()
        {
            var screenX = Random.Range(-16.501f, 16.5f);
            var screenY = Random.Range(-8.5f, 8f);
            return new Vector2(screenX, screenY);
        }

        #endregion
    }
}
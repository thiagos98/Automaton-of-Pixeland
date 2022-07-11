using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class SpawnerObjects : MonoBehaviour
{
    public Tilemap tilemap;
    public List<GameObject> spawnPoolCollectables;
    public List<GameObject> spawnPoolEnemies;
    public GameObject background;

    private SpriteRenderer sr;
    public int[] spawnsCollectableToLevels;
    public int[] spawnsEnemyToLevels;
    public string[] rawInput;
    public int currentLevel;
    private int lenghtGame;
    private void Start()
    {
        sr = background.GetComponent<SpriteRenderer>();
        ConvertJsonToInputData();
        InitializeObjectsPerLevel();
        Spawn(spawnPoolCollectables, spawnsCollectableToLevels, "Collectable");
        Spawn(spawnPoolEnemies, spawnsEnemyToLevels, "enemy");
    }
    
    #region General
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
        spawnsCollectableToLevels = new int[lenghtGame];
        spawnsEnemyToLevels = new int[lenghtGame];
        
        for (int i = 0; i < lenghtGame; i++)
        {
            spawnsCollectableToLevels[i] = int.Parse(rawInput[i][0].ToString());
            spawnsEnemyToLevels[i] = int.Parse(rawInput[i][1].ToString());
        }
    }
    
    private void DestroyObjects(string tag)
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag(tag))
        {
            Destroy(obj);
        }
    }
    
    private bool VerifyCollision(Vector2 pos)
    {
        return tilemap.GetComponent<TilemapCollider2D>().OverlapPoint(pos);
    }
    
    #endregion
    
    # region Spawning
    private void Spawn(IReadOnlyList<GameObject> pool, IReadOnlyList<int> spawnToLevels, string tag)
    {
        DestroyObjects(tag);
        currentLevel = Scenes.GetScene();

        int numberToSpawn = spawnToLevels[currentLevel];

        for (int i = 0; i < numberToSpawn; i++)
        {
            var randomItem = Random.Range(0, pool.Count);
            var toSpawn = pool[randomItem];

            var screenPos = GenerateNewPosition();
            var cantInstantiate = VerifyCollision(screenPos);
            if(!cantInstantiate)
            {
                Instantiate(toSpawn, screenPos, toSpawn.transform.rotation);
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
                Instantiate(toSpawn, screenPos, toSpawn.transform.rotation);
            }
        }
    }

    private Vector2 GenerateNewPosition()
    {
        
        var bounds = sr.bounds;
        float screenX = Random.Range(bounds.min.x, bounds.max.x);
        float screenY = Random.Range(bounds.min.y, bounds.max.y);
        
        return new Vector2(screenX, screenY);
    }
    
    #endregion

}

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
    
    private void Start()
    {
        sr = background.GetComponent<SpriteRenderer>();
        InitializeObjectsPerLevel();
        Spawn(spawnPoolCollectables, spawnsCollectableToLevels, "Collectable");
        Spawn(spawnPoolEnemies, spawnsEnemyToLevels, "enemy");
    }
    
    #region General
    private void InitializeObjectsPerLevel()
    {
        string json = PlayerPrefs.GetString("InputLevel");
        InputData data = JsonUtility.FromJson<InputData>(json);
        rawInput = data.input;

        spawnsCollectableToLevels = new int[rawInput.Length];
        spawnsEnemyToLevels = new int[rawInput.Length];
        
        for (int i = 0; i < rawInput.Length; i++)
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
        int randomItem = 0;
        GameObject toSpawn;
        
        Vector2 screenPos;
        int numberToSpawn = spawnToLevels[SceneManager.GetActiveScene().buildIndex - 1];

        for (int i = 0; i < numberToSpawn; i++)
        {
            randomItem = Random.Range(0, pool.Count);
            toSpawn = pool[randomItem];

            screenPos = GenerateNewPosition();
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

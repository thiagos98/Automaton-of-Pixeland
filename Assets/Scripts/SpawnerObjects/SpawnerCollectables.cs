using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class SpawnerCollectables : MonoBehaviour
{
    public Tilemap tilemap;
    public List<GameObject> spawnPool;
    public GameObject background;

    private SpriteRenderer sr;
    public int[] spawnsToLevels;
    
    private void Start()
    {
        sr = background.GetComponent<SpriteRenderer>();
        SpawnObjects();
    }
    
    

    private void SpawnObjects()
    {
        InitializeNumberCollectiblesPerLevel();
        DestroyObjects();
        int randomItem = 0;
        GameObject toSpawn;
        
        Vector2 screenPos;
        int numberToSpawn = spawnsToLevels[SceneManager.GetActiveScene().buildIndex - 1];

        for (int i = 0; i < numberToSpawn; i++)
        {
            randomItem = Random.Range(0, spawnPool.Count);
            toSpawn = spawnPool[randomItem];

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

    private void DestroyObjects()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("Collectable"))
        {
            Destroy(obj);
        }
    }
    
    private bool VerifyCollision(Vector2 posCollectable)
    {
        return tilemap.GetComponent<TilemapCollider2D>().OverlapPoint(posCollectable);
    }

    
    private void InitializeNumberCollectiblesPerLevel()
    {
        string json = PlayerPrefs.GetString("CollectablesPerLevel");
        Collectables data = JsonUtility.FromJson<Collectables>(json);
        spawnsToLevels = data.numberCollectables;
    }
    
}

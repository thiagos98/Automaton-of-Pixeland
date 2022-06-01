using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class SpawnerCollectables : MonoBehaviour
{
    public Tilemap tilemap;
    
    public int numberToSpawn;
    public List<GameObject> spawnPool;
    [FormerlySerializedAs("quad")] public GameObject background;

    private SpriteRenderer sr;
    
    void Start()
    {
        sr = background.GetComponent<SpriteRenderer>();
        SpawnObjects();
    }

    public void SpawnObjects()
    {
        DestroyObjects();
        int randomItem = 0;
        GameObject toSpawn;
        
        Vector2 screenPos;
        
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
        float screenX, screenY;

        var bounds = sr.bounds;
        screenX = Random.Range(bounds.min.x, bounds.max.x);
        screenY = Random.Range(bounds.min.y, bounds.max.y);
        
        return new Vector2(screenX, screenY);
    }

    private void DestroyObjects()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Collectable"))
        {
            Destroy(o);
        }
    }
    
    private bool VerifyCollision(Vector2 posCollectable)
    {
        var collided = tilemap.GetComponent<TilemapCollider2D>().OverlapPoint(posCollectable);
        return collided;
    }
}

﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class SpawnerObjects : MonoBehaviour
{
    public GameObject cellularAutomataGameObject;
    public Tilemap tilemap;
    public List<GameObject> spawnPoolCollectables;
    public List<GameObject> spawnPoolEnemies;
    public GameObject background;

    private SpriteRenderer sr;
    public int[] spawnsCollectableToLevels;
    public int[] spawnsEnemyToLevels;
    public string[] rawInput;
    private int lenghtGame;

    private void Start()
    {
        tilemap.transform.position = cellularAutomataGameObject.transform.localPosition;
        ExecuteScript();
    }

    public void ExecuteScript()
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
    protected void InitializeObjectsPerLevel()
    {
        spawnsCollectableToLevels = new int[lenghtGame];
        spawnsEnemyToLevels = new int[lenghtGame];
        
        for (int i = 0; i < lenghtGame; i++)
        {
            spawnsCollectableToLevels[i] = int.Parse(rawInput[i][0].ToString());
            spawnsEnemyToLevels[i] = int.Parse(rawInput[i][1].ToString());
        }
    }
    
    private void DestroyObjects(string tagValue)
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag(tagValue))
        {
            Destroy(obj);
        }
    }
    
    protected bool VerifyCollision(Vector2 pos)
    {
        return tilemap.GetComponent<TilemapCollider2D>().OverlapPoint(pos);
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
                    print(cantInstantiate);
                }
                Instantiate(toSpawn, screenPos, toSpawn.transform.rotation);
            }
            
        }
    }

    private Vector2 GenerateNewPosition()
    {
        var boundsTilemap = tilemap.cellBounds;
        var screenX = Random.Range(boundsTilemap.min.x, boundsTilemap.max.x);
        var screenY = Random.Range(boundsTilemap.min.y, boundsTilemap.max.y);
        
        return new Vector2(screenX, screenY);
    }
    
    #endregion

}

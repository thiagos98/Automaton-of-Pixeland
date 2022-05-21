using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    private int FruitScore = 10;
    private GameObject collectedEffect;

    private void Start()
    {
        collectedEffect = Resources.Load("Prefabs/coletavel") as GameObject;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameController.instance.AddScore(FruitScore);
            Instantiate(collectedEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

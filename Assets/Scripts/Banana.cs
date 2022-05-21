using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Banana : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    public GameObject collected;

    [SerializeField] private int bananaScore = 10;
    
    void Start()
    {
        TryGetComponent(out boxCollider);
        TryGetComponent(out spriteRenderer);
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            
            Instantiate(collected, transform.position, quaternion.identity);
            Destroy(gameObject);

            GameController.instance.AddScore(bananaScore);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private GameObject collected;

    [SerializeField] private int bananaScore = 10;
    
    void Start()
    {
        TryGetComponent(out boxCollider);
        TryGetComponent(out spriteRenderer);
        collected = transform.GetChild(0).gameObject;
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            spriteRenderer.enabled = false;
            boxCollider.enabled = false;
            collected.SetActive(true);
            
            GameController.instance.AddScore(bananaScore);
            GameController.instance.UpdateScoreText();
            
            Destroy(gameObject, 0.2f);
        }
    }
}

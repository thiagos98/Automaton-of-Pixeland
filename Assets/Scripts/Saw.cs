using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float speed;
    public float moveTime;
    
    private bool dirRight = true;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        if (dirRight)
        {
            // serra vai para direita
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            // serra vai para esquerda
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        timer += Time.deltaTime;
        
        if(timer > moveTime)
        {
            dirRight = !dirRight;
            timer = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().Die();
        }
    }
}

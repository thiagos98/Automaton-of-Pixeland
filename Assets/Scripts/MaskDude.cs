using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskDude : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public float speed;

    public Transform rightCol;
    public Transform leftCol;

    public Transform headPoint;

    private bool colliding;

    public LayerMask layer;

    private bool playerDestroyed;
    
    void Start()
    {
        TryGetComponent(out rb);
        TryGetComponent(out anim);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);

        colliding = Physics2D.Linecast(rightCol.position, leftCol.position, layer);

        if (colliding)
        {
            transform.localScale = new Vector2(transform.localScale.x*-1f, transform.localScale.y);
            speed *= -1f;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            float height = col.contacts[0].point.y - headPoint.position.y;

            if (height > 0f && !playerDestroyed)
            {
                col.gameObject.GetComponent<Player>().ImpulseForce(5f);
                anim.SetTrigger("die");
                speed = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
                Destroy(gameObject, 0.35f);
            }
            else
            {
                playerDestroyed = true;
                col.gameObject.GetComponent<Player>().Die();
            }
        }
    }
}

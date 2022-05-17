using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float bounceForce = 10f;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out anim);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("jump");
            col.gameObject.GetComponent<Player>().ImpulseForce(bounceForce);
        }
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    
    private Rigidbody2D rig;
    private Animator anim;

    private bool isJumping;
    private bool doubleJump;

    
    private void Start()
    {
        TryGetComponent(out rig);
        TryGetComponent(out anim);
    }
    
    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;
        ActiveAnimation();
    }
    
    private void ActiveAnimation()
    {
        if (Input.GetAxis("Horizontal") > 0f)
        {
            anim.SetBool("run", true);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        if (Input.GetAxis("Horizontal") < 0f)
        {
            anim.SetBool("run", true);
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        if (Input.GetAxis("Horizontal") == 0f)
        {
            anim.SetBool("run", false);
        }
    }
    
    private void ActivateAnimationJump(bool value)
    {
        anim.SetBool("jump", value);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                ActivateAnimationJump(true);
            }
            else
            {
                if (doubleJump)
                {
                    rig.AddForce(new Vector2(0f, JumpForce/2), ForceMode2D.Impulse);
                    doubleJump = false;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // quando personagem colide com o chão
        if (col.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            isJumping = false;
            ActivateAnimationJump(false);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // quando personagem nao esta mais tocando o chao
        if (other.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            isJumping = true;
        }
    }
}
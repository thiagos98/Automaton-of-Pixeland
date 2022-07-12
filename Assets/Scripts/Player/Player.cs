using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    private bool doubleJump;

    private bool isBlowing;

    private bool isJumping;
    
    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer sr;
    
    # region Sound

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource deathSoundEffect;

    # endregion
    
    private void Start()
    {
        isJumping = false;
        doubleJump = true;
        TryGetComponent(out rig);
        TryGetComponent(out anim);
        TryGetComponent(out sr);
    }

    private void Update()
    {
        Move();
        Jump();
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
        if (other.gameObject.layer == LayerMask.NameToLayer("ground")) isJumping = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("fan")) isBlowing = false;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("fan")) isBlowing = true;
    }

    private void Move()
    {
        var movement = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movement * Speed, rig.velocity.y);
        ActiveAnimation(movement);
    }

    private void ActiveAnimation(float movement)
    {
        if (movement > 0f)
        {
            anim.SetBool("run", true);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        if (movement < 0f)
        {
            anim.SetBool("run", true);
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        if (movement == 0f) anim.SetBool("run", false);
    }

    private void ActivateAnimationJump(bool value)
    {
        anim.SetBool("jump", value);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isBlowing)
        {
            if (!isJumping)
            {
                jumpSoundEffect.Play();
                ImpulseForce(JumpForce);
                doubleJump = true;
                ActivateAnimationJump(true);
            }
            else
            {
                if (doubleJump)
                {
                    jumpSoundEffect.Play();
                    ImpulseForce(JumpForce / 2);
                    doubleJump = false;
                }
            }
        }
    }

    public void ImpulseForce(float jumpForce)
    {
        rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    public void Die()
    {
        deathSoundEffect.Play();
        sr.enabled = false;
        GameController.instance.SetGameOver(true);
    }

    public void Live()
    {
        sr.enabled = true;
        transform.position = new Vector3(-6.76f, -3.63f, 0f);
        GameController.instance.SetGameOver(false);
    }
}
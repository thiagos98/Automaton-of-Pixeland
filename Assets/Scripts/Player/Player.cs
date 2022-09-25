using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [FormerlySerializedAs("Speed")] [SerializeField] private float speed;
        [FormerlySerializedAs("JumpForce")] [SerializeField] private float jumpForce;
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
            rig.velocity = new Vector2(movement * speed, rig.velocity.y);
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
                    ImpulseForce(jumpForce);
                    doubleJump = true;
                    ActivateAnimationJump(true);
                }
                else
                {
                    if (doubleJump)
                    {
                        jumpSoundEffect.Play();
                        ImpulseForce(jumpForce / 1.25f);
                        doubleJump = false;
                    }
                }
            }
        }

        public void ImpulseForce(float value)
        {
            rig.AddForce(new Vector2(0f, value), ForceMode2D.Impulse);
        }

        public void Die()
        {
            deathSoundEffect.Play();
            sr.enabled = false;
            GameController.GameController.instance.SetGameOver(true);
        }
    }
}
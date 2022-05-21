using UnityEngine;

public class MaskDude : MonoBehaviour
{
    public float speed;

    public Transform rightCol;
    public Transform leftCol;

    public Transform headPoint;

    public LayerMask layer;
    private Animator anim;

    private bool colliding;

    private bool playerDestroyed;
    private Rigidbody2D rb;

    private void Start()
    {
        TryGetComponent(out rb);
        TryGetComponent(out anim);
    }

    // Update is called once per frame
    private void Update()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);

        colliding = Physics2D.Linecast(rightCol.position, leftCol.position, layer);

        if (colliding)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
            speed *= -1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var height = col.contacts[0].point.y - headPoint.position.y;

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
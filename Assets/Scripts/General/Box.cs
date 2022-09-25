using UnityEngine;

public class Box : MonoBehaviour
{
    public float bounceForce = 10f;
    public bool isUp;

    public int health = 5;
    public GameObject effect;

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (isUp)
                col.gameObject.GetComponent<Player>().ImpulseForce(bounceForce);
            else
                col.gameObject.GetComponent<Player>().ImpulseForce(-bounceForce);

            health -= 1;
            anim.SetTrigger("hit");
        }
    }
}
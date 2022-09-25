using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float bounceForce = 10f;

    private Animator anim;

    // Start is called before the first frame update
    private void Start()
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
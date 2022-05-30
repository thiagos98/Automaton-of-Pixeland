using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallingTime;

    private BoxCollider2D box;

    private TargetJoint2D target;

    // Start is called before the first frame update
    private void Start()
    {
        TryGetComponent(out target);
        TryGetComponent(out box);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) Invoke("Falling", fallingTime);
    }

    private void Falling()
    {
        target.enabled = false;
        box.isTrigger = true;
        Destroy(gameObject, 3f); // destroi o objeto depois de 3 segundos
    }
}
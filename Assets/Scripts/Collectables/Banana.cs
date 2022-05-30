using Unity.Mathematics;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public GameObject collected;

    [SerializeField] private int bananaScore = 10;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        TryGetComponent(out boxCollider);
        TryGetComponent(out spriteRenderer);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Instantiate(collected, transform.position, quaternion.identity);
            Destroy(gameObject);

            GameController.instance.AddScore(bananaScore);
        }
    }
}
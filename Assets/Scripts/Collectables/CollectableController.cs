using UnityEngine;

public class CollectableController : MonoBehaviour
{
    [SerializeField] private AudioSource collectSoundEffect;
    private GameObject collectedEffect;
    private readonly int FruitScore = 10;

    private void Start()
    {
        collectedEffect = Resources.Load("Prefabs/Collectables/coletavel") as GameObject;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Collectable"))
        {
            Destroy(col.gameObject);
            GameController.instance.AddScore(FruitScore);
            var collectedEffectGameObject = Instantiate(collectedEffect, transform.position, Quaternion.identity);
            collectSoundEffect.Play();
            Destroy(collectedEffectGameObject, 1f);
        }
    }
}
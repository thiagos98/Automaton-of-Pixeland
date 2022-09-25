using UnityEngine;

public class Fire : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) col.gameObject.GetComponent<Player.Player>().Die();
    }
}
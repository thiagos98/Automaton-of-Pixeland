using UnityEngine;

public class NextLevelPoint : MonoBehaviour
{ 
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameController.instance.NextLevel();
        }
    }
}
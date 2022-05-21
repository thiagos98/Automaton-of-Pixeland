using UnityEngine;

public class ResetScore : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<GameController>().SetScore(0);
    }
}
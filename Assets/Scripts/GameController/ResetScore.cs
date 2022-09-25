using UnityEngine;

public class ResetScore : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<GameController.GameController>().SetScore(0);
    }
}
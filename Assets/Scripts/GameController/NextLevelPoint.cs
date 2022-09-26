using UnityEngine;

public class NextLevelPoint : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameController.GameController.instance.AddCurrentLevel();
            if (GameController.GameController.instance.GetCurrentLevel() < PlayerPrefs.GetInt("LenghtGame"))
            {
                GameController.GameController.instance.ReloadGame();
            }
            else
            {
                GameController.GameController.instance.SetVictory(true);
                GameController.GameController.instance.SetPauseLevelButton(false);
            }
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NextLevelPoint : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameController.GameController.instance.AddCurrentLevel();
            if (GameController.GameController.instance.GetCurrentLevel() < PlayerPrefs.GetInt("LenghtGame"))
            {
                GameController.GameController.instance.ReloadGame(true);    
            }
            else
            {
                GameController.GameController.instance.SetVictory(true);
            }
        }
    }
}
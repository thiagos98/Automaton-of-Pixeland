using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelPoint : MonoBehaviour
{
    public void RestartLevelPoint()
    {
        gameObject.transform.position = new Vector3(14.8f, 1.68f, 0);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameController.instance.AddCurrentLevel();
            if (GameController.instance.GetCurrentLevel() < PlayerPrefs.GetInt("LenghtGame"))
            {
                GameController.instance.ReloadGame(true);    
            }
            else
            {
                GameController.instance.SetVictory(true);
            }
        }
    }
}
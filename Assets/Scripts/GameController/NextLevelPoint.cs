﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelPoint : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var nextLevel = Scenes.GetScene();
            if (nextLevel < PlayerPrefs.GetInt("LenghtGame"))
            {
                GameController.instance.NextLevel();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0).buildIndex);
            }
        }
    }
}
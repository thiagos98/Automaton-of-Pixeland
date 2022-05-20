using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScore : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<GameController>().SetScore(0);
    }
}

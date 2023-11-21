using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestPuzzle : MonoBehaviour
{
    [SerializeField] private float levelPuzzle;
    private void OnEnable()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().level = levelPuzzle;
    }
}

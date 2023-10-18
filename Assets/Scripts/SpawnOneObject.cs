using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOneObject : MonoBehaviour
{
    [SerializeField] private GameObject strawPrefabs;
    [SerializeField] private float timeSpwan;
    [SerializeField] private Transform[] spwans;
    [SerializeField] private float timeLife;

    private float timeNext;

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        timeNext += Time.deltaTime;
        if (timeNext >= timeSpwan)
        {
            timeNext = 0;
            int straw = Random.Range(0, spwans.Length);
            Instantiate(strawPrefabs, spwans[straw].position, Quaternion.identity);
        }
    }
}

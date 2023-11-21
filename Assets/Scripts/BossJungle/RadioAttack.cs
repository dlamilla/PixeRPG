using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioAttack : MonoBehaviour
{
    private Transform bossForest;
    private void Start()
    {
        bossForest = GameObject.FindWithTag("JefeSelva").transform;
    }

    private void Update()
    {
        transform.position = new Vector3(bossForest.transform.position.x, transform.position.y, transform.position.z);
    }
}


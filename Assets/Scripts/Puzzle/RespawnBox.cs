using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBox : MonoBehaviour
{
    [SerializeField] public Transform box;
    [SerializeField] public Vector3 boxPosition;

    private void OnEnable()
    {
        boxPosition = box.transform.position;
    }


    public void RestartBox()
    {

        box.transform.position = boxPosition;
        
    }
}

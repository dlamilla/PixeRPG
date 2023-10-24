using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    [Header("CheckPoint")]
    [SerializeField] public Vector3 checkPoint;
    [SerializeField] private Transform pointRespawn;

    private void OnEnable()
    {
        checkPoint = pointRespawn.transform.position;
    }
}

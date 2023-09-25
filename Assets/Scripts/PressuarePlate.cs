using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressuarePlate : MonoBehaviour
{
    [SerializeField] private GameObject bridge;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Box1")
        {
            Debug.Log("Piso caja");
            bridge.SetActive(true);
        }
    }
}

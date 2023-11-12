using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressuarePlate : MonoBehaviour
{
    [SerializeField] private GameObject bridge;
    [SerializeField] private GameObject bridgeStop;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Box1")
        {
            bridge.SetActive(true);
            bridgeStop.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Box1")
        {
            bridge.SetActive(false);
            bridgeStop.SetActive(true);
        }
    }
}

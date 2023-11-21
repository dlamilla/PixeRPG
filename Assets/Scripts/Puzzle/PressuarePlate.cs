using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressuarePlate : MonoBehaviour
{
    [SerializeField] private GameObject bridge;
    [SerializeField] private GameObject bridgeStop;
    [SerializeField] private GameObject waterDied;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Box")
        {
            bridge.SetActive(true);
            bridgeStop.SetActive(false);
            waterDied.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Box")
        {
            bridge.SetActive(false);
            bridgeStop.SetActive(true);
            waterDied.SetActive(true);
        }
    }
}

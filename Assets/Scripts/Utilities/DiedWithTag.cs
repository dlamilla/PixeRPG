using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiedWithTag : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().ChangeLife();
            collision.GetComponent<Player>().transform.position = GameObject.FindGameObjectWithTag("Respawn").GetComponent<CheckPointController>().checkPoint;
        }
    }
}

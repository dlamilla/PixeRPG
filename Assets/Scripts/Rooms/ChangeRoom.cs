using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    [SerializeField] private int roomDestination;
    [SerializeField] private int direction;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("RoomsManager").GetComponent<RoomsManager>().OnRoom(roomDestination);

            
        }
    }
}

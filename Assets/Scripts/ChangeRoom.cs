using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    [SerializeField] private int roomDestination;
    [SerializeField] private int direction;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("RoomsManager").GetComponent<RoomsManager>().OnRoom(roomDestination);

            //switch (direction)
            //{
            //    case 0:
            //        GameObject.Find("Player").transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3.03f, 0f);  //Arriba
            //        Debug.Log(transform.position);
            //        break;
            //    case 1:
            //        GameObject.Find("Player").transform.position = new Vector3(gameObject.transform.position.x + 6.83f, gameObject.transform.position.y, 0f); //Derecha
            //        break;
            //    case 2:
            //        GameObject.Find("Player").transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 3.03f, 0f); //Abajo
            //        break;
            //    case 3:
            //        GameObject.Find("Player").transform.position = new Vector3(gameObject.transform.position.x - 6.83f, gameObject.transform.position.y, 0f); //Izquierda
            //        break;
            //}

            print("Toco");
        }
    }
}

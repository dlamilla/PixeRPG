using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    [SerializeField] private int roomDestination;
    private GameObject _transitionAnim;
    private void Start()
    {

        _transitionAnim = GameObject.Find("TransitionLevel");
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    //GameObject.Find("RoomsManager").GetComponent<RoomsManager>().OnRoom(roomDestination);
        //    GameObject.Find("RoomsManager").GetComponent<RoomsManager>().ChangeRoom(roomDestination);

        //}
        if (collision.gameObject.tag == "Player")
        {
            //GameObject.Find("RoomsManager").GetComponent<RoomsManager>().OnRoom(roomDestination);
            //collision.gameObject.GetComponent<Player>().isESCOpen = true;
            StartCoroutine(ChangeSceneTransition());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //GameObject.Find("RoomsManager").GetComponent<RoomsManager>().OnRoom(roomDestination);
            collision.gameObject.GetComponent<Player>().isESCOpen = true;
            StartCoroutine(ChangeSceneTransition());
        }
    }

    IEnumerator ChangeSceneTransition()
    {
        if (_transitionAnim != null)
        {
            _transitionAnim.GetComponent<AnimTransition>().StartAnimationTransition();
        }
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("RoomsManager").GetComponent<RoomsManager>().ChangeRoom(roomDestination);
    }
}

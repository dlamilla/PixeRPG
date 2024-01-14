using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    [SerializeField] private int roomDestination;
    [SerializeField] private GameObject _objFade;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _objFade.SetActive(true);
            StartCoroutine(ChangeSceneTransition());
            
        }
    }


    IEnumerator ChangeSceneTransition()
    {
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("RoomsManager").GetComponent<RoomsManager>().ChangeRoom(roomDestination);
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    [SerializeField] public int currentRoom = 0;
    [SerializeField] public List<GameObject> Rooms;
    [SerializeField] private Transform cameraGlobal;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void OnRoom(int newRoom)
    {
        StartCoroutine(WaitRoom(newRoom));   
    }

    public IEnumerator WaitRoom(int newRoom)
    {
        float t = 0;
        Rooms[newRoom].SetActive(true);

        while (t < 1f)
        {
            t += Time.deltaTime;
            cameraGlobal.position = Vector3.Lerp(new Vector3(Rooms[currentRoom].transform.position.x, Rooms[currentRoom].transform.position.y, -10), new Vector3(Rooms[newRoom].transform.position.x, Rooms[newRoom].transform.position.y, -10), t);
            player.GetComponent<Player>().isESCOpen = true;
            yield return null;
        }

        player.GetComponent<Player>().isESCOpen = false;

        Rooms[currentRoom].SetActive(false);
        currentRoom = newRoom;
    }
}

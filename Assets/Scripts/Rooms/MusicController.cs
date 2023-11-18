using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    //Bosses 9 19 49 99
    [Header("Ambient Sound")]
    [SerializeField] private GameObject caveMusic;
    [SerializeField] private GameObject desertMusic;
    [SerializeField] private GameObject forestMusic;
    [SerializeField] private GameObject mountainMusic;

    [Header("Bosses Sound")]
    [SerializeField] private GameObject musicBoss1;
    [SerializeField] private GameObject musicBoss2;
    [SerializeField] private GameObject musicBoss3;
    [SerializeField] private GameObject musicBoss4;

    // Update is called once per frame
    void Update()
    {
        int room = GameObject.Find("RoomsManager").GetComponent<RoomsManager>().currentRoom;

        if ((room >= 0 && room <= 8) || (room >= 10 && room <= 18) || (room >= 20 && room <= 23))
        {
            Debug.Log("Room primera parte");
            caveMusic.SetActive(true);
            desertMusic.SetActive(false);
            forestMusic.SetActive(false);
            mountainMusic.SetActive(false);

            musicBoss1.SetActive(false);
            musicBoss2.SetActive(false);
            musicBoss3.SetActive(false);
            musicBoss4.SetActive(false);
        }
        else if (room == 9)
        {
            Debug.Log("Room Boss 1");
            caveMusic.SetActive(false);
            desertMusic.SetActive(false);
            forestMusic.SetActive(false);
            mountainMusic.SetActive(false);

            musicBoss1.SetActive(true);
            musicBoss2.SetActive(false);
            musicBoss3.SetActive(false);
            musicBoss4.SetActive(false);
        }
        else if (room == 19)
        {
            Debug.Log("Room Boss 2");
            caveMusic.SetActive(false);
            desertMusic.SetActive(false);
            forestMusic.SetActive(false);
            mountainMusic.SetActive(false);

            musicBoss1.SetActive(false);
            musicBoss2.SetActive(true);
            musicBoss3.SetActive(false);
            musicBoss4.SetActive(false);
        }
        else if (room >= 24 && room <= 48)
        {
            Debug.Log("Room segunda parte");
            caveMusic.SetActive(false);
            desertMusic.SetActive(true);
            forestMusic.SetActive(false);
            mountainMusic.SetActive(false);

            musicBoss1.SetActive(false);
            musicBoss2.SetActive(false);
            musicBoss3.SetActive(false);
            musicBoss4.SetActive(false);
        }
        else if (room == 49)
        {
            Debug.Log("Room Boss 3");
            caveMusic.SetActive(false);
            desertMusic.SetActive(false);
            forestMusic.SetActive(false);
            mountainMusic.SetActive(false);

            musicBoss1.SetActive(false);
            musicBoss2.SetActive(false);
            musicBoss3.SetActive(true);
            musicBoss4.SetActive(false);
        }
        else if (room >= 50 && room <= 74)
        {
            Debug.Log("Room tercera parte");
            caveMusic.SetActive(false);
            desertMusic.SetActive(false);
            forestMusic.SetActive(true);
            mountainMusic.SetActive(false);

            musicBoss1.SetActive(false);
            musicBoss2.SetActive(false);
            musicBoss3.SetActive(false);
            musicBoss4.SetActive(false);
        }
        else if (room >= 75 && room <= 98)
        {
            Debug.Log("Room cuarta parte");
            caveMusic.SetActive(false);
            desertMusic.SetActive(false);
            forestMusic.SetActive(false);
            mountainMusic.SetActive(true);

            musicBoss1.SetActive(false);
            musicBoss2.SetActive(false);
            musicBoss3.SetActive(false);
            musicBoss4.SetActive(false);
        }
        else if (room == 99)
        {
            Debug.Log("Room Boss 4");
            caveMusic.SetActive(false);
            desertMusic.SetActive(false);
            forestMusic.SetActive(false);
            mountainMusic.SetActive(false);

            musicBoss1.SetActive(false);
            musicBoss2.SetActive(false);
            musicBoss3.SetActive(false);
            musicBoss4.SetActive(true);
        }
    }
}

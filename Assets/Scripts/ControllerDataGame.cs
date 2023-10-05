using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ControllerDataGame : MonoBehaviour
{
    private GameObject player;
    private GameObject room;
    public string saveFile;
    public DataPlayer dataPlayer = new DataPlayer();
    [SerializeField] private float timeCurrent;
    private bool isPlayerInRange;

    private void Awake()
    {
        saveFile = Application.dataPath + "/dataGameRPG.json";
        player = GameObject.FindGameObjectWithTag("Player");
        room = GameObject.Find("RoomsManager");
        LoadData();
    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.F))
        //{
        //    timeCurrent += Time.deltaTime;
        //    if (timeCurrent >= 5)
        //    {
        //        Debug.Log("Estas tocandooooo");
        //        LoadData();
        //        timeCurrent = 0;
        //    }
            
        //}
        if (Input.GetKey(KeyCode.F) && isPlayerInRange)
        {
            Debug.Log("Esta tocando 1 vez");
            SaveData();
        }
    }

    //Metodo para cargar los datos del Player
    private void LoadData()
    {
        if (File.Exists(saveFile))
        {
            string arch = File.ReadAllText(saveFile);
            dataPlayer = JsonUtility.FromJson<DataPlayer>(arch);

            player.transform.position = dataPlayer.posicionPlayer;
            player.GetComponent<Player>().health = dataPlayer.healthPlayer;
            player.GetComponent<Player>().exp = dataPlayer.expPlayer;
            player.GetComponent<Player>().level = dataPlayer.levelPlayer;
            room.GetComponent<RoomsManager>().OnRoom(dataPlayer.roomCurrent);

        }
        else
        {
            Debug.Log("El archivo no existe");
        }
    }

    //Metodo para guardar datos
    private void SaveData()
    {
        DataPlayer newData = new DataPlayer()
        {
            posicionPlayer = player.transform.position,
            expPlayer = player.GetComponent<Player>().exp,
            healthPlayer = player.GetComponent<Player>().health,
            levelPlayer = player.GetComponent<Player>().level,
            roomCurrent = room.GetComponent<RoomsManager>().currentRoom
        };

        string charJSON = JsonUtility.ToJson(newData);
        File.WriteAllText(saveFile, charJSON);
        Debug.Log("Archivo guardado");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            //dialogueMark.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            //dialogueMark.SetActive(false);
        }
    }
}

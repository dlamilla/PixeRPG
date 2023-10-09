using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ControllerDataGame : MonoBehaviour
{
    private GameObject player;
    private GameObject room;
    private GameObject bar;
    public string saveFile;
    public DataPlayer dataPlayer = new DataPlayer();
    [SerializeField] private Transform cameraGlobal;
    //private bool isPlayerInRange;

    private void Awake()
    {
        saveFile = Application.dataPath + "/dataGameRPG.json";
        player = GameObject.FindGameObjectWithTag("Player");
        room = GameObject.Find("RoomsManager");
        bar = GameObject.Find("HealthBar");
        //LoadData();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            Debug.Log("Cargar datos");
            LoadData();

        }
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("Guardar datos");
            SaveData();
        }
    }

    //Metodo para cargar los datos del Player
    public void LoadData()
    {
        if (File.Exists(saveFile))
        {
            string arch = File.ReadAllText(saveFile);
            dataPlayer = JsonUtility.FromJson<DataPlayer>(arch);

            player.transform.position = dataPlayer.posicionPlayer;
            player.GetComponent<Player>().health = dataPlayer.healthPlayer;
            player.GetComponent<Player>().exp = dataPlayer.expPlayer;
            player.GetComponent<Player>().level = dataPlayer.levelPlayer;
            cameraGlobal.transform.position = dataPlayer.posicionCamera;
            ChangeRoom(dataPlayer.roomCurrent);
            bar.GetComponent<HealthBar>().UpdateHealthBar(dataPlayer.healthMaxPlayer,dataPlayer.healthPlayer);

        }
        else
        {
            Debug.Log("El archivo no existe");
        }
    }

    //Metodo para guardar datos
    public void SaveData()
    {
        DataPlayer newData = new DataPlayer()
        {
            posicionPlayer = player.transform.position,
            expPlayer = player.GetComponent<Player>().exp,
            healthPlayer = player.GetComponent<Player>().health,
            healthMaxPlayer = player.GetComponent<Player>().hpPlayerMax,
            levelPlayer = player.GetComponent<Player>().level,
            roomCurrent = room.GetComponent<RoomsManager>().currentRoom,
            posicionCamera = cameraGlobal.transform.position
        };

        string charJSON = JsonUtility.ToJson(newData);
        File.WriteAllText(saveFile, charJSON);
        Debug.Log("Archivo guardado");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //isPlayerInRange = true;
            //dialogueMark.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //isPlayerInRange = false;
            //dialogueMark.SetActive(false);
        }
    }

    public void ChangeRoom(int roomNew)
    {
        Debug.Log(roomNew);
        GameObject.Find("RoomsManager").GetComponent<RoomsManager>().OnRoom(roomNew);
    }
}

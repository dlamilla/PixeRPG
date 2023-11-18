using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class ControllerDataGame : MonoBehaviour
{
    private GameObject player;
    private GameObject room;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject healthBar1;
    [SerializeField] private GameObject lifeBar;
    public string saveFile;
    public DataPlayer dataPlayer = new DataPlayer();
    [SerializeField] private Transform cameraGlobal;
    [SerializeField] private GameObject canvasCargar;
    [Header("Botones")]
    [SerializeField] private GameObject textCargar;
    [SerializeField] private GameObject buttonSi;
    [SerializeField] private GameObject buttonNo;
    [SerializeField] private GameObject buttonReanudar;
    [SerializeField] private GameObject buttonSaveExit;
    [SerializeField] private GameObject buttonExit;
    //private bool isPlayerInRange;

    private void Awake()
    {
        saveFile = Application.dataPath + "/dataPlayer.json";
        player = GameObject.FindGameObjectWithTag("Player");
        room = GameObject.Find("RoomsManager");
        //LoadData();
    }

    private void Start()
    {
        if (File.Exists(saveFile))
        {
            canvasCargar.SetActive(true);
            textCargar.SetActive(true);
            buttonSi.SetActive(true);
            buttonNo.SetActive(true);
            buttonReanudar.SetActive(false);
            buttonSaveExit.SetActive(false);
            buttonExit.SetActive(false);
            StartCoroutine(TimeWait());
        }
    }

    private void Update()
    {
        
    }

    //Metodo para cargar los datos del Player
    public void LoadData()
    {
        if (File.Exists(saveFile))
        {
            string arch = File.ReadAllText(saveFile);
            dataPlayer = JsonUtility.FromJson<DataPlayer>(arch);

            
            player.GetComponent<Player>().health = dataPlayer.healthPlayer;
            player.GetComponent<Player>().hpPlayerMax = dataPlayer.healthMaxPlayer;
            player.GetComponent<Player>().exp = dataPlayer.expPlayer;
            player.GetComponent<Player>().level = dataPlayer.levelPlayer;
            cameraGlobal.transform.position = dataPlayer.posicionCamera;
            ChangeRoom(dataPlayer.roomCurrent);
            healthBar.GetComponent<HealthBar>().UpdateHealthBar(dataPlayer.healthMaxPlayer, dataPlayer.healthPlayer);
            healthBar1.GetComponent<HealthBar2>().UpdateHealthBar(dataPlayer.healthMaxPlayer, dataPlayer.healthPlayer);
            player.GetComponent<Player>().life = dataPlayer.lifePlayer;
            player.GetComponent<Player>().lifeMax = dataPlayer.lifeMax;
            lifeBar.GetComponent<LifeBar>().UpdateLifeBar(dataPlayer.lifeMax, dataPlayer.lifePlayer);
            player.GetComponent<Player>().dañoGolpe = dataPlayer.damage;
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

            expPlayer = player.GetComponent<Player>().exp,
            healthPlayer = player.GetComponent<Player>().health,
            healthMaxPlayer = player.GetComponent<Player>().hpPlayerMax,
            levelPlayer = player.GetComponent<Player>().level,
            roomCurrent = room.GetComponent<RoomsManager>().currentRoom,
            posicionCamera = cameraGlobal.transform.position,
            lifePlayer = player.GetComponent<Player>().life,
            lifeMax = player.GetComponent<Player>().lifeMax,
            damage = player.GetComponent<Player>().dañoGolpe
        };

        string charJSON = JsonUtility.ToJson(newData);
        File.WriteAllText(saveFile, charJSON);
    }

    public void ChangeRoom(int roomNew)
    {
        GameObject[] roomGame = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject obj in roomGame)
        {
            obj.gameObject.SetActive(false);
        }
        GameObject.Find("RoomsManager").GetComponent<RoomsManager>().Rooms[roomNew].SetActive(true);
        GameObject.Find("RoomsManager").GetComponent<RoomsManager>().currentRoom = roomNew;
    }

    public void SalirYGuardar()
    {
        canvasCargar.SetActive(false);
        SaveData();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void SalirGame()
    {
        canvasCargar.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Salir()
    {
        canvasCargar.SetActive(false);
        Time.timeScale = 1f;

    }

    public void Cargar()
    {
        canvasCargar.SetActive(false);
        LoadData();
        Time.timeScale = 1f;
    }

    IEnumerator TimeWait()
    {
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0f;
    }

    public void SalirYReiniciar()
    {
        if (File.Exists(saveFile))
        {
            File.Delete(saveFile);
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }
}

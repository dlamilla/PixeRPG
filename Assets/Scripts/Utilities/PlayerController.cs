using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("TextUI")]
    [SerializeField] private TMP_Text textUI;
    [SerializeField] private GameObject UI_Panel;
    [SerializeField] private GameObject canvasPause;
    [SerializeField] private GameObject textCargar; 
    [SerializeField] private GameObject buttonSi;
    [SerializeField] private GameObject buttonNo;
    [SerializeField] private GameObject buttonReanudar;
    [SerializeField] private GameObject buttonSaveExit;
    [SerializeField] private GameObject buttonExit;
    [SerializeField] private GameObject diedCanvas;
    [SerializeField] private GameObject canvasFinal;
    [SerializeField] private GameObject diedYes;
    [SerializeField] private GameObject exitGame;

    private PlayerInput playerInput;
    private float _levelPlayer;
    private float _healthPlayer;
    private bool _receiveDamage;
    private bool _isPause;
    private bool _isESCOpen;
    private bool _canDash;
    private int cont;
    private float _x, _y;
    Player _objPlayer;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        _objPlayer = GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_objPlayer != null)
        {
            _levelPlayer = _objPlayer.level;
            _healthPlayer = _objPlayer.giveHealth;
            _receiveDamage = _objPlayer.isReceiveDamage;
            _isPause = _objPlayer.isPause;
            _isESCOpen = _objPlayer.isESCOpen;
            _canDash = _objPlayer.canDash;
            _x = _objPlayer.x;
            _y = _objPlayer.y;
        }
    }

    //Con la tecla F en teclado y Circulo en mando para interactuar con los letreros
    public void Talking(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            if (GameObject.Find("Sign").activeInHierarchy == true)
            {
                string typeController = playerInput.currentControlScheme;
                GameObject.Find("Sign").GetComponent<DialogueNPC>().StartTalking(typeController);
            }
            else
            {
                Debug.Log("No hay cartel cerca");
            }

        }
    }

    //Con la tecla E en teclado y triangulo en mando para curarse
    public void HealthPlayer(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && _levelPlayer >= 0)
        {
            _objPlayer.GiveHeath(_healthPlayer);
        }
    }

    //Con la tecla K en teclado y Cuadrado en mando para atacar
    public void DamagePlayer(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && _levelPlayer >= 0 && !_receiveDamage)
        {
            _objPlayer.Damage();
        }
    }

    //Con la R en teclado y L1 en mando las cajas regresan a su sitio
    public void RestartLevel(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Box");
            foreach (GameObject item in obj)
            {
                item.GetComponent<RespawnBox>().RestartBox();
            }
        }
    }

    //Con la tecla ESC en teclado y Start en mando para interactuar con los letreros
    public void PauseGame(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && !_isESCOpen)
        {
            cont += 1;
            if (cont == 1)
            {
                _isPause = true;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(buttonReanudar);
                canvasPause.SetActive(true);
                Time.timeScale = 0f;
                textCargar.SetActive(false);
                buttonSi.SetActive(false);
                buttonNo.SetActive(false);
                buttonReanudar.SetActive(true);
                buttonSaveExit.SetActive(true);
                buttonExit.SetActive(true);
            }
            else
            {
                _isPause = false;
                cont = 0;
                canvasPause.SetActive(false);
                Time.timeScale = 1f;
            }

        }
    }

    //Con la tecla Q en teclado y X en mando para dashear
    public void DashPlayer(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && _canDash && _levelPlayer >= 2 && (_x != 0 || _y != 0))
        {
            StartCoroutine(_objPlayer.Dash());
        }
    }

    //Detecta la reconexion del mando
    public void ControlsReconneted(PlayerInput a)
    {
        string b = a.currentControlScheme;
        if (b == "Gamepad")
        {
            StartCoroutine(ChangeControllerReconected());
        }
    }

    IEnumerator ChangeControllerReconected()
    {
        UI_Panel.SetActive(true);
        textUI.text = "Mando reconectado. Modo de juego configurado para mando.";
        yield return new WaitForSeconds(1.5f);
        UI_Panel.SetActive(false);
    }

    //Detecta cuando mando se desconecta y cambia a teclado por defecto
    public void ControlsLost(PlayerInput a)
    {
        string b = a.currentControlScheme;
        if (b == "Gamepad")
        {
            StartCoroutine(ChangeControllerLost());
        }
    }

    IEnumerator ChangeControllerLost()
    {
        UI_Panel.SetActive(true);
        textUI.text = "Mando desconectado. Modo de juego configurado para teclado.";
        yield return new WaitForSeconds(1.5f);
        UI_Panel.SetActive(false);
    }

    //Detecta que dispositivo esta usando Teclado o Mando
    public void ControlsChanged(PlayerInput a)
    {
        string b = a.currentControlScheme;
        if (b == "Gamepad")
        {
            StartCoroutine(ChangeControllerGamepad());
        }
        else if (b == "Keyboard")
        {
            StartCoroutine(ChangeControllerKeyboard());
        }
    }

    IEnumerator ChangeControllerGamepad()
    {
        UI_Panel.SetActive(true);
        textUI.text = "Modo de juego configurado para mando.";
        yield return new WaitForSeconds(1.5f);
        UI_Panel.SetActive(false);
    }

    IEnumerator ChangeControllerKeyboard()
    {
        UI_Panel.SetActive(true);
        textUI.text = "Modo de juego configurado para teclado.";
        yield return new WaitForSeconds(1.5f);
        UI_Panel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InteractUser : MonoBehaviour
{
    [SerializeField] private int idScene;
    [SerializeField] private GameObject buttonKeyboard;
    [SerializeField] private GameObject buttonGamepad;
    private PlayerInput playerInput;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        string typeController = playerInput.currentControlScheme;
        if (typeController == "Keyboard")
        {
            buttonKeyboard.SetActive(true);
            buttonGamepad.SetActive(false);
        }
        else if (typeController == "Gamepad")
        {
            buttonKeyboard.SetActive(false);
            buttonGamepad.SetActive(true);
        }
    }

    public void InteractUI(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            SceneManager.LoadScene(idScene);
        }
    }
}

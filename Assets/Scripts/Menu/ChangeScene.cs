using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private float timeWait;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void CargarNivel(int nivel)
    {
        StartCoroutine(Cargar(nivel));
    }

    public IEnumerator Cargar(int sceneIndex)
    {
        anim.SetTrigger("Entrada");
        yield return new WaitForSeconds(timeWait);
        SceneManager.LoadScene(sceneIndex);
    }

    public void Salir()
    {
        Application.Quit();
    }
}

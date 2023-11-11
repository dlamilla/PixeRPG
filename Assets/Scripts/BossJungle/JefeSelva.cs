using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class JefeSelva : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float hpEnemy;
    [SerializeField] private float normalSpeed; // Velocidad predeterminada de mi enemigo
    [SerializeField] private float increasedSpeed; // Uso esta velocidad para medir a cuanta velocidad va regresar a la lanza
    private ChangeAnimation changeDirections;
    private Animator anim;
    private float currentSpeed;
    private Rigidbody2D rb;
    private Transform player;
    private BoxCollider2D player1;

    [Header("AtaqueLanza")]
    private Transform lanza;
    private bool isFollowingPlayer = true;
    private bool isLanzaDetached = false;
    private float detachTime = 5.0f;
    private float returnToLanzaTime = 5.0f;
    private float detachStartTime = -1f;
    private Vector2 lanzaPosition;
    private Transform jefeSelva; 

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        lanza = transform.Find("Lanza");

        player1 = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
        changeDirections = GetComponent<ChangeAnimation>();

        jefeSelva = GameObject.FindWithTag("JefeSelva").transform;

        currentSpeed = normalSpeed;
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            Vector2 moveDirection = (player.position - transform.position).normalized;

            changeDirections.changeAnim(moveDirection);
            anim.SetBool("enCamino", true);

            // Primero en esta etapa, mi enemigo sigue a mi jugador con la velocidad normal que yo le he dado
            transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
        }
        else if (isLanzaDetached)
        {
            // Segundo, hago llamar a esta linea de codigo para que mi enemigo regrese a la lanza tras los segundos 5 segundos  
            transform.position = Vector2.MoveTowards(transform.position, lanzaPosition, increasedSpeed * Time.deltaTime);

            // Cuando el enemigo llega a la posición de la lanza, vincula la lanza a JefeSelva (Nombre con el que tengo a mi enemigo y tageado tmb), y cambia la velocidad a la que esta predeterminada, no a la acelerada
            if (Vector2.Distance(transform.position, lanzaPosition) < 0.1f)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                isFollowingPlayer = true;
                Debug.Log("SeguirPlayer");
                lanza.parent = jefeSelva;
                currentSpeed = normalSpeed;
            }
        }
    }

    void LateUpdate()
    {
        if (!isLanzaDetached && Time.time >= detachTime)
        {
            // En este punto tras haber hecho mi condicion quiero que cuando pasen los primeros 5 segundos, la lanza se va a desvincular de mi enemigo
            GetComponent<SpriteRenderer>().color = Color.red;
            lanza.parent = null;
            isLanzaDetached = true;
            lanzaPosition = lanza.position;
            detachStartTime = Time.time;
            // Cambia la velocidad a la velocidad aumentada que le he dado para cuando se desvincula de la lanza
            currentSpeed = increasedSpeed;
        }
        else if (isLanzaDetached && isFollowingPlayer && Time.time >= detachStartTime + returnToLanzaTime)
        {
            isFollowingPlayer = false;
        }

        // Llamo a esta condicion solo cuando mi lanza se haya vinculado a mi enemigo (Osea que se haga hijo de mi enemigo xd), si eso ocurre, que el enemigo vuelva a seguir a mi jugador 
        if (lanza.parent == jefeSelva)
        {
            isFollowingPlayer = true;
        }
    }
}

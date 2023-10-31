using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JefeSelva : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float hpEnemy;
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Transform player;
    private Transform lanza;
    private bool isFollowingPlayer = true;
    private bool isLanzaDetached = false;
    private float detachTime = 5.0f;
    private float returnToLanzaTime = 5.0f;
    private float detachStartTime = -1f;
    private Vector2 lanzaPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        lanza = transform.Find("Lanza");

        // Primero aseguro que mi enemigo seguira a mi jugador
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            // El enemigo sigue al jugador por los primeros 5 segundos - parte prueba
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (isLanzaDetached)
        {
            // Mi enemigo se dirigira a la posicion de la lanza despues de otros 5 segundos 
            transform.position = Vector2.MoveTowards(transform.position, lanzaPosition, speed * Time.deltaTime);

            // Esta concidion de abajo esta bajo medida de prueba, no funciona pero lo necesitare
            if (Vector2.Distance(transform.position, lanzaPosition) < 0.1f)
            {
                isFollowingPlayer = true;
            }
        }
    }

    void LateUpdate()
    {
        if (!isLanzaDetached && Time.time >= detachTime)
        {
            // Con esta parte, cuando pasen los primeros 5 segundos, la lanza se va desvincular de mi enemigo
            lanza.parent = null;
            isLanzaDetached = true;
            lanzaPosition = lanza.position;
            detachStartTime = Time.time;
        }
        else if (isLanzaDetached && isFollowingPlayer && Time.time >= detachStartTime + returnToLanzaTime)
        {
            isFollowingPlayer = false;
        }
    }
}

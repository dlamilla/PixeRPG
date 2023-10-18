using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilVeneno : MonoBehaviour
{
    public float speed = 10f; // Velocidad del proyectil
    public float hitDamage = 10f; // Daño del proyectil
    public GameObject destructionEffectPrefab; // Prefab del efecto de destrucción
    public float destructionDelay = 10.0f; // Tiempo de vida del efecto de destrucción

    private Transform player;
    private Vector2 target;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = player.position;
    }

    void Update()
    {
        // Mueve el proyectil hacia el jugador
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if ((Vector2)transform.position == target)
        {
            DestroyProjectile();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.ReceiveDamage(hitDamage);
            }

            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        // Instanciar el efecto de destrucción en la posición del proyectil
        GameObject destructionEffect = Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);

        // Destruir el proyectil
        Destroy(gameObject);

        // Destruir el efecto de destrucción después del tiempo especificado en destructionDelay
        Destroy(destructionEffect, destructionDelay);
    }
}


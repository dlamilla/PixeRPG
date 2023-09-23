using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncaBoss : MonoBehaviour
{
    [Header("DatosJefe")]
    [SerializeField] private float hpEnemy;
    [SerializeField] private float hitDamage;
    [SerializeField] public float moveSpeed;

    [Header("Teleport")]
    [SerializeField] public Transform[] teleportPoints;
    public float teleportTime;
    public float teleportRange;

    [Header("Distancias Stop y Retroceso")]
    public float stoppingDistance;
    public float retreatDistance;

    private float tiempoPorDisparo;
    public float tiempoEntreDisparo;

    public GameObject projectile;

    private Transform target;
    private Animator animator;
    private bool isBacking = false;
    private bool isFollowingPlayer = true;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        InvokeRepeating("Teleport", teleportTime, teleportTime);
        tiempoPorDisparo = tiempoEntreDisparo;
    }

    void Update()
    {
        if (hpEnemy > 0)
        {
            Vector2 directionToPlayer = (target.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if (isFollowingPlayer && !animator.GetBool("isTeleport") && !isBacking)
            {
                if (distanceToPlayer > stoppingDistance)
                {
                    Vector2 newPosition = (Vector2)transform.position + directionToPlayer * moveSpeed * Time.deltaTime;
                    transform.position = new Vector2(newPosition.x, newPosition.y);
                }
                else if (distanceToPlayer < stoppingDistance && distanceToPlayer > retreatDistance)
                {
                    // Guardado por si tengo que recurrir a otra linea de codigo XD
                }
                else if (distanceToPlayer < retreatDistance)
                {
                    Vector2 newPosition = (Vector2)transform.position - directionToPlayer * moveSpeed * Time.deltaTime;
                    transform.position = new Vector2(newPosition.x, newPosition.y);
                }
            }
            if (tiempoPorDisparo <= 0)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
                tiempoPorDisparo = tiempoEntreDisparo;
            }
            else
            {
                tiempoPorDisparo -= Time.deltaTime;
            }
        }
    }

    void Teleport()
    {
        animator.SetBool("isTeleport", true);
        StartCoroutine(CompleteTeleportAnimation());
    }
    public void ReceiveDamage(float damage)
    {
        hpEnemy -= damage;

        if (hpEnemy <= 0)
        {
            Debug.Log("Muerto");
        }
    }

    IEnumerator CompleteTeleportAnimation()
    {
        yield return new WaitForSeconds(1.5f);

        int randomIndex = Random.Range(0, teleportPoints.Length);
        Vector2 teleportPosition = (Vector2)teleportPoints[randomIndex].position;

        transform.position = teleportPosition;

        animator.SetBool("isTeleport", false);
        animator.SetBool("isBacking", true);
        isFollowingPlayer = false;

        yield return new WaitForSeconds(1.15f);

        animator.SetBool("isBacking", false);
        isFollowingPlayer = true;
    }
}

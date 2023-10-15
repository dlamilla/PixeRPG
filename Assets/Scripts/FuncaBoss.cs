using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncaBoss : MonoBehaviour
{
    [Header("DatosJefe")]
    [SerializeField] private float hpEnemyInicial;
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

    [Header("HealthBar")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject healthBarBoss;

    private float tiempoPorDisparo;
    public float tiempoEntreDisparo;

    public GameObject projectile;

    private Transform target;
    private Animator animator;
    private bool isBacking = false;
    private bool isFollowingPlayer = true;

    [SerializeField] private GameObject enemyGameObject;

    private bool hasReachedHalfHP = false;
    private bool isAttacking = false;
    private float attackDuration = 1.3f;
    private float currentAttackTime = 0.0f;

    [SerializeField] private GameObject[] attackObjects; // Debe contener los 4 gameObjects que se activaran cuando el enemigo tenga la mitad de la vida

    private bool hasActivatedObjects = false; // Para rastrear si los objetos ya se han activado

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        InvokeRepeating("Teleport", teleportTime, teleportTime);
        tiempoPorDisparo = tiempoEntreDisparo;
        hpEnemyInicial = hpEnemy;
        healthBar.UpdateHealthBar(hpEnemyInicial, hpEnemy);
    }

    void Update()
    {
        healthBarBoss.SetActive(true);
        if (hpEnemy > 0)
        {
            Vector2 directionToPlayer = (target.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if (directionToPlayer.x < 0)
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * 1, transform.localScale.y);
            }
            else if (directionToPlayer.x > 0)
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y);
            }

            if (!isAttacking && isFollowingPlayer && !animator.GetBool("isTeleport") && !isBacking)
            {
                if (distanceToPlayer > stoppingDistance)
                {
                    Vector2 newPosition = (Vector2)transform.position + directionToPlayer * moveSpeed * Time.deltaTime;
                    transform.position = new Vector2(newPosition.x, newPosition.y);
                }
                else if (distanceToPlayer < stoppingDistance && distanceToPlayer > retreatDistance)
                {
                    // Guardado por si tengo que recurrir a otra línea de código XD
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

            if (hpEnemy <= (hpEnemyInicial / 2) && !hasReachedHalfHP)
            {
                hasReachedHalfHP = true;
                StartAttackAnimation();
            }

            if (isAttacking)
            {
                currentAttackTime += Time.deltaTime;
                if (currentAttackTime >= attackDuration)
                {
                    EndAttackAnimation();
                }
            }
        }

        if (hpEnemy <= 0)
        {
            enemyGameObject.SetActive(false);
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
        healthBar.UpdateHealthBar(hpEnemyInicial, hpEnemy);
        if (hpEnemy <= 0)
        {
            target.GetComponent<Player>().LevelUp(1);
            healthBarBoss.SetActive(false);
            Debug.Log("Muerto");
        }
    }

    void StartAttackAnimation()
    {
        animator.SetBool("isAttackFour", true);
        isAttacking = true;
        currentAttackTime = 0.0f;

        if (!hasActivatedObjects)      // Cree esto para que activar un objeto
        {
            for (int i = 0; i < attackObjects.Length; i++)
            {
                attackObjects[i].SetActive(true);
            }
            hasActivatedObjects = true;
        }
    }

    void EndAttackAnimation()
    {
        animator.SetBool("isAttackFour", false);
        isAttacking = false;
        currentAttackTime = 0.0f;

        isFollowingPlayer = true;
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
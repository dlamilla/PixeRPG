using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AllEnemys_IA
{
    ENEMY_PATROL,  //Spider, Snake
    ENEMY_SHOOT   //Plant
}

[RequireComponent(typeof(ChangeAnimation))]
public class AllEnemysIA : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float hpEnemy;
    [SerializeField] private float speed;
    [SerializeField] private float hitDamage;
    [SerializeField] private float timeForHit;

    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints;
    private int currentPoint;

    [Header("Plant")]
    [SerializeField] private float radiusShoot;

    [Header("Stadistics")]
    [SerializeField] private float expEnemy;

    [SerializeField] private AllEnemys_IA category;
    [SerializeField] private float radiusAttack; //Attack
    [SerializeField] private float radiusSearch; //Find Player

    [SerializeField] private float timeSpwan;
    [SerializeField] private GameObject plantShoot;
    [SerializeField] private float timeNext;

    private NavMeshAgent navMeshAgent;
    private ChangeAnimation changeDirections;
    private Animator anim;
    private Rigidbody2D rb;
    private Transform player;
    private BoxCollider2D player1;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        player1 = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
        changeDirections = GetComponent<ChangeAnimation>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.autoBraking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hpEnemy > 0)
        {
            switch (category)
            {
                //Enemigo que patrulla puntos especificos y cuando te acercas te sigue y te ataca, al huir regresa a patrullar
                case AllEnemys_IA.ENEMY_PATROL:

                    if (Vector2.Distance(transform.position, player.transform.position) <= radiusSearch && Vector2.Distance(transform.position, player.transform.position) > radiusAttack)
                    {
                        navMeshAgent.SetDestination(player.transform.position);
                        Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));

                        anim.SetBool("isRunning", true);

                    }
                    else if (Vector2.Distance(transform.position, player.transform.position) > radiusSearch)
                    {
                        anim.SetBool("isRunning", true);

                        
                        navMeshAgent.SetDestination(waypoints[currentPoint].transform.position);
                        Vector2 temp = Vector2.MoveTowards(transform.position, waypoints[currentPoint].transform.position, speed * Time.deltaTime);
                        changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));

                        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
                        {
                            ChangeGoal();
                        }
                    }
                    else if (Vector2.Distance(transform.position, player.transform.position) <= radiusSearch && Vector2.Distance(transform.position, player.transform.position) <= radiusAttack)
                    {

                        if (player1.enabled)
                        {
                            StartCoroutine(Attack());
                        }
                    }

                    break;
                //Enemigo estatico que solo dispara
                case AllEnemys_IA.ENEMY_SHOOT:

                    if (Vector2.Distance(transform.position, player.transform.position) <= radiusShoot)
                    {
                        if (player1.enabled)
                        {
                            anim.SetBool("Attack", true);
                            timeNext += Time.deltaTime;
                            if (timeNext >= timeSpwan)
                            {
                                timeNext = 0;
                                Instantiate(plantShoot, transform.position, Quaternion.identity);
                            }
                        }
                    }
                    else
                    {
                        anim.SetBool("Attack", false);
                    }
                    break;
            }
        }
    }

    private void ChangeGoal()
    {
        currentPoint++;
        if (currentPoint >= waypoints.Length)
        {
            currentPoint = 0;
        }
    }


    IEnumerator Attack()
    {
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(timeForHit);

        anim.SetBool("Attack", false);
    }

    public void ReceiveDamage(float damage)
    {
        hpEnemy -= damage;
        anim.SetTrigger("Hit");
        StartCoroutine(SpeedHit());
        if (hpEnemy <= 4f)
        {
            //GetComponent<LootBag>().InstantiateLoot(transform.position);
            player.GetComponent<Player>().ExpUp(expEnemy);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isReceiveDamage = false;
            this.gameObject.SetActive(false);
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusSearch);
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
        Gizmos.DrawWireSphere(transform.position, radiusShoot);
    }

    public void AttackPlayer()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(transform.position, radiusAttack);

        foreach (Collider2D collision in objetos)
        {
            if (collision.gameObject.tag == "Player")
            {
                float playerLife = collision.GetComponent<Player>().health;
                if (playerLife > 4f)
                {
                    collision.GetComponent<Player>().ReceiveDamage(hitDamage);
                }

            }
        }

    }

    IEnumerator SpeedHit()
    {
        navMeshAgent.speed = 0f;
        yield return new WaitForSeconds(0.5f);
        navMeshAgent.speed = speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public enum AllEnemys_IA
{
    ENEMY_PATROL,  //Spider, Snake
    ENEMY_SHOOT,   //Plant
    ENEMY_FOLLOW,  //Bat
    ENEMY_PATSHOTT, 
    ENEMY_STATIC   //Tree
}

public enum SFX_Enemys
{
    SPIDER,
    SNAKE,
    PLANT,
    BAT,
    TREE
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
    [SerializeField] private SFX_Enemys sfxCategory;
    [SerializeField] private float radiusAttack; //Attack
    [SerializeField] private float radiusSearch; //Find Player

    [SerializeField] private float timeSpwan;
    [SerializeField] private GameObject plantShoot;
    [SerializeField] private float timeNext;

    [Header("SFX")]
    [SerializeField] private AudioClip spiderHit;
    [SerializeField] private AudioClip plantShooting;
    [SerializeField] private AudioClip snakeHit;
    [SerializeField] private AudioClip batHit;
    [SerializeField] private AudioClip treeHit;

    private NavMeshAgent navMeshAgent;
    private ChangeAnimation changeDirections;
    private Animator anim;
    private Rigidbody2D rb;
    private Transform player;
    private BoxCollider2D player1;
    private AudioSource sfxSound1;
    private AudioSource sfxSound2;
    private void Awake()
    {
        sfxSound1 = gameObject.AddComponent<AudioSource>();
        sfxSound2 = gameObject.AddComponent<AudioSource>();
    }
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
                                switch (sfxCategory)
                                {
                                    case SFX_Enemys.SPIDER:
                                        break;
                                    case SFX_Enemys.SNAKE:
                                        break;
                                    case SFX_Enemys.PLANT:
                                        sfxSound2.clip = plantShooting;
                                        sfxSound2.loop = false;
                                        sfxSound2.Play();
                                        break;
                                    case SFX_Enemys.BAT:
                                        break;
                                    case SFX_Enemys.TREE:
                                        break;
                                    default:
                                        break;
                                }
                                Instantiate(plantShoot, transform.position, Quaternion.identity);
                            }
                        }
                    }
                    else
                    {
                        anim.SetBool("Attack", false);
                    }
                    break;
                //Enemigo que te sige y regresa a su posicion
                case AllEnemys_IA.ENEMY_FOLLOW:
                    if (Vector2.Distance(transform.position, player.transform.position) <= radiusSearch)
                    {
                        navMeshAgent.SetDestination(player.transform.position);
                        //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));

                        anim.SetBool("isRunning", true);
                    }
                    else
                    {
                        navMeshAgent.SetDestination(waypoints[currentPoint].transform.position);
                        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
                        {
                            anim.SetBool("isRunning", false);
                        }
                    }
                    break;
                case AllEnemys_IA.ENEMY_STATIC:
                    if (Vector2.Distance(transform.position, player.transform.position) <= radiusSearch)
                    {
                        navMeshAgent.SetDestination(player.transform.position);
                        Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));

                        navMeshAgent.speed = speed;
                        anim.SetBool("isRunning", true);
                    }
                    else if (Vector2.Distance(transform.position, player.transform.position) <= radiusSearch && Vector2.Distance(transform.position, player.transform.position) <= radiusAttack)
                    {

                        if (player1.enabled)
                        {
                            StartCoroutine(Attack());
                        }
                    }
                    else
                    {
                        navMeshAgent.speed = 0f;
                        anim.SetBool("isRunning", false);
                    }
                    break;
                case AllEnemys_IA.ENEMY_PATSHOTT:
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
                            anim.SetBool("Attack", true);

                            timeNext += Time.deltaTime;
                            if (timeNext >= timeSpwan)
                            {
                                timeNext = 0;
                                switch (sfxCategory)
                                {
                                    case SFX_Enemys.SPIDER:
                                        break;
                                    case SFX_Enemys.SNAKE:
                                        break;
                                    case SFX_Enemys.PLANT:
                                        sfxSound2.clip = plantShooting;
                                        sfxSound2.loop = false;
                                        sfxSound2.Play();
                                        break;
                                    case SFX_Enemys.BAT:
                                        break;
                                    case SFX_Enemys.TREE:
                                        break;
                                    default:
                                        break;
                                }
                                Instantiate(plantShoot, transform.position, Quaternion.identity);
                            }
                            else
                            {
                                anim.SetBool("Attack", false);
                            }
                        }
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
        switch (sfxCategory)
        {
            case SFX_Enemys.SPIDER:
                sfxSound1.clip = spiderHit;
                sfxSound1.loop = false;
                sfxSound1.Play();
                break;
            case SFX_Enemys.SNAKE:
                sfxSound1.clip = snakeHit;
                sfxSound1.loop = false;
                sfxSound1.Play();
                break;
            case SFX_Enemys.BAT:
                sfxSound1.clip = batHit;
                sfxSound1.loop = false;
                sfxSound1.Play();
                break;
            case SFX_Enemys.TREE:
                sfxSound1.clip = treeHit;
                sfxSound1.loop = false;
                sfxSound1.volume = 0.7f;
                sfxSound1.Play();
                break;
            default:
                break;
        }
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

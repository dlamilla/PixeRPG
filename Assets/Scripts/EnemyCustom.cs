using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TypeEnemys{
    ENEMY_SPIDER,
    ENEMY_PLANT,
    ENEMY_BAT
}

[RequireComponent(typeof(ChangeAnimation))]
public class EnemyCustom : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float hpEnemy;
    [SerializeField] private float speed;
    [SerializeField] private float hitDamage;
    [SerializeField] private float timeForHit;

    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints;
    //private Transform startPosition;
    private int currentPoint;

    [Header("Plant")]
    [SerializeField] private float radiusShoot;

    //[SerializeField] private EnemyState currentState;
    [SerializeField] private TypeEnemys category;
    [SerializeField] private float radiusAttack; //Attack
    [SerializeField] private float radiusSearch; //Find Player

    [SerializeField] private float timeSpwan;
    [SerializeField] private GameObject plantShoot;
    [SerializeField] private float timeNext;

    //private NavMeshAgent navMeshAgent;
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

        //navMeshAgent = GetComponent<NavMeshAgent>();
        //navMeshAgent.updateRotation = false;
        //navMeshAgent.updateUpAxis = false;

        
        anim.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (hpEnemy > 0)
        {
            switch (category)
            {
                case TypeEnemys.ENEMY_SPIDER:
                    
                    if (Vector2.Distance(transform.position, player.transform.position) <= radiusSearch && Vector2.Distance(transform.position, player.transform.position) > radiusAttack)
                    {
                        
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));
                        
                        anim.SetBool("isRunning", true);
                        
                    }
                    else if (Vector2.Distance(transform.position, player.transform.position) > radiusSearch)
                    {
                        anim.SetBool("isRunning", true);

                        if (transform.position != waypoints[currentPoint].transform.position)
                        {
                            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentPoint].transform.position, speed * Time.deltaTime);
                            Vector2 temp = Vector2.MoveTowards(transform.position, waypoints[currentPoint].transform.position, speed * Time.deltaTime);
                            changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));
                        }
                        else
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
                case TypeEnemys.ENEMY_PLANT:
                    
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
                case TypeEnemys.ENEMY_BAT:
                    if (Vector2.Distance(transform.position, player.transform.position) <= radiusSearch)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));
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
        
        anim.SetBool("AtaqueArana", true);
        yield return new WaitForSeconds(timeForHit);

        anim.SetBool("AtaqueArana", false);
    }

    public void ReceiveDamage(float damage)
    {
        hpEnemy -= damage;

        if (hpEnemy <= 0)
        {
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            Destroy(gameObject);
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusSearch);
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
        Gizmos.DrawWireSphere(transform.position, radiusShoot);
    }

    public void AttackSpider()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(transform.position, radiusAttack);

        foreach (Collider2D collision in objetos)
        {
            if (collision.gameObject.tag == "Player")
            {
                float playerLife = collision.GetComponent<Player>().health;
                if (playerLife > 0)
                {
                    collision.GetComponent<Player>().ReceiveDamage(hitDamage);
                }
                
            }
        }

    }
}

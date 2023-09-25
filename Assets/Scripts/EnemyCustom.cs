using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TypeEnemys{
    ENEMY_SPIDER,
    ENEMY_PLANT,
    ENEMY_BAT
}

//public enum EnemyState{
//    IDLE,
//    WALK,
//    ATTACK,
//    STAGGER
//}
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
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        changeDirections = GetComponent<ChangeAnimation>();

        //navMeshAgent = GetComponent<NavMeshAgent>();
        //navMeshAgent.updateRotation = false;
        //navMeshAgent.updateUpAxis = false;

        //currentState = EnemyState.IDLE;
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
                        // if (currentState == EnemyState.IDLE || currentState == EnemyState.WALK && currentState != EnemyState.STAGGER)
                        // {
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));
                        //ChangeState(EnemyState.WALK);
                        //Debug.Log("Condicional 1");
                        anim.SetBool("isRunning", true);
                        // }
                    }
                    else if (Vector2.Distance(transform.position, player.transform.position) > radiusSearch)
                    {
                        //Debug.Log("Condicional 2");
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
                        //Debug.Log("Condicional 3");
                        // if (currentState == EnemyState.WALK && currentState != EnemyState.ATTACK)
                        //{
                        StartCoroutine(Attack());
                        //}
                    }

                    break;
                case TypeEnemys.ENEMY_PLANT:
                    
                    if (Vector2.Distance(transform.position, player.transform.position) <= radiusShoot)
                    {
                        anim.SetBool("Attack", true);
                        timeNext += Time.deltaTime;
                        if (timeNext >= timeSpwan)
                        {
                            timeNext = 0;
                            Instantiate(plantShoot, transform.position, Quaternion.identity);
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

    //private void SetAnimFloat(Vector2 setVector)
    //{
    //    anim.SetFloat("X", setVector.x);
    //    anim.SetFloat("Y", setVector.y);
    //}

    //public void changeAnim(Vector2 direction)
    //{
    //    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
    //    {
    //        if (direction.x > 0)
    //        {
    //            SetAnimFloat(Vector2.right);
    //        }
    //        else if (direction.x < 0)
    //        {
    //            SetAnimFloat(Vector2.left);
    //        }
    //    }
    //    else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
    //    {
    //        if (direction.y > 0)
    //        {
    //            SetAnimFloat(Vector2.up);
    //        }
    //        else if (direction.y < 0)
    //        {
    //            SetAnimFloat(Vector2.down);
    //        }
    //    }
    //}
    //public void ChangeState(EnemyState newState)
    //{
    //    if (currentState != newState)
    //    {
    //        currentState = newState;
    //    }
    //}

    IEnumerator Attack()
    {
        //Collider2D[] objetos = Physics2D.OverlapCircleAll(transform.position, radiusAttack);

        //foreach (Collider2D collision in objetos)
        //{
        //    if (collision.gameObject.tag == "Player")
        //    {
        //        Debug.Log("Golpe");
        //        collision.GetComponent<Player>().ReceiveDamage(hitDamage);
        //    }
        //}

        //currentState = EnemyState.ATTACK;
        anim.SetBool("AtaqueArana", true);
        yield return new WaitForSeconds(timeForHit);

        //currentState = EnemyState.WALK;
        anim.SetBool("AtaqueArana", false);
    }

    public void ReceiveDamage(float damage)
    {
        hpEnemy -= damage;

        if (hpEnemy <= 0)
        {
            Destroy(gameObject);
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusSearch);
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("Golpe");
    //        collision.GetComponent<Player>().ReceiveDamage(hitDamage);
    //    }
    //}

    public void AttackSpider()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(transform.position, radiusAttack);

        foreach (Collider2D collision in objetos)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.GetComponent<Player>().ReceiveDamage(hitDamage);
            }
        }

    }
}

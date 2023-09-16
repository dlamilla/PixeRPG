using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeEnemys{
    ENEMY_SPIDER,
    ENEMY_EXTRA
}

public enum EnemyState{
    IDLE,
    WALK,
    ATTACK,
    STAGGER
}
public class EnemyCustom : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float life;
    [SerializeField] private float speed;
    [SerializeField] private float hitDamage;

    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints;
    private Transform startPosition;
    private int currentPoint;

    private EnemyState currentState;
    [SerializeField] private TypeEnemys category;
    [SerializeField] private float radiusAttack; //Attack
    [SerializeField] private float radiusSearch; //Find Player

    private Animator anim;
    private Rigidbody2D rb;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;

        currentState = EnemyState.IDLE;
        //anim.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (category)
        {
            case TypeEnemys.ENEMY_SPIDER:
                if (Vector2.Distance(transform.position, player.transform.position) <= radiusSearch && Vector2.Distance(transform.position, player.transform.position) > radiusAttack)
                {
                    if (currentState == EnemyState.IDLE || currentState == EnemyState.WALK && currentState != EnemyState.STAGGER)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        changeAnim(temp - new Vector2(transform.position.x,transform.position.y));
                        ChangeState(EnemyState.WALK);
                        Debug.Log("Condicional 1");
                        //anim.SetBool("isRunning", true);
                    }
                }
                else if (Vector2.Distance(transform.position, player.transform.position) > radiusSearch)
                {
                    Debug.Log("Condicional 2");
                    //anim.SetBool("isRunning", false);

                    if (Vector2.Distance(transform.position, waypoints[currentPoint].transform.position) > radiusSearch)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentPoint].transform.position, speed * Time.deltaTime);
                        Vector2 temp = Vector2.MoveTowards(transform.position,waypoints[currentPoint].transform.position,speed * Time.deltaTime);
                        changeAnim(temp - new Vector2(transform.position.x, transform.position.y));
                    }
                    else
                    {
                        ChangeGoal();
                    }
                }
                else if (Vector2.Distance(transform.position, player.transform.position) <= radiusSearch && Vector2.Distance(transform.position, player.transform.position) <= radiusAttack)
                {
                    Debug.Log("Condicional 3");
                    if (currentState == EnemyState.WALK && currentState != EnemyState.ATTACK)
                    {
                        StartCoroutine(Attack());
                    }
                }

                break;
            case TypeEnemys.ENEMY_EXTRA: 
                //Seguir al jugador
                //Atacar
                //Vida en la mitad ataque 2
                //Spawnea
                //Muere dar exp
                //Random monedas
                break;
        }
    }

    private void ChangeGoal()
    {
        //if (currentPoint == waypoints.Length - 1)
        //{
        //    currentPoint = 0;
        //    startPosition = waypoints[0];
        //}
        //else
        //{
        //    currentPoint++;
        //    startPosition = waypoints[currentPoint];
        //}

        currentPoint++;
        if (currentPoint >= waypoints.Length)
        {
            currentPoint = 0;
        }
    }

    private void SetAnimFloat(Vector2 setVector)
    {
        //anim.SetFloat("X", setVector.x);
        //anim.SetFloat("Y", setVector.y);
    }

    public void changeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }
    }
    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    IEnumerator Attack()
    {
        currentState = EnemyState.ATTACK;
        //anim.SetBool("AtaqueArana", true);
        yield return new WaitForSeconds(1f);
        Debug.Log("Ataque");
        currentState = EnemyState.WALK;
        //anim.SetBool("AtaqueArana", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusSearch);
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }
    
}

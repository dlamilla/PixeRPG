using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum Snake
{
    Snake_Caminador,
}
[RequireComponent(typeof(ChangeAnimation))]
public class ScriptSnake : MonoBehaviour
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

    [Header("Stadistics")]
    [SerializeField] private float expEnemy;
    [SerializeField] private Snake category;
    [SerializeField] private float radiusAttack;
    [SerializeField] private float radiusSearch;

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
        anim.SetBool("Running", true);

    }

    // Update is called once per frame
    void Update()
    {
        if (hpEnemy > 0)
        {
            switch (category)
            {
                case Snake.Snake_Caminador:

                    if (Vector2.Distance(transform.position, player.transform.position) <= radiusSearch && Vector2.Distance(transform.position, player.transform.position) > radiusAttack)
                    {

                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));

                        anim.SetBool("Running", true);

                    }
                    else if (Vector2.Distance(transform.position, player.transform.position) > radiusSearch)
                    {
                        anim.SetBool("Running", true);

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

        anim.SetBool("AttackSnake", true);
        yield return new WaitForSeconds(timeForHit);

        anim.SetBool("AttackSnake", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusSearch);
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }

    public void ReceiveDamage(float damage)
    {
        hpEnemy -= damage;

        if (hpEnemy <= 4f)
        {
            //GetComponent<LootBag>().InstantiateLoot(transform.position);
            player.GetComponent<Player>().ExpUp(expEnemy);
            Destroy(gameObject);
        }
    }

    public void AttackSnake()
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
}

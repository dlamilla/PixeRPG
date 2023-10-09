using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeSpawn
{
    BABYRAT,
    PLANT_SHOOT
}

[RequireComponent(typeof(ChangeAnimation))]
public class SpawnCustom : MonoBehaviour
{
    [SerializeField] private TypeSpawn category;
    [SerializeField] private float hpEnemy;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float radiusAttack;

    private Vector2 target;

    private Transform player;
    private ChangeAnimation changeDirections;
    private BoxCollider2D player1;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        changeDirections = GetComponent<ChangeAnimation>();
        target = new Vector2(player.position.x, player.position.y);
        player1 = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hpEnemy > 0)
        {
            switch (category)
            {
                case TypeSpawn.BABYRAT:
                    
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));
                    break;

                case TypeSpawn.PLANT_SHOOT:
                    Debug.Log(player1.enabled + "Planta");
                    if (player1.enabled)
                    {
                        
                        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

                        if (transform.position.x == target.x && transform.position.y == target.y)
                        {
                            DestroyProjectile();
                        }
                    }

                    break;
            }
        }
    }


    public void AttackBabby()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(transform.position, radiusAttack);

        foreach (Collider2D collision in objetos)
        {
            if (collision.gameObject.tag == "Player")
            {
                float playerLife = collision.GetComponent<Player>().health;
                if (playerLife > 0)
                {
                    collision.GetComponent<Player>().ReceiveDamage(damage);
                }
            }
        }

    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    public void ReceiveDamage(float damage)
    {
        hpEnemy -= damage;

        if (hpEnemy <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyProjectile();
            other.gameObject.GetComponent<Player>().ReceiveDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }
}

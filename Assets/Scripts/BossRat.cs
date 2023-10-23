using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChangeAnimation))]
public class BossRat : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float hpBoss;
    [SerializeField] private float speed;

    [Header("Hit")]
    [SerializeField] private float hitDamage;
    [SerializeField] private float timeForHit;
    [SerializeField] private float radiusAttack;
    [SerializeField] private float timeFinalAttack;
    [SerializeField] private float timeCurrent;
    [SerializeField] private float changeAttack;

    [Header("Exp")]
    [SerializeField] private float exp;

    [Header("HealthBar")]
    [SerializeField] private HealthBar healthBar;
    private float hpCurrent;
    [SerializeField] private GameObject healthBarBoss;

    [Header("Spawns")]
    [SerializeField] private Transform[] spwans;
    [SerializeField] private float timeSpwan;
    [SerializeField] private GameObject babyRat;
    [SerializeField] private float timeNext;

    
    private Vector2 posBossInitial;
    private ChangeAnimation changeDirections;
    private Animator anim;
    private Transform player;
    private BoxCollider2D bx;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bx = GetComponent<BoxCollider2D>();
        player = GameObject.FindWithTag("Player").transform;
        changeDirections = GetComponent<ChangeAnimation>();
        hpCurrent = hpBoss;
        healthBar.UpdateHealthBar(hpBoss, hpCurrent);
        posBossInitial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(WaitTime());
    }

    public void ReceiveDamage(float damage)
    {
        hpCurrent -= damage;
        healthBar.UpdateHealthBar(hpBoss, hpCurrent);
        if (hpCurrent <= 5f)
        {
            player.GetComponent<Player>().ExpUp(exp);
            player.GetComponent<Player>().LevelUp(1);
            healthBarBoss.SetActive(false);
            gameObject.SetActive(false);          
        }
    }

    IEnumerator AttackBoss()
    {
        anim.SetBool("attackBoss", true);
        yield return new WaitForSeconds(timeForHit);
        anim.SetBool("attackBoss", false);
        
    }

    public void Attack1()
    {
        if (Vector2.Distance(transform.position, player.transform.position) > radiusAttack)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));
            anim.SetBool("isRunning", true);
        }
        else
        {
            StartCoroutine(AttackBoss());
        }
    }

    public void Attack2()
    {
        bx.enabled = false;
        transform.position = Vector2.MoveTowards(transform.position, posBossInitial, speed * Time.deltaTime);
        Vector2 temp = Vector2.MoveTowards(transform.position, posBossInitial, speed * Time.deltaTime);
        changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));
        anim.SetBool("isRunning", true);
        if (new Vector2(transform.position.x,transform.position.y) == posBossInitial)
        {
            
            anim.SetBool("isRunning", false);
            anim.SetBool("attack2", true);
            Shoot();
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }

    public void Shoot()
    {
        timeNext += Time.deltaTime;
        if (timeNext >= timeSpwan)
        {
            timeNext = 0;
            int baby = Random.Range(0, spwans.Length);
            Instantiate(babyRat, spwans[baby].position, Quaternion.identity);
        }
        
    }

    public void Attacks()
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

    public void RestartLife()
    {
        hpCurrent = hpBoss;
        healthBar.UpdateHealthBar(hpBoss, hpCurrent);
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(2.5f);
        MechanicsBoss();
    }

    private void MechanicsBoss()
    {
        healthBarBoss.SetActive(true);
        if (hpCurrent > changeAttack)
        {
            Attack1();
        }
        else
        {
            timeCurrent += Time.deltaTime;
            if (timeCurrent >= 0 && timeCurrent <= timeFinalAttack)
            {
                Attack2();
            }

            if (timeCurrent >= timeFinalAttack)
            {
                bx.enabled = true;
                anim.SetBool("attack2", false);
                Attack1();
            }

        }
    }
}

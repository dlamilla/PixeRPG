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
        healthBarBoss.SetActive(true);
        if (hpCurrent > 25)
        {
            Attack1();
        }
        else
        {
            timeCurrent += Time.deltaTime;
            if (timeCurrent >= 0 && timeCurrent <= timeFinalAttack)
            {
                Debug.Log("Ataque1");
                Attack2();
            }
            
            if (timeCurrent >= timeFinalAttack)
            {
                Debug.Log("Ataque2");
                bx.enabled = true;
                anim.SetBool("attack2", false);
                Attack1();
            }
            
        }
    }

    public void ReceiveDamage(float damage)
    {
        hpCurrent -= damage;
        healthBar.UpdateHealthBar(hpBoss, hpCurrent);
        if (hpCurrent <= 0)
        {
            player.GetComponent<Player>().ExpUp(exp);
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
                Debug.Log("Golpe");
                collision.GetComponent<Player>().ReceiveDamage(hitDamage);
            }
        }

    }
}

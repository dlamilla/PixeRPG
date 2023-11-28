using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

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

    [Header("Rewards")]
    [SerializeField] private float exp;
    [SerializeField] private float extraDamage;
    [SerializeField] private float moreHealthPlayer;

    [Header("HealthBar")]
    [SerializeField] private HealthBar healthBar;
    private float hpCurrent;
    [SerializeField] private GameObject healthBarBoss;

    [Header("Spawns")]
    [SerializeField] private Transform[] spwans;
    [SerializeField] private float timeSpwan;
    [SerializeField] private GameObject babyRat;
    [SerializeField] private float timeNext;

    [Header("SFX")]
    [SerializeField] private AudioClip soundHit;
    [SerializeField] private AudioClip soundDied;
    [SerializeField] private AudioClip soundAngry;
    [SerializeField] private AudioClip soundAttack;

    private Vector2 posBossInitial;
    private ChangeAnimation changeDirections;
    private Animator anim;
    private Transform player;
    private BoxCollider2D bx;
    private NavMeshAgent navMeshAgent;
    private AudioSource sfxRat;

    private void Awake()
    {
        sfxRat = gameObject.AddComponent<AudioSource>();
    }

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
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(WaitTime());
        
    }

    public void ReceiveDamage(float damage)
    {
        hpCurrent -= damage;
        anim.SetTrigger("Hit");
        sfxRat.clip = soundHit;
        sfxRat.loop = false;
        sfxRat.Play();
        healthBar.UpdateHealthBar(hpBoss, hpCurrent);
        if (hpCurrent <= 5f)
        {
            sfxRat.clip = soundDied;
            sfxRat.loop = false;
            sfxRat.Play();
            anim.SetBool("Died", true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isReceiveDamage = false;
            player.GetComponent<Player>().ExpUp(exp);
            player.GetComponent<Player>().LevelUp(1);
            player.GetComponent<Player>().GiveMoreDamage(extraDamage);
            //player.GetComponent<Player>().GiveMoreHealth(moreHealthPlayer);
            healthBarBoss.SetActive(false);
            //gameObject.SetActive(false);          
        }
    }

    IEnumerator AttackBoss()
    {
        anim.SetBool("attackBoss", true);
        navMeshAgent.speed = 0f;
        sfxRat.clip = soundAttack;
        sfxRat.loop = false;
        sfxRat.Play();
        yield return new WaitForSeconds(timeForHit);
        navMeshAgent.speed = speed;
        anim.SetBool("attackBoss", false);
        
    }

    public void Attack1()
    {
        if (Vector2.Distance(transform.position, player.transform.position) > radiusAttack)
        {
            navMeshAgent.SetDestination(player.transform.position);
            //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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
        navMeshAgent.SetDestination(posBossInitial);
        //transform.position = Vector2.MoveTowards(transform.position, posBossInitial, speed * Time.deltaTime);
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
                if (playerLife <= 4f)
                {
                    transform.position = posBossInitial;
                    hpCurrent = hpBoss;
                    healthBar.UpdateHealthBar(hpBoss, hpCurrent);
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
        if (hpCurrent > 5f)
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

    IEnumerator AfterPlayerDied()
    {
        
        
        this.gameObject.SetActive(false);
        healthBarBoss.SetActive(false);
        Debug.Log("Uno");
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Dos");
        this.gameObject.SetActive(true);
        healthBarBoss.SetActive(true);
    }
}

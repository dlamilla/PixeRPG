using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE_BOSS
{
    IDLE,
    WALK,
    JUMP,
    ATTACK,
    FALL,
    WALK2
}

[RequireComponent(typeof(ChangeAnimation))]
public class ForestBoss : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float hpBoss;
    [SerializeField] private float speed;
    [SerializeField] private float speedIncrease;
    [SerializeField] private float speedUp;
    private float resetSpeed;

    [Header("Hit")]
    [SerializeField] private float hitDamage;
    [SerializeField] private float timeForHit;
    [SerializeField] private float radiusAttack;

    [Header("Rewards")]
    [SerializeField] private float exp;
    [SerializeField] private float extraDamage;

    [Header("HealthBar")]
    [SerializeField] private HealthBar healthBar;
    private float hpCurrent;
    [SerializeField] private GameObject healthBarBoss;

    [Header("Point Jump")]
    [SerializeField] private Transform jumpPoint;
    [SerializeField] private Transform[] pointsFall;
    private int randomPoint;

    [Header("State")]
    [SerializeField] private STATE_BOSS currentState;

    [Header("Times State")]
    [SerializeField] private float timeIdle;
    [SerializeField] private float timeWalk;
    [SerializeField] private float timeJump;
    [SerializeField] private float timeFall;
    [SerializeField] private float timeAttack;
    [SerializeField] private float timeToMove;
    private float timeInitial;

    private ChangeAnimation changeDirections;
    private Animator anim;
    private Transform player;
    private BoxCollider2D bx;
    private Rigidbody2D rb;
    private Transform lanza;
    private Vector2 lanzaPosition;
    private Transform jefeSelva;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bx = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        changeDirections = GetComponent<ChangeAnimation>();
        lanza = transform.Find("Lanza");
        jefeSelva = GameObject.FindWithTag("JefeSelva").transform;
        hpCurrent = hpBoss;
        healthBar.UpdateHealthBar(hpBoss, hpCurrent);
        resetSpeed = speed;
        transitions(STATE_BOSS.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == STATE_BOSS.IDLE)
        {
            speed = resetSpeed;
        }
        if (currentState == STATE_BOSS.WALK)
        {
            Mechanic1();
        }
        if (currentState == STATE_BOSS.JUMP)
        {
            StartCoroutine(Mechanic2());
        }

        if (currentState == STATE_BOSS.FALL)
        {
            StartCoroutine(Mechanic3());
        }
        if (currentState == STATE_BOSS.ATTACK)
        {
            StartCoroutine(Mechanic4());
        }
        //if (currentState == STATE_BOSS.WALK2)
        //{
        //    StartCoroutine(Mechanic5());
        //}
    }

    //IEnumerator Mechanic5()
    //{
    //    anim.SetBool("prueba", true);
    //    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 0f * Time.deltaTime);
    //    yield return new WaitForSeconds(1f);
    //    GetComponent<SpriteRenderer>().color = Color.white;
    //    anim.SetBool("prueba", false);
    //    anim.SetBool("enMarcha", false);
    //    anim.SetBool("lanzaGo", false);
    //}

    IEnumerator Mechanic4()
    {
        anim.SetBool("lanzaGo", true);
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("enMarcha", true);
        if (Vector2.Distance(transform.position, player.transform.position) > radiusAttack)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speedIncrease * Time.deltaTime);
            Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speedIncrease * Time.deltaTime);
            changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));
        }
        else
        {
            StartCoroutine(AttackBoss2());
        }
        yield return new WaitForSeconds(5f);
        anim.SetBool("prueba", true);
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 0f * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        anim.SetBool("prueba", false);
        anim.SetBool("enMarcha", false);
        anim.SetBool("lanzaGo", false);
    }

    IEnumerator Mechanic2()
    {
        anim.SetTrigger("inJump");
        transform.position = Vector2.MoveTowards(transform.position, jumpPoint.transform.position, speedUp * Time.deltaTime);
        randomPoint = Random.Range(0, pointsFall.Length);
        yield return new WaitForSeconds(timeToMove);
        
        transform.position = new Vector3(pointsFall[randomPoint].transform.position.x, transform.position.y, transform.position.z);

    }

    IEnumerator Mechanic3()
    {
        anim.SetBool("inFall", true);
        transform.position = Vector2.MoveTowards(transform.position, pointsFall[randomPoint].transform.position, speedUp * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        speed = 0f;
        anim.SetBool("inFall", false);

    }

    public void ReceiveDamage(float damage)
    {
        hpCurrent -= damage;
        //anim.SetTrigger("Hit");
        healthBar.UpdateHealthBar(hpBoss, hpCurrent);
        if (hpCurrent <= 5f)
        {
            //anim.SetBool("Died", true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isReceiveDamage = false;
            healthBarBoss.SetActive(false);
            gameObject.SetActive(false);          
        }
    }

    IEnumerator AttackBoss()
    {
        anim.SetBool("atqNormal", true);
        //navMeshAgent.speed = 0f;
        yield return new WaitForSeconds(timeForHit);
        //navMeshAgent.speed = speed;
        anim.SetBool("atqNormal", false);

    }

    IEnumerator AttackBoss2()
    {
        anim.SetBool("atqNo", true);
        //navMeshAgent.speed = 0f;
        yield return new WaitForSeconds(timeForHit);
        //navMeshAgent.speed = speed;
        anim.SetBool("atqNo", false);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
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

    private void Mechanic1()
    {
        if (Vector2.Distance(transform.position, player.transform.position) > radiusAttack)
        {
            //navMeshAgent.SetDestination(player.transform.position);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));
            anim.SetBool("enCamino", true);
        }
        else
        {
            StartCoroutine(AttackBoss());
        }
    }

    void transitions(STATE_BOSS stateNew)
    {
        currentState = stateNew;

        switch (currentState)
        {
            case STATE_BOSS.IDLE:
                StartCoroutine(TimeState(timeIdle, STATE_BOSS.WALK));
                break;
            case STATE_BOSS.WALK:
                StartCoroutine(TimeState(timeWalk, STATE_BOSS.JUMP));
                break;
            case STATE_BOSS.JUMP:
                StartCoroutine(TimeState(timeJump, STATE_BOSS.FALL));
                break;
            case STATE_BOSS.ATTACK:
                Debug.Log("Ataque con lanza");
                StartCoroutine(TimeState(timeAttack, STATE_BOSS.IDLE));
                break;
            case STATE_BOSS.FALL:
                StartCoroutine(TimeState(timeFall, STATE_BOSS.ATTACK));
                break;
            //case STATE_BOSS.WALK2:
                
            //    //if (lanza.parent == jefeSelva)
            //    //{
            //    //    StartCoroutine(TimeState(timeIdle, STATE_BOSS.IDLE));
            //    //}
            //    break;
        }
    }

    IEnumerator TimeState(float limit, STATE_BOSS current)
    {
        timeInitial = 0f;
        while (timeInitial < limit)
        {
            timeInitial += Time.deltaTime;
            yield return null;
        }

        transitions(current);
    }
}

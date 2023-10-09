using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Live")]
    [SerializeField] private float moveSpeed;
    [SerializeField] public float hpPlayerMax;

    [Header("Give Health")]
    [SerializeField] private float timeNextHealth;
    [SerializeField] private float giveHealth;
    [SerializeField] private float timeCurrent;

    [Header("Statistics")]
    [SerializeField] public float exp;
    [SerializeField] private float expMax;
    [SerializeField] public float level;
    [SerializeField] private float damageExtra;

    [Header("Hit")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float dañoGolpe;
    [SerializeField] private float tiempoEntreAtaques;
    [SerializeField] private float tiempoSiguienteAtaque;
    [SerializeField] private float timeForAttack;

    [Header("HealthBar")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] public float health;

    [Header("Doors Bosses")]
    [SerializeField] private GameObject doorBoss1;
    [SerializeField] private GameObject doorBoss2;

    private float resetSpeed;
    private float x, y;
    private bool isWalking;
    private bool isReceiveDamage;
    private Vector2 moveDir;
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D bx;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bx = GetComponent<BoxCollider2D>();
        
        health = hpPlayerMax;
        healthBar.UpdateHealthBar(hpPlayerMax, health);
        resetSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0 && !isReceiveDamage)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");

            if (x != 0 || y != 0)
            {
                animator.SetFloat("X",x);
                animator.SetFloat("Y",y);
                if (!isWalking)
                {
                    isWalking = true;
                    animator.SetBool("IsMoving", isWalking);
                }
            }else
            {
                if (isWalking)
                {
                    isWalking = false;
                    animator.SetBool("IsMoving", isWalking);
                }
            }
            moveDir = new Vector2(x, y).normalized;

            if (tiempoEntreAtaques > 0)
            {
                tiempoSiguienteAtaque -= Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.K) && tiempoSiguienteAtaque <= 0 && level >= 0)
            {
                Golpe();
                tiempoSiguienteAtaque = tiempoEntreAtaques;
            }
        }


    }

    private void FixedUpdate()
    {
        if (health > 0 && !isReceiveDamage)
        {
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
            //Hit 
            if (x == 1) //Hit Right 
            {
                controladorGolpe.transform.position = new Vector2(transform.position.x + 0.955f, transform.position.y - 0.041f);
            }
            if (x == -1) //Hit Left
            {
                controladorGolpe.transform.position = new Vector2(transform.position.x - 0.969f, transform.position.y - 0.09f);
            }
            if (y == 1) //Hit Up
            {
                controladorGolpe.transform.position = new Vector2(transform.position.x - 0.032f, transform.position.y + 1.239f);
            }
            if (y == -1) //Hit Down
            {
                controladorGolpe.transform.position = new Vector2(transform.position.x - 0.013f, transform.position.y - 0.924f);
            }

            if (level == 1)
            {
                doorBoss1.SetActive(false);
                //GiveMoreDamage(damageExtra);
            }
            if (level == 2)
            {
                doorBoss2.SetActive(false);
            }
            if (Input.GetKey(KeyCode.E) && level >= 0 && health <= 10)
            {
                GiveHeath(giveHealth);

            }
        }
        
    }
    IEnumerator StopMoving()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(timeForAttack);
        moveSpeed = resetSpeed;
    }

    private void Golpe()
    {
        animator.SetTrigger("Golpe");
        StartCoroutine(StopMoving());
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D collision in objetos)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<EnemyCustom>().ReceiveDamage(dañoGolpe);
            }
            if (collision.gameObject.tag == "Boss1")
            {
                collision.GetComponent<BossRat>().ReceiveDamage(dañoGolpe);
            }
            if (collision.gameObject.tag == "Boss2")
            {
                collision.GetComponent<FuncaBoss>().ReceiveDamage(dañoGolpe);
            }
            if (collision.gameObject.tag == "BabyRat")
            {
                collision.GetComponent<SpawnCustom>().ReceiveDamage(dañoGolpe);
            }
        }
    }
    public void GiveHeath(float life)
    {
        moveSpeed = 0;
        timeCurrent += Time.deltaTime;
        if (timeCurrent >= timeNextHealth)
        {
            
            timeCurrent = 0;
            animator.SetTrigger("Health");
            health = life;

        }
        if (health >= 10)
        {
            moveSpeed = resetSpeed;
        }
        
        Debug.Log(hpPlayerMax + " : " + health);
        healthBar.UpdateHealthBar(hpPlayerMax, health);        
    }

    public void GiveMoreDamage(float moreDamage)
    {
        dañoGolpe += moreDamage;
    }


    public void ReceiveDamage(float damage)
    {
        health -= damage;
        StartCoroutine(StopMoving());
        animator.SetTrigger("Hit");
        Debug.Log(hpPlayerMax + " : " + health);
        healthBar.UpdateHealthBar(hpPlayerMax, health);
        isReceiveDamage = true;
        if (health <= 0)
        {
            bx.enabled = false;
            Debug.Log(bx.enabled + "Muerto");
            StartCoroutine(ReloadGame());
        }
    }

    public void ResetDamage()
    {
        isReceiveDamage = false;
        bx.enabled = true;
    }

    private IEnumerator ReloadGame()
    {
        animator.SetBool("Died",true);
        
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("PruebaCheckPoint").GetComponent<ControllerDataGame>().LoadData();
        animator.SetBool("Died", false);
        isReceiveDamage = false;
        //bx.enabled = true;
        Debug.Log("Murio y renaciste");
    }

    public void LevelUp(float levelUp)
    {
        level += levelUp;
    }

    public void ExpUp(float expEnemy)
    {
        exp += expEnemy;
        if (exp >= expMax)
        {
            level++;
            expMax = Mathf.Round(expMax * 1.3f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Door")
        {
            LevelUp(1);
        }
        
    }


}

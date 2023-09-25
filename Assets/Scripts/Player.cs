using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Live")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float hpPlayerMax;

    [Header("Give Health")]
    [SerializeField] private float timeNextHealth;
    [SerializeField] private float giveHealth;
    [SerializeField] private float timeCurrent;

    [Header("Exp and Level")]
    [SerializeField] private float exp;
    [SerializeField] private float expMax;
    [SerializeField] private float level;

    [Header("Hit")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float dañoGolpe;
    [SerializeField] private float tiempoEntreAtaques;
    [SerializeField] private float tiempoSiguienteAtaque;
    [SerializeField] private float timeForAttack;

    [Header("HealthBar")]
    [SerializeField] private HealthBar healthBar;
    private float health;

    [Header("Doors Bosses")]
    [SerializeField] private GameObject doorBoss1;
    [SerializeField] private GameObject doorBoss2;

    private float resetSpeed;
    private float x, y;
    private bool isWalking;
    private Vector2 moveDir;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        health = hpPlayerMax;
        healthBar.UpdateHealthBar(hpPlayerMax, health);
        resetSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
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
        if (Input.GetKey(KeyCode.K) && tiempoSiguienteAtaque <= 0 && level >= 0)
        {
            Golpe();
            tiempoSiguienteAtaque = tiempoEntreAtaques;
        }

        if (level == 1)
        {
            doorBoss1.SetActive(false);
        }
        if (level == 2)
        {
            doorBoss2.SetActive(false);
        }
        if (Input.GetKey(KeyCode.J) && level >= 0)
        {
            GiveHeath(giveHealth);

        }
    }

    private void FixedUpdate()
    {
        if (health > 0)
        {
            //rb.velocity = moveDir * moveSpeed * Time.deltaTime;
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
        StartCoroutine(StopMoving());
        if (hpPlayerMax >= 15)
        {
            timeCurrent += Time.deltaTime;
            if (timeCurrent >= timeNextHealth)
            {
                timeCurrent = 0;
                health += life;

            }

            healthBar.UpdateHealthBar(hpPlayerMax, health);
        }
        
    }


    public void ReceiveDamage(float damage)
    {
        health -= damage;
        Debug.Log(hpPlayerMax + " : " + health);
        healthBar.UpdateHealthBar(hpPlayerMax, health);
        if (health <= 0)
        {
            Debug.Log("Muerto PLayer");
        }
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

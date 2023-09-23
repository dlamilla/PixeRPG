using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Live")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float hpPlayerMax;
    [SerializeField] private float exp;
    [SerializeField] private float level;

    [Header("Hit")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float dañoGolpe;
    [SerializeField] private float tiempoEntreAtaques;
    [SerializeField] private float tiempoSiguienteAtaque;

    [Header("HealthBar")]
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private float health;
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
        if (Input.GetButtonDown("Fire1") && tiempoSiguienteAtaque <= 0)
        {
            Golpe();
            tiempoSiguienteAtaque = tiempoEntreAtaques;
        }
    }

    private void FixedUpdate()
    {
        if (health > 0)
        {
            rb.velocity = moveDir * moveSpeed * Time.deltaTime;
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
    private void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    private void Golpe()
    {
        animator.SetTrigger("Golpe");
        StopMoving();
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D collision in objetos)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<EnemyCustom>().ReceiveDamage(dañoGolpe);
            }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }

    
}

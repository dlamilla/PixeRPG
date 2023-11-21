using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BossScorpion : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float hpEnemy;
    [SerializeField] private float speed;
    private ChangeAnimation changeDirections;
    private Animator anim;
    private Rigidbody2D rb;
    private Transform player;
    private BoxCollider2D player1;
    private bool isGrabbing = false;

    [Header("HealthBar")]
    [SerializeField] private HealthBar healthBar;
    private float hpCurrent;
    [SerializeField] private GameObject healthBarBoss;

    [Header("Projectile")]
    public GameObject projectilePrefab;

    [Header("Rewards")]
    [SerializeField] private float expEnemy;
    [SerializeField] private float extraDamage;

    [Header("SFX")]
    [SerializeField] private AudioClip soundHit;
    [SerializeField] private AudioClip soundDied;

    private bool hasFiredProjectile = false;
    private AudioSource sfxSound;
    private void Awake()
    {
        sfxSound = gameObject.AddComponent<AudioSource>();
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        player1 = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
        changeDirections = GetComponent<ChangeAnimation>();
        hpCurrent = hpEnemy;
        healthBar.UpdateHealthBar(hpEnemy, hpCurrent);
    }

    void Update()
    {
        if (hpCurrent > 14f)
        {
            if (anim.GetBool("Agarre") || isGrabbing)
            {
                // Verifica si el trigger "Agarre" o el booleano "Agarrando" están activos
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Catch_Scorpion"))
                {
                    isGrabbing = false;
                    anim.SetBool("Agarrando", false);
                    anim.ResetTrigger("Agarre");
                }
                rb.velocity = Vector2.zero;
                anim.SetBool("isWalking", false);
            }
            else
            {
                Vector2 moveDirection = (player.position - transform.position).normalized;
                rb.velocity = moveDirection * speed;

                changeDirections.changeAnim(moveDirection);

                anim.SetBool("isWalking", true);

                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shoot_Scorpion"))
                {
                    if (!hasFiredProjectile)
                    {
                        ShootProjectile();
                    }
                }
                else
                {
                    hasFiredProjectile = false;
                }
            }
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null)
        {
            // Instancia el proyectil en la posición del enemigo
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                Vector2 directionToPlayer = (player.position - transform.position).normalized;
                projectileScript.SetDirection(directionToPlayer);
            }
            hasFiredProjectile = true; 
        }
    }
    public void ReceiveDamage(float damage)
    {
        hpCurrent -= damage;
        anim.SetTrigger("Hit");
        sfxSound.clip = soundHit;
        sfxSound.loop = false;
        sfxSound.volume = 0.7f;
        sfxSound.Play();
        healthBar.UpdateHealthBar(hpEnemy, hpCurrent);
        if (hpCurrent <= 14f)
        {
            anim.SetTrigger("Died");
            sfxSound.clip = soundDied;
            sfxSound.loop = false;
            sfxSound.volume = 0.7f;
            sfxSound.Play();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isReceiveDamage = false;
            player.GetComponent<Player>().ExpUp(expEnemy);
            player.GetComponent<Player>().LevelUp(1);
            player.GetComponent<Player>().GiveMoreDamage(extraDamage);
            healthBarBoss.SetActive(false);
            //gameObject.SetActive(false);
        }
    }

    public void RestartLife()
    {
        hpCurrent = hpEnemy;
        healthBar.UpdateHealthBar(hpEnemy, hpCurrent);
    }
}

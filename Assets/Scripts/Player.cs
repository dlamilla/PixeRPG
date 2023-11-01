using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField] private float moveSpeed;
    [SerializeField] public float hpPlayerMax;
    [SerializeField] private float life;
    [SerializeField] private float timeAfterDied;

    [Header("Give Health")]
    [SerializeField] private float timeNextHealth;
    [SerializeField] private float giveHealth;

    [Header("Statistics")]
    [SerializeField] public float exp;
    [SerializeField] private float expMax;
    [SerializeField] public float level;
    [SerializeField] private float damageExtra;

    [Header("Hit")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float dañoGolpe;
    [SerializeField] private float timeForAttack;
    [SerializeField] public bool isReceiveDamage;

    [Header("HealthBar")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] public float health;

    [Header("Doors Bosses")]
    [SerializeField] private GameObject doorBoss1;
    [SerializeField] private GameObject doorBoss2;
    [SerializeField] private GameObject doorBoss3;

    [Header("Boss Scorpion Info")]
    [SerializeField] private float danoAgarre;
    [SerializeField] public Animator enemyAnimator;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    private bool isDashing;
    private bool canDash = true;
    
    private float resetSpeed;
    private float x, y;
    private bool isWalking;
    private bool isReceiveReal;
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D bx;
    private SpriteRenderer playerSpriteRenderer;
    private PlayerInput playerInput;
    private Vector2 inputMov;
    private Vector2 normalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bx = GetComponent<BoxCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();

        health = hpPlayerMax;
        healthBar.UpdateHealthBar(hpPlayerMax, health);
        resetSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        if (health > 4f && !isReceiveDamage && moveSpeed > 0 && !isReceiveReal)
        {
            inputMov = playerInput.actions["Move"].ReadValue<Vector2>();
            normalInput = inputMov.normalized;
            x = inputMov.x;
            y = inputMov.y;

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
        }
    }

    private void FixedUpdate()
    {

        if (health > 4f && !isReceiveDamage && moveSpeed > 0 && !isReceiveReal)
        {
            //Ataque del 3er boss
            Supresion();

            //Mov del player
            if (!isDashing)
            {
                rb.MovePosition(rb.position + inputMov.normalized * moveSpeed * Time.fixedDeltaTime);
            }
            

            //Hitbox del area del daño
            CheckHitBox();

            if (level == 1)
            {
                //Desactiva pase luego de eliminar al boss
                doorBoss1.SetActive(false);
            }
            if (level == 2)
            {
                //Desactiva pase luego de eliminar al boss
                doorBoss2.SetActive(false);
            }
            if (level == 3)
            {
                //Desactiva pase luego de eliminar al boss
                doorBoss3.SetActive(false);
            }

            
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(normalInput.x * dashSpeed, normalInput.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;        
    }

    //Detecta donde impactara el daño al ejecutar la accion
    private void CheckHitBox()
    {
        if (x <= 1 && x > 0) //Hit Right 
        {
            controladorGolpe.transform.position = new Vector2(transform.position.x + 0.955f, transform.position.y - 0.041f);
        }
        if (x >= -1 && x < 0) //Hit Left
        {
            controladorGolpe.transform.position = new Vector2(transform.position.x - 0.969f, transform.position.y - 0.09f);
        }
        if (y <= 1 && y > 0) //Hit Up
        {
            controladorGolpe.transform.position = new Vector2(transform.position.x - 0.032f, transform.position.y + 1.239f);
        }
        if (y >= -1 && y < 0) //Hit Down
        {
            controladorGolpe.transform.position = new Vector2(transform.position.x - 0.013f, transform.position.y - 0.924f);
        }
    }

    //El player desaparece al recibir daño por el BossScorpion
    private void Supresion()
    {
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Supression"))
        {
            playerSpriteRenderer.enabled = false;
            StartCoroutine(StopMoving());
            ReceiveDamage(danoAgarre);
        }
        else
        {
            playerSpriteRenderer.enabled = true;
        }
    }

    //Hace daño a los enemigos segun su tag
    private void Golpe()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D collision in objetos)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<AllEnemysIA>().ReceiveDamage(dañoGolpe);
            }
            if (collision.gameObject.tag == "Boss1")
            {
                collision.GetComponent<BossRat>().ReceiveDamage(dañoGolpe);
            }
            if (collision.gameObject.tag == "Boss2")
            {
                collision.GetComponent<FuncaBoss>().ReceiveDamage(dañoGolpe);
            }
            if (collision.gameObject.tag == "Boss3")
            {
                collision.GetComponent<BossScorpion>().ReceiveDamage(dañoGolpe);
            }
            if (collision.gameObject.tag == "BabyRat")
            {
                collision.GetComponent<SpawnCustom>().ReceiveDamage(dañoGolpe);
            }
        }
    }

    //Con la tecla K en teclado y Cuadrado en mando para atacar
    public void DamagePlayer(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && level >= 0 && !isReceiveDamage)
        {
            animator.SetTrigger("Golpe");
            StartCoroutine(StopMoving());
        }
    }

    //Con la tecla Q en teclado y X en mando para dashear
    public void DashPlayer(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && canDash)
        {

            StartCoroutine(Dash());
        }
    }

    //Tecla espacio para testear reiniciar el juego
    public void RestartGame(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            SceneManager.LoadScene(0);
        }
    }

    //Con la tecla E en teclado y triangulo en mando para curarse
    public void HealthPlayer(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && level >= 0)
        {
            GiveHeath(giveHealth);
        }
    }

    //Con la tecla F en teclado y Circulo en mando para interactuar con los letreros
    public void Talking(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            string typeController = playerInput.currentControlScheme;
            GameObject.Find("Sign").GetComponent<DialogueNPC>().StartTalking(typeController);
        }
    }

    //Con la R en teclado y L1 en mando las cajas regresan a su sitio
    public void RestartLevel(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Box");
            foreach (GameObject item in obj)
            {
                item.GetComponent<RespawnBox>().RestartBox();
            }
        }
    }

    //Aumentar vida
    public void GiveHeath(float life)
    {
        if (health < 10 && health > 0)
        {
            animator.SetTrigger("Health");
            health = life;
            healthBar.UpdateHealthBar(hpPlayerMax, health);
            StartCoroutine(StopMovingAfterHealth());
        }
               
    }

    //Tiempo en que se queda quieto mientras se cura
    IEnumerator StopMovingAfterHealth()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(timeNextHealth);
        moveSpeed = resetSpeed;
    }

    //Metodo sin usar
    public void GiveMoreDamage(float moreDamage)
    {
        dañoGolpe += moreDamage;
    }

    //Recibe daño el player
    public void ReceiveDamage(float damage)
    {
        isReceiveDamage = true;
        health -= damage;
        StartCoroutine(StopMoving());
        animator.SetTrigger("Hit");
        healthBar.UpdateHealthBar(hpPlayerMax, health);
        
        if (health <= 4f)
        {
            bx.enabled = false;
            ChangeLife();
            if (life != 0)
            {
                StartCoroutine(ReloadGame());
            }  
        }
    }

    //Permite detener al player por un tiempo antes de cualquier accion
    IEnumerator StopMoving()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(timeForAttack);
        moveSpeed = resetSpeed;
    }

    //Respawn a chekpoint si life es diferente de 0
    public IEnumerator ReloadGame()
    {
        animator.SetBool("Died",true);
        yield return new WaitForSeconds(1.5f);
        transform.position = GameObject.FindWithTag("Respawn").GetComponent<CheckPointController>().checkPoint;
        StartCoroutine(StopMovingAfterDied()); 
    }

    //Tiempo entre que se muere y renace
    IEnumerator StopMovingAfterDied()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(timeAfterDied);
        moveSpeed = resetSpeed;
        health = hpPlayerMax;
        healthBar.UpdateHealthBar(hpPlayerMax, health);
        animator.SetBool("Died", false);
        isReceiveDamage = false;
    }

    //Usado en la animacion de daño, ultimo frame
    public void ResetDamage()
    {
        isReceiveDamage = false;
        bx.enabled = true;
    }

    //Sube nivel del player, segun el boss asesinado
    public void LevelUp(float levelUp)
    {
        level += levelUp;
    }

    //Aumenta la exp del player, segun los enemigos asesinados
    public void ExpUp(float expEnemy)
    {
        exp += expEnemy;
        if (exp >= expMax)
        {
            //level++;
            expMax = Mathf.Round(expMax * 1.3f);
        }
    }

    //Despues de morir su vida baja 1
    private void ChangeLife()
    {
        life--;
        if (life <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }

    //Luego de pasar la primera habitacion se establece su nivel a 0
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Door")
        {
            LevelUp(1);
        }
    }

    //Detecta que dispositivo esta usando Teclado o Mando
    public void ControlsChanged(PlayerInput playerInput)
    {
        Debug.Log("Cambio de dispositivo: " + playerInput.currentControlScheme);
    }
}

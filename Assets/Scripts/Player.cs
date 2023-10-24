using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Live")]
    [SerializeField] private float moveSpeed;
    [SerializeField] public float hpPlayerMax;

    [Header("Give Health")]
    [SerializeField] private float timeNextHealth;
    [SerializeField] private float giveHealth;
    //[SerializeField] private float timeCurrent;

    [Header("Statistics")]
    [SerializeField] public float exp;
    [SerializeField] private float expMax;
    [SerializeField] public float level;
    [SerializeField] private float damageExtra;

    [Header("Hit")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float dañoGolpe;
    //[SerializeField] private float tiempoEntreAtaques;
    //[SerializeField] private float tiempoSiguienteAtaque;
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
    
    private float resetSpeed;
    private float x, y;
    private bool isWalking;
    private bool isReceiveReal;
    private Vector2 moveDir;
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D bx;
    private SpriteRenderer playerSpriteRenderer;
    private PlayerInput playerInput;
    private Vector2 inputMov;
    

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
            
            //x = Input.GetAxisRaw("Horizontal");
            //y = Input.GetAxisRaw("Vertical");

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
            //moveDir = new Vector2(x, y).normalized;

            //if (tiempoEntreAtaques > 0)
            //{
            //    tiempoSiguienteAtaque -= Time.deltaTime;
            //}
            //if (Input.GetKeyDown(KeyCode.K) && tiempoSiguienteAtaque <= 0 && level >= 0 && !isReceiveReal)
            //{
            //    animator.SetTrigger("Golpe");
            //    StartCoroutine(StopMoving());
            //    tiempoSiguienteAtaque = tiempoEntreAtaques;
            //}
        }


    }

    private void FixedUpdate()
    {
        if (health > 4f && !isReceiveDamage && moveSpeed > 0 && !isReceiveReal)
        {
            
                Supresion();
            

            rb.MovePosition(rb.position + inputMov.normalized * moveSpeed * Time.fixedDeltaTime);
            //Hit 
            CheckHitBox();

            if (level == 1)
            {
                doorBoss1.SetActive(false);
                //health = hpPlayerMax;
                //healthBar.UpdateHealthBar(hpPlayerMax, health);
                //GiveMoreDamage(damageExtra);
            }
            if (level == 2)
            {
                doorBoss2.SetActive(false);
                //health = hpPlayerMax;
                //healthBar.UpdateHealthBar(hpPlayerMax, health);
            }
            if (level == 3)
            {
                doorBoss3.SetActive(false);
                //health = hpPlayerMax;
                //healthBar.UpdateHealthBar(hpPlayerMax, health);
            }
            //if (Input.GetKey(KeyCode.E) && level >= 0 && health <= 10)
            //{
            //    GiveHeath(giveHealth);
            //}
        }

    }

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

    private void Supresion()
    {
    
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Supression"))
        {
            playerSpriteRenderer.enabled = false;
            StartCoroutine(StopMoving());
            //isReceiveReal = true;
            ReceiveDamage(danoAgarre);
            
            
        }
        else
        {
            playerSpriteRenderer.enabled = true;
            //isReceiveReal = false;
            //isReceiveDamage = false;
        }
        

    }
    IEnumerator StopMoving()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(timeForAttack);
        moveSpeed = resetSpeed;
    }

    IEnumerator StopMovingAfterDied()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(1f);
        moveSpeed = resetSpeed;
        health = hpPlayerMax;
        healthBar.UpdateHealthBar(hpPlayerMax, health);
    }

    IEnumerator StopMovingAfterHealth()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(timeNextHealth);
        moveSpeed = resetSpeed;
    }

    private void Golpe()
    {
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
            if (collision.gameObject.tag == "Boss3")
            {
                collision.GetComponent<BossScorpion>().ReceiveDamage(dañoGolpe);
            }
            if (collision.gameObject.tag == "BabyRat")
            {
                collision.GetComponent<SpawnCustom>().ReceiveDamage(dañoGolpe);
            }
            if (collision.gameObject.tag == "Snake")
            {
                collision.GetComponent<ScriptSnake>().ReceiveDamage(dañoGolpe);
            }
        }
    }

    public void DamagePlayer(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && level >= 0 && !isReceiveDamage)
        {
            animator.SetTrigger("Golpe");
            StartCoroutine(StopMoving());
        }
    }

    public void RestartGame(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void HealthPlayer(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && level >= 0)
        {
            GiveHeath(giveHealth);
        }
    }

    public void Talking(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            string typeController = playerInput.currentControlScheme;
            GameObject.Find("Sign").GetComponent<DialogueNPC>().StartTalking(typeController);
        }
    }

    public void RestartLevel(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            GameObject.FindGameObjectWithTag("CheckPoint").GetComponent<ControllerDataGame>().LoadData();
        }
    }

    public void GiveHeath(float life)
    {
        //timeCurrent += Time.deltaTime;
        if (health < 10 && health > 0)
        {
            
            //timeCurrent = 0;
            animator.SetTrigger("Health");
            health = life;
            StartCoroutine(StopMovingAfterHealth());
        }
        healthBar.UpdateHealthBar(hpPlayerMax, health);        
    }

    public void GiveMoreDamage(float moreDamage)
    {
        dañoGolpe += moreDamage;
    }


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
            //if (level > 0 && level <= 1)
            //{
                
            //    if (Boss1.activeInHierarchy)
            //    {
            //        GameObject.FindGameObjectWithTag("Boss1").GetComponent<BossRat>().RestartLife();
            //    }
                
            //}
            //else if (level >= 1 && level < 2)
            //{
                
            //    if (Boss2.activeInHierarchy)
            //    {
            //        GameObject.FindGameObjectWithTag("Boss2").GetComponent<FuncaBoss>().RestartLife();
            //    }
                
            //}
            //else if (level >= 2 && level < 3)
            //{
                
            //    if (Boss3.activeInHierarchy)
            //    {
            //        //GameObject.FindGameObjectWithTag("Boss3").GetComponent<BossScorpion>().RestartLife();
            //    }
            //}
            StartCoroutine(ReloadGame());
        }
    }

    public IEnumerator ReloadGame()
    {
        animator.SetBool("Died",true);
        yield return new WaitForSeconds(1.5f);
        transform.position = GameObject.FindWithTag("Respawn").GetComponent<CheckPointController>().checkPoint;
        StartCoroutine(StopMovingAfterDied());
        animator.SetBool("Died", false);
        isReceiveDamage = false;
    }

    public void ResetDamage()
    {
        isReceiveDamage = false;
        bx.enabled = true;
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
            //level++;
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

    public void ControlsChanged(PlayerInput playerInput)
    {
        Debug.Log("Cambio de dispositivo: " + playerInput.currentControlScheme);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] public float hpPlayerMax;
    [SerializeField] public float lifeMax;
    [SerializeField] private float timeAfterDied;

    [Header("Hit")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] public float dañoGolpe;
    [SerializeField] private float timeForAttack;
    [SerializeField] public bool isReceiveDamage;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    [Header("Give Health")]
    [SerializeField] private float timeNextHealth;
    [SerializeField] private float giveHealth;

    [Header("Statistics")]
    [SerializeField] public float exp;
    [SerializeField] private float expMax;
    [SerializeField] public float level;

    [Header("HealthBar1")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] public float health;

    [Header("HealthBar2")]
    [SerializeField] private HealthBar2 healthBar2;

    [Header("LifeBar")]
    [SerializeField] private LifeBar lifeBar;
    [SerializeField] public float life;

    [Header("ControllerBar")]
    [SerializeField] private GameObject bar1;
    [SerializeField] private GameObject bar2;

    [Header("Doors Bosses")]
    [SerializeField] private GameObject doorBoss1;
    [SerializeField] private GameObject doorBoss2;
    [SerializeField] private GameObject doorBoss3;

    [Header("Boss Scorpion Info")]
    [SerializeField] private float danoAgarre;
    [SerializeField] public Animator enemyAnimator;

    [Header("TextUI")]
    [SerializeField] private TMP_Text textUI;
    [SerializeField] private GameObject UI_Panel;
    [SerializeField] private GameObject canvasPause;
    [SerializeField] private GameObject textCargar;
    [SerializeField] private GameObject buttonSi;
    [SerializeField] private GameObject buttonNo;
    [SerializeField] private GameObject buttonReanudar;
    [SerializeField] private GameObject buttonSaveExit;
    [SerializeField] private GameObject buttonExit;
    [SerializeField] private GameObject diedCanvas;
    [SerializeField] private GameObject canvasFinal;
    [SerializeField] private GameObject diedYes;
    [SerializeField] private GameObject exitGame;

    [Header("Message")]
    [SerializeField] private GameObject message1;
    [SerializeField] private GameObject message2;
    [SerializeField] private GameObject message3;

    [Header("SFX")]
    [SerializeField] private AudioClip axeSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip soundAttack;
    [SerializeField] private AudioClip soundHeal;
    //[SerializeField] private GameObject message4;

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
    private AudioSource sfxSound1;
    private AudioSource sfxSound2;
    private int cont;
    private bool isPause;

    private void Awake()
    {
        sfxSound2 = gameObject.AddComponent<AudioSource>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bx = GetComponent<BoxCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        sfxSound1 = GetComponent<AudioSource>();

        health = hpPlayerMax;
        healthBar.UpdateHealthBar(hpPlayerMax, health);

        health = hpPlayerMax;
        healthBar2.UpdateHealthBar(hpPlayerMax, health);
        healthBar2.UpdateDashBar(1f);

        life = lifeMax;
        lifeBar.UpdateLifeBar(lifeMax, life);

        resetSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        if (health > 4f && !isReceiveDamage && moveSpeed > 0 && !isReceiveReal && !isPause)
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

        if (health > 4f && !isReceiveDamage && moveSpeed > 0 && !isReceiveReal && !isPause)
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

            if (level >= 1)
            {
                //Desactiva pase luego de eliminar al boss
                doorBoss1.SetActive(false);
                //Indicacion al acabar con el boss1
                message1.SetActive(true);
                //Barra nueva
                bar1.SetActive(false);
                bar2.SetActive(true);

            }
            if (level == 2)
            {
                //Desactiva pase luego de eliminar al boss
                //doorBoss2.SetActive(false);
                //Activa barra de vida v2
                //bar1.SetActive(false);
                //bar2.SetActive(true);
                //Indicacion al acabar con el boss2
                message2.SetActive(true);

                //Fin Demo
                StartCoroutine(FinalCanvas());
            }
            if (level == 3)
            {
                //Desactiva pase luego de eliminar al boss
                doorBoss3.SetActive(false);
                //Indicacion al acabar con el boss3
                message3.SetActive(true);
            }
            if (level == 4)
            {   
                //Indicacion al acabar con el boss4
                //message4.SetActive(true);
            }
            if (level == 5)
            {
                StartCoroutine(FinalCanvas());
            }
        }
    }

    IEnumerator FinalCanvas()
    {
        yield return new WaitForSeconds(3f);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(exitGame);
        canvasFinal.SetActive(true);
        level = -1;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(normalInput.x * dashSpeed, normalInput.y * dashSpeed);
        animator.SetBool("Dash", true);
        healthBar2.UpdateDashBar(0f);
        particles.Play();
        sfxSound1.clip = dashSound;
        sfxSound1.Play();
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.velocity = Vector2.zero;
        animator.SetBool("Dash", false);

        yield return new WaitForSeconds(dashCooldown);
        healthBar2.UpdateDashBar(1f);
        canDash = true;        
    }

    //Detecta donde impactara el daño al ejecutar la accion
    private void CheckHitBox()
    {
        if (x <= 1 && x > 0) //Hit Right 
        {
            controladorGolpe.transform.position = new Vector2(transform.position.x + 1.71f, transform.position.y - 0.52f);
        }
        if (x >= -1 && x < 0) //Hit Left
        {
            controladorGolpe.transform.position = new Vector2(transform.position.x - 1.71f, transform.position.y - 0.52f);
        }
        if (y <= 1 && y > 0) //Hit Up
        {
            controladorGolpe.transform.position = new Vector2(transform.position.x + 0.14f, transform.position.y + 1.54f);
        }
        if (y >= -1 && y < 0) //Hit Down
        {
            controladorGolpe.transform.position = new Vector2(transform.position.x - 0.11f, transform.position.y - 1.91f);
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
        sfxSound1.clip = axeSound;
        sfxSound1.Play();

        sfxSound2.clip = soundAttack;
        sfxSound2.loop = false;
        sfxSound2.Play();
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
            if (collision.gameObject.tag == "JefeSelva")
            {
                collision.GetComponent<ForestBoss>().ReceiveDamage(dañoGolpe);
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
        if (callbackContext.started && canDash && level >= 2 && (x != 0 || y != 0))
        {

            StartCoroutine(Dash());
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
        if (callbackContext.started)
        {
            if (GameObject.Find("Sign").activeInHierarchy == true)
            {
                string typeController = playerInput.currentControlScheme;
                GameObject.Find("Sign").GetComponent<DialogueNPC>().StartTalking(typeController);
            }
            else
            {
                Debug.Log("No hay cartel cerca");
            }
            
        }
    }

    //Con la tecla ESC en teclado y Start en mando para interactuar con los letreros
    public void PauseGame(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            cont += 1;
            if (cont == 1)
            {
                isPause = true;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(buttonReanudar);
                canvasPause.SetActive(true);
                Time.timeScale = 0f;
                textCargar.SetActive(false);
                buttonSi.SetActive(false);
                buttonNo.SetActive(false);
                buttonReanudar.SetActive(true);
                buttonSaveExit.SetActive(false);
                buttonExit.SetActive(true);
            }
            else
            {
                cont = 0;
                isPause = false;
                canvasPause.SetActive(false);
                Time.timeScale = 1f;
            }
            
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
            sfxSound1.clip = soundHeal;
            sfxSound1.Play();
            animator.SetTrigger("Health");
            health = life;
            healthBar.UpdateHealthBar(hpPlayerMax, health);
            healthBar2.UpdateHealthBar(hpPlayerMax, health);
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

    //Metodo usado cuando acabas con cualquier boss
    public void GiveMoreDamage(float moreDamage)
    {
        dañoGolpe += moreDamage;
    }

    public void GiveMoreHealth(float moreHealth)
    {
        hpPlayerMax += moreHealth;
    }

    //Recibe daño el player
    public void ReceiveDamage(float damage)
    {
        isReceiveDamage = true;
        health -= damage;
        StartCoroutine(StopMoving());
        animator.SetTrigger("Hit");
        sfxSound2.clip = hitSound;
        sfxSound2.loop = false;
        sfxSound2.Play();
        healthBar.UpdateHealthBar(hpPlayerMax, health);
        healthBar2.UpdateHealthBar(hpPlayerMax, health);
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
        healthBar2.UpdateHealthBar(hpPlayerMax, health);
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
    public void ChangeLife()
    {
        life--;
        lifeBar.UpdateLifeBar(lifeMax, life);
        if (life <= 0)
        {
            //SceneManager.LoadScene(2);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(diedYes);
            diedCanvas.SetActive(true);
            Time.timeScale = 0f;
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
    public void ControlsChanged(PlayerInput a)
    {
        string b = a.currentControlScheme;
        if (b == "Gamepad")
        {
            StartCoroutine(ChangeControllerGamepad());
        }
        else if (b == "Keyboard")
        {
            StartCoroutine(ChangeControllerKeyboard());
        }
    }

    IEnumerator ChangeControllerGamepad()
    {
        UI_Panel.SetActive(true);
        textUI.text = "Modo de juego configurado para mando.";
        yield return new WaitForSeconds(1.5f);
        UI_Panel.SetActive(false);
    }

    IEnumerator ChangeControllerKeyboard()
    {
        UI_Panel.SetActive(true);
        textUI.text = "Modo de juego configurado para teclado.";
        yield return new WaitForSeconds(1.5f);
        UI_Panel.SetActive(false);
    }

    //Detecta cuando mando se desconectad y cambia a teclado por defecto
    public void ControlsLost(PlayerInput a)
    {
        string b = a.currentControlScheme;
        if (b == "Gamepad")
        {
            StartCoroutine(ChangeControllerLost());
        }
    }

    IEnumerator ChangeControllerLost()
    {
        UI_Panel.SetActive(true);
        textUI.text = "Mando desconectado. Modo de juego configurado para teclado.";
        yield return new WaitForSeconds(1.5f);
        UI_Panel.SetActive(false);
    }

    //Detecta la reconexion del mando
    public void ControlsReconneted(PlayerInput a)
    {
        string b = a.currentControlScheme;
        if (b == "Gamepad")
        {
            StartCoroutine(ChangeControllerReconected());
        }
    }

    IEnumerator ChangeControllerReconected()
    {
        UI_Panel.SetActive(true);
        textUI.text = "Mando reconectado. Modo de juego configurado para mando.";
        yield return new WaitForSeconds(1.5f);
        UI_Panel.SetActive(false);
    }

    public void SalirYGuardarAfterDied()
    {
        diedCanvas.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void FinalBoton()
    {
        canvasFinal.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ReiniciarGame()
    {
        int roomCurrent = GameObject.Find("RoomsManager").GetComponent<RoomsManager>().currentRoom;
        GameObject[] roomGame = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject obj in roomGame)
        {
            obj.gameObject.SetActive(false);
        }
        GameObject.Find("RoomsManager").GetComponent<RoomsManager>().Rooms[roomCurrent].SetActive(true);
        GameObject.Find("RoomsManager").GetComponent<RoomsManager>().currentRoom = roomCurrent;

        health = hpPlayerMax;
        healthBar.UpdateHealthBar(hpPlayerMax, health);

        health = hpPlayerMax;
        healthBar2.UpdateHealthBar(hpPlayerMax, health);
        healthBar2.UpdateDashBar(1f);

        life = lifeMax;
        lifeBar.UpdateLifeBar(lifeMax, life);

        diedCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Salir()
    {
        cont = 0;
        isPause = false;
        canvasPause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Bug()
    {
        isReceiveDamage = false;
    }
}

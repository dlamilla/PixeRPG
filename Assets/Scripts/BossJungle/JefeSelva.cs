using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class JefeSelva : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float hpEnemy;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float increasedSpeed;
    private ChangeAnimation changeDirections;
    private Animator anim;
    private float currentSpeed;
    private Rigidbody2D rb;
    private Transform player;
    private BoxCollider2D player1;
    private bool hasLanzaGoAnimationPlayed = false;
    private bool isFollowingPlayer = true;
    private bool isLanzaDetached = false;
    private float detachTime = 5.0f;
    private float returnToLanzaTime = 5.0f;
    private float detachStartTime = -1f;
    private bool isLanzaGoPlaying = false;
    private bool hasJumped = false; 

    [Header("AtaqueLanza")]
    private Transform lanza;
    private Vector2 lanzaPosition;
    private Transform jefeSelva;

    [Header("Ataque Configuración")]
    [SerializeField] private float probabilidadActivacionLanza = 0.5f;
    private float tiempoEsperaInicial = 5.0f;
    private float tiempoInicioEspera;
    private bool haIniciadoEspera = false;

    public enum TipoAtaque
    {
        LanzaAttack,
        SaltoAttack
    }

    [SerializeField]
    private TipoAtaque listaAtaque;
    private bool isLanzaGoResetting = false;

    [SerializeField] private GameObject sombra; 

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        lanza = transform.Find("Lanza");
        player1 = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
        changeDirections = GetComponent<ChangeAnimation>();
        jefeSelva = GameObject.FindWithTag("JefeSelva").transform;
        currentSpeed = normalSpeed;
        listaAtaque = TipoAtaque.LanzaAttack;
        tiempoInicioEspera = Time.time;

        StartCoroutine(ActivarDesactivarSombra());
    }

    void Update()
    {
        if (!haIniciadoEspera && Time.time >= tiempoInicioEspera + tiempoEsperaInicial)
        {
            haIniciadoEspera = true;

            float randomValue = Random.Range(0f, 1f);

            if (randomValue <= probabilidadActivacionLanza)
            {
                listaAtaque = TipoAtaque.LanzaAttack;
                Debug.Log("Ataque de la lanza activado");
            }
            else
            {
                listaAtaque = TipoAtaque.SaltoAttack;
                Debug.Log("Ataque de salto activado");
            }
        }

        if (listaAtaque == TipoAtaque.LanzaAttack)
        {
            UpdateLanzaAttack();
        }
        else if (listaAtaque == TipoAtaque.SaltoAttack)
        {
            UpdateSaltoAttack();
        }
    }

    void LateUpdate()
    {
        switch (listaAtaque)
        {
            case TipoAtaque.LanzaAttack:
                LateUpdateLanzaAttack();
                break;

            case TipoAtaque.SaltoAttack:
                // Lógica para el ataque de salto
                break;
        }
    }

    private void UpdateLanzaAttack()
    {
        if (isFollowingPlayer && !anim.GetBool("lanzaGo"))
        {
            Vector2 moveDirection = (player.position - transform.position).normalized;

            changeDirections.changeAnim(moveDirection);
            anim.SetBool("enCamino", true);

            transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
        }
        else if (isLanzaDetached)
        {
            transform.position = Vector2.MoveTowards(transform.position, lanzaPosition, increasedSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, lanzaPosition) < 0.1f)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                isFollowingPlayer = true;
                Debug.Log("SeguirPlayer");
                lanza.parent = jefeSelva;
                currentSpeed = normalSpeed;
                listaAtaque = TipoAtaque.LanzaAttack;
            }
        }
    }

    private void LateUpdateLanzaAttack()
    {
        if (!isLanzaDetached && Time.time >= detachTime)
        {
            if (!hasLanzaGoAnimationPlayed)
            {
                StartCoroutine(PlayLanzaGoAnimation());
            }
        }
        else if (isLanzaDetached && isFollowingPlayer && Time.time >= detachStartTime + returnToLanzaTime)
        {
            isFollowingPlayer = false;
        }

        if (lanza.parent == jefeSelva)
        {
            isFollowingPlayer = true;
            listaAtaque = TipoAtaque.LanzaAttack;
        }
    }

    private void UpdateSaltoAttack()
    {
        if (!hasJumped)
        {
            StartCoroutine(PlaySaltoAttackAnimation());
        }
    }

    private IEnumerator PlaySaltoAttackAnimation()
    {
        anim.SetTrigger("inJump");

        hasJumped = true;

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(MoveToPositionY(transform, transform.position.y + 16f, 0.85f)); 
    }

    private IEnumerator PlayLanzaGoAnimation()
    {
        // Verificar si la animación ya está en curso
        if (isLanzaGoPlaying || anim.GetBool("lanzaGo"))
        {
            yield break; 
        }

        isLanzaGoPlaying = true;

        
        yield return null;  

        anim.SetBool("lanzaGo", true);

        
        yield return new WaitForSeconds(0f); //Valor Modificable a mis necesidades

        float animationLength = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        //Puedo medir la espera hasta que haya pasado el tiempo de duración de la animación
        yield return new WaitForSeconds(animationLength);

        anim.SetBool("lanzaGo", false);
        hasLanzaGoAnimationPlayed = true;
        isLanzaGoPlaying = false;

        //Seccion donde me perdite ver donde se realiza la desvinculación y aumento de velocidad
        GetComponent<SpriteRenderer>().color = Color.red;
        lanza.parent = null;
        isLanzaDetached = true;
        lanzaPosition = lanza.position;
        detachStartTime = Time.time;
        currentSpeed = increasedSpeed;

        StartCoroutine(ResetLanzaGoAfterDelay());
    }

    private IEnumerator ResetLanzaGoAfterDelay()
    {
        yield return new WaitForSeconds(0f);

        if (!isLanzaGoResetting)
        {
            hasLanzaGoAnimationPlayed = false;
        }
    }

    public void CancelResetLanzaGo()
    {
        isLanzaGoResetting = true;
    }

    private IEnumerator MoveToPositionY(Transform transform, float targetY, float timeToMove)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        Vector3 targetPos = new Vector3(transform.position.x, targetY, transform.position.z);

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }

    IEnumerator ActivarDesactivarSombra()
    {
        while (true)
        {
            //Activa la sombra solo cuando el ataque de salto está seleccionado
            if (listaAtaque == TipoAtaque.SaltoAttack)
            {
                sombra.SetActive(true);

                // Espera 5 segundos
                yield return new WaitForSeconds(5.0f);

                Debug.Log("Desactivar sombra");
                // Desactiva la sombra
                sombra.SetActive(false);
            }
            yield return new WaitForSeconds(1.0f);

        }
    }
}

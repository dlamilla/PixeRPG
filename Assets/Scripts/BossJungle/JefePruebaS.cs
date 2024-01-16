using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JefePruebaS : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float hpEnemy;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float increasedSpeed;
    [SerializeField] private float fallSpeed = 16.0f;
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
    private bool isSaltoAttackInProgress;

    public enum TipoAtaque
    {
        LanzaAttack,
        SaltoAttack,
        Correteo
    }

    [SerializeField]
    private TipoAtaque listaAtaque;
    private bool isLanzaGoResetting = false;
    private bool estaEligiendoAtaque = false;

    [Header("Sombra")]
    [SerializeField] private GameObject sombra;
    private bool sombraSiguiendo = false;

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

        listaAtaque = TipoAtaque.Correteo;

        tiempoInicioEspera = Time.time + Random.Range(0f, tiempoEsperaInicial);
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

                StartCoroutine(ActivarDesactivarSombra());
                isSaltoAttackInProgress = true;
                StartCoroutine(EsperarYCambiarAtaque(4.0f, TipoAtaque.Correteo));
            }
        }

        if (listaAtaque == TipoAtaque.LanzaAttack)
        {
            UpdateLanzaAttack();
        }
        else if (listaAtaque == TipoAtaque.SaltoAttack)
        {
            if (Vector2.Distance(transform.position, player.position) < 2.0f)
            {
                listaAtaque = TipoAtaque.Correteo;
            }
            else
            {
                UpdateSaltoAttack();
            }
        }
    }
    private IEnumerator EsperarYCambiarAtaque(float tiempoEspera, TipoAtaque nuevoAtaque)
    {
        yield return new WaitForSeconds(tiempoEspera);

        // Nueva línea: Reiniciar el estado después de esperar
        isSaltoAttackInProgress = false;
        listaAtaque = nuevoAtaque;
        Debug.Log("Pasando a " + nuevoAtaque.ToString());
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

            case TipoAtaque.Correteo:
                HacerCorrer();
                StartCoroutine(DecidirProximoAtaque());
                break;
        }
    }

    private void HacerCorrer()
    {
        Vector2 moveDirection = (player.position - transform.position).normalized;

        changeDirections.changeAnim(moveDirection);
        anim.SetBool("enCamino", true);

        transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
    }

    private IEnumerator DecidirProximoAtaque()
    {
        if (estaEligiendoAtaque)
        {
            yield break;
        }

        estaEligiendoAtaque = true;

        yield return new WaitForSeconds(5.0f);

        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= probabilidadActivacionLanza)
        {
            listaAtaque = TipoAtaque.LanzaAttack;
            StartCoroutine(PlayLanzaGoAnimation());
            Debug.Log("Ataque de la lanza activado");
        }
        else
        {
            listaAtaque = TipoAtaque.SaltoAttack;
            StartCoroutine(PlaySaltoAttackAnimation());
            yield return new WaitForSeconds(7.0f);
            StartCoroutine(DescenderEnemigoAlJugador());
            Debug.Log("Ataque de salto activado");

            StartCoroutine(ActivarDesactivarSombra());
        }

        haIniciadoEspera = false;
        estaEligiendoAtaque = false;

        hasLanzaGoAnimationPlayed = false;
        hasJumped = false;
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
                listaAtaque = TipoAtaque.Correteo;
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

        anim.SetBool("inFall", true);

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(MoveToPositionY(transform, transform.position.y + 16f, 0.85f));
    }

    private IEnumerator PlayLanzaGoAnimation()
    {
        if (isLanzaGoPlaying || anim.GetBool("lanzaGo"))
        {
            yield break;
        }

        isLanzaGoPlaying = true;

        yield return null;

        anim.SetBool("lanzaGo", true);

        yield return new WaitForSeconds(0f);

        float animationLength = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        yield return new WaitForSeconds(animationLength);

        anim.SetBool("lanzaGo", false);
        hasLanzaGoAnimationPlayed = true;
        isLanzaGoPlaying = false;

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

            targetPos = new Vector3(transform.position.x, targetY, transform.position.z);
            targetPos.y -= fallSpeed * Time.deltaTime;

            yield return null;
        }

        transform.position = targetPos;
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, float timeToMove)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startingPos, targetPosition, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private IEnumerator ActivarDesactivarSombra()
    {
        if (listaAtaque == TipoAtaque.SaltoAttack && !sombraSiguiendo)
        {
            yield return StartCoroutine(SeguirJugadorConSombra());
        }
    }

    private IEnumerator SeguirJugadorConSombra()
    {
        sombra.SetActive(true);
        sombraSiguiendo = true;

        float tiempoSombra = 3.0f;
        float tiempoInicio = Time.time;

        while (Time.time < tiempoInicio + tiempoSombra)
        {
            if (listaAtaque != TipoAtaque.SaltoAttack)
            {
                break;  // Salir del bucle si el ataque cambia antes de que se cumplan los 3 segundos
            }

            sombra.transform.position = new Vector3(player.position.x, player.position.y, sombra.transform.position.z);
            yield return null;
        }

        sombra.SetActive(false);
        sombraSiguiendo = false;
    }

    private IEnumerator DescenderEnemigoAlJugador()
    {
        float targetY = player.position.y;
        float targetX = player.position.x;

        yield return MoveToPosition(new Vector3(targetX, targetY, transform.position.z), 2.0f);

        transform.position = new Vector3(targetX, targetY, transform.position.z);

        isFollowingPlayer = true;
        anim.SetBool("inFall", false);

        listaAtaque = TipoAtaque.Correteo;
    }
}
